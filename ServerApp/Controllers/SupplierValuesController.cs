using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
using ServerApp.Models.BindingTargets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerApp.Controllers
{
	[Route( "api/suppliervalues" )]
	[ApiController]
	public class SupplierValuesController : ControllerBase
	{
		private DataContext m_DataContext;

		public SupplierValuesController( DataContext dataContext )
		{
			m_DataContext = dataContext;
		}


		// GET: api/<SupplierValuesController>
		[HttpGet]
		public IEnumerable<Supplier> Get()
		{
			return m_DataContext.Suppliers;
		}

		// GET api/<SupplierValuesController>/5
		[HttpGet( "{id}" )]
		public Supplier Get( int id )
		{
			Supplier supplier;

			supplier = m_DataContext.Suppliers
				//  Only a single object.
				.FirstOrDefault( s => s.SupplierId == id );

			return ( supplier );
		}

		// POST api/<SupplierValuesController>
		[HttpPost]
		public IActionResult CreateSupplier( [FromBody] SupplierData supplierData )
		{
			IActionResult actionResult;
			if( ModelState.IsValid )
			{
				Supplier supplier = supplierData.Supplier;

				//	Tell the context that the object will be inserted.
				m_DataContext.Add( supplier );

				//	Execute the SQL.
				m_DataContext.SaveChanges();

				//	Provide the new primary key to the client.
				actionResult = Ok( supplier.SupplierId );
			}
			else
			{
				actionResult = BadRequest( ModelState );
			}
			return ( actionResult );
		}

		// PUT api/<SupplierValuesController>/5
		[HttpPut( "{id}" )]
		public IActionResult ReplaceSupplier( long id, [FromBody] SupplierData supplierData )
		{
			IActionResult iActionResult;

			//	Check if id != 0?????
			if( ModelState.IsValid )
			{
				Supplier supplier = supplierData.Supplier;
				supplier.SupplierId = id;

				//	Entity Framework will replace the existing data.
				m_DataContext.Update( supplier );
				m_DataContext.SaveChanges();
				iActionResult = Ok();
			}
			else
			{
				iActionResult = BadRequest( ModelState );
			}
			return ( iActionResult );
		}

		// DELETE api/<SupplierValuesController>/5
		[HttpDelete( "{id}" )]
		public void DeleteSupplier( long id )
		{
			m_DataContext.Remove( new Supplier
			{
				SupplierId = id
			} );
			m_DataContext.SaveChanges();
		}
	}
}
