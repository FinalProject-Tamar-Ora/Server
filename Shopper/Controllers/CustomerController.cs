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
   
    public class CustomerController : ApiController
    {
        CustomerDB db = new CustomerDB();
        //GET: api/Customer
        //public List<Customer_Dto Get()
        //{
        //    //object o = db.GetAllCustomers().Data;
        //    return db.GetAllCustomers();
        //}

        [HttpGet]
        [Route("api/GetCustomerByPasswordName/{password}/{firstname}/{lastname}")]
        public object GetCustomerByPasswordName(string password,string firstname,string lastname)
        {
            object o = db.GetCustomerByPasswordName(password.Trim(),firstname.Trim(),lastname.Trim()).Data;
            return o;
        }

        //הוספת לקוח חדש
        // POST: api/Customer
        public void Post( Customer_Dto c)
        {
            db.AddCustomer(c);
        }
        //עדכון פרטי לקוח
        // PUT: api/Customer/5
        public void Put(Customer_Dto c)
        {
            db.UpdateCustomer(c);
        }

        // DELETE: api/Customer/5
        public void Delete(int id)
        {

        }
    }
}
