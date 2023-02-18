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
        /// This method runs when the Sign In/Out button is clicked
        /// </summary>
        /// <param name="sender">Sign In/Out Button</param>
        /// <param name="e">Event arguments</param>
        private void SignInOutButtonClicked(object sender, EventArgs e)
        {
            Program.ShowPasswordPopUp();//Show password pop up
        }

        /// <summary>
        /// Show or hide all options avialable to developers
        /// </summary>
        public void ToggleDeveloperModeOptions(bool isDeveloperModeOn)
        {
            uploadButton.Visible = isDeveloperModeOn; //Make upload button appear/disapear depending
        }


    }
}