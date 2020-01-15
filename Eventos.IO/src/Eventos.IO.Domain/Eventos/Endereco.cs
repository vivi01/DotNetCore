using Eventos.IO.Domain.Core.Models;
using FluentValidation;
using System;

namespace Eventos.IO.Domain.Eventos
{
	public class Endereco : Entity<Endereco>
	{
		public string Logradouro { get; private set; }

		public string Numero { get; private set; }

		public string Complemento { get; private set; }

		public string Bairro { get; private set; }

		public string Cep { get; private set; }

		public string Cidade { get; private set; }

		public string Estado { get; private set; }

		public Guid? EventoId { get; private set; }

		//	EF propriedade de navegação
		public Evento Evento { get; private set; }

		public Endereco(Guid id, string logradouro, string numero, string complemento, string bairro, string cep, string cidade,
			string estado, Guid? eventoId)
		{
			Id = id;
			Logradouro = logradouro;
			Numero = numero;
			Complemento = complemento;
			Bairro = bairro;
			Cep = cep;
			Cidade = cidade;
			Estado = estado;
			EventoId = eventoId;
		}

		// Construtor para  o EF
		protected Endereco() { }

		public override bool EhValido()
		{
			RuleFor(e => e.Logradouro)
				.NotEmpty().WithMessage("O Logradouro precisa ser fornecido.")
				.Length(2, 150).WithMessage("O Logradouro precisa ter entre 2 e 150 caracteres");

			RuleFor(e => e.Bairro)
				.NotEmpty().WithMessage("O Bairro precisa ser fornecido.")
				.Length(2, 150).WithMessage("O Bairro precisa ter entre 2 e 150 caracteres");

			RuleFor(e => e.Cep)
				.NotEmpty().WithMessage("O Cep precisa ser fornecido.")
				.Length(8).WithMessage("O CEP precisa ter 8 caracteres");

			RuleFor(e => e.Cidade)
				.NotEmpty().WithMessage("A cidade precisa ser fornecido.")
				.Length(2, 150).WithMessage("A cidade precisa ter entre 2 e 150 caracteres");

			RuleFor(e => e.Estado)
				.NotEmpty().WithMessage("O Estado precisa ser fornecido.")
				.Length(2, 150).WithMessage("O Estado precisa ter entre 2 e 150 caracteres");

			RuleFor(e => e.Numero)
				.NotEmpty().WithMessage("O Número precisa ser fornecido.")
				.Length(1, 10).WithMessage("O Número precisa ter entre 1 e 10 caracteres");

			ValidationResult = Validate(this);

			return ValidationResult.IsValid;
		}
	}
}