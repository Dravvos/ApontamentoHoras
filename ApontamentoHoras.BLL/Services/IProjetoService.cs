using ApontamentoHoras.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.BLL.Services
{
    public interface IProjetoService
    {
        Task CriarProjeto(ProjetoDTO dto);
        Task AlterarProjeto(ProjetoDTO dto);
        Task DeletarProjeto(Guid id);
        Task<List<ProjetoDTO>?> ObterProjetosPorUsuario(Guid usuarioId);
        Task<ProjetoDTO?> ObterProjeto(Guid id);
        Task<List<ProjetoDTO>?> ListarProjetos();
    }
}
