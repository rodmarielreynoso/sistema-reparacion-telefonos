using Microsoft.AspNetCore.Identity;

namespace sistema_reparacion_telefonos.Services
{
    public static class TecnicoInitializer
    {
        public static async Task CrearTecnicoAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            var correo = configuration["TecnicoInicial:Correo"];
            var contrasena = configuration["TecnicoInicial:Contrasena"];

            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(contrasena))
            {
                return;
            }

            var usuario = await userManager.FindByEmailAsync(correo);

            if (usuario == null)
            {
                usuario = new IdentityUser
                {
                    UserName = correo,
                    Email = correo,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(
                    usuario,
                    contrasena);

                if (!resultado.Succeeded)
                {
                    var errores = string.Join(
                        "; ",
                        resultado.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"No se pudo crear el técnico: {errores}");
                }
            }

            if (!await userManager.IsInRoleAsync(usuario, "Tecnico"))
            {
                var resultadoRol = await userManager.AddToRoleAsync(
                    usuario,
                    "Tecnico");

                if (!resultadoRol.Succeeded)
                {
                    var errores = string.Join(
                        "; ",
                        resultadoRol.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"No se pudo asignar el rol Tecnico: {errores}");
                }
            }
        }
    }
}