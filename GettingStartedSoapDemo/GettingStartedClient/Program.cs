using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettingStartedClient.ServiceReference1;

namespace GettingStartedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Create an instance of the WCF proxy.
            FridgeClient client = new FridgeClient();

            // Step 2: Call the service operations.
            // Call the Add service operation.
            Console.WriteLine($"There are {client.Add("apple", 10)} apples.");
            Console.WriteLine($"There are {client.Add("orange", 15)} oranges.");
            Console.WriteLine($"There are {client.Subtract("apple", 5)} apples.");
            Console.WriteLine($"There are {client.Get("pear")} pears.");
            client.Close();
        }
    }
}