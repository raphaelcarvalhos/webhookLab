using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelAgentWeb.Models;

namespace TravelAgentWeb.Context
{
    public class TravelAgentDBContext : DbContext
    {
        public TravelAgentDBContext(DbContextOptions<TravelAgentDBContext> options) : base(options) { }
        public DbSet<WebhookSecret> WebhookSecrets { get; set; }
    }
}