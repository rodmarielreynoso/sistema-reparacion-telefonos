using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_reparacion_telefonos.Data;
using sistema_reparacion_telefonos.Models;

namespace sistema_reparacion_telefonos.Controllers
{
    [Authorize(Roles = "Tecnico")]
    public class DispositivosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DispositivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dispositivos
        public async Task<IActionResult> Index()
        {
            var dispositivos = await _context.Dispositivos
                .Include(d => d.Cliente)
                .ToListAsync();

            return View(dispositivos);
        }

        // GET: Dispositivos/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Clientes = await _context.Clientes.ToListAsync();

            return View();
        }

        // POST: Dispositivos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispositivo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = await _context.Clientes.ToListAsync();

            return View(dispositivo);
        }

        // GET: Dispositivos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivos.FindAsync(id);

            if (dispositivo == null)
            {
                return NotFound();
            }

            ViewBag.Clientes = await _context.Clientes.ToListAsync();

            return View(dispositivo);
        }

        // POST: Dispositivos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dispositivo dispositivo)
        {
            if (id != dispositivo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(dispositivo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = await _context.Clientes.ToListAsync();

            return View(dispositivo);
        }

        // GET: Dispositivos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivos
                .Include(d => d.Cliente)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }
    }
}