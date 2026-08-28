using System;
using System.Collections.Generic;

namespace AppTaskZero.Models;

public partial class Gerente
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Setor { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
