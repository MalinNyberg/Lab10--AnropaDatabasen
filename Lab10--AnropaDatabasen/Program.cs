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

                switch (choice) //This switch statement is used to enable the user to select different options based on their input.
                {               // In this case, the switch statement is used to handle the user's menu choices and direct the program to different fmethods depending on the selected option.
                    case "1":

                        string sortingOrder = null;
                        //Asking the user to choose between decending or acending order
                        if (sortingOrder != "stigande" && sortingOrder != "fallande")
                        {
                            Console.Clear();
                            Console.WriteLine("Stigande eller fallande ordning?");
                            Console.Write("[stigande/fallande]:");
                            sortingOrder = Console.ReadLine().ToUpper();
                        }

                        allCustomers(sortingOrder); //Calls the method to show all customers


                        break;

                    case "2":
                        chooseCustomer(); //calls the method for the user to choose a customer
                        break;

                    case "3":
                        addCustomer(); // calls the method for the user to add a customer
                        break;

                    case "4":
                        Environment.Exit(0); // exits the program
                        break;

                }

            }
            //Method to show all customers and their orders
            static void allCustomers(string sortingOrder)
            {
                using (NorthWindContext context = new NorthWindContext())
                {
                    //Create a query to retrieve customer data including their orders from the database
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

                    //this if-statements checks if the user chooses either "stigande" or "fallande" sorting of the customers and the runs the if-statement by choice
                    if (sortingOrder == "stigande")
                    {
                        quary = quary.OrderBy(c => c.CompanyName);  //Sort in ascending order
                    }
                    if (sortingOrder == "fallande")
                    {
                        quary = quary.OrderByDescending(f => f.CompanyName); //sort in decending order
                    }

                    var customers = quary.ToList(); 
                
                    foreach (var customer in customers) 
                    {
                        int shippedOrders = 0;
                     //Count the number of shipped orders
                        foreach (var shipDate in customer.ShippedDate)
                        {
                            shippedOrders++;
                        }
                        //Writes out the customer information
                       Console.WriteLine($"Namn: {customer.CompanyName} \nLand: {customer.Country} \nRegion: {customer.Region} \nNummer: {customer.Phone} \nOrdrar: {customer.OrderCount}");
                        Console.WriteLine();
                    }                   
                                     
                }                

            }
            //Method for the user to choose a specific customer and their orders 
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
                            .ThenInclude(o => o.OrderDetails)  //Include method to specify which related data to retrieve from database along with the customers
                                .ThenInclude(p => p.Product)
                        .SingleOrDefault(c => c.CompanyName == name); //filters the customers and selects a single customer whose company name matches the value in the name variable

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
            //Method for the user to add a new customer
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

                    if (String.IsNullOrEmpty(newCustomer.CustomerId)) //if-statement that gives the new customer a random Customer Id.
                    {
                        newCustomer.CustomerId = GenerateRandomCustomerId();

                    }

                    context.Customers.Add(newCustomer); //Adds and saves the new customer
                    context.SaveChanges();

                    Console.WriteLine("kund tillagd");

                }

            }
            //method to make a random generate for the cusomer id. 
            static string GenerateRandomCustomerId()
            {
                return Guid.NewGuid().ToString().Substring(0, 5);
            }

        }

    }

}
 
    
