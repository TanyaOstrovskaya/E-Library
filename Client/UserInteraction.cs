using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookList.Interaction;

namespace Client
{
    public class MainMenu
    {
        IBookList _client;

        public MainMenu()
        {
#if DEBUG
            _client = new WCFClient("http://127.0.0.1:12345/") ;
#else
            _client = new WCFClient(EnterUrl());
#endif
            }

        public static string EnterUrl()
        {
            Console.Write("Url to request: ");
            var tempUrl = Console.ReadLine();

            if (!tempUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                tempUrl = "http://" + tempUrl;

            return tempUrl;
        }
            
        public void MainDisplay()
        {
            Console.WriteLine("\nMENU\n_________________________________________");            
            Console.WriteLine("PRESS '1' to add new note");
            Console.WriteLine("PRESS '2' to display information about book by ISBN");
            Console.WriteLine("PRESS '3' to search notes by keywords");
            Console.WriteLine("PRESS '4' to save notes in file");
            Console.WriteLine("PRESS '5' to quit");
        }
        public byte InputMenuItem()
        {
            byte key = 0;
            bool isNumberValid = false;
            while (!(isNumberValid))
            {
                if (Byte.TryParse(Console.ReadLine(), out key) == false)
                { 
                    Console.WriteLine("Input Error. Please, try again.");
                }
                else isNumberValid = true;
            }
            return key;
        }

        public void AddNote()
        {
            Console.WriteLine("\n1: ADD new note\n_________________________________________");

            Console.WriteLine("Enter fields of new note:\n");
            _client.AddNewNote(UserInteraction.EnterNote());            
        }

        public DataBookInfo BookDataByISBN()
        {
            Console.WriteLine("\n2: Databook by ISBN\n_________________________________________");
           
            DataBookInfo result = _client.FindNoteByISBN(UserInteraction.EnterISBN().ToString());    
                                
            if (result == null)
                Console.WriteLine("No book with this ISBN");
            else
                UserInteraction.DisplayDataBook(result, false);

            return result;
        }

        public SearchingResult[] FindBooksByKeywords()
        {
            Console.WriteLine("\n3: FIND books\n_________________________________________");
       
            SearchingResult[] results = _client.FindNotesByKeyWords(UserInteraction.EnterKeyWords());
            UserInteraction.DisplaySearchingResults(results.ToArray());

            return results.ToArray();
        }

        public void SaveNotesInFile()
        {
            Console.WriteLine("\n4: SAVE notes in file\n_________________________________________");
            _client.SaveNotesInFile();
        }

        public void Quit()
        {
            Console.WriteLine("\n5: END work\n_________________________________________");
            _client.Quit();
        }
    }

    static public class UserInteraction
    {
        static public DataBookInfo EnterNote()
        {
            Console.WriteLine("Enter name of book:\t");
            string newname = Console.ReadLine();
            Console.WriteLine("Enter author:\t");
            string newauthor = Console.ReadLine();
            int newISBN = EnterISBN();
            Console.WriteLine("Enter annotation:\t");
            string newannotation = Console.ReadLine();
            DateTime newpublicationDate = EnterDate();
            return new DataBookInfo(newname, newauthor, newannotation, newISBN, newpublicationDate);             
          
        }

        static public int EnterISBN()  
        {
            int ISBN = 0;

            Console.WriteLine("Enter ISBN of book: ");
            while (Int32.TryParse(Console.ReadLine(), out ISBN) == false)
            {
                Console.WriteLine("Input Error. Please, try again.");
                Console.WriteLine("Enter ISBN of book:\t");
            }

            return ISBN;
        }

        static public DateTime EnterDate()      //ввод и проверка даты
        {
            DateTime date = default (DateTime);

            Console.WriteLine("Enter date of publication: ");
            while (DateTime.TryParse(Console.ReadLine(), out date) == false)
            {
                Console.WriteLine("Input Error. Please, try again.");
                Console.WriteLine("Enter publication date: ");
            }

            return date;
        }

        static public void DisplayDataBook(DataBookInfo Node, bool isSearchResult) 
        {
            if (Node == null)
                Console.WriteLine("No book with this data");
            else
            {
                if (!isSearchResult)
                    Console.WriteLine("\nINFORMATION ABOUT BOOK WITH ISBN = {0}", Node.ISBN);
                else
                    Console.WriteLine("\nINFORMATION ABOUT DETECTED BOOK");
                Console.WriteLine("_________________________________________");
                Console.WriteLine("Name of book:\t\t{0}", Node.name);
                Console.WriteLine("Author:\t\t\t{0}", Node.author);
                if (!isSearchResult)
                    Console.WriteLine("Annotation:\t\t{0}", Node.annotation);
                Console.WriteLine("ISBN:\t\t\t{0}", Node.ISBN);
                Console.WriteLine("Publication date:\t{0}", Node.publicationDate.ToShortDateString());
            }
        }

        static public List<string> EnterKeyWords()
        {
            string nextWord;
            var KeywordsList = new List<string>();

            Console.WriteLine("Enter keywords; after every keyword press ENTER; press 'S' to stop:");
            while (true)
            {
                nextWord = Console.ReadLine();
                if ((nextWord == "S") || (nextWord == "s"))
                    break;
                KeywordsList.Add(nextWord);
            }

            return KeywordsList;
        }

        static public void DisplaySearchingResults(SearchingResult[] arrayWithResults)
        {
            if ((arrayWithResults.Count() == 0) || (arrayWithResults == null))
            {
                Console.WriteLine("No notes with your keywords.");
            }
            else
            {
                int maxInclusion = -1;
                foreach (var item in arrayWithResults)
                {
                    if (item.count > maxInclusion)
                        maxInclusion = item.count;
                }
                Array.Sort(arrayWithResults);
                Array.Reverse(arrayWithResults);
                foreach (var item in arrayWithResults)
                {
                    DisplayDataBook(item, true);
                    if (item.isFromAnnotation == true)
                        Console.WriteLine("\n//Keywords were in annotation//\n");
                }
            }
        }

    }
}
