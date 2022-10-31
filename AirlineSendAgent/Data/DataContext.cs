using AirlineSendAgent.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineSendAgent.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<WebhookSubscription> WebhookSubscriptions => Set<WebhookSubscription>();
}