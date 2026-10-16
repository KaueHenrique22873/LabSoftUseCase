using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica_PatternSLN.Models
{
    public class Procedimento
    {
        public string Nome { get; set; }
        public decimal Valor { get; set; }

        public Procedimento(string nome, decimal valor)
        {
            Nome = nome;
            Valor = valor;
        }

        public Procedimento Clonar()
        {
            Procedimento novoProcedimento = new Procedimento(this.Nome , this.Valor);
            return novoProcedimento;
        }
    }
}
