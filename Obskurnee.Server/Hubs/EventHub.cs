using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Obskurnee.Models;

namespace Obskurnee.Hubs;

public interface IEventHub
{
    Task DiscussionChanged(Discussion discussion);
}

[Authorize]
public class EventHub : Hub<IEventHub>
{
}
