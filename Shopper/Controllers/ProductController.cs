using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bll;
using Dto;
namespace Shopper.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        ProductDB db = new ProductDB();
        // GET: api/Product
        public object Get()
        {
            return db.GetAllProducts().Data;
        }
        [HttpGet]
        [Route("api/GetProductsByCategoryId/{categoryId}")]
        public object GetProductsByCategoryId(int categoryId)
        {
            
             return db.GetProductsByCategoryId(categoryId).Data;
        }

        [HttpGet]
        [Route("api/GetProductsStartWith/{word}")]
        public object GetProductsStartWith(string word)
        {

            return db.GetProductsStartWith(word).Data;
        }




        // GET: api/Product/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Product
        public void Post([FromBody]Product_Dto p)
        {
            db.AddProduct(p);
        }

        // PUT: api/Product/5
        public void Put(Product_Dto p)
        {
            db.UpdateProduct(p);
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
        }
    }
}
