namespace UI
{
    partial class frmABMRoles
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
            this.dgvListaDeRoles = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDatosDelRol = new System.Windows.Forms.TextBox();
            this.dgvCatalogoDeComponentes = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.tvEstructuraDelRol = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDeRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalogoDeComponentes)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvListaDeRoles
            // 
            this.dgvListaDeRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaDeRoles.Location = new System.Drawing.Point(23, 101);
            this.dgvListaDeRoles.Name = "dgvListaDeRoles";
            this.dgvListaDeRoles.Size = new System.Drawing.Size(260, 250);
            this.dgvListaDeRoles.TabIndex = 0;
            this.dgvListaDeRoles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListaDeRoles_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Tag = "lbl_Lista_de_Roles";
            this.label1.Text = "Lista de Roles";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Tag = "lbl_NombreRol";
            this.label2.Text = "Nombre del Rol";
            // 
            // txtDatosDelRol
            // 
            this.txtDatosDelRol.Location = new System.Drawing.Point(124, 12);
            this.txtDatosDelRol.Name = "txtDatosDelRol";
            this.txtDatosDelRol.Size = new System.Drawing.Size(159, 20);
            this.txtDatosDelRol.TabIndex = 3;
            // 
            // dgvCatalogoDeComponentes
            // 
            this.dgvCatalogoDeComponentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCatalogoDeComponentes.Location = new System.Drawing.Point(315, 101);
            this.dgvCatalogoDeComponentes.Name = "dgvCatalogoDeComponentes";
            this.dgvCatalogoDeComponentes.Size = new System.Drawing.Size(260, 250);
            this.dgvCatalogoDeComponentes.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(312, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 5;
            this.label3.Tag = "lbl_Catalogo_de_Componentes";
            this.label3.Text = "Catalogo de Componentes";
            // 
            // tvEstructuraDelRol
            // 
            this.tvEstructuraDelRol.Location = new System.Drawing.Point(629, 101);
            this.tvEstructuraDelRol.Name = "tvEstructuraDelRol";
            this.tvEstructuraDelRol.Size = new System.Drawing.Size(399, 250);
            this.tvEstructuraDelRol.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(635, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 7;
            this.label4.Tag = "lbl_Estructura_del_Rol";
            this.label4.Text = "Estructura del Rol";
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(315, 404);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(121, 49);
            this.btnAgregar.TabIndex = 8;
            this.btnAgregar.Tag = "btn_Agregar";
            this.btnAgregar.Text = "Agregar >>";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnQuitar
            // 
            this.btnQuitar.Location = new System.Drawing.Point(454, 404);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(121, 49);
            this.btnQuitar.TabIndex = 9;
            this.btnQuitar.Tag = "btn_Quitar";
            this.btnQuitar.Text = "<< Quitar";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(661, 404);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(153, 49);
            this.btnGuardar.TabIndex = 10;
            this.btnGuardar.Tag = "btn_Guardar";
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(852, 404);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(153, 49);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Tag = "btn_Cancelar";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmABMRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1055, 495);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnQuitar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tvEstructuraDelRol);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvCatalogoDeComponentes);
            this.Controls.Add(this.txtDatosDelRol);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvListaDeRoles);
            this.Name = "frmABMRoles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frm_ABM_de_Roles";
            this.Text = "ABM de Roles";
            this.Load += new System.EventHandler(this.frmABMRoles_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDeRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalogoDeComponentes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvListaDeRoles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDatosDelRol;
        private System.Windows.Forms.DataGridView dgvCatalogoDeComponentes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView tvEstructuraDelRol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}