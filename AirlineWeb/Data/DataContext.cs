using AirlineWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebhookSubscription>().Property(p => p.Id).IsRequired();
        modelBuilder.Entity<WebhookSubscription>().Property(p => p.WebhookUri).IsRequired();
        modelBuilder.Entity<WebhookSubscription>().Property(p => p.Secret).IsRequired();
        modelBuilder.Entity<WebhookSubscription>().Property(p => p.WebhookType).IsRequired();
        modelBuilder.Entity<WebhookSubscription>().Property(p => p.WebhookPublisher).IsRequired();

        modelBuilder.Entity<FlightDetail>().Property(p => p.Id).IsRequired();
        modelBuilder.Entity<FlightDetail>().Property(p => p.Code).IsRequired();
        modelBuilder.Entity<FlightDetail>().Property(p => p.Price).IsRequired();
        modelBuilder.Entity<FlightDetail>().Property(p => p.Price).HasPrecision(18, 2);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<WebhookSubscription> WebhookSubscriptions => Set<WebhookSubscription>();
    public DbSet<FlightDetail> FlightDetails => Set<FlightDetail>();
}