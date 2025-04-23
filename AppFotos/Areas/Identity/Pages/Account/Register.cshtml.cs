// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using AppFotos.Data;
using AppFotos.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AppFotos.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        ///     Este objeto será usado para fazer a transposição de dados entre
        ///     este ficheiro (de programação) e a sua respetiva visualização    
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Email do novo utilizador
            /// </summary>
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
            [EmailAddress(ErrorMessage = "Tem de escrever um {0} válido")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     Password do novo utilizador
            /// </summary>
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
            [StringLength(20, ErrorMessage = "A {0} tem de ter pelo menos, {2} e máximo de {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     Confirmação da Password
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Password")]
            [Compare(nameof(Password), ErrorMessage = "A password e a sua confirmação não coincidem.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// Incorporação dos dados de um utilizador
            /// </summary>
            public Utilizadores Utilizador {  get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        ///  Este método 'responde' aos pedidos do browser
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // seo 'return' for nulo, é-lhe atribuído o valor da raiz da aplicação
            returnUrl ??= Url.Content("~/");
            
            // se estiverem definidos métodos alternativos de Registo e Login, ativa
            //
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            // O ModelState avalia o estado do objeto da classe interna 'InputModel'
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                // atribuir ao objeto 'user' o email e o username
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                
                // guardar os dados do 'user' na BD, juntado-lhe a password
                var result = await _userManager.CreateAsync(user, Input.Password);

                // se chegar aqui, consegue escrever os dados do novo utilizador na tabela AspNetUser
                if (result.Succeeded)
                {

                    /* ++++++++++++++++++++++++++++++++++++++++++++ */
                    // guardar os dados do Utilizador na BD
                    /* ++++++++++++++++++++++++++++++++++++++++++++ */
                    // var auxiliar
                    bool haErro = true;

                    // atribuir o ID do utilizador AspNetUser
                    // criado ao objeto Utilizador
                    Input.Utilizador.UserName = Input.Email;
                    try
                    {
                        _context.Add(Input.Utilizador);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        /// NÃO ESQUECER!!!
                        /// HÁ NECESSIDADE DE TRATAR ESTE ERRO!!!
                        /// Se chegam aqui é porque não conseguiram guardar os dados
                        /// O que fazer?
                        ///     - Escrever um 'log' no disco rígido do Servidor
                        ///     - Escrever um erro numa tabela da BD (o que pode não ser possível,
                        ///         dependendo da excepção gerada pela BD)
                        ///     - Apagar o user AspNetUser já criado
                        ///     - Enviar email para o Adiministrador da aplicação a relatar o ocorrido
                        ///     - Notificar a app que há erro
                        ///     - Executar outras ações consideradas necessárias
                        ///         (eventualmente, eliminar a instrução throw)
                        throw;
                    }

                    /* ++++++++++++++++++++++++++++++++++++++++++++ */


                    if (!haErro)
                    {
                        // Obeter ID do novo utilizador
                        var userId = await _userManager.GetUserIdAsync(user);
                        // Obter o Código a ser enviado para o email do novo utilizador
                        // para validar o email, e codificá-lo em UTF8
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        // criar o email e enviá-lo
                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // Se tiver sido definido que o Registo deve ser seguido de validação do email,
                        // redireciona para a página de Confirmação de Registo de um novo Utilizador
                        // este parâmetro está escrito no ficheiro 'Program.cs'
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    // se há erros, mostra-os na página de Registo 
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        /// <summary>
        ///  Cria um objeto vazio do tipo Identity User
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
