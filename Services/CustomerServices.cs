using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess1;
using Services.Model;

namespace Services
{
    public class CustomerServices
    {
        private Repository<DataAccess1.Customers> _customerRepository;
        private Repository<Orders> _ordersRepository;
        

        public CustomerServices()
        {
            _customerRepository = new Repository<Customers>();
            _ordersRepository = new Repository<Orders>();
        }

        public List<Customer> GetAll()
        {
            
                var customers = _customerRepository.Set().Select(c=> new Customer
                {
                    CustomerID = c.CustomerID,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    City = c.City
                }).ToList();
                return customers;
            
        }

        public Customer GetOne(string id)
        {
          
                var customers = _customerRepository.Set().FirstOrDefault(c => c.CustomerID == id);

            Customer customer = null;
            if(customers != null)
                 customer = new Customer
                {
                    CustomerID = customers.CustomerID,
                    CompanyName = customers.CompanyName,
                    ContactName = customers.CompanyName,
                    City = customers.City
                };

                return customer;
                        
        }

        public void Update(Customer customerM)
        {
            
                var customers = _customerRepository.Set().FirstOrDefault(c => c.CustomerID == customerM.CustomerID);

                customers.CustomerID = customerM.CustomerID;
                customers.CompanyName = customerM.CompanyName;
                customers.ContactName = customerM.ContactName;
                customers.City = customerM.City;

                _customerRepository.SaveChanges();
        }

        

        public bool Delete(string id)
        {
            
                var customer = _customerRepository.Set().FirstOrDefault(c => c.CustomerID == id);

                if (customer == null)
                {
                    return false;
                }
                else
                {
                    if (_ordersRepository.Set().Any(p => p.CustomerID == id))
                    {
                        return false;
                    }
                    else
                    {
                        _customerRepository.Remove(customer);
                        _customerRepository.SaveChanges();
                        return true;
                    }
                }
            
        }

        public void Save(Customer newCustomer)
        {
            
                var customer = new Customers
                {
                    CustomerID = newCustomer.CustomerID,
                    CompanyName = newCustomer.CompanyName,
                    ContactName = newCustomer.ContactName,
                    ContactTitle = "",
                    Address = "",
                    City = newCustomer.City,
                    Region = "",
                    PostalCode = "",
                    Country = "",
                    Phone = "",
                    Fax = ""

                };

                try
                {
                    _customerRepository.Persist(customer);
                    _customerRepository.SaveChanges();
                  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            

        }
    }
}
