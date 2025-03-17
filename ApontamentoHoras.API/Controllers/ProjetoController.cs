using ApontamentoHoras.BLL.Services;
using ApontamentoHoras.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApontamentoHoras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoService _service;

        public ProjetoController(IProjetoService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterProjeto(Guid id)
        {
            try
            {
                var projeto = await _service.ObterProjeto(id);
                if(projeto == null)
                    return NotFound();

                return Ok(projeto);
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
        public async Task<IActionResult> ObterProjetosPorUsuario(Guid usuarioId)
        {
            try
            {
                var projetos = await _service.ObterProjetosPorUsuario(usuarioId);
                if (projetos == null || projetos.Any()==false)
                    return NotFound();

                return Ok(projetos);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);

                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProjeto([FromBody] ProjetoDTO dto)
        {
            try
            {
                await _service.CriarProjeto(dto);
                return Ok(dto.Id);
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

        [HttpPut("{projetoId}")]
        public async Task<IActionResult> AtualizarProjeto(Guid projetoId, [FromBody] ProjetoDTO dto)
        {
            try
            {
                if (projetoId == Guid.Empty)
                    return BadRequest("Preencha o Id da requisição");
                if (projetoId != dto.Id)
                    return BadRequest("Id da requisição é diferente do Id do Body");

                await _service.AlterarProjeto(dto);
                return Ok(dto.Id);
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
        public async Task<IActionResult> DeletarProjeto(Guid id)
        {
            try
            {
                if (await _service.ObterProjeto(id) == null)
                    return NotFound();
                
                await _service.DeletarProjeto(id);

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
