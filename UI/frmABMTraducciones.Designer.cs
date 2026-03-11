namespace UI
{
    partial class frmABMTraducciones
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
            this.cmbIdiomas = new System.Windows.Forms.ComboBox();
            this.dgvTraducciones = new System.Windows.Forms.DataGridView();
            this.txtNombreTag = new System.Windows.Forms.TextBox();
            this.txtTraduccion = new System.Windows.Forms.TextBox();
            this.lblSeleccionarIdioma = new System.Windows.Forms.Label();
            this.lblNombreTag = new System.Windows.Forms.Label();
            this.lblTextoTraduccion = new System.Windows.Forms.Label();
            this.btnNuevoTag = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.lblListadoTraducciones = new System.Windows.Forms.Label();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraducciones)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbIdiomas
            // 
            this.cmbIdiomas.FormattingEnabled = true;
            this.cmbIdiomas.Location = new System.Drawing.Point(229, 16);
            this.cmbIdiomas.Name = "cmbIdiomas";
            this.cmbIdiomas.Size = new System.Drawing.Size(136, 21);
            this.cmbIdiomas.TabIndex = 0;
            this.cmbIdiomas.SelectedIndexChanged += new System.EventHandler(this.cmbIdiomas_SelectedIndexChanged);
            // 
            // dgvTraducciones
            // 
            this.dgvTraducciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraducciones.Location = new System.Drawing.Point(43, 86);
            this.dgvTraducciones.Name = "dgvTraducciones";
            this.dgvTraducciones.Size = new System.Drawing.Size(403, 332);
            this.dgvTraducciones.TabIndex = 1;
            this.dgvTraducciones.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTraducciones_CellClick);
            // 
            // txtNombreTag
            // 
            this.txtNombreTag.Location = new System.Drawing.Point(692, 17);
            this.txtNombreTag.Name = "txtNombreTag";
            this.txtNombreTag.Size = new System.Drawing.Size(140, 20);
            this.txtNombreTag.TabIndex = 2;
            // 
            // txtTraduccion
            // 
            this.txtTraduccion.Location = new System.Drawing.Point(528, 86);
            this.txtTraduccion.Multiline = true;
            this.txtTraduccion.Name = "txtTraduccion";
            this.txtTraduccion.Size = new System.Drawing.Size(403, 332);
            this.txtTraduccion.TabIndex = 3;
            // 
            // lblSeleccionarIdioma
            // 
            this.lblSeleccionarIdioma.AutoSize = true;
            this.lblSeleccionarIdioma.Location = new System.Drawing.Point(102, 20);
            this.lblSeleccionarIdioma.Name = "lblSeleccionarIdioma";
            this.lblSeleccionarIdioma.Size = new System.Drawing.Size(97, 13);
            this.lblSeleccionarIdioma.TabIndex = 4;
            this.lblSeleccionarIdioma.Tag = "lbl_Seleccionar_Idioma";
            this.lblSeleccionarIdioma.Text = "Seleccionar Idioma";
            // 
            // lblNombreTag
            // 
            this.lblNombreTag.AutoSize = true;
            this.lblNombreTag.Location = new System.Drawing.Point(592, 20);
            this.lblNombreTag.Name = "lblNombreTag";
            this.lblNombreTag.Size = new System.Drawing.Size(83, 13);
            this.lblNombreTag.TabIndex = 5;
            this.lblNombreTag.Tag = "lbl_Nombre_Tag";
            this.lblNombreTag.Text = "Nombre del Tag";
            // 
            // lblTextoTraduccion
            // 
            this.lblTextoTraduccion.AutoSize = true;
            this.lblTextoTraduccion.Location = new System.Drawing.Point(525, 70);
            this.lblTextoTraduccion.Name = "lblTextoTraduccion";
            this.lblTextoTraduccion.Size = new System.Drawing.Size(122, 13);
            this.lblTextoTraduccion.TabIndex = 6;
            this.lblTextoTraduccion.Tag = "lbl_Texto_Traduccion";
            this.lblTextoTraduccion.Text = "Texto para la traduccion";
            // 
            // btnNuevoTag
            // 
            this.btnNuevoTag.Location = new System.Drawing.Point(325, 463);
            this.btnNuevoTag.Name = "btnNuevoTag";
            this.btnNuevoTag.Size = new System.Drawing.Size(82, 39);
            this.btnNuevoTag.TabIndex = 7;
            this.btnNuevoTag.Tag = "btn_Nuevo_Tag";
            this.btnNuevoTag.Text = "NUEVO";
            this.btnNuevoTag.UseVisualStyleBackColor = true;
            this.btnNuevoTag.Click += new System.EventHandler(this.btnNuevoTag_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(441, 463);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(82, 39);
            this.btnModificar.TabIndex = 8;
            this.btnModificar.Tag = "btn_Modificar";
            this.btnModificar.Text = "MODIFICAR";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(565, 463);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(82, 39);
            this.btnEliminar.TabIndex = 9;
            this.btnEliminar.Tag = "btn_Eliminar";
            this.btnEliminar.Text = "ELIMINAR";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // lblListadoTraducciones
            // 
            this.lblListadoTraducciones.AutoSize = true;
            this.lblListadoTraducciones.Location = new System.Drawing.Point(40, 70);
            this.lblListadoTraducciones.Name = "lblListadoTraducciones";
            this.lblListadoTraducciones.Size = new System.Drawing.Size(124, 13);
            this.lblListadoTraducciones.TabIndex = 10;
            this.lblListadoTraducciones.Tag = "lbl_Listado_Traducciones";
            this.lblListadoTraducciones.Text = "Listado de Traducciones";
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(565, 522);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(82, 39);
            this.btnVolver.TabIndex = 13;
            this.btnVolver.Tag = "btn_Volver";
            this.btnVolver.Text = "VOLVER";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(441, 522);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(82, 39);
            this.btnGuardar.TabIndex = 12;
            this.btnGuardar.Tag = "btn_Guardar";
            this.btnGuardar.Text = "GUARDAR";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(325, 522);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(82, 39);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Tag = "btn_Cancelar";
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmABMTraducciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 575);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblListadoTraducciones);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnNuevoTag);
            this.Controls.Add(this.lblTextoTraduccion);
            this.Controls.Add(this.lblNombreTag);
            this.Controls.Add(this.lblSeleccionarIdioma);
            this.Controls.Add(this.txtTraduccion);
            this.Controls.Add(this.txtNombreTag);
            this.Controls.Add(this.dgvTraducciones);
            this.Controls.Add(this.cmbIdiomas);
            this.Name = "frmABMTraducciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frm_ABM_Traducciones";
            this.Text = "ABM de Traducciones";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmABMTraducciones_FormClosing);
            this.Load += new System.EventHandler(this.frmABMTraducciones_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraducciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbIdiomas;
        private System.Windows.Forms.DataGridView dgvTraducciones;
        private System.Windows.Forms.TextBox txtNombreTag;
        private System.Windows.Forms.TextBox txtTraduccion;
        private System.Windows.Forms.Label lblSeleccionarIdioma;
        private System.Windows.Forms.Label lblNombreTag;
        private System.Windows.Forms.Label lblTextoTraduccion;
        private System.Windows.Forms.Button btnNuevoTag;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Label lblListadoTraducciones;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}