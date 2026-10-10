using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sistema_reparacion_telefonos.Data;
using sistema_reparacion_telefonos.Models;
using System.ComponentModel.DataAnnotations;

namespace sistema_reparacion_telefonos.Controllers
{
    public class RegistroController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RegistroController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegistroViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var correo = model.Correo.Trim();

            var usuarioExistente =
                await _userManager.FindByEmailAsync(correo);

            if (usuarioExistente != null)
            {
                ModelState.AddModelError(
                    nameof(model.Correo),
                    "Este correo ya está registrado.");

                return View(model);
            }

            // Crear la cuenta de Identity
            var usuario = new IdentityUser
            {
                UserName = correo,
                Email = correo,
                EmailConfirmed = false
            };

            var resultado = await _userManager.CreateAsync(
                usuario,
                model.Contrasena);

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            // Asignar el rol Cliente automáticamente
            var resultadoRol = await _userManager.AddToRoleAsync(
                usuario,
                "Cliente");

            if (!resultadoRol.Succeeded)
            {
                await _userManager.DeleteAsync(usuario);

                ModelState.AddModelError(
                    string.Empty,
                    "No se pudo completar el registro. Inténtalo nuevamente.");

                return View(model);
            }

            // Crear el registro del cliente
            var cliente = new Cliente
            {
                Nombre = model.Nombre.Trim(),
                Correo = correo,
                Telefono = model.Telefono.Trim(),
                Direccion = model.Direccion.Trim(),
                IdentityUserId = usuario.Id
            };

            try
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }
            catch
            {
                await _userManager.DeleteAsync(usuario);

                ModelState.AddModelError(
                    string.Empty,
                    "No se pudo guardar el cliente. Verifica los datos e inténtalo nuevamente.");

                return View(model);
            }

            TempData["MensajeRegistro"] =
                "Tu cuenta fue creada correctamente. Ya puedes iniciar sesión.";

            return RedirectToAction("Login", "Account");
        }
    }

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Introduce un correo válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(
            100,
            MinimumLength = 8,
            ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare(
            nameof(Contrasena),
            ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasena { get; set; } = string.Empty;
    }
}