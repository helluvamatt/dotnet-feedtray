namespace FeedTray
{
	partial class FeedTrayOptionsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedTrayOptionsForm));
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.feedsListBox = new System.Windows.Forms.CheckedListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.feedPropertyGrid = new System.Windows.Forms.PropertyGrid();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.feedsListBox);
			this.splitContainer.Panel1.Controls.Add(this.addButton);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.feedPropertyGrid);
			this.splitContainer.Size = new System.Drawing.Size(784, 561);
			this.splitContainer.SplitterDistance = 261;
			this.splitContainer.TabIndex = 0;
			// 
			// feedsListBox
			// 
			this.feedsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.feedsListBox.FormattingEnabled = true;
			this.feedsListBox.Location = new System.Drawing.Point(0, 47);
			this.feedsListBox.Name = "feedsListBox";
			this.feedsListBox.Size = new System.Drawing.Size(261, 514);
			this.feedsListBox.TabIndex = 3;
			this.feedsListBox.SelectedIndexChanged += new System.EventHandler(this.feedsListBox_SelectedIndexChanged);
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(12, 12);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(43, 23);
			this.addButton.TabIndex = 1;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// feedPropertyGrid
			// 
			this.feedPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.feedPropertyGrid.Location = new System.Drawing.Point(0, 0);
			this.feedPropertyGrid.Name = "feedPropertyGrid";
			this.feedPropertyGrid.Size = new System.Drawing.Size(519, 561);
			this.feedPropertyGrid.TabIndex = 0;
			this.feedPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.feedPropertyGrid_PropertyValueChanged);
			// 
			// OptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.splitContainer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OptionsForm";
			this.Text = "FeedTray Options";
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.PropertyGrid feedPropertyGrid;
		private System.Windows.Forms.CheckedListBox feedsListBox;
	}
}

