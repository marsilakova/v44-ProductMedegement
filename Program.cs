using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementSystem
{
    // ==========================================
    // KLASS 1: Produktmodell (Datastruktur)
    // ==========================================
    public class Product
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // ==========================================
    // KLASS 2: Hanterare för produkter och LINQ
    // ==========================================
    public class ProductManager
    {
        private List<Product> products = new List<Product>();

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        // LINQ: OrderBy() - Sorterar produkter efter pris (lägsta till högsta)
        public List<Product> GetSortedProducts()
        {
            return products.OrderBy(p => p.Price).ToList();
        }

        // LINQ: Sum() - Beräknar det totala priset
        public decimal CalculateTotal()
        {
            return products.Sum(p => p.Price);
        }

        // LINQ: Where() - Söker efter produkter baserat på text
        public List<Product> SearchProducts(string query)
        {
            string lowerQuery = query.ToLower();
            return products.Where(p => p.Name.ToLower().Contains(lowerQuery) ||
            p.Category.ToLower().Contains(lowerQuery)).ToList();
        }

        // LINQ: FirstOrDefault() - Kontrollerar om en produkt med samma namn redan finns
        public Product FindExistingProduct(string name)
        {
            return products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    // ==========================================
    // KLASS 3: Startpunkt och Console UI
    // ==========================================
    class Program
    {
        private static ProductManager manager = new ProductManager();

        static void Main(string[] args)
        {
            Console.Title = "Product Management System - Level 3";
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("");
                Console.WriteLine("---- PRODUCT LIST APPLICATION (Level 3) ----");
                Console.WriteLine("");
                Console.ResetColor();
                Console.WriteLine("1. Add Products");
                Console.WriteLine("2. View Sorted Products & Total");
                Console.WriteLine("3. Search Product");
                Console.WriteLine("4. Exit");
                Console.Write("\nChoose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RunInputLoop();
                        break;
                    case "2":
                        ShowProductTable();
                        break;
                    case "3":
                        RunSearch();
                        break;
                    case "4":
                        keepRunning = false;
                        break;
                    default:
                        ShowError("Invalid choice! Press any key to try again...");
                        break;
                }
            }
        }

        // Inmatningsloop för produkter (dynamisk, avslutas med 'q')
        private static void RunInputLoop()
        {
            Console.Clear();
            Console.WriteLine("To stop entering products and return to menu, type 'q' at Category.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($`"--- Product #{manager.GetProducts().Count + 1} ---");
                Console.ResetColor();

                // 1. Inmatning av kategori
                Console.Write("Enter Category: ");
                string category = Console.ReadLine()?.Trim();
                if (category?.ToLower() == "q") break;

                while (string.IsNullOrWhiteSpace(category))
                {
                    ShowError("Category cannot be empty!");
                    Console.Write("Enter Category: ");
                    category = Console.ReadLine()?.Trim();
                    if (category?.ToLower() == "q") break;
                }
                if (category?.ToLower() == "q") break;

                // 2. Inmatning av namn (Felhantering: Tomt produktnamn)
                Console.Write("Enter Product Name: ");
                string name = Console.ReadLine()?.Trim();

                while (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Product name cannot be empty!");
                    Console.Write("Enter Product Name: ");
                    name = Console.ReadLine()?.Trim();
                }

                // Valfritt: använder FirstOrDefault() för att varna om dubbletter
                var duplicate = manager.FindExistingProduct(name);
                if (duplicate != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(`$"[Note: A product with this name already exists in category '{duplicate.Category}']");
                    Console.ResetColor();
                }

                // 3. Inmatning av pris (Felhantering: Ogiltigt pris och negativa värden)
                Console.Write("Enter Price: ");
                string priceInput = Console.ReadLine()?.Trim();
                decimal price;

                // Demonstration av try-catch för att uppfylla kraven på felhantering
                while (true)
                {
                    try
                    {
                        if (!decimal.TryParse(priceInput, out price))
                        {
                            throw new FormatException("Invalid format! Price must be a number.");
                        }
                        if (price < 0)
                        {
                            throw new ArgumentOutOfRangeException("Price cannot be negative values!");
                        }
                        break; // Om allt är korrekt, avsluta valideringsloopen för priset
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                        Console.Write("Enter Price: ");
                        priceInput = Console.ReadLine()?.Trim();
                    }
                }

                // Lägger till produkten i hanteraren
                manager.AddProduct(new Product { Category = category, Name = name, Price = price });

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product added successfully!\n");
                Console.ResetColor();
            }
        }

        // Visar tabell med sortering via OrderBy() och totalsumma via Sum()
        private static void ShowProductTable()
        {
            Console.Clear();
            var sortedList = manager.GetSortedProducts();

            Console.WriteLine("----- PRODUCT LIST (Sorted by Price) -----");
            Console.WriteLine();

            if (!sortedList.Any())
            {
                Console.WriteLine("No products found. Please add some products first.");
            }
            else
            {
                foreach (var product in sortedList)
                {
                    Console.WriteLine($`"{product.Category,-12} | {product.Name,-10} | {product.Price} kr");
                }

                decimal total = manager.CalculateTotal();
                Console.WriteLine(new string('-', 35));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(`$"{"Total Price:",-25} | {total} kr");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Sökfunktion med LINQ Where()
        private static void RunSearch()
        {
            Console.Clear();
            Console.WriteLine("----- SEARCH PRODUCTS -----");
            Console.Write("Enter search term (name or category): ");
            string query = Console.ReadLine()?.Trim();

            var results = manager.SearchProducts(query);

            Console.WriteLine("\n----- SEARCH RESULTS -----");
            if (!results.Any())
            {
                Console.WriteLine("No matching products found.");
            }
            else
            {
                foreach (var product in results)
                {
                    Console.WriteLine($`"{product.Category,-12} | {product.Name,-10} | {product.Price} kr");
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Hjälpmetod för att visa felmeddelanden i röd färg
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(`$"Error: {message}");
            Console.ResetColor();
        }
    }
}