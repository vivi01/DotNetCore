using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Eventos.IO.Site.Controllers
{
	public class EventosController : BaseController
	{
		private readonly IEventoAppService _eventoAppService;

		public EventosController(IEventoAppService eventoAppService,
								 IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(notifications, user)
		{
			_eventoAppService = eventoAppService;
		}

		public IActionResult Index()
		{
			return View(_eventoAppService.ObterTodos());
		}

		public IActionResult Details(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			EventoViewModel eventoViewModel = _eventoAppService.ObterPorId(id.Value);
			if (eventoViewModel == null)
			{
				return NotFound();
			}

			return View(eventoViewModel);
		}

		[Authorize]
		public IActionResult Create()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(EventoViewModel eventoViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(eventoViewModel);
			}

			eventoViewModel.OrganizadorId = OrganizadorId;

			_eventoAppService.Registrar(eventoViewModel);

			ViewBag.RetornoPost = OperacaoValida() ? "success,Evento Registrado com Sucesso!" :
				"error,Evento não Registrado! Verifique as Mensagens";

			return View(eventoViewModel);
		}

		[Authorize]
		public IActionResult Edit(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			EventoViewModel eventoViewModel = _eventoAppService.ObterPorId(id.Value);

			if (eventoViewModel == null)
			{
				return NotFound();
			}
			return View(eventoViewModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(EventoViewModel eventoViewModel)
		{
			if (!ModelState.IsValid) return View(eventoViewModel);

			eventoViewModel.OrganizadorId = OrganizadorId;

			_eventoAppService.Atualizar(eventoViewModel);

			ViewBag.RetornoPost = OperacaoValida() ? "success,Evento Atualizado com Sucesso!" :
				"error,Evento não pode ser Atualizado! Verifique as Mensagens";

			if (_eventoAppService.ObterPorId(eventoViewModel.Id).Online)
			{
				eventoViewModel.Endereco = null;
			}
			else
			{
				eventoViewModel = _eventoAppService.ObterPorId(eventoViewModel.Id);
			}

			return View(eventoViewModel);
		}

		[Authorize]
		public IActionResult Delete(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			EventoViewModel eventoViewModel = _eventoAppService.ObterPorId(id.Value);

			if (eventoViewModel == null)
			{
				return NotFound();
			}

			return View(eventoViewModel);
		}

		[Authorize]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(Guid id)
		{
			_eventoAppService.Excluir(id);

			return RedirectToAction("Index");
		}

		public IActionResult IncluirEndereco(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			EventoViewModel eventoViewModel = _eventoAppService.ObterPorId(id.Value);

			return PartialView("_IncluirEndereco", eventoViewModel);
		}

		public IActionResult AtualizarEndereco(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			EventoViewModel eventoViewModel = _eventoAppService.ObterPorId(id.Value);

			return PartialView("_AtualizarEndereco", eventoViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult IncluirEndereco(EventoViewModel eventoViewModel)
		{
			ModelState.Clear();

			eventoViewModel.Endereco.EventoId = eventoViewModel.Id;

			_eventoAppService.AdicionarEndereco(eventoViewModel.Endereco);

			if (OperacaoValida())
			{
				string url = Url.Action("ObterEndereco", "Eventos", new { id = eventoViewModel.Id });

				return Json(new { success = true, url = url });
			}

			return PartialView("_IncluirEndereco", eventoViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AtualizarEndereco(EventoViewModel eventoViewModel)
		{
			ModelState.Clear();

			_eventoAppService.AtualizarEndereco(eventoViewModel.Endereco);

			if (OperacaoValida())
			{
				string url = Url.Action("ObterEndereco", "Eventos", new { id = eventoViewModel.Id });

				return Json(new { success = true, url = url });
			}

			return PartialView("_AtualizarEndereco", eventoViewModel);
		}

		public IActionResult ObterEndereco(Guid id)
		{
			return PartialView("_DetalhesEndereco", _eventoAppService.ObterPorId(id));
		}
	}
}
