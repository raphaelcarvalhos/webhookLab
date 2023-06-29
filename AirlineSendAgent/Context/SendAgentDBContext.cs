using AirlineSendAgent.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineSendAgent.Context
{
    public class SendAgentDBContext : DbContext
    {
        public SendAgentDBContext(DbContextOptions<SendAgentDBContext> options) : base(options) { }

        public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }
    }
}