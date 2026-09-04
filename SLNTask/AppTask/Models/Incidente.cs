using System;
using System.Collections.Generic;

namespace AppTask.Models;

public partial class Incidente
{
    public int Codigo { get; set; }

    public string DescricaoProblema { get; set; } = null!;

    public DateTime DataIncidente { get; set; }

    public string? Solucao { get; set; }

    public bool Resolvido { get; set; }

    public int FuncionarioId { get; set; }

    public virtual Funcionario Funcionario { get; set; } = null!;
}
