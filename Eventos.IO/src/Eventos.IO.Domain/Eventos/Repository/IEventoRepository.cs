﻿using System;
using System.Collections.Generic;
using Eventos.IO.Domain.Interfaces;

namespace Eventos.IO.Domain.Eventos.Repository
{
	public interface IEventoRepository : IRepository<Evento>
	{
		IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId);

		Endereco ObterEnderecoPorId(Guid enderecoId);

		void AdicionarEndereco(Endereco endereco);

		void AtualizarEndereco(Endereco endereco);
	}
}
