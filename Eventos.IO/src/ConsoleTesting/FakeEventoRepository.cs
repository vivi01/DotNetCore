using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ConsoleTesting
{
	public class FakeEventoRepository : IEventoRepository
	{
		public void Dispose()
		{
			//
		}

		public void Add(Evento obj)
		{
			//
		}

		public Evento ObterPorId(Guid id)
		{
			return new Evento("Fake", DateTime.Now, DateTime.Now, true, 0, true, "Empresa");
		}

		public IEnumerable<Evento> ObterTodos()
		{
			throw new NotImplementedException();
		}

		public void Atualizar(Evento obj)
		{
			//
		}

		public void Remover(Guid id)
		{
			//
		}

		public IEnumerable<Evento> Buscar(Expression<Func<Evento, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public int SaveChanges()
		{
			throw new NotImplementedException();
		}
	}
}