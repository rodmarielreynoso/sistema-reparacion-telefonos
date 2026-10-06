namespace sistema_reparacion_telefonos.Models
{
    public class Dispositivo
    {
        public int Id { get; set; }

        public string Marca { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;

        public string NumeroSerie { get; set; } = string.Empty;

        public string IMEI { get; set; } = string.Empty;

        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }
    }
}