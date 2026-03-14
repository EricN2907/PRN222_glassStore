using Programs;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("Enter Reader ID (or press Enter to exit): ");
            string input = Console.ReadLine();

            // Exit program
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Goodbye! Library client is shutting down.");
                break;
            }

            // Validate input
            if (!int.TryParse(input, out int readerId) || readerId <= 0)
            {
                Console.WriteLine("Invalid input! Please enter a valid Reader ID (positive integer).");
                continue;
            }

            try
            {
                using TcpClient client = new TcpClient("127.0.0.1", 3000);
                using NetworkStream stream = client.GetStream();

                // Send ReaderID to server
                byte[] sendData = Encoding.UTF8.GetBytes(readerId.ToString());
                stream.Write(sendData, 0, sendData.Length);

                // Receive response
                byte[] buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                List<BorrowRecord>? records =
                    JsonSerializer.Deserialize<List<BorrowRecord>>(jsonResponse);

                // Process response
                if (records == null || records.Count == 0)
                {
                    Console.WriteLine($"No borrow records found for Reader ID {readerId}.");
                }
                else
                {
                    Console.WriteLine($"=== Borrow History for Reader ID: {readerId}");

                    foreach (var r in records)
                    {
                        Console.WriteLine($"Book ID: {r.BookID}");
                        Console.WriteLine($"Title: {r.Title}");
                        Console.WriteLine($"Author: {r.Author}");
                        Console.WriteLine($"Borrow Date: {r.BorrowDate:yyyy-MM-dd}");

                        if (r.ReturnDate == null)
                            Console.WriteLine("Return Date: Not returned yet");
                        else
                            Console.WriteLine($"Return Date: {r.ReturnDate:yyyy-MM-dd}");

                        Console.WriteLine($"Status: {r.Status}");
                        Console.WriteLine("---");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Library server is not running. Please try again later.");
            }
        }
    }
}