using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess1;
using Services.Model;

namespace Services
{
    public class OrdenServices
    {
        private Repository<DataAccess1.Customers> _customerRepository;
        private Repository<Orders> _ordersRepository;
        private Repository<Order_Details> _detailsRepository;
        private Repository<Employees> _employeeRepository;

        public OrdenServices()
        {
            _customerRepository = new Repository<Customers>();
            _ordersRepository = new Repository<Orders>();
            _detailsRepository = new Repository<Order_Details>();
            _employeeRepository = new Repository<Employees>();
        }

        public List<Order> GetAll()
        {

            var orders = _ordersRepository.Set().Select(c => new Order
            {

                OrderID = c.OrderID,
                CustomerID = c.CustomerID,
                EmployeeID = c.EmployeeID,
                OrderDate = c.OrderDate,
                RequiredDate = c.RequiredDate,
                ShippedDate = c.ShippedDate,
                ShipVia = c.ShipVia,
                Freight = c.Freight,
                ShipName = c.ShipName,
                ShipAddress = c.ShipAddress,
                ShipCity = c.ShipCity,
                ShipRegion = c.ShipRegion,
                ShipPostalCode = c.ShipPostalCode,
                ShipCountry = c.ShipCountry,
                Customers = new Customer
                {                   
                    ContactName = c.Customers.ContactName,                    
                },

                Order_Details = c.Order_Details.Select(x => new Order_Detail
                {
                    OrderID = x.OrderID,
                    ProductID = x.ProductID,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    Discount = x.Discount
                }).ToList(),
                Shippers = c.Shippers

            }).ToList();
            return orders;

        }

        public Order GetOne(int id)
        {

            var order = _ordersRepository.Set().FirstOrDefault(c => c.OrderID == id);

            var customerDB = _customerRepository.Set().FirstOrDefault(c => c.CustomerID == order.CustomerID);

            var newCustomer = new Customer
            {
                CustomerID = customerDB.CustomerID,
                CompanyName = customerDB.CompanyName,
                ContactName = customerDB.ContactName,
                City = customerDB.City,
                Country = customerDB.Country
            };

            var employeeDB = _employeeRepository.Set().FirstOrDefault(c => c.EmployeeID == order.EmployeeID);

            var employee = new Employee
            {
                EmployeeID = employeeDB.EmployeeID,
                LastName = employeeDB.LastName,
                FirstName = employeeDB.FirstName,
                Country = employeeDB.Country
            }; 

            var customer = new Order
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipVia = order.ShipVia,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,
                Customers = newCustomer,
                Employees = employee,
                Order_Details = order.Order_Details.Select(x => new Order_Detail
                {
                    OrderID = x.OrderID,
                    ProductID = x.ProductID,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    Discount = x.Discount
                }).ToList(),
                Shippers = order.Shippers
            };



            return customer;

        }

        public void Update(Order orderP, Employee employeeP, Customer customerP)
        {
            var customerService = new CustomerServices();
            var customer = customerService.GetOne(customerP.CustomerID);

            var employeeService = new EmployeeServices();
            var employee = employeeService.GetOne(employeeP.FirstName, employeeP.LastName);

            var employeeDB = _employeeRepository.Set().FirstOrDefault(c => c.EmployeeID == employee.EmployeeID);

            var newEmployee = new Employees
            {
                EmployeeID = employeeDB.EmployeeID,
                LastName = employeeDB.LastName,
                FirstName = employeeDB.FirstName,
                Title = employeeDB.Title,
                TitleOfCourtesy = employeeDB.TitleOfCourtesy,
                BirthDate = employeeDB.BirthDate,
                HireDate = employeeDB.HireDate,
                Address = employeeDB.Address,
                City = employeeDB.City,
                Region = employeeDB.Region,
                PostalCode = employeeDB.PostalCode,
                Country = employeeDB.Country,
                HomePhone = employeeDB.HomePhone,
                Extension = employeeDB.Extension,
                Photo = employeeDB.Photo,
                PhotoPath = employeeDB.PhotoPath,
                Employees1 = employeeDB.Employees1,
                Employees2 = employeeDB.Employees2,
                Orders = employeeDB.Orders,
                Territories = employeeDB.Territories
            };

            var newCustomer = new Customers
            {
                CustomerID = customer.CustomerID,
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactTitle = "",
                Address = "",
                City = customer.City,
                Region = "",
                PostalCode = "",
                Country = customer.Country,
                Phone = "",
                Fax = "",
                Orders = null,
                CustomerDemographics = null,
            };


            var order = _ordersRepository.Set().FirstOrDefault(c => c.OrderID == orderP.OrderID);

            var details = GetOrder_Details(orderP);

            order.OrderID = orderP.OrderID;
            order.CustomerID = newCustomer.CustomerID;
            order.EmployeeID = newEmployee.EmployeeID;
            order.OrderDate = orderP.OrderDate;
            order.RequiredDate = orderP.RequiredDate;
            order.ShippedDate = orderP.ShippedDate;
            order.ShipVia = orderP.ShipVia;
            order.Freight = orderP.Freight;
            order.ShipName = orderP.ShipName;
            order.ShipAddress = orderP.ShipAddress;
            order.ShipCity = orderP.ShipCity;
            order.ShipRegion = orderP.ShipRegion;
            order.ShipPostalCode = orderP.ShipPostalCode;
            order.ShipCountry = orderP.ShipCountry;
            order.Order_Details = details;
            order.Shippers = orderP.Shippers;

            _ordersRepository.SaveChanges();
        }



