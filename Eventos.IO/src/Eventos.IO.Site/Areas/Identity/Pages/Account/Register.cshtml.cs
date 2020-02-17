using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Eventos.IO.Site.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;
		private readonly IOrganizadorAppService _organizadorAppService;

		public RegisterModel(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender,
			IDomainNotificationHandler<DomainNotification> notifications,
			IOrganizadorAppService organizadorAppService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
			_organizadorAppService = organizadorAppService;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public class InputModel
		{
			[Required(ErrorMessage = "O Nome é requerido")]
			public string Nome { get; set; }

			[Required(ErrorMessage = "O CPF é requerido")]
			[StringLength(11)]
			public string Cpf { get; set; }

			[Required(ErrorMessage = "O E-mail é requerido")]
			[EmailAddress(ErrorMessage = "E-mail com Formato Inválido")]
			public string Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Senha")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirme a Senha")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }
		}

		public void OnGet(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");
			if (ModelState.IsValid)
			{
				IdentityUser user = new IdentityUser { UserName = Input.Email, Email = Input.Email };

				IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
				if (result.Succeeded)
				{
					List<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>()
					{
						new System.Security.Claims.Claim("Eventos", "Ler"),
						new System.Security.Claims.Claim("Eventos", "Gravar")
					};

					await _userManager.AddClaimsAsync(user, claims);

					_logger.LogInformation("User created a new account with password.");

					OrganizadorViewModel organizador = new OrganizadorViewModel
					{
						Id = Guid.Parse(user.Id),
						Email = user.Email,
						Nome = Input.Nome,
						Cpf = Input.Cpf
					};

					_organizadorAppService.Registrar(organizador);

					string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					string callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { userId = user.Id, code = code },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
