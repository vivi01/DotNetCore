using System;
using System.Collections.Generic;
using Eventos.IO.Domain.Core.Models;
using Eventos.IO.Domain.Eventos;

namespace Eventos.IO.Domain.Organizadores
{
	public class Organizador : Entity<Organizador>
	{
		public string Nome { get; private set; }

		public string Cpf { get; private set; }

		public string Email { get; private set; }

		public Organizador(Guid id, string nome, string cpf, string email)
		{
			Id = id;
			Nome = nome;
			Cpf = cpf;
			Email = email;
		}

		//EF Propriedade de Navegação
		public virtual ICollection<Evento> Eventos { get; set; }

		//EF Constructor
		public Organizador() { }

		public override bool EhValido()
		{
			return true;
		}
	}
}