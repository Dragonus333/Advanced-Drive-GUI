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
            string zipFilePath = openFileDialog.FileName; //Get the path to the zip file

            if (response != DialogResult.OK) //User didn't get file
            {
                MessageBox.Show("File not found. Please select a file.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell user
                uploadButton.Enabled = true; //Undisable button
                return;  //Exit method
            }
            else if (!zipFilePath.EndsWith(".zip")) //User didn't get a zip file
            {
                MessageBox.Show("Zip file not selected. Please select a zip file.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //Tell user
                uploadButton.Enabled = true; //Undisable button
                return;  //Exit method
            }

            ReadConfigFiles(zipFilePath); //Attempt to upload the files

            uploadButton.Enabled = true; //Undisable button

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
                    Text = AddSpaces(functionBlock.name),
                    ToolTipText = functionBlock.description,
                        
                };
                tabControl.TabPages.Add(tabPage);//Add tabpage to tabpages
                functionBlock.tabPage = tabPage; //Link function block and tabPage
                //Create flowlayout for inside tabpage
                FlowLayoutPanel tabPageflowLayoutPanel = new()
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
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
                        Text = AddSpaces(block.instance),
                        AutoSize = true,

                    };
                    toolTips.SetToolTip(groupBox, block.id.ToString()); //Set tooltip to id

                    functionBlock.highestContainedId = block.id; //Set the highest contained id (This will be the last one assigned)

                    //TODO update this everytime form is adjusted
                    int calcualtedMaxHeight = tabControl.Height - 75; //Get maximum height for groupbox

                    //Create  for inside groupbox
                    FlowLayoutPanel groupBoxflowLayoutPanel = new()
                    {
                        Dock = DockStyle.Fill,
                        AutoSize = true,
                        MaximumSize = new Size(10000000, calcualtedMaxHeight), //Width can be anything, height is limited
                        FlowDirection = FlowDirection.TopDown,
                        WrapContents = true,

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
        /// Add panels to a flow layout box based on what is in parameters
        /// </summary>
        /// <param name="parameters">The parameters that provide the info</param>
        /// <param name="flowLayoutPanel">The flow layout to add the panels to</param>
        private static void AddPanels(List<Parameter> parameters, FlowLayoutPanel flowLayoutPanel)
        {
            foreach (Parameter parameter in parameters) //For each parameter
            {
                //Create panel
                Panel panel = new()
                {
                    Size = new Size(600, 60),
                    AutoSize = true,
                };
                flowLayoutPanel.Controls.Add(panel); //Add panel to layout

                //Create label
                Label label = new()
                {
                    Text = AddSpaces(parameter.name),
                    Location = new Point(5, 10),
                    AutoSize = true,
                };
                panel.Controls.Add(label); //Add label to panel

                //Create textbox
                TextBox textBox = new()
                {
                    Location = new Point(250, 6),
                    Size = new Size(150, 26),
                };
                panel.Controls.Add(textBox); //Add textbox to panel

                parameter.textBoxes.Add(textBox); //Link parameter and textbox

            }
        }
    }
}