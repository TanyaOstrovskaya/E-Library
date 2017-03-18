using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Controls;
using Client;
using BookList.Interaction;

namespace WPFInteraction
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyModel _connectionModel;

        public DelegatedCommand ExitCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            FirstWindow fw = new FirstWindow();
            if (fw.ShowDialog() == true)
            {
                var url = fw.ResultUrl;
                this.DataContext = new MyModel(new WCFClient(url));
            }
            else
            {
                this.Close();
            }

            this.Closed += (sender, ea) => { if (_connectionModel != null) _connectionModel.Dispose(); };

            this.ExitCommand = new DelegatedCommand(o => this.Close());
        }

    }
}
