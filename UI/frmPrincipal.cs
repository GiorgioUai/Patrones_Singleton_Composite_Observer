using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
using BL;

namespace UI
{
    /// <summary>
    /// Formulario contenedor principal (MDI). 
    /// Implementa ISesionObserver e IIdiomaObserver para reaccionar a cambios de seguridad e idioma.
    /// </summary>
    public partial class frmPrincipal : Form, ISesionObserver, IIdiomaObserver
    {
        #region "Atributos / Variables Privadas"

        private SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();
        private IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();

        #endregion

        #region "Constructor"

        public frmPrincipal()
        {
            InitializeComponent();
        }

        #endregion

        #region "Eventos del Formulario"

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            idiomasToolStripMenuItem.DropDownDirection = ToolStripDropDownDirection.BelowLeft;

            _sesionManager.Suscribir(this);
            _idiomaManager.Suscribir(this);

            ActualizarSesion();
            ActualizarIdioma();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_sesionManager._Usuario != null)
            {
                string mensaje = _idiomaManager.ObtenerTexto("msg_ConfirmarLogout");
                string titulo = _idiomaManager.ObtenerTexto("cap_Confirmacion");

                DialogResult respuesta = MessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                _sesionManager.LogOut();
            }

            _sesionManager.Desuscribir(this);
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación de Interfaces (Observer)"

        public void ActualizarSesion()
        {
            ValidarSeguridadMenu(this.MainMenuStrip.Items);
        }

        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de Seguridad (Permisos)"

        private void ValidarSeguridadMenu(ToolStripItemCollection pItems)
        {
            foreach (ToolStripItem item in pItems)
            {
                if (item.Tag != null)
                {
                    string permisoRequerido = item.Tag.ToString();
                    item.Visible = _sesionManager.TienePermiso(permisoRequerido);
                }

                if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                {
                    ValidarSeguridadMenu(menuItem.DropDownItems);
                }
            }
        }

        #endregion

        #region "Eventos del Menú Seguridad"

        private void gestionDePermisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmABMPermisos formPermisos = new frmABMPermisos();
            formPermisos.ShowDialog();
        }

        private void gestionDeRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmABMRoles formRoles = new frmABMRoles();
            formRoles.ShowDialog();
        }

        /// <summary>
        /// Integración del nuevo formulario de Gestión de Usuarios y Permisos.
        /// Se implementa lógica para evitar múltiples instancias abiertas.
        /// </summary>
        private void asignarPermisosYRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Buscamos si el formulario ya está abierto
            Form frmEncontrado = this.MdiChildren.FirstOrDefault(x => x is frmGestionUsuariosPermiso);

            if (frmEncontrado != null)
            {
                // Si existe, lo traemos al frente
                frmEncontrado.Activate();
            }
            else
            {
                // Si no existe, creamos la instancia y la mostramos como hija MDI
                frmGestionUsuariosPermiso form = new frmGestionUsuariosPermiso();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void blanqueoDeContrasenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo en desarrollo.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region "Eventos de Cambio de Idioma (Menú)"

        private void españolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _idiomaManager.CambiarIdioma("ES");
        }

        private void inglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _idiomaManager.CambiarIdioma("EN");
        }

        #endregion

        #region "Cierre de Sesión"

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

            if (_sesionManager._Usuario == null)
            {
                frmLogIn login = (frmLogIn)Application.OpenForms["frmLogIn"];
                if (login != null)
                {
                    login.limpiarCampos();
                    login.Show();
                }
            }
        }

        #endregion
    }
}