using System.ComponentModel.DataAnnotations;

namespace TravelAgentWeb.Models
{
    public class WebhookSecret
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Secret { get; set; }
        [Required]
        public string Publisher { get; set; }
    }
}