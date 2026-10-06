namespace sistema_reparacion_telefonos.Models
{
    public class Reparacion
    {
        public int Id { get; set; }

        public int DispositivoId { get; set; }

        public string Problema { get; set; } = string.Empty;

        public string Diagnostico { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public string Estado { get; set; } = "Recibido";

        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        public DateTime? FechaEntrega { get; set; }

        public Dispositivo? Dispositivo { get; set; }
    }
}