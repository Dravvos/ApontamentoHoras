using System;
using System.Collections.Generic;

namespace ApontamentoHoras.Data;

public partial class TabelaGeralItem
{
    public Guid Id { get; set; }

    public Guid TabelaGeralId { get; set; }

    public string Sigla { get; set; } = null!;

    public string? Descricao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string UsuarioInclusao { get; set; } = null!;

    public DateTime? DataAlteracao { get; set; }

    public string? UsuarioAlteracao { get; set; }

    public virtual ICollection<Apontamento> Apontamentos { get; set; } = new List<Apontamento>();

    public virtual TabelaGeral TabelaGeral { get; set; } = null!;
}
