using System;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Context;
using AirlineWeb.Dtos;
using AirlineWeb.MessageBus;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightDetailController : ControllerBase
    {
        private readonly AirlineDBContext _context;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBus;

        public FlightDetailController(AirlineDBContext context, IMapper mapper, IMessageBusClient messageBus)
        {
            _context = context;
            _mapper = mapper;
            _messageBus = messageBus;
        }

        [HttpGet("flightcode/{flightcode}", Name = "GetSubscritionByFlightCode")]
        public async Task<IActionResult> GetSubscritionByFlightCode(string flightcode){
            var flightDetail = await _context.FlightDetails.FirstOrDefaultAsync(fD => fD.FlightCode.Equals(flightcode));
            if (flightDetail == null) return NotFound("Nenhum detalhe de voo encontrado com esse código.");
            return Ok(_mapper.Map<FlightDetailReadDto>(flightDetail));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlightDetail(FlightDetailCreateDto model){
            try
            {
                var flightDetail = _context.FlightDetails.FirstOrDefault(fD => fD.FlightCode.Equals(model.FlightCode));
                if (flightDetail != null ) return Conflict("Flight detail já existe.");
                flightDetail = _mapper.Map<FlightDetail>(model);
                await  _context.FlightDetails.AddAsync(flightDetail);
                if (await _context.SaveChangesAsync() <= 0) return BadRequest("Erro ao tentar adicionar flight detail.");
                var flightDetailReadDto = _mapper.Map<FlightDetailReadDto>(flightDetail);
                return CreatedAtRoute(nameof(GetSubscritionByFlightCode), new { flightcode = flightDetailReadDto.FlightCode}, flightDetailReadDto);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpPut("id/{id}")]
        public ActionResult UpdateFlightDetail(int id, FlightDetailUpdateDto model){
            try
            {
                var flightDetail = _context.FlightDetails.FirstOrDefault(fD => fD.Id.Equals(id));
                if (flightDetail == null ) return NotFound("Flight detail não encontrado.");

                decimal oldPrice = flightDetail.Price;

                _mapper.Map(model, flightDetail);
                if ( _context.SaveChanges() <= 0) return BadRequest("Não houve alteração nos dados, nenhuma ação tomada.");
                if (oldPrice != model.Price){
                    var message = new NotificationMessageDto{
                        WebhookType = "priceChange",
                        OldPrice = oldPrice,
                        NewPrice = model.Price,
                        FlightCode = model.FlightCode
                    };
                _messageBus.SendMessage(message);
                }

                var flightDetailReadDto = _mapper.Map<FlightDetailReadDto>(flightDetail);

                return CreatedAtRoute(nameof(GetSubscritionByFlightCode),  new { flightcode = flightDetailReadDto.FlightCode}, flightDetailReadDto);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }
    }
}