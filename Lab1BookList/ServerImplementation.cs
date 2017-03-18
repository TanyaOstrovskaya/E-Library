using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Runtime.Serialization.Json;
using MyIniFile;
using System.Web.Script.Serialization;
using BookList.Interaction;

namespace Lab1BookList
{
    public class Server
    {
        public string url { get; private set; }

        public BookList bookList { get; private set; }

        public IniFileContent file { get; private set; }

        const ushort port = 12345;

        public Server(string url, BookList bookList, IniFileContent file)
        {
            this.url = url;
            this.bookList = bookList;
            this.file = file;
        }

        public void DoServer()
        {
            using (var listener = new HttpListener())
            {
                byte menuItemNumber = 0;
                while (menuItemNumber != 6)
                {
                    listener.Prefixes.Add(string.Format("http://{0}:{1}/", "*", port));
                    listener.Start();

                    Console.WriteLine("Waiting for connection.. ");

                    bool IsListening = true;
                    while (IsListening)
                    {
                        HttpListenerContext context = listener.GetContext();                                             
                        switch (context.Request.Url.PathAndQuery)
                        {
                            case "/downloadAll":
                                {
                                    this.DownloadAll(context);
                                    break;
                                }
                            case "/add":
                                {                                    
                                    this.AddNewNote(context);
                                    break;
                                }
                            case "/searchByISBN":
                                {
                                    this.SendNoteByISBN(context);
                                    break;
                                }
                            case "/searchByKeywords":
                                {
                                    this.SendNotesByKeywords(context);
                                    break;
                                }
                            case "/save":
                                {
                                    this.SaveNotesInFile();
                                    break;
                                }
                            case "/quit":
                                {
                                    IsListening = false;
                                    break;
                                }
                            default:
                                break;
                        }
                        Console.WriteLine(new string('-', 70));
                        Console.WriteLine("\nWaiting for connection.. ");
                    }
                }
            }
        }
        
        public List<DataBookInfo> DownloadAll(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            
            JavaScriptSerializer jss = new JavaScriptSerializer();

            var listToResponse = bookList.DownloadAll();
            var dataToResponse = jss.Serialize(listToResponse);
            var streamToResponse = context.Response.OutputStream;
            using (var sw = new StreamWriter(streamToResponse))
            {
                sw.Write(dataToResponse);
                streamToResponse.Flush();
                streamToResponse.Close();
            }
            return listToResponse;
        }

        public void AddNewNote(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;

            if (!request.HasEntityBody)
                return;

            using (var sr = new StreamReader(request.InputStream))
            {
                string dataToDeserialize = sr.ReadToEnd();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var newBook = jss.Deserialize<DataBookInfo>(dataToDeserialize);

                bookList.AddNewNote(newBook);

                Console.WriteLine("Added book {0}", newBook.name);
            }         
            context.Response.Close();
        }

        public void SendNoteByISBN(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            int ISBN = 0;
            using (var sr = new StreamReader(request.InputStream))
            {
                ISBN = Int32.Parse(sr.ReadToEnd());
            }
            var bookToResponse = bookList.FindNoteByISBN(ISBN.ToString());

            JavaScriptSerializer jss = new JavaScriptSerializer();
            var dataToResponse = jss.Serialize(bookToResponse);
            var responseStream = context.Response.OutputStream;
            var dataToResponseBytes = Encoding.UTF8.GetBytes(dataToResponse);
            responseStream.Write(dataToResponseBytes,0, dataToResponse.Length) ;
            responseStream.Flush();
           
            context.Response.Close();
        }

        public void SendNotesByKeywords(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            List<string> keywords = new List<string>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            using (var sr = new StreamReader(request.InputStream))
            {
                keywords = jss.Deserialize<List<string>>(sr.ReadToEnd());
            }
            var listToResponse = bookList.FindNotesByKeyWords(keywords);
            var dataToResponse = jss.Serialize(listToResponse);
            var streamToResponse = context.Response.OutputStream;
            using (var sw = new StreamWriter(streamToResponse))
            {
                sw.Write(dataToResponse);
                streamToResponse.Flush();
                streamToResponse.Close();
            }          
        }
                
        public void SaveNotesInFile()
        {
            bookList.SaveNotesInFile();
        }        
    }
}  



