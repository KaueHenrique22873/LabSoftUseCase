using System;
using System.Collections.Generic;

namespace PetShop.Models;

public partial class Agendamento
{
    public int Codigo { get; set; }

    public string MotivoAgendamento { get; set; } = null!;

    public DateTime DataHora { get; set; }

    public int CodigoPet { get; set; }

    public virtual Pet CodigoPetNavigation { get; set; } = null!;
}
