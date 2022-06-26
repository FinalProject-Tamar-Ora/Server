using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Bll
{
    [Serializable]

    public class CustomerDB
    {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();
        public List<Customer_Tbl> GetAllCustomers()
        {
            return db.Customer_Tbl.ToList();

        }
        public void AddCustomer(Customer_Dto c)
        {

            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                Customer_Tbl ct = db.Customer_Tbl.Where(p => p.FirstName == c.FirstName && c.LastName == p.LastName && c.Password == p.Password
                && p.Email == c.Email).FirstOrDefault();
                if(ct==null)
                {
                    db.Customer_Tbl.Add(c.DtoTODal());
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }



        public RequestResult GetCustomerByPasswordName(string password, string firstname, string lastname)
        {
            Customer_Tbl c = db.Customer_Tbl.Where(p => p.Password == password && p.FirstName == firstname && p.LastName == lastname).FirstOrDefault();
            return new RequestResult() { Data = Customer_Dto.DalToDto(c), Message = "success", Status = true };
        }
        public int GetCustomerById(int custId)
        {
            return db.Customer_Tbl.Where(p => p.CustomerId == custId).FirstOrDefault().CustomerId;

        }
        public void UpdateCustomer(Customer_Dto c)
        {

            Customer_Tbl ct = db.Customer_Tbl.Where(p => p.CustomerId == c.CustomerId).FirstOrDefault();
            ct.FirstName = c.FirstName;
            ct.LastName = c.LastName;
            ct.Email = c.Email;
            ct.Password = c.Password;
            db.SaveChanges();
        }



    }
}
