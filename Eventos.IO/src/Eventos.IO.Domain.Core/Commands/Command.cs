using Eventos.IO.Domain.Core.Events;
using System;

namespace Eventos.IO.Domain.Core.Commands
{
	public class Command : Message
	{
		public DateTime TimeStamp { get; private set; }

		public Command()
		{
			TimeStamp = DateTime.Now;
		}
	}
}
