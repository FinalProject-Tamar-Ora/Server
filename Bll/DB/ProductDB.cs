using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Dal;

namespace Bll
{
  public  class ProductDB
  {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();

        public RequestResult GetAllProducts()
        {
                List<Product_Dto> lst = new List<Product_Dto>();
                foreach (var item in db.Product_Tbl.ToList())
                {
                    lst.Add(Product_Dto.DalToDto(item));
                }
                return new RequestResult() { Data = lst, Message = "success", Status = true };
        }
        public RequestResult GetAllCategories()
        {
            List<ProductCategory_Dto> lst = new List<ProductCategory_Dto>();
            foreach (var item in db.ProductCategory_Tbl.ToList())
            {
                lst.Add(ProductCategory_Dto.DalToDto(item));
            }
            return new RequestResult() { Data = lst, Message = "success", Status = true };
        }
        public RequestResult GetProductsByCategoryId(int categoryId)
        {
            List<Product_Dto> lst = new List<Product_Dto>();
            foreach (var item in db.Product_Tbl.ToList())
            {
                if(item.ProductCategoryId==categoryId)
                        lst.Add(Product_Dto.DalToDto(item));
            }
            return new RequestResult() { Data = lst, Message = "success", Status = true };
        }
        public RequestResult GetProductsStartWith(string word)
        {
            List<Product_Tbl> lstTbl = db.Product_Tbl.Where(p => p.Name.StartsWith(word)).ToList();
            List<Product_Dto> lst = new List<Product_Dto>();
            foreach (var item in lstTbl)
            {
                    lst.Add(Product_Dto.DalToDto(item));
            }
            return new RequestResult() { Data = lst, Message = "success", Status = true };
        }
        public void AddProduct(Product_Dto d)
        {
            db.Product_Tbl.Add(d.DtoTODal());
            db.SaveChanges();
        }
        public void UpdateProduct(Product_Dto pr)
        {

            Product_Tbl ct = db.Product_Tbl.Where(p => p.ProductId == pr.ProductId).FirstOrDefault();
            ct.ProductId = pr.ProductId;
            ct.Name = pr.Name;
            ct.ProductCategoryId = pr.ProductCategoryId;
            db.SaveChanges();
        }
        

        public string GetNameOfProductByProdId(int prodId)
        {
            //return Convert.ToString(db.GetNameOfProductByProdId(prodId).FirstOrDefault());
            string str = db.Product_Tbl.Where(p => p.ProductId == prodId).FirstOrDefault().Name.ToString();
            return str;
        }
        public string GetDescriptionOfProductByProdId(int prodId)
        {
            //return Convert.ToString(db.GetDescriptionOfProductByProdId(prodId).FirstOrDefault());
            string str = db.Product_Tbl.Where(p => p.ProductId == prodId).FirstOrDefault().Description.ToString();
            return str;
        }
        public string GetImageOfProductByProdId(int prodId)
        {
            //return  Convert.ToString(db.GetImageOfProductByProdId(prodId).FirstOrDefault());
            string str = db.Product_Tbl.Where(p => p.ProductId == prodId).FirstOrDefault().Image.ToString();
            return str;
        }
    }
}
