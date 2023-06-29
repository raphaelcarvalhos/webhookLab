using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TravelAgentWeb.Context;
using TravelAgentWeb.Dtos;

namespace TravelAgentWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly TravelAgentDBContext _context;

        public NotificationsController(TravelAgentDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult FlightChanged(FlightDetailUpdateDto model){
            Console.WriteLine($"Webhook Recebido. Publisher: {model.Publisher}");

            var secretModel = _context.WebhookSecrets.FirstOrDefault(s =>
                s.Publisher.Equals(model.Publisher) &&
                s.Secret.Equals(model.Secret));

            if (secretModel == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Secret inválido - Webhook ignorado.");
                Console.ResetColor();
                return Ok();
            } else {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Secret válido.");
                Console.WriteLine($"Preço Antigo {model.OldPrice} | Preço Atualizado {model.NewPrice}");
                Console.ResetColor();
                return Ok();
            }
        }
    }
}