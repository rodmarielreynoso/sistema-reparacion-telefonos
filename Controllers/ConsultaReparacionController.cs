using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_reparacion_telefonos.Data;

namespace sistema_reparacion_telefonos.Controllers
{
    public class ConsultaReparacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultaReparacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Mostrar el formulario de consulta
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Buscar una reparación mediante su código
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            string codigoSeguimiento,
            string telefono)
        {
            if (string.IsNullOrWhiteSpace(codigoSeguimiento) ||
                string.IsNullOrWhiteSpace(telefono))
            {
                ViewBag.Mensaje =
                    "Debes introducir el código de seguimiento y el teléfono registrado.";

                return View();
            }

            codigoSeguimiento = codigoSeguimiento.Trim();
            telefono = telefono.Trim();

            var reparacion = await _context.Reparaciones
                .AsNoTracking()
                .Include(r => r.Dispositivo)
                .ThenInclude(d => d!.Cliente)
                .FirstOrDefaultAsync(r =>
                    r.CodigoSeguimiento == codigoSeguimiento &&
                    r.Dispositivo != null &&
                    r.Dispositivo.Cliente != null &&
                    r.Dispositivo.Cliente.Telefono == telefono);

            if (reparacion == null)
            {
                ViewBag.Mensaje =
                    "No se encontró una reparación con esos datos. Verifica el código y el teléfono.";

                return View();
            }

            return View("Resultado", reparacion);
        }
    }
}