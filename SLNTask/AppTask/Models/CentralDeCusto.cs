using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppTask.Models;

public partial class CentralDeCusto
{
    public int Codigo { get; set; }

    public string NomeCentral { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorMetaAnual { get; set; }
}
