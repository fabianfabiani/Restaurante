using Entidades;

namespace LaboratorioApi.Services
{
    public interface IEncuestaService
    {
        Task<IEnumerable<Encuesta>> GetEncuestasAsync();
        Task<Encuesta> GetEncuestaByIdAsync(int id);
        Task<Encuesta> CreateEncuestaAsync(Encuesta nuevaEncuesta);
        Task<bool> DeleteEncuestaAsync(int id);
        Task<List<string>> ValidarCampos(Encuesta Encuesta);
    }
}
