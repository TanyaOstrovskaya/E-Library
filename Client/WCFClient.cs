using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using BookList.Interaction;

namespace Client
{
    public class WCFClient : IBookList, IDisposable
    {
        class ClientImpl : ClientBase<IBookList>
        {
            public IBookList Proxy { get { return this.Channel; } }

            public ClientImpl(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
            {
                var endpointBehaviour = new WebHttpBehavior()
                {
                    DefaultBodyStyle = WebMessageBodyStyle.Wrapped,
                    DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                    DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                };
                this.ChannelFactory.Endpoint.Behaviors.Add(endpointBehaviour);
            }           
        }

        readonly ClientImpl _client;
        readonly IBookList _proxy;

        public WCFClient(string url)
        {
            var myBinding = new WebHttpBinding();
            var myEndpoint = new EndpointAddress(url);

            _client = new ClientImpl(myBinding, myEndpoint);
            _client.Open();
            _proxy = _client.Proxy;
        }

        public List<DataBookInfo> DownloadAll()
        {
            return _proxy.DownloadAll();
        }

        public void AddNewNote(DataBookInfo newNote)
        {
            _proxy.AddNewNote(newNote); 
        }

        public DataBookInfo FindNoteByISBN(string ISBN)
        {
            return _proxy.FindNoteByISBN(ISBN);
        }

        public SearchingResult[] FindNotesByKeyWords(List<string> KeywordsArray)
        {
            return _proxy.FindNotesByKeyWords(KeywordsArray);
        }

        public void SaveNotesInFile()
        {
            _proxy.SaveNotesInFile();
        }

        public void Quit()
        {
            _proxy.Quit();
        }

        public void Dispose()
        {
            _client.Close();
        }
    }
}
