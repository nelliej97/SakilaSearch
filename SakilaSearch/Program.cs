using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

class Program
{
    static void Main()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.Write("Ange skådespelarens förnamn: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Ange skådespelarens efternamn: ");
            string lastName = Console.ReadLine() ?? "";

            string query = @"
            SELECT f.title
            FROM actor a
            JOIN film_actor fa ON a.actor_id = fa.actor_id
            JOIN film f ON fa.film_id = f.film_id
            WHERE a.first_name = @firstName AND a.last_name = @lastName;
        ";

            var connectionString = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sakila;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            var command = connectionString.CreateCommand();
            command.CommandText = query;

            {
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);

                connectionString.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Inga filmer hittades för den skådespelaren.");
                    }
                    else
                    {
                        Console.WriteLine("Filmer:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"- {reader["title"]}");
                        }
                    }
                }
                Console.Write("\nVill du göra en ny sökning? (ja/nej): ");
                string answer = Console.ReadLine() ?? "".ToLower();

                if (answer != "ja")
                {
                    running = false;
                    Console.WriteLine("Avslutar...");
                }

                
            }
        }
    }
}
