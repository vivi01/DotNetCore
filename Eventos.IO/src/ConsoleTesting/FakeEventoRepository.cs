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

		public Evento GetById(Guid id)
		{
			return new Evento("Fake", DateTime.Now, DateTime.Now, true, 0, true, "Empresa");
		}

		public IEnumerable<Evento> GetAll()
		{
			throw new NotImplementedException();
		}

		public void Update(Evento obj)
		{
			//
		}

		public void Remove(Guid id)
		{
			//
		}

		public IEnumerable<Evento> Find(Expression<Func<Evento, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public int SaveChanges()
		{
			throw new NotImplementedException();
		}
	}
}