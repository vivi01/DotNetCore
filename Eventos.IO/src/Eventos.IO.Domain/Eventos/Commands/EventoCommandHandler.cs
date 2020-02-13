using Eventos.IO.Domain.CommandHandlers;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Events;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using System;

namespace Eventos.IO.Domain.Eventos.Commands
{
	public class EventoCommandHandler : CommandHandler,
		IHandler<RegistrarEventoCommand>,
		IHandler<AtualizarEventoCommand>,
		IHandler<ExcluirEventoCommand>,
		IHandler<AtualizarEnderecoEventoCommand>,
		IHandler<IncluirEnderecoEventoCommand>
	{
		private readonly IEventoRepository _eventoRepository;
		private readonly IBus _bus;


		public EventoCommandHandler(IEventoRepository eventoRepository,
			IUnitOfWork uow,
			IBus bus,
			IDomainNotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
		{
			_eventoRepository = eventoRepository;
			_bus = bus;
		}

		public void Handle(RegistrarEventoCommand message)
		{
			Endereco endereco = new Endereco(message.Endereco.Id, message.Endereco.Logradouro, message.Endereco.Numero, message.Endereco.Complemento, message.Endereco.Bairro, message.Endereco.Cep, message.Endereco.Cidade, message.Endereco.Estado, message.Endereco.EventoId.Value);

			Evento evento = Evento.EventoFactory.NovoEventoCompleto(message.Id, message.Nome, message.DescricaoCurta,
				message.DescricaoLonga, message.DataInicio, message.DataFim, message.Gratuito, message.Valor, message.Online,
				message.NomeEmpresa, message.OrganizadorId, endereco, message.CategoriaId);

			if (!EventoValido(evento))
			{
				return;
			}

			//validações de Negócios


			//Persistência
			_eventoRepository.Adicionar(evento);

			if (Commit())
			{
				_bus.RaiseEvent(new EventoRegistradoEvent(evento.Id, evento.Nome, evento.DataInicio,
					evento.DataFim, evento.Gratuito, evento.Valor, evento.Online, evento.NomeEmpresa));
			}
		}

		public void Handle(AtualizarEventoCommand message)
		{
			Evento eventoAtual = _eventoRepository.ObterPorId(message.Id);

			if (!EventoExistente(message.Id, message.MessageType))
			{
				return;
			}

			//TODO: Validar se o evento pertence a pessoa que esta editando

			Evento evento = Evento.EventoFactory.NovoEventoCompleto(message.Id, message.Nome, message.DescricaoCurta,
				message.DescricaoLonga, message.DataInicio, message.DataFim, message.Gratuito, message.Valor, message.Online,
				message.NomeEmpresa, message.OrganizadorId, eventoAtual.Endereco, message.CategoriaId);

			if (!evento.Online && evento.Endereco == null)
			{
				_bus.RaiseEvent(new DomainNotification(message.MessageType, "Não é possível atualizar um evento sem informar o endereço"));
				return;
			}

			if (!EventoValido(evento))
			{
				return;
			}

			_eventoRepository.Atualizar(evento);

			if (Commit())
			{
				_bus.RaiseEvent(new EventoAtualizadoEvent(evento.Id, evento.Nome, evento.DescricaoCurta,
					evento.DescricaoLonga, evento.DataInicio, evento.DataFim, evento.Gratuito,
					evento.Valor, evento.Online, evento.NomeEmpresa));
			}
		}

		public void Handle(ExcluirEventoCommand message)
		{
			if (!EventoExistente(message.Id, message.MessageType))
			{
				return;
			}

			_eventoRepository.Remover(message.Id);

			if (Commit())
			{
				_bus.RaiseEvent(new EventoExcluidoEvent(message.Id));
			}
		}

		private bool EventoValido(Evento evento)
		{
			if (evento.EhValido())
			{
				return true;
			}

			NotificarValidacoesErro(evento.ValidationResult);
			return false;
		}

		private bool EventoExistente(Guid id, string messageType)
		{
			Evento evento = _eventoRepository.ObterPorId(id);

			if (evento != null)
			{
				return true;
			}

			_bus.RaiseEvent(new DomainNotification(messageType, "Evento não Encontrado!"));

			return false;
		}

		public void Handle(AtualizarEnderecoEventoCommand message)
		{
			Endereco endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.Cep, message.Cidade, message.Estado, message.EventoId.Value);

			if (!endereco.EhValido())
			{
				NotificarValidacoesErro(endereco.ValidationResult);
				return;
			}

			_eventoRepository.AtualizarEndereco(endereco);

			if (Commit())
			{
				_bus.RaiseEvent(new EnderecoEventoAtualizadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.Cep, endereco.Cidade, endereco.Estado, endereco.EventoId.Value));
			}
		}

		public void Handle(IncluirEnderecoEventoCommand message)
		{
			Endereco endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.Cep, message.Cidade, message.Estado, message.EventoId.Value);

			if (!endereco.EhValido())
			{
				NotificarValidacoesErro(endereco.ValidationResult);
				return;
			}

			_eventoRepository.AdicionarEndereco(endereco);

			if (Commit())
			{
				_bus.RaiseEvent(new EnderecoEventoAdicionadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.Cep, endereco.Cidade, endereco.Estado, endereco.EventoId.Value));
			}
		}
	}
}
