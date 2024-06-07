using iLG.API.Services.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace iLG.API.Hubs
{
    public abstract class HubBase(ILogger<HubBase> logger, ITokenService tokenService) : Hub
    {
        private static readonly Dictionary<string, HashSet<string>> _rooms = [];

        public override Task OnConnectedAsync()
        {
            if (!IsAuthorized())
            {
                return OnDisconnectedAsync(new Exception("Invalid Token"));
            }

            logger.LogInformation($"Client {Context.ConnectionId} connected.");
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnID", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (exception != null)
            {
                logger.LogError(exception, $"Client {Context.ConnectionId} disconnected with an error.");
            }
            else
            {
                logger.LogInformation($"Client {Context.ConnectionId} disconnected.");
            }
            return base.OnDisconnectedAsync(exception);
        }

        protected bool IsAuthorized()
        {
            var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString();

            if (string.IsNullOrEmpty(accessToken) || tokenService.IsAccessTokenValid(accessToken))
                return false;

            var user = tokenService.GetPrincipalFromExpiredToken(accessToken);

            if (user is null)
                return false;

            Context.GetHttpContext().User = user;
            return true;
        }

        private async Task LogMessage(string message, LogLevel logLevel = LogLevel.Information)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    logger.LogInformation(message);
                    break;

                case LogLevel.Warning:
                    logger.LogWarning(message);
                    break;

                case LogLevel.Error:
                    logger.LogError(message);
                    break;

                case LogLevel.Critical:
                    logger.LogCritical(message);
                    break;

                default:
                    logger.LogDebug(message);
                    break;
            }

            await Clients.All.SendAsync("LogMessage", message, logLevel.ToString());
        }

        protected async Task JoinRoom(string roomName)
        {
            if (!_rooms.TryGetValue(roomName, out HashSet<string>? value))
            {
                value = ([]);
                _rooms[roomName] = value;
            }

            value.Add(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await LogMessage($"Client {Context.ConnectionId} joined room {roomName}.");
        }

        protected async Task LeaveRoom(string roomName)
        {
            if (_rooms.TryGetValue(roomName, out HashSet<string>? value))
            {
                value.Remove(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await LogMessage($"Client {Context.ConnectionId} left room {roomName}.");
            }
        }

        protected async Task BroadcastToRoom(string roomName, string message)
        {
            if (_rooms.ContainsKey(roomName))
            {
                await Clients.Group(roomName).SendAsync("ReceiveMessage", message);
            }
        }
    }
}