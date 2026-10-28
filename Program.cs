using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementSystem
{
    // KLASS 1: Produktmodell
    public class Product
    {
        public required string Category { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }

    // KLASS 2: Hanterare för produkter och LINQ
    public class ProductManager
    {
        private readonly List<Product> products = new List<Product>();

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public List<Product> GetSortedProducts()
        {
            return products.OrderBy(p => p.Price).ToList();
        }

        public decimal CalculateTotal()
        {
            return products.Sum(p => p.Price);
        }

        public List<Product> SearchProducts(string query)
        {
            string lowerQuery = query ?? "";
            return products.Where(p => p.Name.ToLower().Contains(lowerQuery.ToLower()) ||
            p.Category.ToLower().Contains(lowerQuery.ToLower())).ToList();
        }

        public Product? FindExistingProduct(string name)
        {
            return products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    // KLASS 3: Startpunkt och Console UI
    class Program
    {
        private static readonly ProductManager manager = new ProductManager();

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

                string choice = Console.ReadLine() ?? "";

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
                Console.WriteLine("--- Product #" + (manager.GetProducts().Count + 1) + " ---");
                Console.ResetColor();

                // 1. Inmatning av kategori
                Console.Write("Enter Category: ");
                string category = Console.ReadLine()?.Trim() ?? "";
                if (category.ToLower() == "q") break;

                while (string.IsNullOrWhiteSpace(category))
                {
                    ShowError("Category cannot be empty!");
                    Console.Write("Enter Category: ");
                    category = Console.ReadLine()?.Trim() ?? "";
                    if (category.ToLower() == "q") break;
                }
                if (category.ToLower() == "q") break;

                // 2. Inmatning av namn
                Console.Write("Enter Product Name: ");
                string name = Console.ReadLine()?.Trim() ?? "";

                while (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Product name cannot be empty!");
                    Console.Write("Enter Product Name: ");
                    name = Console.ReadLine()?.Trim() ?? "";
                }

                var duplicate = manager.FindExistingProduct(name);
                if (duplicate != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Note: A product with this name already exists in category " + duplicate.Category + "]");
                    Console.ResetColor();
                }

                // 3. Inmatning av pris
                Console.Write("Enter Price: ");
                string priceInput = Console.ReadLine()?.Trim() ?? "";
                decimal price;

                while (!decimal.TryParse(priceInput, out price) || price < 0)
                {
                    ShowError("Invalid format or negative value! Price must be a positive number.");
                    Console.Write("Enter Price: ");
                    priceInput = Console.ReadLine()?.Trim() ?? "";
                }

                // Sparar produkten
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
                    Console.WriteLine(string.Format("{0,-12} | {1,-10} | {2} kr", product.Category, product.Name, product.Price));
                }

                decimal total = manager.CalculateTotal();
                Console.WriteLine(new string('-', 35));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(string.Format("{0,-25} | {1} kr", "Total Price:", total));
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
            string query = Console.ReadLine()?.Trim() ?? "";

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
                    Console.WriteLine(string.Format("{0,-12} | {1,-10} | {2} kr", product.Category, product.Name, product.Price));
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Hjälpmetod för att visa felmeddelanden i röd färg
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + message);
            Console.ResetColor();
        }
    }
}