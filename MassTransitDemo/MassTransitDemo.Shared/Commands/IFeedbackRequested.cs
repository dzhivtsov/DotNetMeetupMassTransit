using System;

namespace MassTransitDemo.Shared.Commands
{
    public interface IFeedbackRequested
    {
        Guid OrderId { get; }
    }
}
