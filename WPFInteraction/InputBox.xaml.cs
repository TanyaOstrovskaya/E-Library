using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookList.Interaction;

namespace WPFInteraction
{
    public partial class InputBox : Window
    {
        public InputBox()
        {
            InitializeComponent();
        }
      
        public DataBookInfo newItemFromInputbox;
        public bool isValid = true;


        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            int tempISBN = 0;
            DateTime tempDate = DateTime.Now;
            if ((Int32.TryParse(this.ISBN_text.Text, out tempISBN) == true) &&
                (DateTime.TryParse(this.date_text.Text, out tempDate) == true) &&
                (this.nameText.Text != "") && (this.author_text.Text != "") &&
                (this.annotationText.Text != "") && (this.ISBN_text.Text != "") &&
                (this.date_text.Text != ""))                
            {
                this.newItemFromInputbox = new DataBookInfo(this.nameText.Text, this.author_text.Text,
                                       this.annotationText.Text, tempISBN, tempDate);
                this.isValid = true;                                  
            }
            else
            {              
                this.newItemFromInputbox = null;
                this.isValid = false;
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
