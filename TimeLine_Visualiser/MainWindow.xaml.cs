using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeLine_Visualiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string filename;
        private BackgroundWorker worker;
        string username;
        string password;
        string host;
        
        public MainWindow()
        {
            InitializeComponent();
            main = this;
        }


        //Thread for changing the status msg.
        internal static MainWindow main;
        internal string StatusMsg
        {
            get { return Status_msg_box.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { Status_msg_box.Content = value; })); }

        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog file_dialog = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            file_dialog.DefaultExt = ".txt";
           


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = file_dialog.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = file_dialog.FileName;
               BrowseText.Text = filename;
               BtnRun.IsEnabled = true;
            }

        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {

            //Get Database Creds, 
            username = db_usr_box.Text;
            password = db_pass_box.Password;
            host = db_url_box.Text;

            // Creates a background thread so the UI can play it cool.  
            worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;


            //kallar på worker_DoWork metoden. för att få den att jobba, sen sätter vad som ska hända när workern är färdig.
            worker.DoWork += worker_DoWork;

            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();

           //Disable the run button. 
           BtnRun.IsEnabled = false;
           
            //Enable the Cancel button. 
           Btn_cancel.IsEnabled = true;

           

        }



        //Background thread
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            MainWindow.main.StatusMsg = "Worker is starting... ";


            //Send Login Cred and filename to parser
            Events_Parse_Save parse = new Events_Parse_Save();

            parse.CreateCredUri(username,password,host);

            parse.parse(filename);

        }



        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
            //Re Enable Run Btn. 
            BtnRun.IsEnabled = true;
            Btn_cancel.IsEnabled = false;

            MainWindow.main.StatusMsg = "Ready";
        
        }


        //{NOT USED} 
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

           


        }


        //Kill the Worker, cancel operations.
        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync(); //Kill the worker. 


            //Re enable buttons. 
            BtnRun.IsEnabled = true;
            Btn_cancel.IsEnabled = false;
            Events_Parse_Save.Aborted = true;
            MainWindow.main.StatusMsg = "Ready";
            
        
        }


        //About Button.
        private void Btn_about_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();

            about.ShowDialog();
        }


       
       
    }
}
