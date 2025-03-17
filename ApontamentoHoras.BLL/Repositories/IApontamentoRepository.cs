using ApontamentoHoras.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.BLL.Repositories
{
    public interface IApontamentoRepository
    {
        Task RegistrarApontamento(ApontamentoDTO dto);
        Task AlterarApontamento(ApontamentoDTO apontamento);
        Task DeletarApontamento(Guid id);
        Task<List<ApontamentoDTO>> ObterApontamentosPorUsuario(Guid usuarioId);
        Task<List<ApontamentoDTO>> ObterApontamentosPorData(DateOnly data);
        Task<List<ApontamentoDTO>> ObterApontamentosPorUsuarioEPorData(Guid usuarioId, DateOnly data);
        Task<ApontamentoDTO> ObterApontamento(Guid apontamentoId);
    }
}
