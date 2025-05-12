using DevPack.Domain.Abstractions;
using DevPack.Domain.Core;
using DevPack.Domain.Messaging;
using Microsoft.EntityFrameworkCore;

namespace DevPack.Domain.Mediator.Extensions;

public static class MediatorExtension
{
    public static void PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEvents = ctx.ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();

        var tasks = domainEvents?
            .Select(async (domainEvent) =>
            {
                await mediator.PublishEvent((Event)domainEvent);
            })
            .ToList();

        if (tasks != null && tasks.Count != 0) 
            Task.WhenAll(tasks);
    }
    
    public static async Task PublishEventsAsync<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEvents = ctx.ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();
        
        var tasks = domainEvents?
            .Select(async domainEvent =>
            {
                await mediator.PublishEvent((Event)domainEvent);
            })
            .ToList();

        if (tasks != null && tasks.Count != 0) 
            await Task.WhenAll(tasks);

    }
}