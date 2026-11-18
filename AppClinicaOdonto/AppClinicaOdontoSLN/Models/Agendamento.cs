using System;
using System.Collections.Generic;

namespace AppClinicaOdontoSLN.Models;

public partial class Agendamento
{
    public int Codigo { get; set; }

    public DateTime DataHora { get; set; }

    public int CodigoProcedimento { get; set; }

    public int CodigoPaciente { get; set; }

    public int CodigoMedico { get; set; }

    public virtual Medico? CodigoMedicoNavigation { get; set; }

    public virtual Paciente? CodigoPacienteNavigation { get; set; }

    public virtual Procedimento? CodigoProcedimentoNavigation { get; set; }
}
