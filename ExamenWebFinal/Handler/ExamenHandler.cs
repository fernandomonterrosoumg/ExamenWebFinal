using Dapper;
using ExamenWebFinal.Helpers;
using ExamenWebFinal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExamenWebFinal.Handler
{
    public class ExamenHandler : ControllerBase
    {
        private readonly OracleDbManager _OracleDbManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExamenHandler(OracleDbManager oracleDbManager, IHttpContextAccessor httpContextAccessor)
        {
            _OracleDbManager = oracleDbManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #region inserts
        public async Task<ActionResult<RespuestaHttp>> AgregarMusicoAGrupo(int idGrupo, MusicoDto musicoDto)
        {
            IDbTransaction transaccion = _OracleDbManager.BeginTransaction(_OracleDbManager.GetConnection());
            try
            {
                // Verificar si el músico ya pertenece al grupo
                var parametrosVerificacion = new Dictionary<string, object>
                {
                     {"GrupoId" ,idGrupo },
                    {"MusicoId" , musicoDto.IdMusico }
                };

                string sqlVerificar = "SELECT COUNT(*) FROM musicosgrupos WHERE idgrupo = :GrupoId AND idmusico = :MusicoId";
                int existe = (await _OracleDbManager.DapperExecuteQuery<int>(sqlVerificar, parametrosVerificacion, transaccion)).FirstOrDefault();

                if (existe > 0)
                {
                    return Ok(RespuestaHttp.BuildResponse(false, "El músico ya pertenece al grupo."));
                }

                var parametrosMusicoGrupo = new Dictionary<string, object>
                {
                     {"GrupoId" ,idGrupo},
                    {"MusicoId" , musicoDto.IdMusico },
                    {"Instrumento" , musicoDto.Instrumento },
                    {"FechaInicio" , musicoDto.FechaInicio },
                };

                string sqlInsertar = "INSERT INTO musicosgrupos (idgrupo, idmusico, instrumento, fechainicio) VALUES (:GrupoId, :MusicoId, :Instrumento, :FechaInicio)";
                await _OracleDbManager.DapperExecuteQuery<int>(sqlInsertar, parametrosMusicoGrupo, transaccion);

                _OracleDbManager.CommitTransaction(transaccion);

                return Ok(RespuestaHttp.BuildResponse(true, "Músico agregado al grupo exitosamente."));
            }
            catch (Exception ex)
            {
                _OracleDbManager.RollbackTransaction(transaccion);
                return BadRequest(RespuestaHttp.BuildResponse(false, "Error al agregar el músico al grupo: " + ex.Message));
            }
        }
        #endregion

        public async Task<ActionResult<RespuestaHttp>> GetMusicosPorGenero(int idGenero)
        {
            try
            {
                var sql = @"
            SELECT m.* FROM musico m
            INNER JOIN musicosgrupos mg ON m.idmusico = mg.idmusico
            INNER JOIN generosgrupos gg ON mg.idgrupo = gg.idgrupo
            WHERE gg.idgenero = :IdGenero";

                var musicos = await _OracleDbManager.DapperExecuteQuery<MusicoDto>(sql, new Dictionary<string, object> { { "idGenero", idGenero } });
                return Ok(RespuestaHttp.BuildResponse(true, "Listado de músicos obtenido.", musicos));
            }
            catch (Exception ex)
            {
                return BadRequest(RespuestaHttp.BuildResponse(false, "Error al obtener los músicos: " + ex.Message));
            }
        }

        public async Task<ActionResult<RespuestaHttp>> GetGrupoMasGeneros()
        {
            try
            {
                var sql = @"
            SELECT g.idgrupo, g.nombre, g.formacion, g.disgregacion, COUNT(gg.idgenero) AS cantidad_generos
FROM grupo g
LEFT JOIN generosgrupos gg ON g.idgrupo = gg.idgrupo
GROUP BY g.idgrupo, g.nombre, g.formacion, g.disgregacion
ORDER BY cantidad_generos DESC
FETCH FIRST 1 ROWS ONLY";

                var grupo = await _OracleDbManager.DapperExecuteQuery<GrupoDto>(sql);
                return Ok(RespuestaHttp.BuildResponse(true, "Grupo con más géneros musicales obtenido.", grupo.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                return BadRequest(RespuestaHttp.BuildResponse(false, "Error al obtener el grupo: " + ex.Message));
            }
        }

        public async Task<ActionResult<RespuestaHttp>> GetGruposConMusicos()
        {
            try
            {
                var sql = @"
            SELECT g.*, m.* FROM grupo g
            LEFT JOIN musicosgrupos mg ON g.idgrupo = mg.idgrupo
            LEFT JOIN musico m ON mg.idmusico = m.idmusico";

                var grupoConMusicos = await _OracleDbManager.DapperExecuteQuery<GrupoConMusicosDto>(sql);
                return Ok(RespuestaHttp.BuildResponse(true, "Listado de grupos con músicos obtenido.", grupoConMusicos));
            }
            catch (Exception ex)
            {
                return BadRequest(RespuestaHttp.BuildResponse(false, "Error al obtener los grupos: " + ex.Message));
            }
        }

    }
}
