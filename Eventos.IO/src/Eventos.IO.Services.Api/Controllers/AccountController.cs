using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Domain.Organizadores.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Eventos.IO.Services.Api.Controllers
{
	public class AccountController : BaseController
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly ILogger _logger;
		private readonly IBus _bus;

		public AccountController(UserManager<IdentityUser> userManager,
								SignInManager<IdentityUser> signInManager,
								ILoggerFactory loggerFactory,
								IBus bus,
								IDomainNotificationHandler<DomainNotification> notifications,
								IUser user) : base(notifications, user, bus)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_bus = bus;

			_logger = loggerFactory.CreateLogger<AccountController>();
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("nova-conta")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return Response(model);
			}

			IdentityUser user = new IdentityUser { UserName = model.Input.Email, Email = model.Input.Email };

			IdentityResult result = await _userManager.CreateAsync(user, model.Input.Password);

			if (result.Succeeded)
			{
				RegistrarOrganizadorCommand registroCommand = new RegistrarOrganizadorCommand(Guid.Parse(user.Id), Input.Nome, Input.Cpf, Input.Email);
				_bus.SendCommand(registroCommand);

				if (!OperacaoValida())
				{
					await _userManager.DeleteAsync(user);
					return Response(model);
				}

				_logger.LogInformation(1, "Usuário criado com sucesso");

				return Response(model);
			}

			AdicionarErrosIdentity(result);
			return Response(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("conta")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				NotificarErroModelInvalida();
				return Response(model);
			}

			var result = await _signInManager.PasswordSignInAsync(model.Input.Email, model.Input.Password, false, lockoutOnFailure: true);

			if (result.Succeeded)
			{
				_logger.LogInformation(1, "Usuário logado com Sucesso");
				return Response(model);
			}

			NotificarErro(result.ToString(), "Falha ao realizar o login");
			return Response(model);
		}

		[BindProperty]
		public InputModel Input { get; set; }

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
	}
}