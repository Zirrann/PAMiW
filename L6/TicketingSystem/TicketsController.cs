using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data;
using TicketingSystem.Hubs;
using TicketingSystem.Models;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketDbContext _context;
        private readonly IHubContext<TicketHub> _hubContext;

        public TicketsController(TicketDbContext context, IHubContext<TicketHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.Include(t => t.Comments).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("TicketCreated", ticket);
            return CreatedAtAction(nameof(GetTickets), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id) return BadRequest();

            var existingTicket = await _context.Tickets.FindAsync(id);
            if (existingTicket == null) return NotFound();

            existingTicket.Status = ticket.Status;
            existingTicket.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("TicketUpdated", existingTicket);
            return NoContent();
        }
    }
}
