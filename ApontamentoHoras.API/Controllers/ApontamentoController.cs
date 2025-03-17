using ApontamentoHoras.BLL.Services;
using ApontamentoHoras.Data;
using ApontamentoHoras.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApontamentoHoras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApontamentoController : ControllerBase
    {
        private readonly IApontamentoService _service;

        public ApontamentoController(IApontamentoService service)
        {
            _service = service;
        }

        [HttpGet("{data}")]
        public async Task<IActionResult> BuscarApontamentos(DateOnly data)
        {
            try
            {
                var apontamentos = await _service.ObterApontamentosPorData(data);
                if (apontamentos == null || apontamentos.Any() == false)
                    return NotFound();
                return Ok(apontamentos);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);

                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("[action]/{usuarioId}")]
        public async Task<IActionResult> BuscarApontamentosPorUsuario(Guid usuarioId)
        {
            try
            {
                var apontamentos = await _service.ObterApontamentosPorUsuario(usuarioId);
                if (apontamentos == null || apontamentos.Any() == false)
                    return NotFound();
                return Ok(apontamentos);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);

                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("[action]/{usuarioId}")]
        public async Task<IActionResult> BuscarApontamentosPorUsuarioEUsuario(DateOnly data, Guid usuarioId)
        {
            try
            {
                var apontamentos = await _service.ObterApontamentosPorUsuarioEPorData(usuarioId, data);
                if (apontamentos == null || apontamentos.Any() == false)
                    return NotFound();
                return Ok(apontamentos);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);

                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarApontamento([FromBody] ApontamentoDTO dto)
        {
            try
            {
                await _service.RegistrarApontamento(dto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);

                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarApontamento(Guid apontamentoId, [FromBody] ApontamentoDTO dto)
        {
            try
            {
                if (apontamentoId == Guid.Empty)
                    return BadRequest("Preencha o Id da requisição");
                if (apontamentoId != dto.Id)
                    return BadRequest("Id da requisição é diferente do Id do Body");

                await _service.AlterarApontamento(dto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);

                return StatusCode(500, ex.InnerException.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarApontamento(Guid id)
        {
            try
            {
                if (await _service.ObterApontamento(id) == null)
                    return NotFound();

                await _service.DeletarApontamento(id);

                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);

                return StatusCode(500, ex.InnerException.Message);
            }
        }
    }
}
