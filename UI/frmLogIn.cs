using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
using BL;

namespace UI
{
    /// <summary>
    /// Formulario de autenticación de usuarios con soporte Multiidioma y UX mejorada.
    /// </summary>
    public partial class frmLogIn : Form, IIdiomaObserver
    {
        #region "Atributos / Variables Privadas"

        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();
        private bool _placeholderActivo = true;

        #endregion

        #region "Constructor"

        public frmLogIn()
        {
            InitializeComponent();
        }

        #endregion

        #region "Eventos del Formulario"

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            _idiomaManager.Suscribir(this);

            // SEGURIDAD: Configuramos la máscara de password por código
            txtPassword.PasswordChar = '*';
            txtPassword.UseSystemPasswordChar = true;

            ActualizarIdioma();
        }

        private void frmLogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación de IIdiomaObserver"

        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);

            if (_placeholderActivo)
            {
                EstablecerPlaceholder();
            }

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de Placeholder (Email)"

        private void EstablecerPlaceholder()
        {
            txtUsuario.Text = _idiomaManager.ObtenerTexto("txt_Placeholder_Email");
            txtUsuario.ForeColor = Color.Gray;
            _placeholderActivo = true;
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            string placeholderTraducido = _idiomaManager.ObtenerTexto("txt_Placeholder_Email");

            if (txtUsuario.Text == placeholderTraducido)
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
                _placeholderActivo = false;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                EstablecerPlaceholder();
            }
        }

        #endregion

        #region "Eventos de Botones (Lógica de Negocio)"

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBL negocio = new UsuarioBL();
                string emailParaLogin = _placeholderActivo ? "" : txtUsuario.Text;

                bool exito = negocio.LogIn(emailParaLogin, txtPassword.Text);

                if (exito)
                {
                    var usuarioLogueado = SesionManagerBL.GetInstance()._Usuario;
                    string msgExito = _idiomaManager.ObtenerTexto("msg_SesionIniciada");
                    string tituloExito = _idiomaManager.ObtenerTexto("btn_Ingresar");

                    MessageBox.Show($"{msgExito}\nBienvenido: {usuarioLogueado.Nombre} {usuarioLogueado.Apellido}",
                                    tituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmPrincipal principal = new frmPrincipal();
                    principal.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_ErrorLogin"),
                                    _idiomaManager.ObtenerTexto("frm_LogIn"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Gestiona el registro de nuevos usuarios con validación de campos y asignación de Rol automático.
        /// </summary>
        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar que los campos no estén vacíos (Placeholder incluido)
                if (_placeholderActivo || string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_CamposIncompletos"),
                                    _idiomaManager.ObtenerTexto("cap_Atencion"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Crear objeto BE con los datos de la UI
                UsuarioBE nuevoUsuario = new UsuarioBE
                {
                    Email = txtUsuario.Text,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    IdIdioma = _idiomaManager.IdiomaActual.Id // Hereda el idioma activo en la app
                };

                // 3. Llamar a la BL para procesar registro, hash y Rol base
                UsuarioBL negocio = new UsuarioBL();
                bool exito = negocio.Registrar(nuevoUsuario, txtPassword.Text);

                if (exito)
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_RegistroExitoso"),
                                    _idiomaManager.ObtenerTexto("cap_Registro"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region "Métodos de Soporte"

        /// <summary>
        /// Limpia los campos de texto y restablece placeholders. 
        /// AHORA ES PUBLIC para ser llamado desde el formulario principal.
        /// </summary>
        public void limpiarCampos()
        {
            if (txtNombre != null) txtNombre.Clear();
            if (txtApellido != null) txtApellido.Clear();

            // Borrado físico de la clave
            txtPassword.Text = string.Empty;
            txtPassword.Clear();

            txtUsuario.Text = "";
            EstablecerPlaceholder();

            txtUsuario.Focus();
        }

        #endregion
    }
}