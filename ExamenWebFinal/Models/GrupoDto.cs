namespace ExamenWebFinal.Models
{
    public class GrupoDto
    {
        public int IdGrupo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaFormacion { get; set; }
        public DateTime? FechaDisgregacion { get; set; } // Nullable por si el grupo aún está activo

    }
}
