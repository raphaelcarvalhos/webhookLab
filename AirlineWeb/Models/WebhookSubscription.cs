using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineWeb.Models
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