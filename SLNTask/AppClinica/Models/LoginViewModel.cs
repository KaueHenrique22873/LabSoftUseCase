using System.ComponentModel.DataAnnotations;
namespace AppClinica.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Display(Name = "CPF do Paciente")]
        public string Cpf { get; set; } = string.Empty;
    }
}