using System;
using System.Collections.Generic;

namespace AppTaskZero.Models;

public partial class Incidente
{
    public int Codigo { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public string Prioridade { get; set; } = null!;

    public string StatusIncidente { get; set; } = null!;

    public DateTime DataAbertura { get; set; }

    public DateTime? DataResolucao { get; set; }

    public int FuncionarioId { get; set; }

    public int? TarefaId { get; set; }

    public virtual Funcionario Funcionario { get; set; } = null!;

    public virtual Tarefa? Tarefa { get; set; }
}
