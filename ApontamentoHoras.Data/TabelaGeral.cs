using System;
using System.Collections.Generic;

namespace ApontamentoHoras.Data;

public partial class TabelaGeral
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string UsuarioInclusao { get; set; } = null!;

    public DateTime? DataAlteracao { get; set; }

    public string? UsuarioAlteracao { get; set; }

    public virtual ICollection<TabelaGeralItem> TabelaGeralItems { get; set; } = new List<TabelaGeralItem>();
}
