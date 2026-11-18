using System;
using System.Collections.Generic;

namespace AppClinicaOdontoSLN.Models;

public partial class Usuario
{
    public int Codigo { get; set; }

    public string Username { get; set; } = null!;

    public string Senha { get; set; } = null!;
}
