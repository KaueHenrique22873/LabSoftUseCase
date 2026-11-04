using System;
using System.Collections.Generic;

namespace PetShop.Models;

public partial class Tutor
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string PerfilAcesso { get; set; } = null!;

    public DateTime DataCriacao { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
