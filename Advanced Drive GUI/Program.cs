using Newtonsoft.Json;
using System.Data;
using System.IO.Compression;

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

            //Remove existing stored config details
            FunctionBlock.ListOfAll.Clear(); //Clear all function blocks
            Parameter.ListOfAll.Clear(); //Clear all parameters

            using ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read); //Read the .zip file

            List<ConfigFile> ConfigFiles = new(); //Start a list of config files

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
        /// This method takes a param file and uploads all it's contents to the form.
        /// </summary>
        /// <param name="txtFilePath">The text file to use</param>
        public static void ReadParamFiles(string txtFilePath)
        {
            foreach (Parameter parameter in Parameter.ListOfAll) //For each parameter
            {
                parameter.values.Clear(); //Get rid of any exisiting values
            }

            foreach (string line in File.ReadAllLines(txtFilePath)) //For each line in file
            {
                string firstPart = line.Split('.')[0]; //Get the first part of the line before the '.'
                FunctionBlock? functionBlock = FunctionBlock.ListOfAll.SingleOrDefault(f => f.name == firstPart); //Use that to locate function block
                if (functionBlock is null) //If unable to find
                {
                    continue; //Skip to next line
                }

                string secondPart = line.Split(new char[] { '.', ' ','[' })[1]; //Get the second part of the line between '.' and the ' '
                Parameter? parameter = functionBlock.parameters.SingleOrDefault(f => f.name == secondPart); //Use it to find the correct parameter
                if (parameter is null) //If unable to find
                {
                    continue; //Skip to next line
                }

                string valuePart = line.Split(new char[] { '=', ';' }, StringSplitOptions.TrimEntries)[1]; //Get the actual value stored by the file in between the '=' and ';'

                //This code converts the object into it's corrosponding data type based on the parameter type
                object actualValue = parameter.type switch
                {
                    "string32" => valuePart = valuePart.Trim('"'), //If string, trim quote marks
                    "bool" => bool.Parse(valuePart), //If boolean, convert to bool
                    "float32" => float.Parse(valuePart), //If floating-point number, convert to float
                    "uint32" => uint.Parse(valuePart), //If unsigned integer, convert to uint
                    _ => throw new Exception($"Unknown object type for parameter called {parameter.type}"), //If it's none of those, throw error message
                };

                if (parameter.type == "string32") //If the actual value is a string
                {
                    string text = ((string)actualValue).PadRight(parameter.dimensions*2); //Get the string and pad it out based on it's dimension
                    for (int i = 0; i < parameter.dimensions * 2; i += 2) //For every two characters
                    {
                        string charPair = text.Substring(i, 2); //Get them
                        parameter.values.Add(charPair); //Add each as a value
                        int index = parameter.values.Count - 1; //Get the index of the new added value
                        parameter.textBoxes[index].Text += charPair.ToString(); //Put the value as a string into the appropriate textbox
                    }
                } 
                else
                {
                    parameter.values.Add(actualValue); //Add the actual value to the parameter
                    int index = parameter.values.Count - 1; //Get the index of the added value
                    parameter.textBoxes[index].Text += actualValue.ToString(); //Put the value as a string into the appropriate textbox
                }


            }
        }

        /// <summary>
        /// This function formats parameter names by adding spaces, spliting up numbers and letters, expanding shortened words and sorting out other issues
        /// </summary>
        /// <param name="text">The text we want to format</param>
        /// <returns>The formatted parameter name</returns>
        public static string FormatParameterNames(string text)
        {
            text = string.Concat(text.Select(c => Char.IsUpper(c) ? " " + c : c.ToString())).TrimStart(' ');

            text = AddSpaceBetweenLettersAndNumbers(text);

            text = text.Replace("I D", "ID");
            text = text.Replace("Id", "ID");
            text = text.Replace("Descrip", "Description");
            text = text.Replace(" No", " Number");

            return text;
        }

        /// <summary>
        /// This method takes a string parameter and returns a string with spaces added between letters and numbers.
        /// </summary>
        /// <param name="str">The string to do this to</param>
        /// <returns>The string with spaces between letters and numbers</returns>
        static string AddSpaceBetweenLettersAndNumbers(string str)
        {
            string result = "";
            char? previousChar = null;  //nullable char to keep track of the previous character
            foreach (char currentChar in str) //For each character
            {
                if ((Char.IsLetter(currentChar) && previousChar.HasValue && Char.IsNumber(previousChar.Value)) ||
                    (Char.IsNumber(currentChar) && previousChar.HasValue && Char.IsLetter(previousChar.Value))) //If it is between a letter and number
                {
                    result += " "; //Add a space
                }
                result += currentChar; //Add letter to final result
                previousChar = currentChar;  // update the previous character to the current character
            }
            return result; //Return result
        }


    }
}