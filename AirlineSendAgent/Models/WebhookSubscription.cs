using System.ComponentModel.DataAnnotations;

namespace AirlineSendAgent.Models
{
    public class WebhookSubscription
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string WebhookURL { get; set; }
        [Required]
        public string Secret { get; set; }
        [Required]
        public string WebhookType { get; set; }
        [Required]
        public string WebhookPublisher { get; set; }
    }
}