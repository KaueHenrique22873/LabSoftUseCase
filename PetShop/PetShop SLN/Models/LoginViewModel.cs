using System.ComponentModel.DataAnnotations;

namespace PetShop_SLN.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Display(Name = "CPF do Cliente")]
        public string Cpf { get; set; } = string.Empty;
    }
}
