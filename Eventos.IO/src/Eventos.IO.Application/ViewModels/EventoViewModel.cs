using System;
using System.ComponentModel.DataAnnotations;

namespace Eventos.IO.Application.ViewModels
{
	public class EventoViewModel
	{
		public EventoViewModel()
		{
			Id = Guid.NewGuid();
			Endereco = new EnderecoViewModel();
			Categoria = new CategoriaViewModel();
		}

		[Key]
		public Guid Id { get; set; }

		[Required(ErrorMessage = "O nome é requerido")]
		[MinLength(2, ErrorMessage = "O Tamanho mínimo do nome é {1}")]
		[MaxLength(150, ErrorMessage = "O Tamanho máximo do nome é {1}")]
		[Display(Name = "Nome do Evento")]
		public string Nome { get; set; }

		[Display(Name = "Descrição Curta do Evento")]
		public string DescricaoCurta { get; set; }

		[Display(Name = "Descrição Longa do Evento")]
		public string DescricaoLonga { get; set; }

		[Display(Name = "Ínicio do Evento")]
		[Required(ErrorMessage = "A Data é Requerida!")]
		public DateTime DataInicio { get; set; }

		[Display(Name = "Fim do Evento")]
		[Required(ErrorMessage = "A Data é Requerida!")]
		public DateTime DataFim { get; set; }

		[Display(Name = "Será Gratuito?")]
		public bool Gratuito { get; set; }

		[Display(Name = "Valor")]
		[DataType(DataType.Currency, ErrorMessage = "Moeda em Formato Inválido!")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		public decimal Valor { get; set; }

		[Display(Name = "Será Online?")]
		public bool Online { get; set; }

		[Display(Name = "Empresa / Grupo Organizador")]
		public string NomeEmpresa { get; set; }

		public EnderecoViewModel Endereco { get; set; }

		public CategoriaViewModel Categoria { get; set; }

		public Guid CategoriaId { get; set; }

		public Guid OrganizadorId { get; set; }

		public Guid EnderecoId { get; set; }
	}
}