using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SignalR.Client.Hubs;

namespace SignalRGroupSample.ServerClient {
    class Program {

        static void Main(string[] args) {

            var connection = new HubConnection("http://localhost:9669/");

            var myHub = connection.CreateProxy("SignalRGroupSample.Hubs.Chat");
            
            myHub.On("addMessage", data => Console.WriteLine(data));

            connection.Start().Wait();

            myHub.Invoke("Send", "Hi from server...").ContinueWith(task => {
                if (task.IsFaulted) {
                    Console.WriteLine("An error occurred during the method call {0}", task.Exception.GetBaseException());
                } else {
                    Console.WriteLine("Successfully called MethodOnServer");
                }
            });

            Console.WriteLine("connection started...");
            Console.ReadLine();
        }
    }
}
