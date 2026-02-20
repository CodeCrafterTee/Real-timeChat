using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RealTimeChat.Data;
using Microsoft.EntityFrameworkCore;

namespace RealTimeChat.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly ApplicationDbContext _context;

    public ChatController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int roomId = 1)
    {
        var rooms = await _context.ChatRooms.ToListAsync();
        var messages = await _context.Messages
            .Include(m => m.User)
            .Where(m => m.ChatRoomId == roomId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        ViewBag.Rooms = rooms;
        ViewBag.CurrentRoomId = roomId;
        return View(messages);
    }
}