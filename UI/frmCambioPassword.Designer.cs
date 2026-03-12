namespace UI
{
    partial class frmCambioPassword
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
            this.lvlNueva = new System.Windows.Forms.Label();
            this.txtPasswordNueva = new System.Windows.Forms.TextBox();
            this.lblRepetir = new System.Windows.Forms.Label();
            this.txtPasswordRepetir = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvlNueva
            // 
            this.lvlNueva.AutoSize = true;
            this.lvlNueva.Location = new System.Drawing.Point(60, 39);
            this.lvlNueva.Name = "lvlNueva";
            this.lvlNueva.Size = new System.Drawing.Size(99, 13);
            this.lvlNueva.TabIndex = 0;
            this.lvlNueva.Tag = "lbl_NuevaPassword";
            this.lvlNueva.Text = "Nueva Contraseña:";
            // 
            // txtPasswordNueva
            // 
            this.txtPasswordNueva.Location = new System.Drawing.Point(197, 36);
            this.txtPasswordNueva.Name = "txtPasswordNueva";
            this.txtPasswordNueva.Size = new System.Drawing.Size(173, 20);
            this.txtPasswordNueva.TabIndex = 1;
            // 
            // lblRepetir
            // 
            this.lblRepetir.AutoSize = true;
            this.lblRepetir.Location = new System.Drawing.Point(58, 76);
            this.lblRepetir.Name = "lblRepetir";
            this.lblRepetir.Size = new System.Drawing.Size(101, 13);
            this.lblRepetir.TabIndex = 2;
            this.lblRepetir.Tag = "lbl_RepetirPassword";
            this.lblRepetir.Text = "Repetir Contraseña:";
            // 
            // txtPasswordRepetir
            // 
            this.txtPasswordRepetir.Location = new System.Drawing.Point(197, 73);
            this.txtPasswordRepetir.Name = "txtPasswordRepetir";
            this.txtPasswordRepetir.Size = new System.Drawing.Size(173, 20);
            this.txtPasswordRepetir.TabIndex = 3;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(68, 142);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(105, 48);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Tag = "btn_Aceptar";
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(252, 142);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(105, 48);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Tag = "btn_Cancelar";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmCambioPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 307);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtPasswordRepetir);
            this.Controls.Add(this.lblRepetir);
            this.Controls.Add(this.txtPasswordNueva);
            this.Controls.Add(this.lvlNueva);
            this.Name = "frmCambioPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "cap_CambioPassword";
            this.Text = "Actualizar Contraseña";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCambioPassword_FormClosing);
            this.Load += new System.EventHandler(this.frmCambioPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lvlNueva;
        private System.Windows.Forms.TextBox txtPasswordNueva;
        private System.Windows.Forms.Label lblRepetir;
        private System.Windows.Forms.TextBox txtPasswordRepetir;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}