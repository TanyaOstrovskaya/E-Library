using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using BookList.Interaction;
using MyIniFile;

namespace Lab1BookList
{
    class Program
    {
        static void Main(string[] args)
        {            
            IniFileContent file = new IniFileContent(@"C:\Users\user\Google Диск\Lab5\IniFile.txt");
            BookList bookList = new BookList(file);

            var myBinding = new WebHttpBinding();

            //Server server = new Server("http://127.0.0.1:12345/", bookList, file);                      
            //server.DoServer();

            using(var host = new ServiceHost(bookList, new Uri("http://127.0.0.1:12344/")))
            {
                var ep = host.AddServiceEndpoint(typeof(IBookList),  myBinding, "http://127.0.0.1:12344/");

                ep.EndpointBehaviors.Add(new WebHttpBehavior()
                {
                    DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                    DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                    DefaultBodyStyle = WebMessageBodyStyle.Wrapped,
                });

                try
                {
                    host.Open();
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


                Console.WriteLine("Working at");
                foreach (var item in host.Description.Endpoints)
                    Console.WriteLine(item.ListenUri);

                Console.ReadKey();

                host.Close();
            }
        }
    }
}
