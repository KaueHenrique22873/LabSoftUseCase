using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop_SLN.Models;
using System.Security.Claims;

namespace PetShop_SLN.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbPetShopContext _context;

        public AccountController(DbPetShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ReservaServico");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Cpf == model.Cpf);

            if (cliente == null)
            {
                ModelState.AddModelError("", "CPF não encontrado. Faça seu cadastro primeiro.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Codigo.ToString()),
                new Claim(ClaimTypes.Name, cliente.Nome),
                new Claim("CPF", cliente.Cpf)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "ReservaServico");
        }
        [IgnoreAntiforgeryToken] // Impede que o servidor bloqueie o pedido do JS
        [Route("Account/LogoutOnClose")]
        public async Task<IActionResult> LogoutOnClose()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            // Apaga também o cookie fisicamente da resposta, por precaução
            Response.Cookies.Delete(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}