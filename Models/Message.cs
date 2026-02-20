using System;

namespace RealTimeChat.Models;

public class Message
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
}