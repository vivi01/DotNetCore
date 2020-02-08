using System;
using System.ComponentModel.DataAnnotations;

namespace Eventos.IO.Application.ViewModels
{
	public class OrganizadorViewModel
	{
		[Key]
		public Guid Id { get; set; }

		[Required(ErrorMessage = "O Nome é requerido")]
		public string Nome { get; set; }

		[Required(ErrorMessage = "O CPF é requerido")]
		[StringLength(11)]
		public string Cpf { get; set; }

		[Required(ErrorMessage = "O E-mail é requerido")]
		[EmailAddress(ErrorMessage = "E-mail com Formato Inválido")]
		public string Email { get; set; }
	}
}