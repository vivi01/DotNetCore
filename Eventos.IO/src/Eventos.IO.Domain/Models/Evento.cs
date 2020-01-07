using Eventos.IO.Domain.Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Models
{
	public class Evento : Entity<Evento>
	{
		public Evento(
			string nome,
			DateTime dataInicio,
			DateTime dataFim,
			bool gratuito,
			decimal valor,
			bool online,
			string nomeEmpresa)
		{
			Id = new Guid();
			Nome = nome;
			DataInicio = dataInicio;
			DataFim = dataFim;
			Gratuito = gratuito;
			Valor = valor;
			Online = online;
			NomeEmpresa = nomeEmpresa;

		}
		public string Nome { get; private set; }

		public string DescricaoCurta { get; private set; }

		public string DescricaoLonga { get; private set; }

		public DateTime DataInicio { get; private set; }

		public DateTime DataFim { get; private set; }

		public bool Gratuito { get; private set; }

		public decimal Valor { get; private set; }

		public bool Online { get; private set; }

		public string NomeEmpresa { get; private set; }

		public Categoria Categoria { get; private set; }

		public ICollection<Tags> Tags { get; private set; }

		public Endereco Endereco { get; private set; }

		public Organizador Organizador { get; private set; }

		public override bool EhValido()
		{
			Validar();

			return ValidationResult.IsValid;
		}

		#region Validações

		private void Validar()
		{
			ValidarNome();
			ValidarValor();
			ValidarData();
			ValidarLocal();
			ValidarNomeEmpresa();

			ValidationResult = Validate(this);
		}

		private void ValidarNome()
		{
			RuleFor(e => e.Nome)
				.NotEmpty().WithMessage("O nome do evento deve ser fornecido.")
				.Length(2, 150).WithMessage("O nome do evento deve ter entre 2 e 150 caracteres.");
		}

		private void ValidarValor()
		{
			if (!Gratuito)
			{
				RuleFor(e => e.Valor)
					.ExclusiveBetween(1, 50000)
					.WithMessage("O valor deve estar entre 1.00 e 50.000");
			}

			if (Gratuito)
			{
				RuleFor(e => e.Valor)
					.ExclusiveBetween(0, 0).When(e => e.Gratuito)
					.WithMessage("O valor deve não deve ser preenchido quando o evento é gratuito");
			}
		}

		private void ValidarData()
		{
			RuleFor(e => e.DataInicio)
				.GreaterThan(e => e.DataFim)
				.WithMessage("A data de inicio deve ser maior que a data final do evento");

			RuleFor(e => e.DataInicio)
				.LessThan(DateTime.Now)
				.WithMessage("A data de inicio não deve ser maior que a data atual");
		}

		private void ValidarLocal()
		{
			if (Online)
			{
				RuleFor(e => e.Endereco)
					.Null().When(e => e.Online)
					.WithMessage("O Evento não deve possuir endereço se for Online");
			}

			if (!Online)
			{
				RuleFor(e => e.Endereco)
					.NotNull().When(e => e.Online == false)
					.WithMessage("O Evento deve possuir um Endereço");
			}
		}

		private void ValidarNomeEmpresa()
		{
			RuleFor(e => e.NomeEmpresa)
				.NotEmpty().WithMessage("O nome do organizador precisa ser fornecido")
				.Length(2, 150).WithMessage("O nome do Organizador deve ter entre 2 e 150 caracteres");
		}


		#endregion
	}
}
