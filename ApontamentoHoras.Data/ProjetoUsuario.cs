using System;
using System.Collections.Generic;

namespace ApontamentoHoras.Data;

public partial class ProjetoUsuario
{
    public Guid Id { get; set; }

    public Guid ProjetoId { get; set; }

    public string UsuarioId { get; set; } = null!;

    public DateTime DataInclusao { get; set; }

    public string UsuarioInclusao { get; set; } = null!;

    public DateTime? DataAlteracao { get; set; }

    public string? UsuarioAlteracao { get; set; }

    public virtual Projeto Projeto { get; set; } = null!;

    public virtual AspNetUser Usuario { get; set; } = null!;
}
