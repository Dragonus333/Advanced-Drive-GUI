using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using static Advanced_Drive_GUI.Program;

namespace Advanced_Drive_GUI
{
    public partial class MainForm : System.Windows.Forms.Form
    {

        /// <summary>
        /// This is the method that initialize the form
        /// </summary>
        public MainForm()
        {
            InitializeComponent(); //Intializes all the components of the form
        }

        /// <summary>
        /// This method runs when the connect/disconnect drive button is clicked
        /// </summary>
        /// <param name="sender">Connect/Disconnect Button</param>
        /// <param name="e">Event arguments</param>
        private void ConnectDriveButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method runs when the Sign In/Out button is clicked.
        /// It shows the password popup
        /// </summary>
        /// <param name="sender">Sign In/Out Button</param>
        /// <param name="e">Event arguments</param>
        private void SignInOutButtonClicked(object sender, EventArgs e)
        {
            Program.DoSignInProcess();//Show password pop up
        }

        /// <summary>
        /// Show or hide all options avialable to developers
        /// </summary>
        public void ToggleDeveloperModeOptions(bool isDeveloperModeOn)
        {
            uploadButton.Visible = isDeveloperModeOn; //Make upload button appear/disapear depending
        }

        /// <summary>
        /// The method runs when the upload button is clicked.
        /// </summary>
        /// <param name="sender">The Upload Button</param>
        /// <param name="e">Event arguments</param>
        private void UploadZip(object sender, EventArgs e)
        {
            uploadButton.Enabled = false; //Disable button

            DialogResult response = openFileDialog.ShowDialog(); //Show file open dialog

            if (response != DialogResult.OK) //User didn't get file
            {
                MessageBox.Show("File not found. Please select a file.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell user
                uploadButton.Enabled = true; //Undisable button
                return;  //Exit method
            }

            string zipFilePath = openFileDialog.FileName; //Get the path to the zip file

            ReadConfigFiles(zipFilePath); //Attempt to upload the files

            uploadButton.Enabled = true; //Undisable button

        }

        
    }
}