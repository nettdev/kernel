using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nett.Kernel;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Events?.Any() ?? false);

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Events)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearEvents());

        var tasks = domainEvents.Select(async (domainEvent) => await mediator.Publish(domainEvent));

        await Task.WhenAll(tasks);
    }
}