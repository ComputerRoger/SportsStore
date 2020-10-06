using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerApp.Controllers
{
	//  Match requests to api/orders.
	[Route("api/orders")]
	[ApiController]
	public class OrderValuesController : ControllerBase
	{
		private DataContext m_DataContext;

		//	Constructor
		public OrderValuesController(DataContext dataContext)
		{
			m_DataContext = dataContext;
		}

		[HttpGet]
		public IEnumerable<Order> GetOrders()
		{
			IEnumerable<Order> orders;

			//	Perform the query.
			orders = m_DataContext.Orders
				//	Join Products that belong to the order.
				.Include(order => order.Products)
				//	Join Payments that belong to the order.
				.Include(order => order.Payment);
			return orders;
		}

		[HttpPost("{id}")]
		public void MarkShipped(long id)
		{
			Order order = m_DataContext.Orders.Find(id);

			if (order != null)
			{
				order.Shipped = true;
				m_DataContext.SaveChanges();
			}
		}

		[AllowAnonymous]		//	All access by any user.
		[HttpPost]
		public IActionResult CreateOrder([FromBody] Order order)
		{
			IActionResult iActionResult;

			order.OrderId = 0;
			order.Shipped = false;
			order.Payment.Total = GetPrice(order.Products);

			//	Get the money from the payment processor.
			ProcessPayment(order.Payment);

			if (order.Payment.AuthCode != null)
			{
				m_DataContext.Add(order);
				m_DataContext.SaveChanges();

				//	This interface must match the client.
				iActionResult = Ok(new
				{
					orderId = order.OrderId,
					authCode = order.Payment.AuthCode,
					amount = order.Payment.Total
				});
			}
			else
			{
				iActionResult = BadRequest("Payment rejected.");
			}
			return (iActionResult);
		}

		//////////////////////////		Methods		///////////////////////////

		//	Do NOT trust the client to provide price information.
		//	Critical and sentitive business logic goes here.
		private decimal GetPrice(IEnumerable<CartLine> lines)
		{
			//	Get a collection of product foreign keys that belong to the order.
			IEnumerable<long> ids = lines.Select(line => line.ProductId);

			//	Get the products that are included within the order.
			IEnumerable<Product> products = m_DataContext.Products
				.Where(product => ids.Contains(product.ProductId));

			decimal sum;

			sum = products.Select(product => lines
			   .First(line => line.ProductId == product.ProductId)
			   .Quantity * product.Price)
				.Sum();

			return (sum);
		}

		private void ProcessPayment(Payment payment)
		{
			//	Integrate your payment system here.
			payment.AuthCode = "12345";
		}

	}
}
