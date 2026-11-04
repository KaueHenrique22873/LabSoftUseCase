using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Display(Name = "CPF do Cliente")]

        public string Cpf { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
