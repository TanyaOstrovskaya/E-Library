using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using BookList.Interaction;
using Client;

namespace WPFInteraction
{
    public class MyModel : DependencyObject, IDisposable
    {
        public DelegatedCommand AddBookCommand { get; private set; }
        public DelegatedCommand FindBookByIsbnCommand { get; private set; }
        public DelegatedCommand FindBookByKeywordsCommand { get; private set; }
        public DelegatedCommand SaveCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region DataBookInfo SelectedBook

        public DataBookInfo SelectedBook
        {
            get { return (DataBookInfo)GetValue(SelectedBookProperty); }
            set { SetValue(SelectedBookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBook.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBookProperty =
            DependencyProperty.Register("SelectedBook", typeof(DataBookInfo), typeof(MyModel), new PropertyMetadata(null));

        #endregion

        #region ObservableCollection<DataBookInfo> Books

        public ObservableCollection<DataBookInfo> Books
        {
            get { return (ObservableCollection<DataBookInfo>)GetValue(BooksProperty); }
            set { SetValue(BooksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Books.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BooksProperty =
            DependencyProperty.Register("Books", typeof(ObservableCollection<DataBookInfo>), typeof(MyModel), new PropertyMetadata(null));

        #endregion

        Client.WCFClient _wcfClient;

        public MyModel(WCFClient wCFClient)
        {
            this._wcfClient = wCFClient;

            this.AddBookCommand = new DelegatedCommand(this.AddBookCommandImpl);
            this.FindBookByIsbnCommand = new DelegatedCommand(this.FindBookByIsbnCommandImpl);
            this.FindBookByKeywordsCommand = new DelegatedCommand(this.FindBookByKeywordsCommandImpl);
            this.SaveCommand = new DelegatedCommand(this.SaveCommandImpl);

            this.DownloadAll();
        }

        private void DownloadAll()
        {
            List<DataBookInfo> booklist = _wcfClient.DownloadAll();
            this.Books = new ObservableCollection<DataBookInfo>(booklist);
        }

        private void AddBookCommandImpl(object obj)
        {
            InputBox inputBox = new InputBox();
            inputBox.ShowDialog();
            DataBookInfo newItem = inputBox.newItemFromInputbox;

            if (inputBox.isValid == true)
            {
                _wcfClient.AddNewNote(newItem);
                this.Books.Add(newItem);
            }
            else
            {
                Error errWindow = new Error();
                errWindow.ShowDialog();
                AddBookCommandImpl(null);
            }
        }

        private void FindBookByIsbnCommandImpl(object obj)
        {
            InputISBN inputBox = new InputISBN();
            inputBox.ShowDialog();
            int ISBN = inputBox.ISBNFromInputbox;
            if (ISBN != 0)
            {
                this.Books = new ObservableCollection<DataBookInfo>() {
                    _wcfClient.FindNoteByISBN(ISBN.ToString())
                };
            }
            else
            {
                Error errWindow = new Error();
                errWindow.ShowDialog();
                FindBookByIsbnCommandImpl(null);
            }
        }

        private void FindBookByKeywordsCommandImpl(object obj)
        {
            InputKeyWords inputBox = new InputKeyWords();
            inputBox.ShowDialog();
            var keyWordsList = inputBox.keyWordsFromInputBox;
            
            if (keyWordsList != null)
            {
                this.Books = new ObservableCollection<DataBookInfo>(_wcfClient.FindNotesByKeyWords(keyWordsList));
            }
            else
            {
                Error errWindow = new Error();
                errWindow.ShowDialog();
                FindBookByKeywordsCommandImpl(null);
            }
        }

        private void SaveCommandImpl(object obj)
        {
            _wcfClient.SaveNotesInFile();
        }

        public void Dispose()
        {
            _wcfClient.Quit();
            _wcfClient.Dispose();
        }

    }
}
