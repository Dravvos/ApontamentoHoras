using ApontamentoHoras.BLL.Repositories;
using ApontamentoHoras.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.BLL.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly IProjetoRepository _repository;

        public ProjetoService(IProjetoRepository repository)
        {
            _repository = repository;
        }

        public async Task AlterarProjeto(ProjetoDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Nome do projeto não pode estar vazio");
            if (string.IsNullOrEmpty(dto.UsuarioAlteracao))
                throw new ArgumentNullException("Usuário de alteração não pode estar vazio");

            dto.DataAlteracao = DateTime.Now;
            await _repository.AlterarProjeto(dto);
        }

        public async Task CriarProjeto(ProjetoDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Nome do projeto não pode estar vazio");
            if (string.IsNullOrEmpty(dto.UsuarioInclusao))
                throw new ArgumentNullException("Usuário de inclusão não pode estar vazio");

            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.Now;
            await _repository.CriarProjeto(dto);
        }

        public async Task DeletarProjeto(Guid id)
        {
            await _repository.DeletarProjeto(id);
        }

        public async Task<List<ProjetoDTO>?> ListarProjetos()
        {
            return await _repository.ListarProjetos();
        }

        public async Task<ProjetoDTO?> ObterProjeto(Guid id)
        {
            return await _repository.ObterProjeto(id);
        }

        public async Task<List<ProjetoDTO>?> ObterProjetosPorUsuario(Guid usuarioId)
        {
            return await _repository.ObterProjetosPorUsuario(usuarioId);
        }
    }
}
