using System;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Context;
using AirlineWeb.Dtos;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AirlineWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookSubscriptionController : ControllerBase
    {
        private readonly AirlineDBContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public WebhookSubscriptionController(AirlineDBContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("secret/{secret}", Name = "GetSubscritionBySecret")]
        public async Task<IActionResult> GetSubscritionBySecret(string secret){
            var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(s => s.Secret.Equals(secret));
            if (subscription == null) return NotFound();
            return Ok(_mapper.Map<WebhookSubscriptionReadDto>(subscription));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(WebhookSubscriptionCreateDto model){
            try
            {
                var subscription = _context.WebhookSubscriptions.FirstOrDefault(s => s.WebhookURL.Equals(model.WebhookURL));
                if (subscription != null ) return Conflict("WebHookURL já existe.");
                subscription = _mapper.Map<WebhookSubscription>(model);
                subscription.Secret = Guid.NewGuid().ToString();
                subscription.WebhookPublisher = _configuration["WebhookPublisher"];
                await _context.WebhookSubscriptions.AddAsync(subscription);
                if (await _context.SaveChangesAsync() <= 0) return BadRequest("Erro ao tentar inscrever webhook.");
                var webhookSubscriptionReadDto = _mapper.Map<WebhookSubscriptionReadDto>(subscription);
                return CreatedAtRoute(nameof(GetSubscritionBySecret), new { secret = webhookSubscriptionReadDto.Secret}, webhookSubscriptionReadDto);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }
    }
}