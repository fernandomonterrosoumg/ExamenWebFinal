using ExamenWebFinal.Handler;
using ExamenWebFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamenWebFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenController : ControllerBase
    {

        private readonly ExamenHandler _ExamenHandler;
        public ExamenController(ExamenHandler examenHandler)
        {
            _ExamenHandler = examenHandler;
        }

        [HttpPost("postMusicoAGrupo")]
        public async Task<ActionResult<RespuestaHttp>> postMusicoAGrupo(int idGrupo, MusicoDto musicoDto)
        {
            return await _ExamenHandler.AgregarMusicoAGrupo(idGrupo,musicoDto);
        }

        [HttpGet("GetGruposConMusicos")]
        public async Task<ActionResult<RespuestaHttp>> GetGruposConMusicos()
        {
            return await _ExamenHandler.GetGruposConMusicos();
        }

        [HttpGet("GetGrupoMasGeneros")]
        public async Task<ActionResult<RespuestaHttp>> GetGrupoMasGeneros()
        {
            return await _ExamenHandler.GetGrupoMasGeneros();
        }

        [HttpGet("GetMusicosPorGenero/{idGenero}")]
        public async Task<ActionResult<RespuestaHttp>> GetMusicosPorGenero(int idGenero)
        {
            return await _ExamenHandler.GetMusicosPorGenero(idGenero);
        }
    }
}
