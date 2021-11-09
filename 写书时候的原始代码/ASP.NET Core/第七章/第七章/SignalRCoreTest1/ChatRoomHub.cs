using Microsoft.AspNetCore.SignalR;

namespace SignalRCoreTest1
{
    public class ChatRoomHub:Hub
    {
        public Task SendPublicMessage(string message)
        {
            string connId = this.Context.ConnectionId;
            string msg = $"{connId} {DateTime.Now}:{message}";
            return Clients.All.SendAsync("ReceivePublicMessage", msg);
        }
        /*

        public Task SendPrivateMessage(string toUserName,string message)
        {

        }*/
    }
}
