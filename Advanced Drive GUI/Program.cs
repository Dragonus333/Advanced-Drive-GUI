using Newtonsoft.Json;
using System.IO.Compression;
using System.Windows.Forms;

namespace Advanced_Drive_GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main form of the program
        /// </summary>
        private static readonly MainForm mainForm = new(); //Creating the main form

        /// <summary>
        /// A flag for if the devloper mode has been unlocked before whilst the program is running
        /// </summary>
        public static bool DeveloperModeUnlocked { get; private set; }

        /// <summary>
        /// The boolean for wheather the developer mode is on or not.
        /// </summary>
        private static bool developerModeOn;
        public static bool DeveloperModeOn
        {
            get
            {
                return developerModeOn; //Return developerModeOn
            }
            private set
            {
                developerModeOn = value; //Change the value
                mainForm?.ToggleDeveloperModeOptions(value); //Toggle the options accordingly
            }
        }


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize(); //Boiler Plate code
            Application.Run(mainForm); //Running the form
        }

        public static void DoSignInProcess()
        {
            //If developer mode has already been unlocked
            if (DeveloperModeUnlocked)
            {
                DeveloperModeOn = !DeveloperModeOn; //Turn dev mode on/off
                return; //Skip the rest. No need to reconfirm
            }

            DialogResult dialogResult = CreatePasswordPopUp(out TextBox textBox).ShowDialog(); //Show the form

            string userInput = textBox.Text; //Get the user input

            string PASSWORD = "password"; //This is the password the user must enter

            if (dialogResult == DialogResult.OK) //If the user clicked ok
            {
                if (userInput == PASSWORD) //If the user result is exactly the same as the password
                {
                    DeveloperModeUnlocked = true; //They have unlocked developer mode
                    MessageBox.Show("Developer Mode Unlocked"
                    , "Correct Password", MessageBoxButtons.OK, MessageBoxIcon.Warning); //Tell the user 
                    DeveloperModeOn = true; //Dev mode is now on
                    //Update all
                }
                else //If the user got the password wrong
                {
                    MessageBox.Show("The password is incorrect"
                    , "Wrong Password", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell the user
                     //The the developer option is not checked
                }

            }
            else //If the user quit in some way
            {
                //The the developer option is not checked
            }
        }

        /// <summary>
        /// This function creates the password input form and returns it.
        /// </summary>
        /// <param name="textBox">The textbox we will get the answer from</param>
        /// <returns>The created form</returns>
        private static Form CreatePasswordPopUp(out TextBox textBox)
        {
            //Password Label
            Label label = new();
            label.SetBounds(11, 20, 87, 26);
            label.AutoSize = true;
            label.Text = "Password:";

            //Input textbox
            textBox = new TextBox();
            textBox.SetBounds(102, 20, 87, 26);

            //OK button
            Button buttonOk = new();
            buttonOk.SetBounds(11, 60, 87, 35);
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;

            //Cancel Button
            Button buttonCancel = new();
            buttonCancel.SetBounds(100, 60, 87, 35);
            buttonCancel.Text = "Cancel";
            buttonCancel.DialogResult = DialogResult.Cancel;

            //The form itself
            Form form = new()
            {
                ClientSize = new Size(200, 110),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false,

                Text = "Sign In",
                AcceptButton = buttonOk,
                CancelButton = buttonCancel
            };
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel }); //Add the controls

            return form;
        }

        /// <summary>
        /// This method takes the zip file and converts all it's contents into c# objects.
        /// </summary>
        /// <param name="zipFilePath">The zip file to use</param>
        public static void ReadConfigFiles(string zipFilePath)
        {

            using ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read); //Read the .zip file

            List<ConfigFile> ConfigFiles = new(); //Start a list of config files
            ConfigFile blockIDFile = new();

            foreach (ZipArchiveEntry jsonFile in zip.Entries) //For each json file in there
            {
                string jsonText = new StreamReader(jsonFile.Open()).ReadToEnd(); //Get the text from the file

                ConfigFile? configFile; //Create a config file object
                try
                {
                    configFile = JsonConvert.DeserializeObject<ConfigFile>(jsonText); //Turn the json into a C# object called ConfigFile
                }
                catch (JsonException) //If there is a problem with the file
                {
                    MessageBox.Show("Please upload a .zip file with the correct files.", "Zip File Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell user
                    return; //Exit
                }
                
                if (configFile != null) //If the config file has been created correctly
                {
                    ConfigFiles.Add(configFile); //Add it to list
                }

            }

            mainForm.ConvertConfigFilesToTabs(ConfigFiles); //Convert the files into user interface
        }

        /// <summary>
        /// This function adds spaces before capital letters
        /// </summary>
        /// <param name="text">The text we want to make readable by adding spaces</param>
        /// <returns>The more readabe and spaced out text</returns>
        public static string AddSpaces(string text)
        {
            //TODO expand this function
            //Ideas:
            // - Numbers go together
            // - No is replaced with Number
            // - Descrip is replaced with full word
            // - I D is put togther 
            // - Id is capitalised?
            return string.Concat(text.Select(c => Char.IsUpper(c) || Char.IsDigit(c) ? " " + c : c.ToString())).TrimStart(' ');
        }

        
    }
}