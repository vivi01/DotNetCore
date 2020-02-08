﻿using System;
using Eventos.IO.Domain.Core.Commands;

namespace Eventos.IO.Domain.Organizadores.Commands
{
	public class RegistrarOrganizadorCommand : Command
	{
		public Guid Id { get; private set; }

		public string Nome { get; private set; }

		public string Cpf { get; private set; }

		public string Email { get; private set; }

		public RegistrarOrganizadorCommand(Guid id, string nome, string cpf, string email)
		{
			Id = id;
			Nome = nome;
			Cpf = cpf;
			Email = email;
		}
	}
}