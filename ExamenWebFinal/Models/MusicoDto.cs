namespace ExamenWebFinal.Models
{
    public class MusicoDto
    {
        public int IdMusico { get; set; }
        public string Nombre { get; set; }
        public string Instrumento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
