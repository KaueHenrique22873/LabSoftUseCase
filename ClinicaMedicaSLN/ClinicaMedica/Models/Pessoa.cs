using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica
{
    public abstract class Pessoa
    {
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

        protected Pessoa(int codigo, string nome, string cpf, string telefone)
        {
            Codigo = codigo;
            Nome = nome;
            Cpf = cpf;
            Telefone = telefone;
        }

        public abstract void ExibirFicha();
    }
}
