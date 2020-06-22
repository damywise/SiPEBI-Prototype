namespace SipebiPrototype.DbMaker {
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
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.buttonCreateEntriSipebiTable = new System.Windows.Forms.Button();
      this.buttonCreateFormalWordItemTable = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
      this.splitContainerMain.Panel1.SuspendLayout();
      this.splitContainerMain.Panel2.SuspendLayout();
      this.splitContainerMain.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainerMain
      // 
      this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
      this.splitContainerMain.Margin = new System.Windows.Forms.Padding(5);
      this.splitContainerMain.Name = "splitContainerMain";
      this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainerMain.Panel1
      // 
      this.splitContainerMain.Panel1.Controls.Add(this.richTextBoxDebug);
      // 
      // splitContainerMain.Panel2
      // 
      this.splitContainerMain.Panel2.Controls.Add(this.flowLayoutPanel1);
      this.splitContainerMain.Size = new System.Drawing.Size(875, 526);
      this.splitContainerMain.SplitterDistance = 411;
      this.splitContainerMain.SplitterWidth = 7;
      this.splitContainerMain.TabIndex = 0;
      // 
      // richTextBoxDebug
      // 
      this.richTextBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
      this.richTextBoxDebug.Location = new System.Drawing.Point(0, 0);
      this.richTextBoxDebug.Margin = new System.Windows.Forms.Padding(5);
      this.richTextBoxDebug.Name = "richTextBoxDebug";
      this.richTextBoxDebug.Size = new System.Drawing.Size(875, 411);
      this.richTextBoxDebug.TabIndex = 0;
      this.richTextBoxDebug.Text = "";
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.buttonCreateEntriSipebiTable);
      this.flowLayoutPanel1.Controls.Add(this.buttonCreateFormalWordItemTable);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(875, 108);
      this.flowLayoutPanel1.TabIndex = 1;
      // 
      // buttonCreateEntriSipebiTable
      // 
      this.buttonCreateEntriSipebiTable.Location = new System.Drawing.Point(5, 5);
      this.buttonCreateEntriSipebiTable.Margin = new System.Windows.Forms.Padding(5);
      this.buttonCreateEntriSipebiTable.Name = "buttonCreateEntriSipebiTable";
      this.buttonCreateEntriSipebiTable.Size = new System.Drawing.Size(875, 43);
      this.buttonCreateEntriSipebiTable.TabIndex = 0;
      this.buttonCreateEntriSipebiTable.Text = "Create Entri Sipebi Table";
      this.buttonCreateEntriSipebiTable.UseVisualStyleBackColor = true;
      this.buttonCreateEntriSipebiTable.Click += new System.EventHandler(this.buttonCreateEntriSipebiTable_Click);
      // 
      // buttonCreateFormalWordItemTable
      // 
      this.buttonCreateFormalWordItemTable.Location = new System.Drawing.Point(5, 58);
      this.buttonCreateFormalWordItemTable.Margin = new System.Windows.Forms.Padding(5);
      this.buttonCreateFormalWordItemTable.Name = "buttonCreateFormalWordItemTable";
      this.buttonCreateFormalWordItemTable.Size = new System.Drawing.Size(875, 43);
      this.buttonCreateFormalWordItemTable.TabIndex = 1;
      this.buttonCreateFormalWordItemTable.Text = "Create Formal Word Item Table";
      this.buttonCreateFormalWordItemTable.UseVisualStyleBackColor = true;
      this.buttonCreateFormalWordItemTable.Click += new System.EventHandler(this.buttonCreateFormalWordItemTable_Click);
      // 
      // DbMakerForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(875, 526);
      this.Controls.Add(this.splitContainerMain);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(5);
      this.Name = "DbMakerForm";
      this.Text = "Sipebi DB Maker v1.0";
      this.splitContainerMain.Panel1.ResumeLayout(false);
      this.splitContainerMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
      this.splitContainerMain.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.RichTextBox richTextBoxDebug;
    private System.Windows.Forms.Button buttonCreateEntriSipebiTable;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Button buttonCreateFormalWordItemTable;
  }
}

