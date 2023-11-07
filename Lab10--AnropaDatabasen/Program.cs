using Lab10__AnropaDatabasen.Data;
using Lab10__AnropaDatabasen.Models.Dbmodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;

namespace Lab10__AnropaDatabasen
{
    internal class Program
    {

        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Välj en funktion:");
                Console.WriteLine("1. Hämta alla kunder");
                Console.WriteLine("2. Visa kund och deras ordrar");
                Console.WriteLine("3. Lägg till kund");
                Console.WriteLine("4. Avsluta");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":

                        string sortingOrder = null;

                        if (sortingOrder != "stigande" && sortingOrder != "fallande")
                        {
                            Console.Clear();
                            Console.WriteLine("Stigande eller fallande ordning?");
                            Console.Write("[stigande/fallande]:");
                            sortingOrder = Console.ReadLine().ToUpper();
                        }

                        allCustomers(sortingOrder);


                        break;

                    case "2":
                        chooseCustomer();
                        break;

                    case "3":
                        addCustomer();
                        break;

                    case "4":
                        Environment.Exit(0);
                        break;

                }

            }

            static void allCustomers(string sortingOrder)
            {
                using (NorthWindContext context = new NorthWindContext())
                {

                    var quary = context.Customers
                        .Select(c => new
                        {
                            ShippedDate = c.Orders.Select(o => o.ShippedDate),
                            c.CompanyName,
                            c.Country,
                            c.Region,
                            c.Phone,
                            OrderCount = c.Orders.Count()
                        });

                    if (sortingOrder == "stigande")
                    {
                        quary = quary.OrderBy(c => c.CompanyName);
                    }
                    if (sortingOrder == "fallande")
                    {
                        quary = quary.OrderByDescending(f => f.CompanyName);
                    }

                    var customers = quary.ToList();

                    foreach (var customer in customers)
                    {
                        int shippedOrders = 0;

                        foreach (var shipDate in customer.ShippedDate)
                        {
                            shippedOrders++;
                        }

                       Console.WriteLine($"Namn: {customer.CompanyName} \nLand: {customer.Country} \nRegion: {customer.Region} \nNummer: {customer.Phone} \nOrdrar: {customer.OrderCount}");
                        Console.WriteLine();
                    }                   
                                     
                }                

            }

            static void chooseCustomer()
            {

                Console.WriteLine("Välj vilken kund du vill visa: ");
                string name = Console.ReadLine();
                Console.WriteLine();

                using (NorthWindContext context = new NorthWindContext())
                {
                    try
                    {
                        var customer = context.Customers
                            .Include(c => c.Orders)
                            .ThenInclude(o => o.OrderDetails)
                                .ThenInclude(p => p.Product)
                        .SingleOrDefault(c => c.CompanyName == name);

                        if (customer != null)
                        {
                            Console.WriteLine($"Namn:{customer.CompanyName}\nKontakt:{customer.ContactName}\nTitel:{customer.ContactTitle}\nAdress:{customer.Address}\nStad:{customer.City}\nRegion:{customer.Region}\nPostkod:{customer.PostalCode}\nLand:{customer.Country}\nTelefonnummer:{customer.Phone}\nFax:{customer.Fax}\nOrdrar:{customer.Orders}");


                        }
                        else
                        {
                            Console.WriteLine("Kunden hittades inte");
                        }
                    }

                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"kunden hittades inte");
                    }
                }
                
            }

            static void addCustomer()
            {
                using (NorthWindContext context = new NorthWindContext())
                {
                    Customer newCustomer = new Customer();

                    Console.WriteLine("Fyll i kunduppgifter:");
                    Console.WriteLine("Skippa ett fält genom att trycka enter");

                    Console.Write($"Företagsnamn: ");
                    newCustomer.CompanyName = Console.ReadLine();

                    Console.Write($"Namn på kontakt: ");
                    newCustomer.ContactName = Console.ReadLine();

                    Console.Write($"Titel: ");
                    newCustomer.ContactTitle = Console.ReadLine();

                    Console.Write($"Adress: ");
                    newCustomer.Address = Console.ReadLine();

                    Console.Write($"Stad: ");
                    newCustomer.City = Console.ReadLine();

                    Console.Write($"Region: ");
                    newCustomer.Region = Console.ReadLine();

                    Console.Write($"Postkod: ");
                    newCustomer.PostalCode = Console.ReadLine();

                    Console.Write($"Land: ");
                    newCustomer.Country = Console.ReadLine();

                    Console.Write($"Telefonnummer: ");
                    newCustomer.Phone = Console.ReadLine();

                    Console.Write($"Fax: ");
                    newCustomer.Fax = Console.ReadLine();

                    if (String.IsNullOrEmpty(newCustomer.CustomerId))
                    {
                        newCustomer.CustomerId = GenerateRandomCustomerId();

                    }

                    context.Customers.Add(newCustomer);
                    context.SaveChanges();

                    Console.WriteLine("kund tillagd");

                }

            }

            static string GenerateRandomCustomerId()
            {
                return Guid.NewGuid().ToString().Substring(0, 5);
            }

        }

    }

}
 
    
