using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Dto;
using Bll;
namespace Shopper.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class PurchasesHistoryController : ApiController
    {
        PurchasesHistoryDB p = new PurchasesHistoryDB();
        // GET: api/PurchasesHistory
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/GetAllPurchasesHistoryByCustomerId/{customerId}")]
        public object Get(int customerId)
         {
            return p.GetAllPurchasesHistoryByCustomerId(customerId).Data; 
        }

        [Route("api/GetJewishHebrewDateByForeignDate/{phId}")]
        public object GetJewishHebrewDateByForeignDate(int phId)
        {
            DateTime d = p.GetDate(phId);
            // string str =
            return GeneralFunctions.GetHebrewJewishDateString(d).Data;
            //return str;
        }



        // POST: api/PurchasesHistory
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PurchasesHistory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PurchasesHistory/5
        public void Delete(int id)
        {
        }
        
    }
}
