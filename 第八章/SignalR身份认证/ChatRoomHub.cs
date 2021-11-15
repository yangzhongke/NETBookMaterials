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

        public async Task<string> SendPrivateMessage(string destUserName, string message)
        {
            User? destUser = UserManager.FindByName(destUserName);
            if (destUser == null)
            {
                return "DestUserNotFound";
            }
            string destUserId = destUser.Id.ToString();
            string srcUserName = this.Context.User!.FindFirst(ClaimTypes.Name)!.Value;
            string time = DateTime.Now.ToShortTimeString();
            await this.Clients.User(destUserId).SendAsync("ReceivePrivateMessage",
                srcUserName, time, message);
            return "ok";
        }

    }
}
