using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApontamentoHoras.DTO
{
    public class ProjetoDTO:BaseDTO
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? UsuarioInclusao { get; set; }
        public string? UsuarioAlteracao { get; set; }
    }
}
