using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using BookList.Interaction;
using System.Web.Script.Serialization;

namespace ClientImplementation
{
    class Client : IBookList
    {
        public string url { get; set; }

        public Client(string url)
        {
            this.url = url;
        }

        public List <DataBookInfo> DownloadAll()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            var request = (HttpWebRequest)WebRequest.Create(this.url.TrimEnd('/') + "/downloadAll");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
          
            var response = request.GetResponse();
            var searchResult = new List<DataBookInfo>();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                searchResult = jss.Deserialize<List<DataBookInfo>>(sr.ReadToEnd());
            }
            return searchResult;
        }

        public void AddNewNote(DataBookInfo newItem)
        {            
            JavaScriptSerializer jss = new JavaScriptSerializer();

            var request = (HttpWebRequest)WebRequest.Create(this.url.TrimEnd('/') + "/add");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                var dataToSerialize = jss.Serialize(newItem);
                sw.Write(dataToSerialize);
            }

            using (var response = request.GetResponse())
            {
                response.Close();
            }
        }

        public DataBookInfo FindNoteByISBN(string ISBN)
        {
            var request = (HttpWebRequest)WebRequest.Create(this.url.TrimEnd('/') + "/searchByISBN");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";         

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {                
                sw.Write(ISBN);
            }
            DataBookInfo searchResult;
            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(responseStream))
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var stringToDeserialize = sr.ReadToEnd();
                    searchResult = jss.Deserialize<DataBookInfo>(stringToDeserialize);
                    responseStream.Flush();
                    responseStream.Close();
                }
            }                
            return searchResult;
        }

        public SearchingResult[] FindNotesByKeyWords(List<string> keywords)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            var request = (HttpWebRequest)WebRequest.Create(this.url.TrimEnd('/') + "/searchByKeywords");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                var dataToSerialize = jss.Serialize(keywords);
                sw.Write(dataToSerialize);
            }

            var response = request.GetResponse();
            var searchResult = new List<SearchingResult>();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                searchResult = jss.Deserialize<List<SearchingResult>>(sr.ReadToEnd());
            }
            return searchResult.ToArray();
        }
       
        public void Quit()
        {
            var request = (HttpWebRequest)WebRequest.Create(this.url.TrimEnd('/') + "/quit");            
        }       

        public void SaveNotesInFile()
        {
            var request = (HttpWebRequest)WebRequest.Create(this.url.TrimEnd('/') + "/save");
            using (request.GetResponse());
        } 
    }
}
