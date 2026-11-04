using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PetShop.Models;

namespace PetShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbPetShopContext _context;

        public AccountController(DbPetShopContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Agendamento");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Busca o cliente pelo CPF
            var cliente = await _context.Tutor
                .FirstOrDefaultAsync(c => c.Cpf == model.Cpf);

            // Valida se o cliente existe e se a senha confere
            if (cliente == null || cliente.Senha != model.Senha)
            {
                ModelState.AddModelError("", "CPF ou senha inválidos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Codigo.ToString()),
                new Claim(ClaimTypes.Name, cliente.Nome),
                new Claim("CPF", cliente.Cpf),
                new Claim(ClaimTypes.Role, cliente.PerfilAcesso ?? "user")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Agendamento");
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Agendamento");
            }
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Nome,Cpf,Senha")] Tutor tutor)
        {
            // Remove da validação os campos do modelo que não são enviados pelo formulário da View
            ModelState.Remove("PerfilAcesso");
            ModelState.Remove("DataCriacao");
            ModelState.Remove("Pets");

            if (ModelState.IsValid)
            {
                // Verificação de duplicação por CPF
                var cpfExiste = await _context.Tutor.AnyAsync(t => t.Cpf == tutor.Cpf);
                if (cpfExiste)
                {
                    ModelState.AddModelError("Cpf", "Este CPF já está cadastrado.");
                    return View(tutor);
                }

                // Configuração dos campos padrão do sistema
                tutor.PerfilAcesso = "user";
                tutor.DataCriacao = DateTime.Now;

                _context.Tutor.Add(tutor);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Conta criada com sucesso! Faça login para continuar.";
                return RedirectToAction("Login");
            }

            return View(tutor);
        }

        // POST/GET: Account/Logout
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}