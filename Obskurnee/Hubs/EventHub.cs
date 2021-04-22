using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Obskurnee.Models;
using System.Threading.Tasks;

namespace Obskurnee.Hubs
{
    public interface IEventHub
    {
        Task DiscussionChanged(Discussion discussion);
    }

    [AllowAnonymous]
    public class EventHub : Hub<IEventHub>
    {
    }
}