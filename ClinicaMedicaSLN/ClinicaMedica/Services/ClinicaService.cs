using ClinicaMedica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Services
{
    public class ClinicaService
    {
        // Declaração das Listas Genéricas
        public List<Paciente> Pacientes { get; private set; } = new List<Paciente>();
        public List<Medico> Medicos { get; private set; } = new List<Medico>();
        public List<Consulta> Consultas { get; private set; } = new List<Consulta>();

        public void CadastrarPaciente(Paciente paciente) => Pacientes.Add(paciente);

        public void CadastrarMedico(Medico medico) => Medicos.Add(medico);

        public void AgendarConsulta(int codigo, DateTime dataHora, int codigoPaciente, int codigoMedico)
        {
            // Busca dos objetos na lista usando LINQ
            var paciente = Pacientes.FirstOrDefault(p => p.Codigo == codigoPaciente);
            var medico = Medicos.FirstOrDefault(m => m.Codigo == codigoMedico);

            if (paciente == null)
            {
                Console.WriteLine("❌ Erro: Paciente não encontrado!");
                return;
            }

            if (medico == null)
            {
                Console.WriteLine("❌ Erro: Médico não encontrado!");
                return;
            }

            var novaConsulta = new Consulta(codigo, dataHora, paciente, medico);
            Consultas.Add(novaConsulta);

            Console.WriteLine("✅ Consulta agendada com sucesso!");

            // Disparando notificação via Interface
            novaConsulta.EnviarNotificacao($"sua consulta com Dr(a). {medico.Nome} foi confirmada.");
        }

        public void ExibirRelatorioGeral()
        {
            Console.WriteLine("\n=================== RELATÓRIO DE CONSULTAS ===================");
            if (Consultas.Count == 0)
            {
                Console.WriteLine("Nenhuma consulta registrada.");
                return;
            }

            foreach (var consulta in Consultas)
            {
                Console.WriteLine(consulta.GerarResumo());
                Console.WriteLine("--------------------------------------------------------------");
            }
        }
    }
}
