using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Dto;
using Restaurante.Entities;
using System.Collections.Generic;


namespace Restaurante.Interface
{
    public interface IMesaService
    {
        public Task<ActionResult<List<MesaListarDTO>>> GeTAll();
        public Task CrearMesa([FromBody] MesaRequestDto mesa);

        // Método para obtener la mesa más usada (más encuestas asociadas)
        public Task<Mesa> ObtenerMesaMasUsadaAsync();

        // Método para obtener la mesa menos usada
        public Task<Mesa> ObtenerMesaMenosUsadaAsync();

        Task<object> ObtenerMejorComentarioAsync();
        Task<object> ObtenerPeorComentarioAsync();




    }



}
