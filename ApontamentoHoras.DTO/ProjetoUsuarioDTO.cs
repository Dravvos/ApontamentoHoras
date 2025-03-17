using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.DTO
{
    public class ProjetoUsuarioDTO:BaseDTO
    {
        public Guid UsuarioId { get;set; }
        public Guid ProjetoId { get; set; }
    }
}
