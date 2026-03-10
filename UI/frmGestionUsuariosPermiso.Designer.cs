namespace UI
{
    partial class frmGestionUsuariosPermiso
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGestionUsuariosPermiso));
            this.dgvListaDeUsuarios = new System.Windows.Forms.DataGridView();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtFiltro = new System.Windows.Forms.TextBox();
            this.tvCatalogoPermisos = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tvPermisosAsignados = new System.Windows.Forms.TreeView();
            this.lblListaUsuarios = new System.Windows.Forms.Label();
            this.lblListaDeRoles_Permisos = new System.Windows.Forms.Label();
            this.lblAsignacionUsuarioPermisos = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDeUsuarios)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvListaDeUsuarios
            // 
            this.dgvListaDeUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaDeUsuarios.Location = new System.Drawing.Point(25, 122);
            this.dgvListaDeUsuarios.Name = "dgvListaDeUsuarios";
            this.dgvListaDeUsuarios.Size = new System.Drawing.Size(303, 382);
            this.dgvListaDeUsuarios.TabIndex = 0;
            this.dgvListaDeUsuarios.Tag = "dgv_Lista_de_Usuarios";
            this.dgvListaDeUsuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListaDeUsuarios_CellClick);
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(41, 49);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(43, 13);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Tag = "lbl_Usuario";
            this.lblUsuario.Text = "Usuario";
            // 
            // txtFiltro
            // 
            this.txtFiltro.Location = new System.Drawing.Point(101, 46);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(215, 20);
            this.txtFiltro.TabIndex = 2;
            this.txtFiltro.TextChanged += new System.EventHandler(this.txtFiltro_TextChanged);
            // 
            // tvCatalogoPermisos
            // 
            this.tvCatalogoPermisos.ImageIndex = 0;
            this.tvCatalogoPermisos.ImageList = this.imageList1;
            this.tvCatalogoPermisos.Location = new System.Drawing.Point(371, 121);
            this.tvCatalogoPermisos.Name = "tvCatalogoPermisos";
            this.tvCatalogoPermisos.SelectedImageIndex = 0;
            this.tvCatalogoPermisos.Size = new System.Drawing.Size(317, 382);
            this.tvCatalogoPermisos.TabIndex = 3;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icons8-folder-16.png");
            this.imageList1.Images.SetKeyName(1, "icons8-key-16.png");
            // 
            // tvPermisosAsignados
            // 
            this.tvPermisosAsignados.ImageIndex = 0;
            this.tvPermisosAsignados.ImageList = this.imageList1;
            this.tvPermisosAsignados.Location = new System.Drawing.Point(860, 121);
            this.tvPermisosAsignados.Name = "tvPermisosAsignados";
            this.tvPermisosAsignados.SelectedImageIndex = 0;
            this.tvPermisosAsignados.Size = new System.Drawing.Size(317, 382);
            this.tvPermisosAsignados.TabIndex = 4;
            // 
            // lblListaUsuarios
            // 
            this.lblListaUsuarios.AutoSize = true;
            this.lblListaUsuarios.Location = new System.Drawing.Point(31, 97);
            this.lblListaUsuarios.Name = "lblListaUsuarios";
            this.lblListaUsuarios.Size = new System.Drawing.Size(88, 13);
            this.lblListaUsuarios.TabIndex = 7;
            this.lblListaUsuarios.Tag = "lbl_Lista_Usuarios";
            this.lblListaUsuarios.Text = "Lista de Usuarios";
            // 
            // lblListaDeRoles_Permisos
            // 
            this.lblListaDeRoles_Permisos.AutoSize = true;
            this.lblListaDeRoles_Permisos.Location = new System.Drawing.Point(376, 95);
            this.lblListaDeRoles_Permisos.Name = "lblListaDeRoles_Permisos";
            this.lblListaDeRoles_Permisos.Size = new System.Drawing.Size(127, 13);
            this.lblListaDeRoles_Permisos.TabIndex = 8;
            this.lblListaDeRoles_Permisos.Tag = "lbl_Lista_Roles_Permisos";
            this.lblListaDeRoles_Permisos.Text = "Lista de Roles y Permisos";
            // 
            // lblAsignacionUsuarioPermisos
            // 
            this.lblAsignacionUsuarioPermisos.AutoSize = true;
            this.lblAsignacionUsuarioPermisos.Location = new System.Drawing.Point(819, 91);
            this.lblAsignacionUsuarioPermisos.Name = "lblAsignacionUsuarioPermisos";
            this.lblAsignacionUsuarioPermisos.Size = new System.Drawing.Size(168, 13);
            this.lblAsignacionUsuarioPermisos.TabIndex = 9;
            this.lblAsignacionUsuarioPermisos.Tag = "lbl_Asignacion_Usuarios_Permisos";
            this.lblAsignacionUsuarioPermisos.Text = "Asigancios de Permisos al Usuario";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(411, 536);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(118, 43);
            this.btnGuardar.TabIndex = 10;
            this.btnGuardar.Tag = "btn_Guardar";
            this.btnGuardar.Text = "GUARDAR";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(606, 536);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(118, 43);
            this.btnVolver.TabIndex = 11;
            this.btnVolver.Tag = "btn_Volver";
            this.btnVolver.Text = "VOLVER";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlAcciones.Controls.Add(this.btnQuitar);
            this.pnlAcciones.Controls.Add(this.btnAgregar);
            this.pnlAcciones.Location = new System.Drawing.Point(709, 121);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(128, 383);
            this.pnlAcciones.TabIndex = 12;
            // 
            // btnQuitar
            // 
            this.btnQuitar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnQuitar.Location = new System.Drawing.Point(24, 219);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(91, 36);
            this.btnQuitar.TabIndex = 8;
            this.btnQuitar.Tag = "btn_Quitar";
            this.btnQuitar.Text = "<< QUITAR";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAgregar.Location = new System.Drawing.Point(24, 124);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(91, 36);
            this.btnAgregar.TabIndex = 7;
            this.btnAgregar.Tag = "btn_Agregar";
            this.btnAgregar.Text = "AGREGAR >>";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // frmGestionUsuariosPermiso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 596);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.lblAsignacionUsuarioPermisos);
            this.Controls.Add(this.lblListaDeRoles_Permisos);
            this.Controls.Add(this.lblListaUsuarios);
            this.Controls.Add(this.tvPermisosAsignados);
            this.Controls.Add(this.tvCatalogoPermisos);
            this.Controls.Add(this.txtFiltro);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.dgvListaDeUsuarios);
            this.Name = "frmGestionUsuariosPermiso";
            this.Tag = "frm_GestionUsuariosPermiso";
            this.Text = "Asignacion de Permisos y Roles a Usuarios";
            this.Load += new System.EventHandler(this.frmGestionUsuariosPermiso_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDeUsuarios)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvListaDeUsuarios;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtFiltro;
        private System.Windows.Forms.TreeView tvCatalogoPermisos;
        private System.Windows.Forms.TreeView tvPermisosAsignados;
        private System.Windows.Forms.Label lblListaUsuarios;
        private System.Windows.Forms.Label lblListaDeRoles_Permisos;
        private System.Windows.Forms.Label lblAsignacionUsuarioPermisos;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnAgregar;
    }
}