namespace UI
{
    partial class frmPrincipal
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idiomasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.españolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seguridadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDePermisosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestiondeRolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asignarPermisosYRolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDeIdiomasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blanqueoDeContraseñaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambioDeContraseñaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cerrarToolStripMenuItem,
            this.idiomasToolStripMenuItem,
            this.seguridadToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1149, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // cerrarToolStripMenuItem
            // 
            this.cerrarToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.cerrarToolStripMenuItem.Tag = "menu_cerrar";
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // idiomasToolStripMenuItem
            // 
            this.idiomasToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.idiomasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.españolToolStripMenuItem,
            this.inglesToolStripMenuItem});
            this.idiomasToolStripMenuItem.Name = "idiomasToolStripMenuItem";
            this.idiomasToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.idiomasToolStripMenuItem.Tag = "menu_idiomas";
            this.idiomasToolStripMenuItem.Text = "Idiomas";
            // 
            // españolToolStripMenuItem
            // 
            this.españolToolStripMenuItem.Name = "españolToolStripMenuItem";
            this.españolToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.españolToolStripMenuItem.Tag = "menu_idioma_es";
            this.españolToolStripMenuItem.Text = "Español";
            this.españolToolStripMenuItem.Click += new System.EventHandler(this.españolToolStripMenuItem_Click);
            // 
            // inglesToolStripMenuItem
            // 
            this.inglesToolStripMenuItem.Name = "inglesToolStripMenuItem";
            this.inglesToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.inglesToolStripMenuItem.Tag = "menu_idioma_en";
            this.inglesToolStripMenuItem.Text = "Inglés";
            this.inglesToolStripMenuItem.Click += new System.EventHandler(this.inglesToolStripMenuItem_Click);
            // 
            // seguridadToolStripMenuItem
            // 
            this.seguridadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestionDePermisosToolStripMenuItem,
            this.gestiondeRolesToolStripMenuItem,
            this.asignarPermisosYRolesToolStripMenuItem,
            this.gestionDeIdiomasToolStripMenuItem,
            this.blanqueoDeContraseñaToolStripMenuItem,
            this.cambioDeContraseñaToolStripMenuItem});
            this.seguridadToolStripMenuItem.Name = "seguridadToolStripMenuItem";
            this.seguridadToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.seguridadToolStripMenuItem.Tag = "Seguridad_MenuPrincipal";
            this.seguridadToolStripMenuItem.Text = "Seguridad";
            // 
            // gestionDePermisosToolStripMenuItem
            // 
            this.gestionDePermisosToolStripMenuItem.Name = "gestionDePermisosToolStripMenuItem";
            this.gestionDePermisosToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.gestionDePermisosToolStripMenuItem.Tag = "Seguridad_GestionPermisos";
            this.gestionDePermisosToolStripMenuItem.Text = "Gestion de Permisos";
            this.gestionDePermisosToolStripMenuItem.Click += new System.EventHandler(this.gestionDePermisosToolStripMenuItem_Click);
            // 
            // gestiondeRolesToolStripMenuItem
            // 
            this.gestiondeRolesToolStripMenuItem.Name = "gestiondeRolesToolStripMenuItem";
            this.gestiondeRolesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.gestiondeRolesToolStripMenuItem.Tag = "Seguridad_GestionRoles";
            this.gestiondeRolesToolStripMenuItem.Text = "Gestion de Roles";
            this.gestiondeRolesToolStripMenuItem.Click += new System.EventHandler(this.gestionDeRolesToolStripMenuItem_Click);
            // 
            // asignarPermisosYRolesToolStripMenuItem
            // 
            this.asignarPermisosYRolesToolStripMenuItem.Name = "asignarPermisosYRolesToolStripMenuItem";
            this.asignarPermisosYRolesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.asignarPermisosYRolesToolStripMenuItem.Tag = "Seguridad_Asignacion";
            this.asignarPermisosYRolesToolStripMenuItem.Text = "Asignar Permisos y Roles";
            this.asignarPermisosYRolesToolStripMenuItem.Click += new System.EventHandler(this.asignarPermisosYRolesToolStripMenuItem_Click);
            // 
            // gestionDeIdiomasToolStripMenuItem
            // 
            this.gestionDeIdiomasToolStripMenuItem.Name = "gestionDeIdiomasToolStripMenuItem";
            this.gestionDeIdiomasToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.gestionDeIdiomasToolStripMenuItem.Tag = "Seguridad_GestionIdiomas";
            this.gestionDeIdiomasToolStripMenuItem.Text = "Gestion de Idiomas";
            this.gestionDeIdiomasToolStripMenuItem.Click += new System.EventHandler(this.gestionDeIdiomasToolStripMenuItem_Click);
            // 
            // blanqueoDeContraseñaToolStripMenuItem
            // 
            this.blanqueoDeContraseñaToolStripMenuItem.Name = "blanqueoDeContraseñaToolStripMenuItem";
            this.blanqueoDeContraseñaToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.blanqueoDeContraseñaToolStripMenuItem.Tag = "Seguridad_BlanqueoPass";
            this.blanqueoDeContraseñaToolStripMenuItem.Text = "Blanqueo de Contraseña";
            this.blanqueoDeContraseñaToolStripMenuItem.Click += new System.EventHandler(this.blanqueoDeContrasenaToolStripMenuItem_Click);
            // 
            // cambioDeContraseñaToolStripMenuItem
            // 
            this.cambioDeContraseñaToolStripMenuItem.Name = "cambioDeContraseñaToolStripMenuItem";
            this.cambioDeContraseñaToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.cambioDeContraseñaToolStripMenuItem.Tag = "Seguridad_CambioPass";
            this.cambioDeContraseñaToolStripMenuItem.Text = "Cambio de Contraseña";
            this.cambioDeContraseñaToolStripMenuItem.Click += new System.EventHandler(this.cambioDeContraseñaToolStripMenuItem_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 674);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frm_Principal";
            this.Text = "Formulario Principal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem idiomasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem españolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inglesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seguridadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionDePermisosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestiondeRolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asignarPermisosYRolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionDeIdiomasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blanqueoDeContraseñaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambioDeContraseñaToolStripMenuItem;
    }
}