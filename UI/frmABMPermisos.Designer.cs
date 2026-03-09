namespace UI
{
    partial class frmABMPermisos
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
            this.dgvListaDePermisos = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNombreDelPermiso = new System.Windows.Forms.Label();
            this.txtPermiso = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDePermisos)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvListaDePermisos
            // 
            this.dgvListaDePermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaDePermisos.Location = new System.Drawing.Point(272, 76);
            this.dgvListaDePermisos.Name = "dgvListaDePermisos";
            this.dgvListaDePermisos.Size = new System.Drawing.Size(288, 461);
            this.dgvListaDePermisos.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Tag = "lbl_Lista_de_Permisos";
            this.label1.Text = "Lista de Permisos";
            // 
            // lblNombreDelPermiso
            // 
            this.lblNombreDelPermiso.AutoSize = true;
            this.lblNombreDelPermiso.Location = new System.Drawing.Point(45, 62);
            this.lblNombreDelPermiso.Name = "lblNombreDelPermiso";
            this.lblNombreDelPermiso.Size = new System.Drawing.Size(101, 13);
            this.lblNombreDelPermiso.TabIndex = 2;
            this.lblNombreDelPermiso.Tag = "lbl_Nombre_Del_Permiso";
            this.lblNombreDelPermiso.Text = "Nombre del Permiso";
            // 
            // txtPermiso
            // 
            this.txtPermiso.Location = new System.Drawing.Point(46, 78);
            this.txtPermiso.Name = "txtPermiso";
            this.txtPermiso.Size = new System.Drawing.Size(194, 20);
            this.txtPermiso.TabIndex = 3;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(46, 125);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(102, 53);
            this.btnGuardar.TabIndex = 4;
            this.btnGuardar.Tag = "btn_Guardar";
            this.btnGuardar.Text = "GUARDAR";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(44, 248);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(102, 53);
            this.btnEliminar.TabIndex = 5;
            this.btnEliminar.Tag = "btn_Eliminar";
            this.btnEliminar.Text = "ELIMINAR";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(48, 371);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(102, 53);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Tag = "btn_Cancelar";
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(48, 484);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(102, 53);
            this.btnVolver.TabIndex = 7;
            this.btnVolver.Tag = "btn_Volver";
            this.btnVolver.Text = "VOLVER";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // frmABMPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 573);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtPermiso);
            this.Controls.Add(this.lblNombreDelPermiso);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvListaDePermisos);
            this.Name = "frmABMPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frm_ABM_de_Permisos";
            this.Text = "ABM de Permisos";
            this.Load += new System.EventHandler(this.frmABMPermisos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDePermisos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvListaDePermisos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNombreDelPermiso;
        private System.Windows.Forms.TextBox txtPermiso;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnVolver;
    }
}