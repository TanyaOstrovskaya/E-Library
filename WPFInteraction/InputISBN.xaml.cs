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

namespace WPFInteraction
{
    /// <summary>
    /// Логика взаимодействия для InputISBN.xaml
    /// </summary>
    public partial class InputISBN : Window
    {
        public InputISBN()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, TextChangedEventArgs e)
        {
            this.Close();
        }

        public int ISBNFromInputbox;

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            int tempISBN = 0;
            if ((Int32.TryParse(this.ISBN_Text.Text, out tempISBN) == true) && (this.ISBN_Text.Text != ""))
                this.ISBNFromInputbox = tempISBN;
            else
                this.ISBNFromInputbox = 0;            
            this.Close();
        }

        private void cancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
