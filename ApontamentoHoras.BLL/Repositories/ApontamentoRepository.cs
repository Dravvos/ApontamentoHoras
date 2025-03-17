using ApontamentoHoras.Data;
using ApontamentoHoras.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.BLL.Repositories
{
    public class ApontamentoRepository : IApontamentoRepository
    {

        private readonly ApontamentoHorasContext con;

        public ApontamentoRepository(ApontamentoHorasContext con)
        {
            this.con = con;
        }

        public async Task AlterarApontamento(ApontamentoDTO dto)
        {
            var apontamento = await con.Apontamentos.FirstAsync(x => x.Id == dto.Id);
            apontamento.Descricao = dto.Descricao;
            apontamento.DataApontamento = dto.DataApontamento;
            apontamento.DataAlteracao= dto.DataAlteracao;
            apontamento.HorarioFim = dto.HorarioFim;
            apontamento.HorarioInicio = dto.HorarioInicio;
            apontamento.IdTgservico = dto.IdTgservico;
            apontamento.ProjetoId = dto.ProjetoId;
            apontamento.UsuarioId = dto.UsuarioId.ToString();
            
            await con.SaveChangesAsync();
        }

        public async Task DeletarApontamento(Guid id)
        {
            var apontamento = await con.Apontamentos.FirstAsync(x => x.Id == id);
            con.Apontamentos.Remove(apontamento);
            await con.SaveChangesAsync();
        }

        public async Task<ApontamentoDTO> ObterApontamento(Guid apontamentoId)
        {
            var apontamento = await con.Apontamentos.FirstAsync(x => x.Id == apontamentoId);
            var dto = Map<ApontamentoDTO>.Convert(apontamento);
            return dto;
        }

        public async Task<List<ApontamentoDTO>> ObterApontamentosPorData(DateOnly data)
        {
            var apontamentos = await con.Apontamentos.Where(x => x.DataApontamento == data).ToListAsync();
            var dto = Map<List<ApontamentoDTO>>.Convert(apontamentos);
            return dto;
        }

        public async Task<List<ApontamentoDTO>> ObterApontamentosPorUsuario(Guid usuarioId)
        {
            var apontamentos = await con.Apontamentos.Where(x => x.UsuarioId == usuarioId.ToString()).ToListAsync();
            return Map<List<ApontamentoDTO>>.Convert(apontamentos);
        }

        public async Task<List<ApontamentoDTO>> ObterApontamentosPorUsuarioEPorData(Guid usuarioId, DateOnly data)
        {
            var apontamentos = await con.Apontamentos.Where(x => x.DataApontamento == data && x.UsuarioId == usuarioId.ToString()).ToListAsync();
            return Map<List<ApontamentoDTO>>.Convert(apontamentos);
        }

        public async Task RegistrarApontamento(ApontamentoDTO dto)
        {
            var model = Map<Apontamento>.Convert(dto);
            await con.Apontamentos.AddAsync(model);
            await con.SaveChangesAsync();
        }
    }
}
