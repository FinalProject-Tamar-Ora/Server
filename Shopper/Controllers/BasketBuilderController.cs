using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bll;
using Bll.Algorithm;
using Dto;
namespace Shopper.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BasketBuilderController : ApiController
    {
        BasketBuilderDB db = new BasketBuilderDB();

        [HttpGet]
        [Route("api/GetPurchaseOffer/{customerId}")]
        public object GetPurchaseOffer(int customerId)
        {

            return db.GetPurchaseOffer(customerId).Data;
        }
        [HttpGet]
        [Route("api/GetFinallyPurchase/{customerId}")]
        public object GetFinallyPurchase(int customerId)
        {

            return db.GetFinallyPurchase(customerId).Data;
        }
       
       [HttpPost]
       [Route("api/AddActPur/{customerId}")]
        public void AddActPur( int customerId,List<ActuallyPurchase_Dto> lst)
        { 
            db.ActuallyPurcase(customerId, lst);
        }
        [HttpGet]
        [Route("api/GetCurrentJewishHebrewDate")]
        public object GetCurrentJewishHebrewDate()
        {

            // string str =
            return GeneralFunctions.GetHebrewJewishDateString(new DateTime(2021,09,10)).Data;
            //return str;
        }





        // GET: api/BuildBasket
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/BuildBasket/5
        public string Get(int id)
        {
            return "value";
        }

       

        // PUT: api/BuildBasket/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BuildBasket/5
        public void Delete(int id)
        {
        }
    }
}
