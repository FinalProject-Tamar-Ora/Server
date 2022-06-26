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

    public class PurchasePrognosisController : ApiController
    {
        PurchasePrognosisDB db = new PurchasePrognosisDB();
        
        // GET: api/PurchasePrognosis
        public object Get()
        {
            return db.GetAllPurchasePrognosis().Data;
        }

        // GET: api/PurchasePrognosis/5
        public object Get(int prognosisId)
        {
            object o = db.GetPurchasePrognosisById(prognosisId).Data;
            return o;
        }

        // POST: api/PurchasePrognosis
        public void Post([FromBody]PurchasePrognosis_Dto p)
        {
            //db.AddNewProduct(p);

        }

        // PUT: api/PurchasePrognosis/5
        public void Put( [FromBody]List<PurchasePrognosis_Dto> lst)
        {
            //db.UpdateList(lst);
        }

        // DELETE: api/PurchasePrognosis/5
        public void Delete(int id)
        {
        }
    }
}
 