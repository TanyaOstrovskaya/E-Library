using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;


namespace ClientImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu menuItems = new MainMenu();
            byte k = 0;
            while (k != 5)
            {
                menuItems.MainDisplay();
                k = menuItems.InputMenuItem();
                switch (k)
                {                    
                    case 1:
                        menuItems.AddNote();
                        break;
                    case 2:
                        menuItems.BookDataByISBN();
                        break;
                    case 3:
                        menuItems.FindBooksByKeywords();
                        break;
                    case 4:
                        menuItems.SaveNotesInFile();
                        break;
                    case 5:
                        menuItems.Quit();
                        break;
                    default:
                        Console.WriteLine("Input Error. Please, try again.");
                        menuItems.MainDisplay();
                        break;
                }
            }
        }       
    }    
}
