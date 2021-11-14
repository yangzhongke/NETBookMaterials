using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRCoreTest2;
using System.Security.Claims;

namespace SignalRCoreTest1
{
    [Authorize]
    public class ChatRoomHub:Hub
    {
        public Task SendPublicMessage(string message)
        {
            //不应该泄露ConnectionId
            //string connId = this.Context.ConnectionId;
            //string msg = $"{connId} {DateTime.Now}:{message}";
            string name = this.Context.User!.FindFirst(ClaimTypes.Name)!.Value;
            string msg = $"{name} {DateTime.Now}:{message}";
            return Clients.All.SendAsync("ReceivePublicMessage", msg);
        }
    }
}
