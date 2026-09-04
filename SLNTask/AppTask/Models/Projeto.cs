using System;
using System.Collections.Generic;

namespace AppTask.Models;

public partial class Projeto
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public DateTime? DataInicio { get; set; }

    public DateTime? DataFim { get; set; }
}
