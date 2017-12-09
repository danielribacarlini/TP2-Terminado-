using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess1;

namespace Services
{
    public class ProductsServices
    {
        private Repository<Products> _productsRepository;

        public ProductsServices()
        {
            _productsRepository = new Repository<Products>();
        }

        public Product GetOne(string productName)
        {

            var product = _productsRepository.Set().FirstOrDefault(c => c.ProductName == productName);

            Product newProduct = null;

            if (product != null)
            {
                newProduct = new Product
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    SupplierID = product.SupplierID,
                    CategoryID = product.CategoryID,
                    QuantityPerUnit = product.QuantityPerUnit,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder,
                    ReorderLevel = product.ReorderLevel,
                    Discontinued = product.Discontinued,
                    Categories = product.Categories,
                    Order_Details = product.Order_Details,
                    Suppliers = product.Suppliers
                };
            }

            return newProduct;

        }
    }
}
