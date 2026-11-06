using System.ComponentModel.DataAnnotations;

namespace appPetShop.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "O usuário é obrigatório.")]
    [Display(Name = "Usuário")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;
}
