using System;
using System.Collections.Generic;

namespace AppClinicaOdontoSLN.Models;

public partial class Paciente
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
