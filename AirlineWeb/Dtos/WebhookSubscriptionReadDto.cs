using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineWeb.Dtos
{
    public class WebhookSubscriptionReadDto
    {
        public int Id { get; set; }
        public string WebhookURL { get; set; }
        public string Secret { get; set; }
        public string WebhookType { get; set; }
        public string WebhookPublisher { get; set; }
    }
}