using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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
    /// Логика взаимодействия для InputKeyWords.xaml
    /// </summary>
    public partial class InputKeyWords : Window
    {
        public InputKeyWords()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
               
        public List<string> keyWordsFromInputBox; 

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.keyword_Text.Text != "")            
                this.keyWordsFromInputBox = this.keyword_Text.Text.Split(new char[] { ' ' }).ToList<string>();            
            else
                this.keyWordsFromInputBox = null;            
            this.Close();
        }
    }
}
