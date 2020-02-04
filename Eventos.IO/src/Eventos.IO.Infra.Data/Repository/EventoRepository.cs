using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Eventos.IO.Infra.Data.Repository
{
	public class EventoRepository : Repository<Evento>, IEventoRepository
	{
		public EventoRepository(EventosContext context) : base(context)
		{

		}

		public override IEnumerable<Evento> ObterTodos()
		{
			var sql = "SELECT * FROM EVENTOS E " +
			               "WHERE E.EXCLUIDO = 0 " + 
				           "ORDER BY E.DATAFIM DESC ";

			return Db.Database.GetDbConnection().Query<Evento>(sql);
		}

		public void AdicionarEndereco(Endereco endereco)
		{
			Db.Enderecos.Add(endereco);
		}

		public void AtualizarEndereco(Endereco endereco)
		{
			Db.Enderecos.Update(endereco);
		}

		public Endereco ObterEnderecoPorId(Guid enderecoId)
		{
			return Db.Enderecos.Find(enderecoId);
		}

		public IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId)
		{
			return Db.Eventos.Where(e => e.OrganizadorId == organizadorId);
		}

		public override Evento ObterPorId(Guid id)
		{
			var sql = "SELECT * FROM Eventos E " +
			               "LEFT JOIN Enderecos END " +
			               "ON E.Id = END.Id " +
			               "WHERE E.Id = @uid";
			var evento = Db.Database.GetDbConnection().Query<Evento, Endereco, Evento>(sql,
				(e, end) =>
				{
					if(end != null)
						e.AtribuirEndereco(end);

					return e;
				}, new { uid = id});

			return evento.FirstOrDefault();
		}
	}
}