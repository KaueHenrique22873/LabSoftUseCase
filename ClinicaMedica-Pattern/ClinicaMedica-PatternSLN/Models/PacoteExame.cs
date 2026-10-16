using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica_PatternSLN.Models
{
    public class PacoteExame
    {
        public string NomePacote { get; set; }
        public string OrientacoesJejum { get; set; }
        public string NomePaciente { get; set; }
        public List<Procedimento> Procedimentos { get; set; }

        public PacoteExame(string nomePacote, string orientacoesJejum)
        {
            NomePacote = nomePacote;
            OrientacoesJejum = orientacoesJejum;
            NomePaciente = "NÃO ATRIBUÍDO";
            Procedimentos = new List<Procedimento>();
        }

        public void AdicionarProcedimento(Procedimento procedimento)
        {
            Procedimentos.Add(procedimento);
        }

        // 1. CÓPIA PROFUNDA (DEEP COPY) - O Jeito Correto!
        // Instancia um novo pacote E uma nova lista com novos procedimentos clonados
        public PacoteExame Clonar()
        {
            // Criamos um novo pacote na memória usando 'new'
            PacoteExame pacoteClonado = new PacoteExame(this.NomePacote, this.OrientacoesJejum);
            pacoteClonado.NomePaciente = this.NomePaciente;

            // Percorremos a lista original e clonamos item por item para a nova lista
            foreach (Procedimento item in this.Procedimentos)
            {
                Procedimento itemCopia = item.Clonar(); // Instancia um novo procedimento
                pacoteClonado.Procedimentos.Add(itemCopia);
            }

            return pacoteClonado;
        }

        // 2. CÓPIA SUPERFICIAL (SHALLOW COPY) - Apenas para entender o Erro
        // Copia apenas o pacote, mas aponta para a MESMA lista de procedimentos na memória
        public PacoteExame ClonarSuperficial()
        {
            PacoteExame pacoteRaso = new PacoteExame(this.NomePacote, this.OrientacoesJejum);
            pacoteRaso.NomePaciente = this.NomePaciente;

            // PERIGO: Não criamos uma nova lista, reutilizamos a mesma referência!
            pacoteRaso.Procedimentos = this.Procedimentos;

            return pacoteRaso;
        }

        public void ExibirResumo()
        {
            Console.WriteLine($"\n[PACOTE]: {NomePacote} | Paciente: {NomePaciente}");
            Console.WriteLine($"  Orientação: {OrientacoesJejum}");
            Console.WriteLine("  Procedimentos inclusos:");
            decimal total = 0;
            foreach (var p in Procedimentos)
            {
                Console.WriteLine($"   - {p.Nome}: R$ {p.Valor:F2}");
                total += p.Valor;
            }
            Console.WriteLine($"  Total: R$ {total:F2}");
        }
    }
}

