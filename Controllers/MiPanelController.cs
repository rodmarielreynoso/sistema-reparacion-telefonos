using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_reparacion_telefonos.Data;
using sistema_reparacion_telefonos.Models;

namespace sistema_reparacion_telefonos.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class MiPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MiPanelController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdentityUserId == userId);

            if (cliente == null)
            {
                return View(new MiPanelViewModel
                {
                    ClienteVinculado = false
                });
            }

            var dispositivos = await _context.Dispositivos
                .AsNoTracking()
                .Where(d => d.ClienteId == cliente.Id)
                .OrderBy(d => d.Marca)
                .ThenBy(d => d.Modelo)
                .ToListAsync();

            var reparaciones = await _context.Reparaciones
                .AsNoTracking()
                .Include(r => r.Dispositivo)
                .Where(r => r.Dispositivo != null &&
                    r.Dispositivo.ClienteId == cliente.Id)
                .OrderByDescending(r => r.FechaIngreso)
                .ToListAsync();

            return View(new MiPanelViewModel
            {
                ClienteVinculado = true,
                Cliente = cliente,
                Dispositivos = dispositivos,
                Reparaciones = reparaciones
            });
        }
    }
}
