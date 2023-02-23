using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Advanced_Drive_GUI
{
    public partial class DatabaseDownloadForm : Form
    {
        public DatabaseDownloadForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Searches the database and populates the form's dataGridView with the results.
        /// </summary>
        /// <param name="searchText">The text used to query the database</param>
        private void searchDatabase(string searchText)
        {
            //search database
        }

        /// <summary>
        /// Gets the location of the config from the database, downloads it and then applies it 
        /// </summary>
        /// <param name="configName">The name of the config to retreive from the database</param>
        private void applyConfigFromDatabase(string configName)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            searchDatabase(searchBarTextBox.Text); //query the database using the text in the search bar
        }

        private void searchBarTextBox_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) //if the enter key is pressed
            {
                searchDatabase(searchBarTextBox.Text); //query the database using the text in the search bar
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close(); //closes the form
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            applyConfigFromDatabase(selectedConfigTextBox.Text); //apply the config the user has chosen
        }
    }
}
