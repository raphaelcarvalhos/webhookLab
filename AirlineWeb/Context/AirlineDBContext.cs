using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Context
{
    public class AirlineDBContext : DbContext
    {
        public AirlineDBContext(DbContextOptions<AirlineDBContext> options) : base(options) { }
        public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }
        public DbSet<FlightDetail> FlightDetails { get; set; }
    }
}