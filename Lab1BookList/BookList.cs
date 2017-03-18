using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using BookList.Interaction;
using MyIniFile;

namespace Lab1BookList
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BookList : IEnumerable<DataBookInfo>, IBookList
    {
        List<DataBookInfo> _booklist;
        IniFileContent file;

        public BookList(IniFileContent file)
        {
            _booklist = new List<DataBookInfo>();
            this.file = file;
        }

        //public List<DataBookInfo> DowlloadAll()
        //{
        //    return _booklist;
        //}

        public void AddNewNote(DataBookInfo newNote)
        {
            _booklist.Add(newNote);
        }

        public DataBookInfo FindNoteByISBN(string ISBNStr)
        {
            var ISBN = int.Parse(ISBNStr);

            foreach (var item in _booklist)
            {
                if (item.ISBN == ISBN)
                {
                    return item;
                }
            }
            return null;
        }

        public SearchingResult[] FindNotesByKeyWords(List<string> KeywordsArray)
        {
            var ListWithResults = new List<SearchingResult>();
            int countInNode = 0;
            int countInAnnot = 0;
            foreach (var item in _booklist) 
            {
                foreach (var word in KeywordsArray) 
                {
                    countInNode += CountWordInNote(item, word);
                    countInAnnot += CountWordInAnnotation(item, word);
                }
                if (countInAnnot + countInNode > 0) 
                {
                    SearchingResult nextNode = new SearchingResult(item.name, item.author, item.annotation, item.ISBN, item.publicationDate,
                    countInAnnot + countInNode,
                    countInAnnot > 0); 
                    ListWithResults.Add(nextNode);
                }
                countInAnnot = 0;
                countInNode = 0;
            }
            return ListWithResults.ToArray();
        }

        public int CountWordInNote(DataBookInfo node, string Keyword)
        {
            int count = 0, index = 0;
            while ((index = node.name.IndexOf(Keyword, index) + 1) != 0)
                count++;
            index = 0;
            while ((index = node.author.IndexOf(Keyword, index) + 1) != 0)
                count++;
            return count;
        }

        public int CountWordInAnnotation(DataBookInfo node, string Keyword) 
        {
            int count = 0, index = 0;
            while ((index = node.annotation.IndexOf(Keyword, index) + 1) != 0)
                count++;
            return count;
        }

        public void SaveNotesInFile()
        {
            int count = 0;
            foreach (var book in _booklist)
            {
                if (book != null)
                {
                    count++;
                    file.AddSection("Book " + book.ISBN.ToString());
                    file.AddKeyValue("name", book.name);
                    file.AddKeyValue("author", book.author);
                    file.AddKeyValue("annotation", book.annotation);
                    file.AddKeyValue("ISBN", book.ISBN.ToString());
                    file.AddKeyValue("publication_date", book.publicationDate.ToString());
                }                
            }
            file.SaveToSourceFile();
        }    

        public List<DataBookInfo> DownloadAll()
        {
            IniFileKeyValueLine currLine = null;
            short countFilelds = 0;
            string currName = null;
            string currAuthor = null;
            int currISBN = 0;
            string currAnnotation = null;
            DateTime currPublDate = DateTime.Today;

            _booklist.Clear();

            file = file.ReadFromFile(file.SourceName);
            //foreach (var line in file)
            //{
            //    if (line.)
            //}


            foreach (var line in file)
            {
                if (line is IniFileKeyValueLine)
                {
                    currLine = (IniFileKeyValueLine)line;
                    if (currLine.Key == "name")
                    {
                        countFilelds++;
                        currName = currLine.Value;
                    }
                    if (currLine.Key == "author")
                    {
                        countFilelds++;
                        currAuthor = currLine.Value;
                    }
                    if (currLine.Key == "annotation")
                    {
                        countFilelds++;
                        currAnnotation = currLine.Value;
                    }
                    if (currLine.Key == "ISBN")
                    {
                        countFilelds++;
                        currISBN = Int32.Parse(currLine.Value);
                    }
                    if (currLine.Key == "publication_date")
                    {
                        countFilelds++;
                        currPublDate = DateTime.Parse(currLine.Value);
                    }
                }
                if (countFilelds == 5)
                {
                    countFilelds = 0;
                    _booklist.Add(new DataBookInfo(currName, currAuthor, currAnnotation, currISBN, currPublDate));
                }
            }
            return _booklist;
        }

        public void Quit()
        {
        }

        public IEnumerator<DataBookInfo> GetEnumerator()
        {
            return _booklist.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _booklist.GetEnumerator();
        }     
    }
}
