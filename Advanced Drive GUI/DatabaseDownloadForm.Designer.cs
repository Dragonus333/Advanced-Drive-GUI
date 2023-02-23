namespace Advanced_Drive_GUI
{
    partial class DatabaseDownloadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchButton = new System.Windows.Forms.Button();
            this.searchBarTextBox = new System.Windows.Forms.TextBox();
            this.dbQueryResultsDataGridView = new System.Windows.Forms.DataGridView();
            this.selectedConfigTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfigName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dbQueryResultsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(430, 11);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(94, 29);
            this.searchButton.TabIndex = 0;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // searchBarTextBox
            // 
            this.searchBarTextBox.Location = new System.Drawing.Point(12, 12);
            this.searchBarTextBox.Name = "searchBarTextBox";
            this.searchBarTextBox.Size = new System.Drawing.Size(412, 27);
            this.searchBarTextBox.TabIndex = 1;
            this.searchBarTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchBarTextBox_KeyPressed);
            // 
            // dbQueryResultsDataGridView
            // 
            this.dbQueryResultsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dbQueryResultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dbQueryResultsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ConfigName});
            this.dbQueryResultsDataGridView.Location = new System.Drawing.Point(12, 46);
            this.dbQueryResultsDataGridView.Name = "dbQueryResultsDataGridView";
            this.dbQueryResultsDataGridView.RowHeadersVisible = false;
            this.dbQueryResultsDataGridView.RowHeadersWidth = 51;
            this.dbQueryResultsDataGridView.RowTemplate.Height = 29;
            this.dbQueryResultsDataGridView.Size = new System.Drawing.Size(512, 336);
            this.dbQueryResultsDataGridView.TabIndex = 2;
            // 
            // selectedConfigTextBox
            // 
            this.selectedConfigTextBox.Location = new System.Drawing.Point(12, 393);
            this.selectedConfigTextBox.Name = "selectedConfigTextBox";
            this.selectedConfigTextBox.Size = new System.Drawing.Size(312, 27);
            this.selectedConfigTextBox.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(430, 391);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(94, 29);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(330, 391);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(94, 29);
            this.applyButton.TabIndex = 5;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // ID
            // 
            this.ID.FillWeight = 30F;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            // 
            // ConfigName
            // 
            this.ConfigName.FillWeight = 90F;
            this.ConfigName.HeaderText = "Name";
            this.ConfigName.MinimumWidth = 6;
            this.ConfigName.Name = "ConfigName";
            // 
            // DatabaseDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 430);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.selectedConfigTextBox);
            this.Controls.Add(this.dbQueryResultsDataGridView);
            this.Controls.Add(this.searchBarTextBox);
            this.Controls.Add(this.searchButton);
            this.Name = "DatabaseDownloadForm";
            this.Text = "DatabaseDownloadForm";
            ((System.ComponentModel.ISupportInitialize)(this.dbQueryResultsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button searchButton;
        private TextBox searchBarTextBox;
        private DataGridView dbQueryResultsDataGridView;
        private TextBox selectedConfigTextBox;
        private Button cancelButton;
        private Button applyButton;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn ConfigName;
    }
}