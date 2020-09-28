using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using ServerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using ServerApp.Models.BindingTargets;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json;
using System.Reflection;
using System.ComponentModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerApp.Controllers
{


	//[Route("api/[controller]")]
	//  Match requests to api/products, not api/productValues.
	[Route("api/productvalues")]
	[ApiController]
	public class ProductValuesController : ControllerBase
	{
		private DataContext m_DataContext;

		public ProductValuesController(DataContext dataContext)
		{
			m_DataContext = dataContext;
		}

		//	HEAD executes equivalently to GET except no body is returned, just the headers.
		// HEAD api/<ProductValuesController>/5
		[HttpHead("{id}")]
		public void Head(int id)
		{
		}

		// GET api/<ProductValuesController>/5
		[HttpGet("{id}")]
		public Product Get(int id)
		{
			Product product;

			//Thread.Sleep(3000);
			//return m_DataContext.Products.Find((long)id);
			product = m_DataContext.Products
				//  Join the supplier.
				//.Include( p => p.Supplier )
				//	Instead of just joining the supplier, join the supplier and their associated supplier products.
				.Include(p => p.Supplier).ThenInclude(s => s.Products)
				//  Join the ratings.
				.Include(p => p.Ratings)
				//  Only a single object.
				.FirstOrDefault(p => p.ProductId == id);

			if (product == null)
			{
				//  The product does not exist.
			}
			else
			{
				if (product.Supplier == null)
				{
					//  The supplier is not assigned.
				}
				else
				{
					//  Prevent a circular reference to this same product.
					//product.Supplier.Products = null;

					//	Instead of just nulling out the products, fill the supplier products with new objects from the query.
					product.Supplier.Products = product.Supplier.Products.Select(p =>
					   new Product
					   {
						   ProductId = p.ProductId,
						   Name = p.Name,
						   Category = p.Category,
						   Description = p.Description,
						   Price = p.Price
					   });
				}

				if (product.Ratings == null)
				{
					//  There are no ratings.
				}
				else
				{
					//  Prevent circular references to this same product.
					foreach (Rating rating in product.Ratings)
					{
						rating.Product = null;
					}
				}
			}
			return (product);
		}

		// GET: api/<ProductValuesController>
		[HttpGet]
		public IActionResult GetProducts(string category, string search,
			bool isRelatedRequired = false, bool isMetadata = false)
		{
			IActionResult iActionResult;
			//	The query will be constructed step-by-step and will be executed only when the results are enumerated.
			IQueryable<Product> query = m_DataContext.Products;

			//	Determine if a category filter will apply.
			if (string.IsNullOrWhiteSpace(category))
			{
				//	No filter by category.
			}
			else
			{
				//	Apply a filter by category.
				string categoryLc = category.ToLower();
				query = query.Where(p => p.Category.ToLower().Contains(categoryLc));
			}

			//	Determine if a search filter will apply.
			if (string.IsNullOrWhiteSpace(search))
			{
				//	No search filter is applied.
			}
			else
			{
				//	Apply a filter to search by name or by description.
				string searchLc = search.ToLower();
				query = query.Where(p => p.Name.ToLower().Contains(searchLc) ||
				   p.Description.ToLower().Contains(searchLc));
			}


			if (isRelatedRequired)
			{
				//	Join the product suppliers and join the product ratings.
				query = query.Include(p => p.Supplier).Include(p => p.Ratings);

				//	Remove circular references.
				List<Product> listProduct = query.ToList();
				listProduct.ForEach(p =>
			   {
				   if (p.Supplier == null)
				   {
					   //	Already null so no circular references.
				   }
				   else
				   {
					   //	Remove any possible circular reference.
					   p.Supplier.Products = null;
				   }
				   if (p.Ratings == null)
				   {
					   //	Already null so no circular references.
				   }
				   else
				   {
					   //	Remove any possible circular reference.
					   p.Ratings.ForEach(r => r.Product = null);
				   }
			   });
				if (isMetadata)
				{
					iActionResult = CreateMetadata(listProduct);
				}
				else
				{
					iActionResult = Ok(listProduct);
				}
			}
			else
			{
				//	Return the simple dataset.
				if (isMetadata)
				{
					iActionResult = CreateMetadata(query);
				}
				else
				{
					iActionResult = Ok(query);
				}
			}
			return iActionResult;
		}

		private IActionResult CreateMetadata(IEnumerable<Product> products)
		{
			IActionResult iActionResult;

			//	The client and server keys of this type must match.
			iActionResult = Ok(
				new
				{
					products = products,
					categories = m_DataContext.Products.Select(product => product.Category).Distinct().OrderBy(category => category)
				});
			return (iActionResult);
		}

		// POST api/<ProductValuesController>
		[HttpPost]
		public IActionResult CreateProduct([FromBody] ProductData productData)
		{
			IActionResult actionResult;
			if (ModelState.IsValid)
			{
				Product product = productData.Product;
				if (product.Supplier != null &&
					product.Supplier.SupplierId != 0)
				{
					//	If the entity being added has references to other entities that are not yet tracked
					//	then these new entities will also be added to the context
					//	AND WILL BE INSERTED into the database the next time that SaveChanges is called!

					//	Attach the supplier so it will NOT be inserted when SaveChanges is called.
					m_DataContext.Attach(product.Supplier);
				}

				//	Tell the context that the object will be inserted.
				m_DataContext.Add(product);

				//	Execute the SQL.
				m_DataContext.SaveChanges();

				//	Provide the new primary key to the client.
				actionResult = Ok(product.ProductId);
			}
			else
			{
				actionResult = BadRequest(ModelState);
			}
			return (actionResult);
		}

		// PUT api/<ProductValuesController>/5
		[HttpPut("{id}")]
		public IActionResult ReplaceProduct(long id, [FromBody] ProductData productData)
		{
			IActionResult iActionResult;

			//	Check if id != 0?????
			if (ModelState.IsValid)
			{
				Product product = productData.Product;
				product.ProductId = id;
				if (product.Supplier != null && product.Supplier.SupplierId != 0)
				{
					//	A supplier already exists, do not create a new provider.
					m_DataContext.Attach(product.Supplier);
				}
				//	Entity Framework will replace the existing data.
				m_DataContext.Update(product);
				m_DataContext.SaveChanges();
				iActionResult = Ok();
			}
			else
			{
				iActionResult = BadRequest(ModelState);
			}
			return (iActionResult);
		}

		// PATCH api/<ProductValuesController>/5
		[HttpPatch("{id}")]
		public IActionResult Patch(long id, [FromBody] JsonPatchDocument<ProductData> jsonPatchDocument)
		{
			IActionResult iActionResult;

			//	Get the current record from the database.
			Product product = m_DataContext.Products
				.Include(p => p.Supplier)
				.First(p => p.ProductId == id);

			//	Initialize the buffer with the current record object.
			ProductData productData = new ProductData { Product = product };

			//	Apply the patch operations to the buffer, which applies those changes to the current record object.
			jsonPatchDocument.ApplyTo(productData, ModelState);

			if (ModelState.IsValid && TryValidateModel(productData))
			{
				//	The buffer is valid so the product changes may be saved.
				if (product.Supplier != null &&
					product.Supplier.SupplierId != 0)
				{
					//	Do not create a supplier since it exists.
					m_DataContext.Add(product.Supplier);
				}

				//	Persist the patched changes.
				m_DataContext.SaveChanges();
				iActionResult = Ok();
			}
			else
			{
				//	The changes can not be applied.
				iActionResult = BadRequest(ModelState);
			}
			return (iActionResult);
		}


		// DELETE api/<ProductValuesController>/5
		[HttpDelete("{id}")]
		public void DeleteProduct(long id)
		{
			m_DataContext.Products.Remove(new Product
			{
				ProductId = id
			});
			m_DataContext.SaveChanges();
		}
	}
}
