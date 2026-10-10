using Microsoft.AspNetCore.Identity;

namespace sistema_reparacion_telefonos.Services
{
    public static class IdentityInitializer
    {
        public static async Task InicializarAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Tecnico", "Cliente" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var resultado = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!resultado.Succeeded)
                    {
                        var errores = string.Join(
                            "; ",
                            resultado.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"No se pudo crear el rol {role}: {errores}");
                    }
                }
            }
        }
    }
}