namespace sistema_reparacion_telefonos.Models
{
    public class MiPanelViewModel
    {
        public bool ClienteVinculado { get; set; }

        public Cliente? Cliente { get; set; }

        public List<Dispositivo> Dispositivos { get; set; } = new();

        public List<Reparacion> Reparaciones { get; set; } = new();
    }
}
