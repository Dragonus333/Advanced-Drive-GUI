using static Advanced_Drive_GUI.Program;

namespace Advanced_Drive_GUI
{
    public partial class MainForm : Form
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
            uploadParambutton.Visible = isDeveloperModeOn; //Make param button appear/disapear depending
        }

        /// <summary>
        /// The method runs when the upload config button is clicked. It starts the upload config process if it can
        /// </summary>
        /// <param name="sender">The upload config button</param>
        /// <param name="e">Event arguments</param>
        private void UploadConfigFile(object sender, EventArgs e)
        {
            UploadAnyFile(uploadConfigButton, openConfigFileDialog, ".zip", ReadConfigFiles);
        }

        /// <summary>
        /// The method runs when the upload param button is clicked. It starts the upload param process if it can
        /// </summary>
        /// <param name="sender">The upload param button</param>
        /// <param name="e">Event arguments</param>
        private void UploadParamFile(object sender, EventArgs e)
        {
            UploadAnyFile(uploadParambutton, openParamFileDialog, ".txt", ReadParamFiles);
        }

        /// <summary>
        /// The method runs when an upload button is clicked. It starts an upload process if it can
        /// </summary>
        /// <param name="buttonToDisable">This is the button that is disabled whilst the program is running</param>
        /// <param name="fileDialog">This is the file dialog used by the user to select a file</param>
        /// <param name="fileExt">This is the file extension used to confirm the file is the correct type</param>
        /// <param name="methodUsedToReadFile">This is the method used to read the file</param>
        private static void UploadAnyFile(Button buttonToDisable, FileDialog fileDialog,string fileExt, Action<string> methodUsedToReadFile)
        {
            buttonToDisable.Enabled = false; //Disable button whilst method running

            DialogResult response = fileDialog.ShowDialog(); //Show open file dialog
            string filePath = fileDialog.FileName; //Get the path to the file

            if (response != DialogResult.OK) //User didn't get file
            {
                MessageBox.Show($"File not found. Please select a {fileExt} file.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell user
            }
            else if (filePath.EndsWith(fileExt)) //If user got a file with the correct file extension 
            {
                methodUsedToReadFile(filePath); //Read the file
            }
            else //Else the file wasn't the correct type
            {
                MessageBox.Show($"Appropriate file type not selected. Please select a compatiable {fileExt} file.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell user

            }

            buttonToDisable.Enabled = true; //Undisable button
        }


        /// <summary>
        /// This method converts the config files into tabs and populates those tabs with groupboxes and panels based on the config
        /// </summary>
        /// <param name="configFiles">The files we need to base the GUI on</param>
        public void ConvertConfigFilesToTabs(List<ConfigFile> configFiles)
        {
            
            ConfigFile? blockIdFile = configFiles.SingleOrDefault(c => c.BlockIds is not null); //Find the block id json
            if (blockIdFile is null || blockIdFile.BlockIds is null) //If we didn't find it correctly
            {
                return; //Quit
            }

            tabControl.TabPages.Clear(); //Get rid of all existing tabs

            FunctionBlock[] functionBlocks = configFiles.Select(c => c.FunctionBlock).OfType<FunctionBlock>().ToArray(); //Get function block array without nulls
            foreach (FunctionBlock functionBlock in functionBlocks) //For each function block 
            {

                //Create tabPage
                TabPage tabPage = new()
                {
                    Name = functionBlock.name,
                    Text = FormatNames(functionBlock.name),
                    ToolTipText = functionBlock.description,
                        
                };
                tabControl.TabPages.Add(tabPage);//Add tabpage to tabpages
                functionBlock.tabPage = tabPage; //Link function block and tabPage
                //Create flowlayout for inside tabpage
                FlowLayoutPanel tabPageflowLayoutPanel = new()
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = true,
                };
                tabPage.Controls.Add(tabPageflowLayoutPanel);//Add to tabPage

                //Find blockIds that links to this functionblock name
                List <BlockId> blocks = blockIdFile.BlockIds.FindAll(b => b.block == functionBlock.name);
                foreach (BlockId block in blocks) //For each block
                {
                    //Create groupbox
                    GroupBox groupBox = new()
                    {
                        Name = block.instance,
                        Text = FormatNames(block.instance),
                        AutoSize = true,
                        Font = new Font(Font.FontFamily, 10),

                    };
                    toolTips.SetToolTip(groupBox, block.id.ToString()); //Set tooltip to id

                    functionBlock.highestContainedId = block.id; //Set the highest contained id (This will be the last one assigned)

                    int calculatedMaxHeight = tabControl.Height - 75; //Get maximum height for groupbox

                    //Create  for inside groupbox
                    FlowLayoutPanel groupBoxflowLayoutPanel = new()
                    {
                        Dock = DockStyle.Fill,
                        AutoSize = true,
                        MaximumSize = new Size(10000000, calculatedMaxHeight), //Width can be anything, height is limited
                        FlowDirection = FlowDirection.TopDown,
                        WrapContents = true,
                        Font = Font,

                    };
                    groupBox.Controls.Add(groupBoxflowLayoutPanel); //Add to groupbox

                    AddPanels(functionBlock.parameters, groupBoxflowLayoutPanel); //Add Panels

                    tabPageflowLayoutPanel.Controls.Add(groupBox); //Add groupbox to tabpage flowlayout
                }

                if (blocks.Count == 0) //If there are no blocks for it
                {
                    functionBlock.highestContainedId = 1000;//Set this high so it goes at the back
                    AddPanels(functionBlock.parameters, tabPageflowLayoutPanel); //Just add panels directly to tab
                }
                
            }

            // Arrange the tabs in the order of the highest contained id:
            Array.Sort(functionBlocks); //Sort the functionBlocks according to their highest contained id
            foreach (FunctionBlock functionBlock in functionBlocks.Reverse()) //For each function block, starting from the back
            {
                tabControl.TabPages.Remove(functionBlock.tabPage); //Remove from it's exisiting position
                tabControl.TabPages.Insert(0, functionBlock.tabPage); //Add to the start
            }
            tabControl.SelectedIndex = 0;//Make user select the new first tab
           
        }

        /// <summary>
        /// Add panels to a flow layout box based on what infomation is provided about the parameters
        /// </summary>
        /// <param name="parameters">The parameters that provide the info</param>
        /// <param name="flowLayoutPanel">The flow layout to add the panels to</param>
        private void AddPanels(List<Parameter> parameters, FlowLayoutPanel flowLayoutPanel)
        {
            string[] positionNames = new string[] { "Upper", "Lower", "Default", "Value" }; //These are position names

            foreach (Parameter parameter in parameters) //For each parameter
            {
                //Create panel
                Panel panel = new()
                {
                    Size = new Size(600, 60),
                    AutoSize = true,
                };
                toolTips.SetToolTip(panel, parameter.description); //Add description as tooltip
                flowLayoutPanel.Controls.Add(panel); //Add panel to layout

                //Create label
                Label label = new()
                {
                    Text = FormatNames(parameter.name),
                    Location = new Point(5, 10),
                    AutoSize = true,
                };
                panel.Controls.Add(label); //Add label to panel
                toolTips.SetToolTip(label, parameter.description); //Add description as tooltip


                for (int i = 0; i < parameter.dimensions; i++)
                {
                    
                    TextBox textBox = new(); //Create textbox
                    textBox.TextChanged += TextBoxTextChanged; //Assign function

                    if (parameter.dimensions == 1) //If only one textbox 
                    {
                        //Calculate normal location and size
                        textBox.Location = new Point(250, 6);
                        textBox.Size = new Size(150, 26);

                        Label unitsLabel = new()
                        {
                            Text = FormatNames(parameter.units).PadRight(2),
                            Location = new Point(textBox.Location.X + textBox.Size.Width + 5, 10),
                            AutoSize = true,
                        };
                        panel.Controls.Add(unitsLabel); //Add units label to panel
                    }
                    else if (parameter.dimensions == 3 || parameter.dimensions == 4) //If only three or four textboxes
                    {
                        //Create label
                        Label positionLabel = new()
                        {
                            Text = positionNames[i] +":", //Put position name

                            //Calculate location before textboxes
                            Location = new Point(250 + (75  * i*2), 6), 

                            //Calculate size or get autosize
                            //Size = new Size(75, 26),
                            AutoSize = true,
                        };
                        panel.Controls.Add(positionLabel); //Add label to panel
                        
                        //Calculate textbox position after labels
                        textBox.Location = new Point(250 + (75 + 75 * i*2), 6);
                        //Set Size
                        textBox.Size = new Size(75, 26);

                        toolTips.SetToolTip(textBox, positionNames[i]); //Add position name as tooltip

                    }
                    else //If any other size
                    {
                        textBox.Location = new Point(250+(40*i), 6); //Set positions close together but not overlapping
                        textBox.Size = new Size(40, 26); //Set smaller size
                    }

                    

                    panel.Controls.Add(textBox); //Add textbox to panel
                    parameter.textBoxes.Add(textBox); //Link parameter and textbox
                    parameter.values.Add(""); //Add default value as well

                }

            }
        }

        /// <summary>
        ///  This function runs whenever the textbox recieves new text
        /// </summary>
        /// <param name="sender">A textbox</param>
        /// <param name="e">Event Arguments</param>
        private void TextBoxTextChanged(object? sender, EventArgs e)
        {
            if (sender is null) //If there is no sender
            {
                return; //Quit. This shouldn't happen
            }

            TextBox textBox = (TextBox)sender; //Convert the sender into textbox
            Parameter parameter = Parameter.ListOfAll.Single(p => p.textBoxes.Contains(sender));//Get the parameter the textbox is linked to

            string changedText = textBox.Text;//Get the newly changed text
            string error = ""; //Initialize potential error

            object? objectTheTextRepresents = null; //Initialize potential object to save

            try //Try to
            {
                objectTheTextRepresents = ConvertStringToParameterValue(parameter, ref changedText); //Convert the string to the correct object
            }
            catch (Exception) //If it doesn't work
            {
                error = $"The text inputted into {parameter.name} can't be converted to a {parameter.type}"; //Set error

                if (changedText == "" || changedText == ".") //If it's either of these strings
                {
                    errorProvider.SetError(textBox, null); //Don't set error
                    return; //Quit early
                    //As it might become something later, no need to report
                }
            }
            
            //Test it's within the bounds
            if (objectTheTextRepresents is int or float) //If it is a number
            {
                float number = (float)objectTheTextRepresents; //Get the number
                if (parameter.lowerLimit > number || number > parameter.upperLimit) //If it is outside it's limits
                {
                    error = $"The text inputted into {parameter.name} must be between {parameter.lowerLimit} and {parameter.upperLimit}";//Set error
                }
            }
            
            if (error != "" || objectTheTextRepresents == null) //If there is an error or the object wasn't found
            {
                if (errorProvider.GetError(textBox) != error) //If it's a new error message
                {
                    errorProvider.SetError(textBox, error); //Show the error next to the textbox

                    if (uploadParambutton.Enabled == true) //If we're not uploading
                    {
                        MessageBox.Show(error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell user in a messagebox
                    }
                    
                }
            } 
            else //If there is no error
            {
                errorProvider.SetError(textBox, null); //Extinguish error next to textbox if it is there
                int textBoxIndex = parameter.textBoxes.IndexOf(textBox); //Get index of textbox
                parameter.values[textBoxIndex] = objectTheTextRepresents; //Save the object to the correct value
            }


        }

        /// <summary>
        /// This is a recursive method that clears all textboxes below the controls collection given
        /// </summary>
        /// <param name="controls">The controls to clear if they are textboxes and go down if they are not</param>
        public static void ClearTextboxesOnThisLevelAndBelow(Control.ControlCollection controls)
        {
            foreach (Control control in controls) //For each Control on this level (if there is any)
            {
                if (control is TextBox textBox) //If it's a textbox
                {
                    textBox.Clear(); //Clear it
                }
                else //Else
                {
                    ClearTextboxesOnThisLevelAndBelow(control.Controls); //Go down a level and repeat
                }
            }
        }

        /// <summary>
        /// This method runs when the form is resized. 
        /// It allows the groupboxes to expand downwards as much as possible.
        /// </summary>
        /// <param name="sender">The Main Form</param>
        /// <param name="e">Event Arguments</param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            //TODO Remove some of these restrictions once test GUI is gone

            int calculatedMaxHeight = tabControl.Height - 75; //Calculate the optimal height for the height of the tab control

            foreach (TabPage tab in tabControl.TabPages) //For each tab
            {
                if (tab.Controls.Count == 0) //If there are no tabs in the tab
                {
                    continue; //Skip this one
                }

                foreach (GroupBox groupBox in tab.Controls[0].Controls.OfType<GroupBox>()) //For each groupbox in tab
                {
                    if (groupBox.Controls.Count != 0 && groupBox.Controls[0] is FlowLayoutPanel flowLayout) //If the groupbox has a flowlayout in
                    {
                        flowLayout.MaximumSize = new Size(10000000, calculatedMaxHeight); //Change maximum height based on the tab control height
                    }
                }
            }
        }

        private void OpenDatabaseDownloadForm(object sender, EventArgs e)
        {
            DatabaseDownloadForm form = new();
            form.Show(); //open DatabaseDownloadForm
        }
    }
}