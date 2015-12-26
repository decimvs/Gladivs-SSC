namespace gladivsSSC
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.label1 = new System.Windows.Forms.Label();
            this.txtRutaDesat = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnDesar = new System.Windows.Forms.Button();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pnStringsAmagats = new System.Windows.Forms.Panel();
            this.msgCaptionVerificarReinici = new System.Windows.Forms.Label();
            this.msgVerificacioReiniciarAplicacio = new System.Windows.Forms.Label();
            this.msgErrorCaptionGeneric = new System.Windows.Forms.Label();
            this.msgErrorNoPermesRutaDesatCaptruresBlanc = new System.Windows.Forms.Label();
            this.msgErrorCreantDirectoriDesatCaptures = new System.Windows.Forms.Label();
            this.ckbAutoDesatCaptures = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnStringsAmagats.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtRutaDesat
            // 
            resources.ApplyResources(this.txtRutaDesat, "txtRutaDesat");
            this.txtRutaDesat.Name = "txtRutaDesat";
            // 
            // btnCancelar
            // 
            resources.ApplyResources(this.btnCancelar, "btnCancelar");
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnDesar
            // 
            resources.ApplyResources(this.btnDesar, "btnDesar");
            this.btnDesar.Name = "btnDesar";
            this.btnDesar.UseVisualStyleBackColor = true;
            this.btnDesar.Click += new System.EventHandler(this.btnDesar_Click);
            // 
            // btnExaminar
            // 
            resources.ApplyResources(this.btnExaminar, "btnExaminar");
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // pnStringsAmagats
            // 
            this.pnStringsAmagats.Controls.Add(this.msgCaptionVerificarReinici);
            this.pnStringsAmagats.Controls.Add(this.msgVerificacioReiniciarAplicacio);
            this.pnStringsAmagats.Controls.Add(this.msgErrorCaptionGeneric);
            this.pnStringsAmagats.Controls.Add(this.msgErrorNoPermesRutaDesatCaptruresBlanc);
            this.pnStringsAmagats.Controls.Add(this.msgErrorCreantDirectoriDesatCaptures);
            resources.ApplyResources(this.pnStringsAmagats, "pnStringsAmagats");
            this.pnStringsAmagats.Name = "pnStringsAmagats";
            // 
            // msgCaptionVerificarReinici
            // 
            resources.ApplyResources(this.msgCaptionVerificarReinici, "msgCaptionVerificarReinici");
            this.msgCaptionVerificarReinici.Name = "msgCaptionVerificarReinici";
            // 
            // msgVerificacioReiniciarAplicacio
            // 
            resources.ApplyResources(this.msgVerificacioReiniciarAplicacio, "msgVerificacioReiniciarAplicacio");
            this.msgVerificacioReiniciarAplicacio.Name = "msgVerificacioReiniciarAplicacio";
            // 
            // msgErrorCaptionGeneric
            // 
            resources.ApplyResources(this.msgErrorCaptionGeneric, "msgErrorCaptionGeneric");
            this.msgErrorCaptionGeneric.Name = "msgErrorCaptionGeneric";
            // 
            // msgErrorNoPermesRutaDesatCaptruresBlanc
            // 
            resources.ApplyResources(this.msgErrorNoPermesRutaDesatCaptruresBlanc, "msgErrorNoPermesRutaDesatCaptruresBlanc");
            this.msgErrorNoPermesRutaDesatCaptruresBlanc.Name = "msgErrorNoPermesRutaDesatCaptruresBlanc";
            // 
            // msgErrorCreantDirectoriDesatCaptures
            // 
            resources.ApplyResources(this.msgErrorCreantDirectoriDesatCaptures, "msgErrorCreantDirectoriDesatCaptures");
            this.msgErrorCreantDirectoriDesatCaptures.Name = "msgErrorCreantDirectoriDesatCaptures";
            // 
            // ckbAutoDesatCaptures
            // 
            resources.ApplyResources(this.ckbAutoDesatCaptures, "ckbAutoDesatCaptures");
            this.ckbAutoDesatCaptures.Name = "ckbAutoDesatCaptures";
            this.ckbAutoDesatCaptures.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1"),
            resources.GetString("comboBox1.Items2"),
            resources.GetString("comboBox1.Items3"),
            resources.GetString("comboBox1.Items4")});
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRutaDesat);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnExaminar);
            this.groupBox1.Controls.Add(this.ckbAutoDesatCaptures);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // Settings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnStringsAmagats);
            this.Controls.Add(this.btnDesar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.groupBox2);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowInTaskbar = false;
            this.pnStringsAmagats.ResumeLayout(false);
            this.pnStringsAmagats.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRutaDesat;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnDesar;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel pnStringsAmagats;
        private System.Windows.Forms.Label msgErrorNoPermesRutaDesatCaptruresBlanc;
        private System.Windows.Forms.Label msgErrorCreantDirectoriDesatCaptures;
        private System.Windows.Forms.Label msgErrorCaptionGeneric;
        private System.Windows.Forms.CheckBox ckbAutoDesatCaptures;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label msgVerificacioReiniciarAplicacio;
        private System.Windows.Forms.Label msgCaptionVerificarReinici;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}