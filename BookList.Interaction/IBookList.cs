using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace BookList.Interaction
{
    [ServiceContract]
    public interface IBookList
    {
        [OperationContract]
        List<DataBookInfo> DownloadAll();
        [OperationContract]
        void AddNewNote(DataBookInfo newNote);
        [OperationContract]
        DataBookInfo FindNoteByISBN(string ISBN);
        [OperationContract]
        SearchingResult[] FindNotesByKeyWords(List<string> KeywordsArray);
        [OperationContract]
        void SaveNotesInFile();
        [OperationContract]
        void Quit();
    }
}