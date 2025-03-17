using ApontamentoHoras.BLL.Repositories;
using ApontamentoHoras.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.BLL.Services
{
    public class ApontamentoService:IApontamentoService
    {
        private readonly IApontamentoRepository _repository;

        public ApontamentoService(IApontamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task AlterarApontamento(ApontamentoDTO dto)
        {
            ValidarApontamentoDTO(dto);
            dto.DataAlteracao = DateTime.Now;

            await _repository.RegistrarApontamento(dto);
        }

        public async Task DeletarApontamento(Guid id)
        {
            await _repository.DeletarApontamento(id);
        }

        public async Task<ApontamentoDTO> ObterApontamento(Guid apontamentoId)
        {
            return await _repository.ObterApontamento(apontamentoId);
        }

        public async Task<List<ApontamentoDTO>> ObterApontamentosPorData(DateOnly data)
        {
            return await _repository.ObterApontamentosPorData(data);
        }

        public async Task<List<ApontamentoDTO>> ObterApontamentosPorUsuario(Guid usuarioId)
        {
            return await _repository.ObterApontamentosPorUsuario(usuarioId);
        }

        public async Task<List<ApontamentoDTO>> ObterApontamentosPorUsuarioEPorData(Guid usuarioId, DateOnly data)
        {
            return await _repository.ObterApontamentosPorUsuarioEPorData(usuarioId, data);
        }

        public async Task RegistrarApontamento(ApontamentoDTO dto)
        {
            ValidarApontamentoDTO(dto);
            dto.DataInclusao = DateTime.Now;
            dto.Id = Guid.NewGuid();

            await _repository.RegistrarApontamento(dto);
        }

        private static void ValidarApontamentoDTO(ApontamentoDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Descricao))
                throw new ArgumentNullException("Descrição do apontamento não pode estar vazia");
            if (dto.IdTgservico == Guid.Empty)
                throw new ArgumentNullException("Selecione o tipo de serviço realizado");
            if (dto.DataApontamento > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Não é possível apontar algo que você ainda não fez");
            if (dto.HorarioInicio > dto.HorarioFim)
                throw new ArgumentException("O horário de início não pode ser maior que o horário de fim");
            if (dto.ProjetoId == Guid.Empty)
                throw new ArgumentNullException("Selecione o projeto em que trabalhou");
            if (dto.UsuarioId == Guid.Empty)
                throw new ArgumentNullException("Preencha o usuário que fez o apontamento");
        }
    }
}
