using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.DTO
{
    public class ApontamentoDTO : BaseDTO
    {
        public DateOnly DataApontamento { get; set; }
        public string? Descricao { get; set; }
        public Guid ProjetoId { get; set; }
        public Guid IdTgservico { get; set; }
        public Guid UsuarioId { get; set; }
        public TimeOnly HorarioInicio { get; set; }
        public TimeOnly HorarioFim { get; set; }
    }
}
