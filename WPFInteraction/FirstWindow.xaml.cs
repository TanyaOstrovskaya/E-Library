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
    /// Логика взаимодействия для FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : Window
    {
        #region string Host

        public string Host
        {
            get { return (string)GetValue(HostProperty); }
            set { SetValue(HostProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Host.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HostProperty =
            DependencyProperty.Register("Host", typeof(string), typeof(FirstWindow), new PropertyMetadata("localhost"), o => !string.IsNullOrWhiteSpace(o as string));

        #endregion

        #region ushort Port

        public ushort Port
        {
            get { return (ushort)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Port.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(ushort), typeof(FirstWindow), new PropertyMetadata((ushort)12344), o => o is ushort ? true : ushort.Parse(o as string) > 0);

        #endregion

        public string ResultUrl { get; private set; }

        public FirstWindow()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Host))
            {
                this.ResultUrl = $"http://{this.Host}:{this.Port}/";
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                Error errWindow = new Error();
                errWindow.ShowDialog();
            }
        }
    }
}
