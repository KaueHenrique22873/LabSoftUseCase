using ClinicaMedica_PatternSLN.Models;
using ClinicaMedica_PatternSLN.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== SISTEMA DA CLÍNICA VIDA & SAÚDE - PATTERN PROTOTYPE ===\n");

        GerenciadorPrototipos gerenciador = new GerenciadorPrototipos();

        // 1. CONFIGURAÇÃO DOS MODELOS BASE (Protótipos)
        PacoteExame checkupCardio = new PacoteExame("Check-up Cardiológico", "Jejum de 8h e evitar cafeína");
        checkupCardio.AdicionarProcedimento(new Procedimento("Eletrocardiograma", 150.00m));
        checkupCardio.AdicionarProcedimento(new Procedimento("Ecocardiograma", 350.00m));

        // Cadastra o modelo no gerenciador
        gerenciador.CadastrarPrototipo("CARDIO_PADRAO", checkupCardio);

        Console.WriteLine("--- Modelo Base Cadastrado ---");
        checkupCardio.ExibirResumo();

        // 2. USO CORRETO: Clonando via Cópia Profunda (Deep Copy)
        Console.WriteLine("\n------------------------------------------------");
        Console.WriteLine("--> Atendimento 1: Clonando modelo base via Clonar() [Deep Copy]");

        PacoteExame atendimento1 = gerenciador.ObterClone("CARDIO_PADRAO");
        atendimento1.NomePaciente = "Carlos Eduardo";

        // Adicionando procedimento extra exclusivo para o Paciente 1
        atendimento1.AdicionarProcedimento(new Procedimento("Holter 24h", 250.00m));

        atendimento1.ExibirResumo();

        // 3. DEMONSTRAÇÃO DO ERRO DA CÓPIA SUPERFICIAL (Shallow Copy)
        Console.WriteLine("\n------------------------------------------------");
        Console.WriteLine("--> Atendimento 2: Exemplo de Clonagem Superficial (Incorreta)");

        PacoteExame modeloOriginal = gerenciador.ObterClone("CARDIO_PADRAO");
        PacoteExame atendimentoErro = modeloOriginal.ClonarSuperficial(); // Compartilha a mesma lista
        atendimentoErro.NomePaciente = "Ana Maria";

        // Adiciona procedimento no pacote da Ana Maria
        atendimentoErro.AdicionarProcedimento(new Procedimento("Glicemia em Jejum", 30.00m));

        Console.WriteLine("\n[Atendimento da Ana Maria com o novo exame]:");
        atendimentoErro.ExibirResumo();

        Console.WriteLine("\n[VERIFICAÇÃO]: O Modelo Base também foi alterado indevidamente:");
        modeloOriginal.ExibirResumo(); // A alteração afetou o modelo original porque a lista era a mesma!

        // 4. CONFIRMAÇÃO DO ISOLAMENTO DO ATENDIMENTO 1
        Console.WriteLine("\n------------------------------------------------");
        Console.WriteLine("[VERIFICAÇÃO]: Atendimento 1 (Deep Copy) permaneceu intacto:");
        atendimento1.ExibirResumo();
    }
}