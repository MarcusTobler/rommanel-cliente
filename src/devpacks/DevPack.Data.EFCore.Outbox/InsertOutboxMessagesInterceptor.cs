
using DevPack.Domain.Abstractions;
using DevPack.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace DevPack.Data.EFCore.Outbox;

public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private static void InsertOutboxMessages(DbContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<IEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToArray();
        
        var outboxMessages = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(), 
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, SerializerSettings),
                DateTime.UtcNow))
            .ToList();
        
        domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

        context.Set<OutboxMessage>().AddRange(outboxMessages);
        
        /*
        var outboxMessages = context.ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(), 
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, SerializerSettings),
                DateTime.UtcNow))
            .ToList();
        */
        
        //context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}