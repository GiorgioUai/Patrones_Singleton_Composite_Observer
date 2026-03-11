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
    /// Formulario de autenticación de usuarios con soporte Multiidioma.
    /// Implementa el patrón Observer para reaccionar a cambios de idioma.
    /// </summary>
    public partial class frmLogIn : Form, IIdiomaObserver
    {
        #region "Atributos Privados"

        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();
        private readonly SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();
        private bool _placeholderActivo = true;

        #endregion

        #region "Constructor y Observer"

        public frmLogIn()
        {
            InitializeComponent();
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            // Suscripción al idioma
            _idiomaManager.Suscribir(this);

            txtPassword.PasswordChar = '*';
            txtPassword.UseSystemPasswordChar = true;

            ActualizarIdioma();
        }

        private void frmLogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Desuscripción obligatoria
            _idiomaManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación IIdiomaObserver"

        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);

            if (_placeholderActivo) EstablecerPlaceholder();

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        #endregion

        #region "Lógica de UX y Login"

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

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBL negocio = new UsuarioBL();
                string emailParaLogin = _placeholderActivo ? "" : txtUsuario.Text;

                if (negocio.LogIn(emailParaLogin, txtPassword.Text))
                {
                    var usuarioLogueado = _sesionManager._Usuario;
                    string msgExito = _idiomaManager.ObtenerTexto("msg_SesionIniciada");

                    MessageBox.Show($"{msgExito}\nBienvenido: {usuarioLogueado.Nombre}", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmPrincipal principal = new frmPrincipal();
                    principal.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_ErrorLogin"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void btnSalir_Click(object sender, EventArgs e) => Application.Exit();

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            try
            {
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

                UsuarioBE nuevoUsuario = new UsuarioBE
                {
                    Email = txtUsuario.Text,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    IdIdioma = _idiomaManager.IdiomaActual.Id
                };

                UsuarioBL negocio = new UsuarioBL();
                if (negocio.Registrar(nuevoUsuario, txtPassword.Text))
                {
                    MessageBox.Show(_idiomaManager.ObtenerTexto("msg_RegistroExitoso"),
                                    _idiomaManager.ObtenerTexto("cap_Registro"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void limpiarCampos()
        {
            if (txtNombre != null) txtNombre.Clear();
            if (txtApellido != null) txtApellido.Clear();
            txtPassword.Text = string.Empty;
            txtUsuario.Text = "";
            EstablecerPlaceholder();
            txtUsuario.Focus();
        }

        #endregion
    }
}