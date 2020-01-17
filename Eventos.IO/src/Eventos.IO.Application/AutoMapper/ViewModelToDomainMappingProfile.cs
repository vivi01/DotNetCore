﻿using AutoMapper;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Eventos.Commands;

namespace Eventos.IO.Application.AutoMapper
{
	public class ViewModelToDomainMappingProfile : Profile
	{
		public ViewModelToDomainMappingProfile()
		{
			CreateMap<EventoViewModel, RegistrarEventoCommand>()
				.ConstructUsing(e => new RegistrarEventoCommand(e.Nome, e.DescricaoCurta, e.DescricaoLonga, e.DataInicio, e.DataFim, e.Gratuito, e.Valor, e.Online, e.NomeEmpresa, e.OrganizadorId, e.CategoriaId,
					new IncluirEnderecoEventoCommand(e.Endereco.Id, e.Endereco.Logradouro, e.Endereco.Numero, e.Endereco.Complemento, e.Endereco.Bairro, e.Endereco.Cep, e.Endereco.Cidade, e.Endereco.Estado, e.Id)));

			CreateMap<EventoViewModel, AtualizarEventoCommand>()
				.ConstructUsing(e => new AtualizarEventoCommand(e.Id, e.Nome, e.DescricaoCurta, e.DescricaoLonga, e.DataInicio, e.DataFim, e.Gratuito, e.Valor, e.Online, e.NomeEmpresa, e.OrganizadorId, e.CategoriaId));

			CreateMap<EventoViewModel, ExcluirEventoCommand>()
				.ConstructUsing(e => new ExcluirEventoCommand(e.Id));
		}
	}
}