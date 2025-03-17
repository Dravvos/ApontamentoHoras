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
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly ApontamentoHorasContext con;

        public ProjetoRepository(ApontamentoHorasContext con)
        {
            this.con = con;
        }

        public async Task AlterarProjeto(ProjetoDTO dto)
        {
            var projeto = await con.Projetos.FirstAsync(x => x.Id == dto.Id);
            projeto.Descricao = dto.Descricao;
            projeto.Nome = dto.Nome;
            projeto.DataAlteracao = dto.DataAlteracao;
            projeto.UsuarioAlteracao = dto.UsuarioAlteracao;
            await con.SaveChangesAsync();
        }

        public async Task CriarProjeto(ProjetoDTO dto)
        {
            var model = Map<Projeto>.Convert(dto);
            await con.Projetos.AddAsync(model);
            await con.SaveChangesAsync();
        }

        public async Task DeletarProjeto(Guid id)
        {
            var projeto = await con.Projetos.FirstAsync(x => x.Id == id);
            con.Projetos.Remove(projeto);
            await con.SaveChangesAsync();
        }

        public async Task<List<ProjetoDTO>?> ListarProjetos()
        {
            var projetos = await con.Projetos.ToListAsync();
            var dto = Map<List<ProjetoDTO>>.Convert(projetos);
            return dto;
        }

        public async Task<ProjetoDTO?> ObterProjeto(Guid id)
        {
            var projeto = await con.Projetos.FirstOrDefaultAsync(x => x.Id == id);
            if (projeto == null)
                return null;

            var dto = Map<ProjetoDTO>.Convert(projeto);
            return dto;
        }

        public async Task<List<ProjetoDTO>?> ObterProjetosPorUsuario(Guid usuarioId)
        {
            var projetos = await con.ProjetoUsuarios.Where(x => x.UsuarioId == usuarioId.ToString()).Select(x => x.Projeto).ToListAsync();
            var dto = Map<List<ProjetoDTO>>.Convert(projetos);
            return dto;
        }
    }
}
