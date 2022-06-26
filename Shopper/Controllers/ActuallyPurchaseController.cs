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
    public class ActuallyPurchaseController : ApiController
    {
        ActuallyPurchaseDB dbap = new ActuallyPurchaseDB();

        [HttpGet]
        [Route("api/GetActuallyPurchasesByPurchasesHistoryId/{purchaseHistoryId}")]
        public List<ActuallyPurchase_Dto> GetActuallyPurchasesByPurchasesHistoryId(int purchaseHistoryId)
        {
           return dbap.GetActuallyPurchasesByPurchasesHistoryId(purchaseHistoryId);
        }

        // GET: api/ActuallyPurchase/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ActuallyPurchase
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ActuallyPurchase/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ActuallyPurchase/5
        public void Delete(int id)
        {
        }
    }
}
