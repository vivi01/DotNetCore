using System;
using AutoMapper;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Organizadores.Commands;

namespace Eventos.IO.Application.AutoMapper
{
	public class ViewModelToDomainMappingProfile : Profile
	{
		public ViewModelToDomainMappingProfile()
		{
			CreateMap<EventoViewModel, RegistrarEventoCommand>()
				.ConstructUsing(e => new RegistrarEventoCommand(e.Nome, e.DescricaoCurta, e.DescricaoLonga, e.DataInicio, e.DataFim, e.Gratuito, e.Valor, e.Online, e.NomeEmpresa, e.OrganizadorId, e.CategoriaId,
					new IncluirEnderecoEventoCommand(e.Endereco.Id, e.Endereco.Logradouro, e.Endereco.Numero, e.Endereco.Complemento, e.Endereco.Bairro, e.Endereco.Cep, e.Endereco.Cidade, e.Endereco.Estado, e.Id)));

			CreateMap<EnderecoViewModel, IncluirEnderecoEventoCommand>()
				.ConstructUsing(c => new IncluirEnderecoEventoCommand(Guid.NewGuid(), c.Logradouro, c.Numero, c.Complemento, c.Bairro, c.Cep, c.Cidade, c.Estado, c.EventoId));

			CreateMap<EnderecoViewModel, AtualizarEnderecoEventoCommand>()
				.ConstructUsing(c => new AtualizarEnderecoEventoCommand(Guid.NewGuid(), c.Logradouro, c.Numero, c.Complemento, c.Bairro, c.Cep, c.Cidade, c.Estado, c.EventoId));

			CreateMap<EventoViewModel, AtualizarEventoCommand>()
				.ConstructUsing(e => new AtualizarEventoCommand(e.Id, e.Nome, e.DescricaoCurta, e.DescricaoLonga, e.DataInicio, e.DataFim, e.Gratuito, e.Valor, e.Online, e.NomeEmpresa, e.OrganizadorId, e.CategoriaId));

			CreateMap<EventoViewModel, ExcluirEventoCommand>()
				.ConstructUsing(e => new ExcluirEventoCommand(e.Id));

			//Organizador
			CreateMap<OrganizadorViewModel, RegistrarOrganizadorCommand>()
				.ConstructUsing(o => new RegistrarOrganizadorCommand(o.Id, o.Nome, o.Cpf, o.Email));
		}
	}
}