using Contracts.Money;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkTest.Models;

namespace WorkTest.Controllers
{
	public class HomeController : Controller
	{
		private readonly IBus _bus;
		//private readonly ILogger<HomeController> _logger;

		public HomeController(IBus bus)
		{
			_bus = bus;
		}


		public async Task<IActionResult> SendMessage()
		{
			var x = await _bus.Request<IGetMoneyRequest, IGetMoneyResponse>(new GetMoneyRequest()
			{
				OrderId = Guid.NewGuid()
			});

			ViewBag.Mess = x.Message.Message;

			return View("Index");
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}