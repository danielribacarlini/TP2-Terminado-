using Services;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            var op = "";
            do
            {
                Console.WriteLine("Menu\n=====================================================================");
                Console.WriteLine("L - Listar \nM - Modificar \nD - Borrar \nC - Nuevo \nK - Mejor Cliente \nJ - Mejor Producto \nS - Salir");

                var band = true;

                do
                {
                    op = Console.ReadLine();
                    if (op != "M" && op != "D" && op != "C" && op != "S" && op != "L" && op != "K" && op != "J")
                    {
                        Console.WriteLine("ingrese valor correcto");
                        band = true;
                    }
                    else
                    {
                        band = false;
                    }
                } while (band);

                switch (op)
                {
                    case "M":
                        Edit();
                        break;
                    case "D":
                        Delete();
                        break;
                    case "C":
                        Create();
                        break;
                    case "L":
                        Listar();
                        break;
                    case "K":
                        ListarMejor();
                        break;
                    case "J":
                        ListaMejorProducto();
                        break;
                    default:
                        break;
                }
            } while (op != "S");
        }

        private static void ListaMejorProducto()
        {
            var orderServices = new OrdenServices();
            var bestProduccts = orderServices.BestProduct();

            foreach (var item in bestProduccts)
            {
                Console.WriteLine($"{item.Country}   {item.Product}");
            }

        }

        private static void ListarMejor()
        {
            var orderServices = new OrdenServices();
            var bestCustomers = orderServices.BestCustomer();

            foreach (var item in bestCustomers)
            {
                Console.WriteLine($"{item.Country}  {item.Name} {item.Total}");
            }
        }

        public static int ReadId()
        {
            
            var id = 0;
            var result = false;

            do
            {
                Console.WriteLine("Ingrese Id Categoria");
                result = Int32.TryParse(Console.ReadLine(), out id);
                if (!result && id <0)
                {
                    Console.WriteLine("ingrese un valor correcto");
                }
            } while (!result && id <0);
            return id;
        }

        public static void Edit()
        {            
            var ordenServices = new OrdenServices();
            var band1 = true;
            Order order;
            Employee employee = null;
            Customer customer = null;
            
            do
            {
                var id = ReadId();
                order = ordenServices.GetOne(id);

                if (order != null)
                {
                    band1 = false;
                    employee = CargaEmployee();
                    customer = CargaCustomer();                       
                    Console.WriteLine("Ingrese nuevo Ship Name");
                    order.ShipName = Console.ReadLine();
                    Console.WriteLine("inrgese nuevo Ship Adress");
                    order.ShipAddress = Console.ReadLine();
                    Console.WriteLine("Ingrese nueva Ship City");
                    order.ShipCity = Console.ReadLine();
                    Console.WriteLine("ingrese ship region");
                    order.ShipRegion = Console.ReadLine();
                    Console.WriteLine("ingrese ship postal code");
                    order.ShipPostalCode = Console.ReadLine();
                    Console.WriteLine("ingrese ship country");
                    order.ShipCountry = Console.ReadLine();

                    var productServices = new ProductsServices();
                    Product product;

                    foreach (var detail in order.Order_Details)
                    {
                        

                        do
                        {
                            Console.WriteLine("ingrese  nombre del producto");
                            var productName = Console.ReadLine();

                            product = productServices.GetOne(productName);

                            if (product != null)
                            {
                                detail.ProductID = product.ProductID;
                                detail.UnitPrice = (decimal)product.UnitPrice;
                            }
                            else
                            {
                                Console.WriteLine("no se encontro el producto");
                            }

                        } while (product == null);

                        Console.WriteLine("ingrese cantidad");

                        short cant;
                        var result = false;
                        do
                        {
                            result = short.TryParse(Console.ReadLine(), out cant);
                            if (cant > 0 && result)
                            {
                                detail.Quantity = cant;
                            }
                            else
                            {
                                Console.WriteLine("ingrese valor correcto");
                            }

                        } while (cant <= 0 || !result);

                        Console.WriteLine("ingrese discount");

                        var result1 = false;
                        float discount;
                        do
                        {
                            result1 = float.TryParse(Console.ReadLine(), out discount);
                            if (discount >= 0 && discount <= 30 && result1)
                            {
                                detail.Discount = discount;
                            }
                            else
                            {
                                Console.WriteLine("ingrese valor correcto");
                            }

                        } while (discount < 0 || discount > 30 || !result1);
                    }

                    //
                   

                }
                else
                {
                    Console.WriteLine("No existe la orden");
                    
                }

            } while (band1);

            ordenServices.Update(order, employee, customer);
        }

        public static void Delete()
        {
            var ordenServices = new OrdenServices();
            var id = ReadId();

            var order = ordenServices.GetOne(id);
            if (order.Employees.Country == "México" || order.Employees.Country == "Francia")
            {
                Console.WriteLine("Np se pueden borrar orden con cliente de este pais");
            }
            else
            {
                if (ordenServices.Delete(id))
                {
                    Console.WriteLine("Cliente Borrado correctamente");
                }
                else
                {
                    Console.WriteLine("No se pudo borrar el cliente");
                }
            }
                           
        }

        public static void Listar()
        {
            var orderServicies = new OrdenServices();
            var orders = orderServicies.GetAll();

            foreach (var item in orders)
            {
                float importe = 0;
                foreach (var detail in item.Order_Details)
                {
                    importe = importe + (((float)detail.UnitPrice - ((float)detail.UnitPrice * detail.Discount)) * (float)detail.Quantity);
                }
                Console.WriteLine($"{item.OrderID} {item.Customers.ContactName} {importe}");
            }
            Console.WriteLine("\n\n");
        }

        public static void Create()
        {
            var orderServices = new OrdenServices();
            var newOrder = new Order();

            var employee = CargaEmployee();
            var customer = CargaCustomer();

            Console.WriteLine("Ingrese nuevo Ship Name");
            newOrder.ShipName = Console.ReadLine();
            Console.WriteLine("inrgese nuevo Ship Adress");
            newOrder.ShipAddress = Console.ReadLine();
            Console.WriteLine("Ingrese nueva Ship City");
            newOrder.ShipCity = Console.ReadLine();
            Console.WriteLine("ingrese ship region");
            newOrder.ShipRegion = Console.ReadLine();
            Console.WriteLine("ingrese ship postal code");
            newOrder.ShipPostalCode = Console.ReadLine();
            Console.WriteLine("ingrese ship country");
            newOrder.ShipCountry = Console.ReadLine();


            var details = new List<Order_Detail>();
            var op = "";
            do
            {            
                var detail = new Order_Detail();
                var productServices = new ProductsServices();
                Product product;

                do
                {
                    Console.WriteLine("ingrese  nombre del producto");
                    var productName = Console.ReadLine();

                    product = productServices.GetOne(productName);

                    if (product != null)
                    {
                        detail.ProductID = product.ProductID;
                        detail.UnitPrice = (decimal)product.UnitPrice;
                    }
                    else
                    {
                         Console.WriteLine("no se encontro el producto");
                    }
                
                } while (product == null);

                Console.WriteLine("ingrese cantidad");

                short cant;
                var result = false;
                do
                {            
                    result = short.TryParse(Console.ReadLine(), out cant);
                    if(cant > 0 && result)
                    {
                        detail.Quantity = cant;
                    }
                    else
                    {
                        Console.WriteLine("ingrese valor correcto");
                    }
                
                } while (cant <= 0 || !result);

                

                var result1 = false;
                float discount;
                do
                {
                    Console.WriteLine("ingrese descuento:");
                    result1 = float.TryParse(Console.ReadLine(), out discount);
                    if(discount >= 0 && discount <= 0.3 && result1)
                    {
                         detail.Discount = discount;
                    }
                    else
                    {
                        Console.WriteLine("ingrese valor correcto");
                    }

                } while (discount < 0 || discount >0.3 || !result1);

                detail.Orders = newOrder;
                detail.Products = product;

                details.Add(detail);

                do
                {
                    Console.WriteLine("Agregar otro ? S / N");
                    op = Console.ReadLine();
                    if (op != "S" && op != "N")
                    {
                        Console.WriteLine("ingrese valor correcto");
                    }
                } while (op != "S" && op != "N");

            } while (op == "S");

            newOrder.Order_Details = details;

            
            var ordenId = orderServices.Save(newOrder, employee, customer);
            float total = 0; 
            foreach (var item in newOrder.Order_Details)
            {
                total = total + (((float)item.UnitPrice - (float)item.UnitPrice * item.Discount) * (float)item.Quantity);
            }
            Console.WriteLine($"Orden {ordenId}, importe tota:{total}  guardada correctamente \n\n");
        }

        public static Employee CargaEmployee()
        {
            var employeeService = new EmployeeServices();
            var employeeName = "";
            var employeeLast = "";

            Employee employee;

            do
            {
                Console.WriteLine("ingrese nombre del empleado");
                employeeName = Console.ReadLine();
                Console.WriteLine("ingrese apellido del empleado");
                employeeLast = Console.ReadLine();

                employee = employeeService.GetOne(employeeName, employeeLast);

                if (employee == null)
                {
                    Console.WriteLine("Empleado no encontrado.");
                }

            } while (employee == null);

            return employee;
        }

        public static Customer CargaCustomer()
        {
            var customerService = new CustomerServices();
            var customerId = "";

            Customer customer;

            do
            {
                Console.WriteLine("ingrese id del cliente");
                customerId = Console.ReadLine();

                customer = customerService.GetOne(customerId);

                if (customer == null)
                {
                    Console.WriteLine("Cliente no encontrado.");
                }

            } while (customer == null);

            return customer;
        }
    }
}

