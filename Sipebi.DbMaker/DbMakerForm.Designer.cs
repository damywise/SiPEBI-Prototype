namespace Sipebi.DbMaker {
  partial class DbMakerForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
			this.splitContainerMain = new System.Windows.Forms.SplitContainer();
			this.richTextBoxDebug = new System.Windows.Forms.RichTextBox();
			this.flowLayoutPanelOptions = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonCreateEntriSipebiTable = new System.Windows.Forms.Button();
			this.buttonCreateFormalWordItemTableFromTable = new System.Windows.Forms.Button();
			this.buttonCreateFileFromFormalWordItemTable = new System.Windows.Forms.Button();
			this.buttonCreateFormalWordItemTableFromFile = new System.Windows.Forms.Button();
			this.buttonTransferFormalWordItemsToWords = new System.Windows.Forms.Button();
			this.buttonSerializeWordsTable = new System.Windows.Forms.Button();
			this.buttonCompressEncryptSerializedWordsTable = new System.Windows.Forms.Button();
			this.buttonReadSerializeEncryptedCompressedWordsFile = new System.Windows.Forms.Button();
			this.buttonCreateCompressEncryptSerializedSettingsFile = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
			this.splitContainerMain.Panel1.SuspendLayout();
			this.splitContainerMain.Panel2.SuspendLayout();
			this.splitContainerMain.SuspendLayout();
			this.flowLayoutPanelOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainerMain
			// 
			this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
			this.splitContainerMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.splitContainerMain.Name = "splitContainerMain";
			this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerMain.Panel1
			// 
			this.splitContainerMain.Panel1.Controls.Add(this.richTextBoxDebug);
			// 
			// splitContainerMain.Panel2
			// 
			this.splitContainerMain.Panel2.Controls.Add(this.flowLayoutPanelOptions);
			this.splitContainerMain.Size = new System.Drawing.Size(512, 673);
			this.splitContainerMain.SplitterDistance = 356;
			this.splitContainerMain.SplitterWidth = 5;
			this.splitContainerMain.TabIndex = 0;
			// 
			// richTextBoxDebug
			// 
			this.richTextBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBoxDebug.Location = new System.Drawing.Point(0, 0);
			this.richTextBoxDebug.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.richTextBoxDebug.Name = "richTextBoxDebug";
			this.richTextBoxDebug.Size = new System.Drawing.Size(512, 356);
			this.richTextBoxDebug.TabIndex = 0;
			this.richTextBoxDebug.Text = "";
			// 
			// flowLayoutPanelOptions
			// 
			this.flowLayoutPanelOptions.Controls.Add(this.buttonCreateEntriSipebiTable);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonCreateFormalWordItemTableFromTable);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonCreateFileFromFormalWordItemTable);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonCreateFormalWordItemTableFromFile);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonTransferFormalWordItemsToWords);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonSerializeWordsTable);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonCompressEncryptSerializedWordsTable);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonReadSerializeEncryptedCompressedWordsFile);
			this.flowLayoutPanelOptions.Controls.Add(this.buttonCreateCompressEncryptSerializedSettingsFile);
			this.flowLayoutPanelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanelOptions.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanelOptions.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.flowLayoutPanelOptions.Name = "flowLayoutPanelOptions";
			this.flowLayoutPanelOptions.Size = new System.Drawing.Size(512, 312);
			this.flowLayoutPanelOptions.TabIndex = 1;
			// 
			// buttonCreateEntriSipebiTable
			// 
			this.buttonCreateEntriSipebiTable.Location = new System.Drawing.Point(3, 4);
			this.buttonCreateEntriSipebiTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCreateEntriSipebiTable.Name = "buttonCreateEntriSipebiTable";
			this.buttonCreateEntriSipebiTable.Size = new System.Drawing.Size(506, 26);
			this.buttonCreateEntriSipebiTable.TabIndex = 0;
			this.buttonCreateEntriSipebiTable.Text = "Create Entri Sipebi Table From KBBI Database (Erase Existing)";
			this.buttonCreateEntriSipebiTable.UseVisualStyleBackColor = true;
			this.buttonCreateEntriSipebiTable.Click += new System.EventHandler(this.buttonCreateEntriSipebiTable_Click);
			// 
			// buttonCreateFormalWordItemTableFromTable
			// 
			this.buttonCreateFormalWordItemTableFromTable.Location = new System.Drawing.Point(3, 38);
			this.buttonCreateFormalWordItemTableFromTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCreateFormalWordItemTableFromTable.Name = "buttonCreateFormalWordItemTableFromTable";
			this.buttonCreateFormalWordItemTableFromTable.Size = new System.Drawing.Size(506, 26);
			this.buttonCreateFormalWordItemTableFromTable.TabIndex = 1;
			this.buttonCreateFormalWordItemTableFromTable.Text = "Create Formal Word Item Table From Entri Sipebi Table (Erase Existing)";
			this.buttonCreateFormalWordItemTableFromTable.UseVisualStyleBackColor = true;
			this.buttonCreateFormalWordItemTableFromTable.Click += new System.EventHandler(this.buttonCreateFormalWordItemTableFromTable_Click);
			// 
			// buttonCreateFileFromFormalWordItemTable
			// 
			this.buttonCreateFileFromFormalWordItemTable.Location = new System.Drawing.Point(3, 72);
			this.buttonCreateFileFromFormalWordItemTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCreateFileFromFormalWordItemTable.Name = "buttonCreateFileFromFormalWordItemTable";
			this.buttonCreateFileFromFormalWordItemTable.Size = new System.Drawing.Size(506, 26);
			this.buttonCreateFileFromFormalWordItemTable.TabIndex = 3;
			this.buttonCreateFileFromFormalWordItemTable.Text = "Create File From Formal Word Item Table";
			this.buttonCreateFileFromFormalWordItemTable.UseVisualStyleBackColor = true;
			this.buttonCreateFileFromFormalWordItemTable.Click += new System.EventHandler(this.buttonCreateFileFromFormalWordItemTable_Click);
			// 
			// buttonCreateFormalWordItemTableFromFile
			// 
			this.buttonCreateFormalWordItemTableFromFile.Location = new System.Drawing.Point(3, 106);
			this.buttonCreateFormalWordItemTableFromFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCreateFormalWordItemTableFromFile.Name = "buttonCreateFormalWordItemTableFromFile";
			this.buttonCreateFormalWordItemTableFromFile.Size = new System.Drawing.Size(506, 26);
			this.buttonCreateFormalWordItemTableFromFile.TabIndex = 2;
			this.buttonCreateFormalWordItemTableFromFile.Text = "Create Formal Word Item Table From File (Erase Existing)";
			this.buttonCreateFormalWordItemTableFromFile.UseVisualStyleBackColor = true;
			this.buttonCreateFormalWordItemTableFromFile.Click += new System.EventHandler(this.buttonCreateFormalWordItemTableFromFile_Click);
			// 
			// buttonTransferFormalWordItemsToWords
			// 
			this.buttonTransferFormalWordItemsToWords.Location = new System.Drawing.Point(3, 140);
			this.buttonTransferFormalWordItemsToWords.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonTransferFormalWordItemsToWords.Name = "buttonTransferFormalWordItemsToWords";
			this.buttonTransferFormalWordItemsToWords.Size = new System.Drawing.Size(506, 26);
			this.buttonTransferFormalWordItemsToWords.TabIndex = 4;
			this.buttonTransferFormalWordItemsToWords.Text = "Transfer Formal Word Item Table To Words Table (Erase Existing)";
			this.buttonTransferFormalWordItemsToWords.UseVisualStyleBackColor = true;
			this.buttonTransferFormalWordItemsToWords.Click += new System.EventHandler(this.buttonTransferFormalWordItemsToWords_Click);
			// 
			// buttonSerializeWordsTable
			// 
			this.buttonSerializeWordsTable.Location = new System.Drawing.Point(3, 174);
			this.buttonSerializeWordsTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonSerializeWordsTable.Name = "buttonSerializeWordsTable";
			this.buttonSerializeWordsTable.Size = new System.Drawing.Size(506, 26);
			this.buttonSerializeWordsTable.TabIndex = 5;
			this.buttonSerializeWordsTable.Text = "Serialize Words Table";
			this.buttonSerializeWordsTable.UseVisualStyleBackColor = true;
			this.buttonSerializeWordsTable.Click += new System.EventHandler(this.buttonSerializeWordsTable_Click);
			// 
			// buttonCompressEncryptSerializedWordsTable
			// 
			this.buttonCompressEncryptSerializedWordsTable.Location = new System.Drawing.Point(3, 208);
			this.buttonCompressEncryptSerializedWordsTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCompressEncryptSerializedWordsTable.Name = "buttonCompressEncryptSerializedWordsTable";
			this.buttonCompressEncryptSerializedWordsTable.Size = new System.Drawing.Size(506, 26);
			this.buttonCompressEncryptSerializedWordsTable.TabIndex = 6;
			this.buttonCompressEncryptSerializedWordsTable.Text = "Compress-Encrypt Serialized Words Table";
			this.buttonCompressEncryptSerializedWordsTable.UseVisualStyleBackColor = true;
			this.buttonCompressEncryptSerializedWordsTable.Click += new System.EventHandler(this.buttonCompressEncryptSerializedWordsTable_Click);
			// 
			// buttonReadSerializeEncryptedCompressedWordsFile
			// 
			this.buttonReadSerializeEncryptedCompressedWordsFile.Location = new System.Drawing.Point(3, 242);
			this.buttonReadSerializeEncryptedCompressedWordsFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonReadSerializeEncryptedCompressedWordsFile.Name = "buttonReadSerializeEncryptedCompressedWordsFile";
			this.buttonReadSerializeEncryptedCompressedWordsFile.Size = new System.Drawing.Size(506, 26);
			this.buttonReadSerializeEncryptedCompressedWordsFile.TabIndex = 8;
			this.buttonReadSerializeEncryptedCompressedWordsFile.Text = "Read-Serialize Encrypted-Compressed Words File";
			this.buttonReadSerializeEncryptedCompressedWordsFile.UseVisualStyleBackColor = true;
			this.buttonReadSerializeEncryptedCompressedWordsFile.Click += new System.EventHandler(this.buttonReadSerializeEncryptedCompressedWordsFile_Click);
			// 
			// buttonCreateCompressEncryptSerializedSettingsFile
			// 
			this.buttonCreateCompressEncryptSerializedSettingsFile.Location = new System.Drawing.Point(3, 276);
			this.buttonCreateCompressEncryptSerializedSettingsFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCreateCompressEncryptSerializedSettingsFile.Name = "buttonCreateCompressEncryptSerializedSettingsFile";
			this.buttonCreateCompressEncryptSerializedSettingsFile.Size = new System.Drawing.Size(506, 26);
			this.buttonCreateCompressEncryptSerializedSettingsFile.TabIndex = 9;
			this.buttonCreateCompressEncryptSerializedSettingsFile.Text = "Create Compress Encrypt Serialized Settings File";
			this.buttonCreateCompressEncryptSerializedSettingsFile.UseVisualStyleBackColor = true;
			this.buttonCreateCompressEncryptSerializedSettingsFile.Click += new System.EventHandler(this.buttonCreateCompressEncryptSerializedSettingsFile_Click);
			// 
			// DbMakerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(512, 673);
			this.Controls.Add(this.splitContainerMain);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "DbMakerForm";
			this.Text = "Sipebi DB Maker v1.0";
			this.splitContainerMain.Panel1.ResumeLayout(false);
			this.splitContainerMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
			this.splitContainerMain.ResumeLayout(false);
			this.flowLayoutPanelOptions.ResumeLayout(false);
			this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.RichTextBox richTextBoxDebug;
    private System.Windows.Forms.Button buttonCreateEntriSipebiTable;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOptions;
    private System.Windows.Forms.Button buttonCreateFormalWordItemTableFromTable;
    private System.Windows.Forms.Button buttonCreateFormalWordItemTableFromFile;
    private System.Windows.Forms.Button buttonCreateFileFromFormalWordItemTable;
		private System.Windows.Forms.Button buttonTransferFormalWordItemsToWords;
		private System.Windows.Forms.Button buttonSerializeWordsTable;
		private System.Windows.Forms.Button buttonCompressEncryptSerializedWordsTable;
		private System.Windows.Forms.Button buttonReadSerializeEncryptedCompressedWordsFile;
		private System.Windows.Forms.Button buttonCreateCompressEncryptSerializedSettingsFile;
	}
}

