using EscalasPascom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EscalasPascom.ViewModels;

namespace EscalasPascom.Controllers
{
    public class ContaController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public ContaController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 1. LOGIN (GET) - Exibe o formulário
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // 2. LOGIN (POST) - Processa o formulário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, // Usamos o Email como UserName para login (comum)
                    model.Senha,
                    model.Lembrar,
                    lockoutOnFailure: true
                );

                if (result.Succeeded)
                {
                    // Redireciona para a URL original ou Home
                    return RedirectToLocal(returnUrl);
                }

                // Adicione outras verificações (IsLockedOut, RequiresTwoFactor, etc.)

                ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
            }
            return View(model);
        }

        // 3. LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Método auxiliar para redirecionar corretamente
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        // 4. ACESSO NEGADO
        [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}
