using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_reparacion_telefonos.Data;
using sistema_reparacion_telefonos.Models;

namespace sistema_reparacion_telefonos.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Página principal con estadísticas
    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Tecnico"))
        {
            return View();
        }

        ViewBag.TotalClientes = await _context.Clientes.CountAsync();

        ViewBag.TotalDispositivos = await _context.Dispositivos.CountAsync();

        ViewBag.TotalReparaciones = await _context.Reparaciones.CountAsync();

        ViewBag.ReparacionesPendientes = await _context.Reparaciones
            .CountAsync(r => r.Estado != "Entregado");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
        });
    }
}