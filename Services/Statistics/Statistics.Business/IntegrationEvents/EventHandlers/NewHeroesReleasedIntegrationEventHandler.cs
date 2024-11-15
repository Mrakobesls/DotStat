﻿using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Statistics.Business.IntegrationEvents.Events;
using Statistics.Business.Services;

namespace Statistics.Business.IntegrationEvents.EventHandlers;

public class NewHeroesReleasedIntegrationEventHandler(IHeroCommands heroCommands, ILogger<NewHeroesReleasedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<NewHeroesReleasedIntegrationEvent>
{
    public async Task Handle(NewHeroesReleasedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        await heroCommands.AddRange(@event.Heroes.Select(x => new Types.Hero { Id = x.OriginalId, Name = x.Name }));
    }
}