        public bool Delete(int id)
        {

            var order = _ordersRepository.Set().FirstOrDefault(c => c.OrderID == id);
            var details = _detailsRepository.Set().Where(c=> c.OrderID == id);

            foreach (var detail in details)
            {
                _detailsRepository.Remove(detail);
            }

            _detailsRepository.SaveChanges();

            if (order == null)
            {                
                return false;
            }
            else
            {
                _ordersRepository.Remove(order);
                _ordersRepository.SaveChanges();
                return true;
            }

        }

        public int Save(Order order, Employee employeeP, Customer customerP)
        {
            var customerService = new CustomerServices();
            var customer = customerService.GetOne(customerP.CustomerID);

            var employeeService = new EmployeeServices();
            var employee = employeeService.GetOne(employeeP.FirstName, employeeP.LastName);

            var employeeDB = _employeeRepository.Set().FirstOrDefault(c => c.EmployeeID == employee.EmployeeID);

            var customerDB = _customerRepository.Set().FirstOrDefault(c => c.CustomerID == customer.CustomerID);

            var details = GetOrder_Details(order);

           

            var orderNew = new Orders
            {
                OrderID = order.OrderID,
                CustomerID = customerDB.CustomerID,
                EmployeeID = employeeDB.EmployeeID,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipVia = order.ShipVia,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,
                Order_Details = details,
                Shippers = order.Shippers
            };
            

            try
            {
                _ordersRepository.Persist(orderNew);
                _ordersRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orderNew.OrderID;
            
        }


        public static List<Order_Details> GetOrder_Details(Order order)
        {
            var details = order.Order_Details.Select(c => new Order_Details
            {
                OrderID = c.OrderID,
                ProductID = c.ProductID,
                UnitPrice = c.UnitPrice,
                Quantity = c.Quantity,
                Discount = c.Discount
            }).ToList();

            return details;
        }

        public List<Customers> GetAllCustomer()
        {
            var customers = _customerRepository.Set().ToList();
            return customers;

        }

        public  List<MejorClioente> BestCustomer()
        {
            
            var customer = GetAllCustomer();

            return customer
                .GroupBy(x => x.Country)
                .Select(x => new MejorClioente
                {
                    Country = x.Key,
                    Name = x
                        .OrderByDescending(c => c.Orders
                        .Sum(g => g.Order_Details
                        .Sum(d => d.Quantity * d.Products.UnitPrice)))
                        .Select(h => h.ContactName)
                        .FirstOrDefault(),
                    Total = x.Select(v => v.Orders
                        .Where(c => c.CustomerID == x
                        .OrderByDescending(w => w.Orders
                        .Sum(g => g.Order_Details
                        .Sum(d => d.Quantity * d.Products.UnitPrice)))
                        .Select(h => h.CustomerID)
                        .FirstOrDefault())
                        .Sum(g => g.Order_Details
                        .Sum(d => d.Quantity * d.Products.UnitPrice)))
                        .Sum()
                })
                .ToList();
        }

        public List<CountryBestProduct> BestProduct()
        {
            var customers = GetAllCustomer();

            return customers
                .GroupBy(x => x.Country)
                .Select(x => new CountryBestProduct
                {
                    Country = x.Key,
                    Product = x
                    .SelectMany(c => c.Orders)
                    .SelectMany(v => v.Order_Details)
                    .GroupBy(b => b.ProductID)
                    .OrderByDescending(t => t.Count())
                    .FirstOrDefault()
                    .Select(b => b.Products.ProductName)
                    .FirstOrDefault()

                })
                .ToList();
            
        }

    }
}
