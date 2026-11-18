using System;
using System.Collections.Generic;

namespace AppClinicaOdontoSLN.Models;

public partial class Medico
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cro { get; set; } = null!;

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
