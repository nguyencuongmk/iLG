using iLG.API.Services.Abstractions;

namespace iLG.API.Hubs
{
    public class ChatHub(ILogger<ChatHub> logger, ITokenService tokenService) : HubBase(logger, tokenService)
    {
        public async Task JoinChatRoom(string roomName)
        {
            if (IsAuthorized())
            {
                await JoinRoom(roomName);
            }
        }

        public async Task LeaveChatRoom(string roomName)
        {
            if (IsAuthorized())
            {
                await LeaveRoom(roomName);
            }
        }

        public async Task SendMessage(string roomName, string message)
        {
            if (IsAuthorized())
            {
                await BroadcastToRoom(roomName, message);
            }
        }
    }
}
