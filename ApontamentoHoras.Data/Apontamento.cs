using System;
using System.Collections.Generic;

namespace ApontamentoHoras.Data;

public partial class Apontamento
{
    public Guid Id { get; set; }

    public string Descricao { get; set; } = null!;

    public DateOnly DataApontamento { get; set; }

    public TimeOnly? HorarioInicio { get; set; }

    public TimeOnly? HorarioFim { get; set; }

    public string UsuarioId { get; set; } = null!;

    public Guid ProjetoId { get; set; }

    public Guid IdTgservico { get; set; }

    public DateTime DataInclusao { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public virtual TabelaGeralItem IdTgservicoNavigation { get; set; } = null!;

    public virtual Projeto Projeto { get; set; } = null!;

    public virtual AspNetUser Usuario { get; set; } = null!;
}
