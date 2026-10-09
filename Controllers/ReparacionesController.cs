using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_reparacion_telefonos.Data;
using sistema_reparacion_telefonos.Models;

namespace sistema_reparacion_telefonos.Controllers
{
public class ReparacionesController : Controller
{
private readonly ApplicationDbContext _context;


    public ReparacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Reparaciones
    public async Task<IActionResult> Index()
    {
        var reparaciones = await _context.Reparaciones
            .Include(r => r.Dispositivo)
            .ThenInclude(d => d!.Cliente)
            .ToListAsync();

        return View(reparaciones);
    }

    // GET: Reparaciones/Create
    public async Task<IActionResult> Create()
    {
        await CargarDispositivos();

        return View();
    }

    // POST: Reparaciones/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Reparacion reparacion)
    {
        if (ModelState.IsValid)
        {
            _context.Reparaciones.Add(reparacion);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CargarDispositivos();

        return View(reparacion);
    }

    // GET: Reparaciones/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reparacion = await _context.Reparaciones.FindAsync(id);

        if (reparacion == null)
        {
            return NotFound();
        }

        await CargarDispositivos();

        return View(reparacion);
    }

    // POST: Reparaciones/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Reparacion reparacion)
    {
        if (id != reparacion.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(reparacion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CargarDispositivos();

        return View(reparacion);
    }

    // GET: Reparaciones/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reparacion = await _context.Reparaciones
            .Include(r => r.Dispositivo)
            .ThenInclude(d => d!.Cliente)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reparacion == null)
        {
            return NotFound();
        }

        return View(reparacion);
    }

    // Cargar dispositivos para los formularios
    private async Task CargarDispositivos()
    {
        ViewBag.Dispositivos = await _context.Dispositivos
            .Include(d => d.Cliente)
            .ToListAsync();
    }
}

}
