namespace Restaurante.Dto
{
    public class UsuarioRequestUpdateDto
    {
        public int IdSector { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
    }
}
