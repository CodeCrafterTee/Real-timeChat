using Microsoft.AspNetCore.SignalR;
using RealTimeChat.Data;
using RealTimeChat.Models;
using Microsoft.EntityFrameworkCore;

namespace RealTimeChat.Hubs;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;

    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string userId, int roomId, string message)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;

        var msg = new Message
        {
            UserId = userId,
            ChatRoomId = roomId,
            Content = message,
            Timestamp = DateTime.Now
        };
        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();

        await Clients.Group($"Room-{roomId}").SendAsync("ReceiveMessage", user.DisplayName, message, msg.Timestamp.ToString("HH:mm"));
    }

    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Room-{roomId}");
    }

    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Room-{roomId}");
    }

    public async Task Typing(int roomId, string displayName)
    {
        await Clients.OthersInGroup($"Room-{roomId}").SendAsync("UserTyping", displayName);
    }
}