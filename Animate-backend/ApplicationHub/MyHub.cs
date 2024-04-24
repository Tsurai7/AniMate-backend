using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Animate_backend.ApplicationHub;

public class MyHub : Hub
{
    [Authorize]
    public async Task Send(string message, string userName)
    {
        Console.WriteLine($"Received message: {message}");
        await Clients.All.SendAsync("Receive", message, userName);
    }
}