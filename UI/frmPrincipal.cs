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

        private readonly SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();
        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();

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
            // Ajuste estético de menús
            idiomasToolStripMenuItem.DropDownDirection = ToolStripDropDownDirection.BelowLeft;

            // SUSCRIPCIÓN AL PATRÓN OBSERVER
            _sesionManager.Suscribir(this);
            _idiomaManager.Suscribir(this);

            // Inicialización de estado
            ActualizarSesion();
            ActualizarIdioma();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_sesionManager._Usuario != null)
            {
                string mensaje = _idiomaManager.ObtenerTexto("msg_ConfirmarLogout") ?? "¿Desea cerrar la sesión actual?";
                string titulo = _idiomaManager.ObtenerTexto("cap_Confirmacion") ?? "Confirmación";

                DialogResult respuesta = MessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                _sesionManager.LogOut();
            }

            // DESUSCRIPCIÓN OBLIGATORIA: Limpieza de referencias en Singletons
            _sesionManager.Desuscribir(this);
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación de Interfaces (Observer)"

        /// <summary>
        /// Responde a cambios en la sesión disparando la validación recursiva de menús.
        /// </summary>
        public void ActualizarSesion()
        {
            if (this.MainMenuStrip != null)
            {
                ValidarSeguridadMenu(this.MainMenuStrip.Items);
            }
        }

        /// <summary>
        /// Traduce los menús y controles del formulario principal.
        /// </summary>
        public void ActualizarIdioma()
        {
            // Traduce controles y menús (Traductor debe manejar ToolStripItems)
            Traductor.Traducir(this.Controls);

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de Seguridad (Permisos Composite)"

        /// <summary>
        /// Navega recursivamente el menú principal habilitando/ocultando ítems según el Composite de seguridad.
        /// </summary>
        private void ValidarSeguridadMenu(ToolStripItemCollection pItems)
        {
            foreach (ToolStripItem item in pItems)
            {
                // Si el item tiene un Tag, validamos contra el árbol de permisos del usuario
                if (item.Tag != null)
                {
                    string permisoRequerido = item.Tag.ToString();
                    item.Visible = _sesionManager.TienePermiso(permisoRequerido);
                }
                else
                {
                    // Si no tiene Tag, lo dejamos visible por defecto (ej: menú "Archivo" o "Cerrar")
                    item.Visible = true;
                }

                // Recursividad: Si es un menú desplegable, validamos sus hijos
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
        /// Gestiona la apertura de la ventana de gestión de traducciones como hija MDI.
        /// </summary>
        private void gestionDeIdiomasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmEncontrado = this.MdiChildren.FirstOrDefault(x => x is frmABMTraducciones);

            if (frmEncontrado != null)
            {
                frmEncontrado.Activate();
            }
            else
            {
                frmABMTraducciones form = new frmABMTraducciones();
                form.MdiParent = this;
                form.Show();
            }
        }

        /// <summary>
        /// Gestiona la apertura de la ventana de asignación de seguridad como hija MDI.
        /// </summary>
        private void asignarPermisosYRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmEncontrado = this.MdiChildren.FirstOrDefault(x => x is frmGestionUsuariosPermiso);

            if (frmEncontrado != null)
            {
                frmEncontrado.Activate();
            }
            else
            {
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

        #region "Cierre de Sesión y Navegación"

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Dispara FormClosing donde se maneja el LogOut y la desuscripción
            this.Close();

            // Si el cierre fue efectivo (no cancelado), volvemos al Login
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