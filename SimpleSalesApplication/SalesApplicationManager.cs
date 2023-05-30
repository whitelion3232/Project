using System;
using System.Data;
using System.Data.SqlClient;

namespace SimpleSalesApplication
{
    class SalesApplicationManager
    {
        private string connectionString;

        public SalesApplicationManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Run()
        {
            // Prompt user for action
            Console.WriteLine("Welcome to the Sales Application!");
            Console.WriteLine("1. Create Customer");
            Console.WriteLine("2. Create Article");
            Console.WriteLine("3. Create Sales Invoice");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateCustomer();
                    break;
                case 2:
                    CreateArticle();
                    break;
                case 3:
                    CreateSalesInvoice();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Exiting...");
                    break;
            }
        }

        private void CreateCustomer()
        {
            Console.Write("Enter Customer Name: ");
            string customerName = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Customers (Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name",customerName);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Customer created successfully!");
            }
        }

        private void CreateArticle()
        {
            Console.Write("Enter Article Name: ");
            string articleName = Console.ReadLine();
            Console.Write("Enter Price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Articles (Name, Price) VALUES (@Name, @Price)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", articleName);
                    command.Parameters.AddWithValue("@Price", price);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Article created successfully!");
            }
        }

        private void CreateSalesInvoice()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Check if the customer exists
                string query = "SELECT COUNT(*) FROM Customers WHERE Id = @CustomerId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    int count = (int)command.ExecuteScalar();
                    if (count == 0)
                    {
                        Console.WriteLine("Customer does not exist!");
                        return;
                    }
                }

                Console.Write("Enter Invoice Date (YYYY-MM-DD): ");
                DateTime invoiceTime = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Article ID: ");
                int articleId = int.Parse(Console.ReadLine());
                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                // Insert the sales invoice
                query = "INSERT INTO Invoices (CustomerId, InvoiceTime) VALUES (@CustomerId, @InvoiceTime); SELECT SCOPE_IDENTITY()";
                int invoiceId;
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@InvoiceTime", invoiceTime);
                    invoiceId = Convert.ToInt32(command.ExecuteScalar());
                }

                // Insert the invoice item
                query = "INSERT INTO InvoiceItems (InvoiceId, ArticleId, Quantity) VALUES (@InvoiceId, @ArticleId, @Quantity)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    command.Parameters.AddWithValue("@ArticleId", articleId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Sales Invoice created successfully!");
            }
        }
    }
}