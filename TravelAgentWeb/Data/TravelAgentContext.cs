using Microsoft.EntityFrameworkCore;
using TravelAgentWeb.Models;

namespace TravelAgentWeb.Data;

public class TravelAgentContext : DbContext
{
    public TravelAgentContext(DbContextOptions<TravelAgentContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebhookSecret>().Property(p => p.Id).IsRequired();
        modelBuilder.Entity<WebhookSecret>().Property(p => p.Secret).IsRequired();
        modelBuilder.Entity<WebhookSecret>().Property(p => p.Publisher).IsRequired();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<WebhookSecret> SubscriptionSecrets => Set<WebhookSecret>();
}