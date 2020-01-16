using System.Collections.Generic;

namespace Eventos.IO.Application.ViewModels
{
	public class EstadosViewModel
	{
		public string UF { get; set; }

		public string Nome { get; set; }

		public static List<EstadosViewModel> ListarEstados()
		{
			return new List<EstadosViewModel>()
			{
				new EstadosViewModel() { UF = "AC", Nome = "Acre" },
				new EstadosViewModel() { UF = "AL", Nome = "Alagoas" },
				new EstadosViewModel() { UF = "AP", Nome = "Amapá" },
				new EstadosViewModel() { UF = "AM", Nome = "Amazonas" },
				new EstadosViewModel() { UF = "BA", Nome = "Bahia" },
				new EstadosViewModel() { UF = "CE", Nome = "Ceará" },
				new EstadosViewModel() { UF = "DF", Nome = "Distrito Federal" },
				new EstadosViewModel() { UF = "ES", Nome = "Espírito Santo" },
				new EstadosViewModel() { UF = "GO", Nome = "Goiás" },
				new EstadosViewModel() { UF = "MA", Nome = "Maranhão" },
				new EstadosViewModel() { UF = "MT", Nome = "Mato Grosso" },
				new EstadosViewModel() { UF = "MS", Nome = "Mato Grosso do Sul" },
				new EstadosViewModel() { UF = "MG", Nome = "Minas Gerais" },
				new EstadosViewModel() { UF = "PA", Nome = "Pará" },
				new EstadosViewModel() { UF = "PB", Nome = "Paraíba" },
				new EstadosViewModel() { UF = "PR", Nome = "Paraná" },
				new EstadosViewModel() { UF = "PE", Nome = "Pernambuco" },
				new EstadosViewModel() { UF = "PI", Nome = "Piauí" },
				new EstadosViewModel() { UF = "RJ", Nome = "Rio de Janeiro" },
				new EstadosViewModel() { UF = "RN", Nome = "Rio Grande do Norte" },
				new EstadosViewModel() { UF = "RS", Nome = "Rio Grande do Sul" },
				new EstadosViewModel() { UF = "RO", Nome = "Rondônia" },
				new EstadosViewModel() { UF = "RR", Nome = "Roraima" },
				new EstadosViewModel() { UF = "SC", Nome = "Santa Catarina" },
				new EstadosViewModel() { UF = "SP", Nome = "São Paulo" },
				new EstadosViewModel() { UF = "SE", Nome = "Sergipe" },
				new EstadosViewModel() { UF = "TO", Nome = "Tocantins" },
			};
		}
	}
}