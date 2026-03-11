using System;
using System.Windows.Forms;
using BE;
using BE.Interfaces;
using BL;

namespace UI
{
    /// <summary>
    /// Formulario para el Alta, Baja y Modificación de Permisos (Patentes simples).
    /// Permite la depuración de registros duplicados y gestión de seguridad atómica.
    /// Implementa el patrón Observer para reaccionar a cambios de idioma y sesión.
    /// </summary>
    public partial class frmABMPermisos : Form, IIdiomaObserver, ISesionObserver
    {
        #region "Atributos y Constructor"

        private readonly PermisoBL _permisoBL;
        private readonly IdiomaManagerBL _idiomaManager = IdiomaManagerBL.GetInstance();
        private readonly SesionManagerBL _sesionManager = SesionManagerBL.GetInstance();

        public frmABMPermisos()
        {
            InitializeComponent();
            _permisoBL = new PermisoBL();
            ConfigurarEsteticaFormulario();
        }

        #endregion

        #region "Eventos de Formulario (Ciclo de Vida)"

        /// <summary>
        /// Garantiza que al abrir la ventana, se suscriba a los observadores y cargue los datos.
        /// </summary>
        private void frmABMPermisos_Load(object sender, EventArgs e)
        {
            // Suscripción al patrón Observer
            _idiomaManager.Suscribir(this);
            _sesionManager.Suscribir(this);

            ConfigurarGrilla();
            CargarGrilla();

            // Ejecución inicial de actualizaciones
            ActualizarIdioma();
            ActualizarSesion();
        }

        /// <summary>
        /// Desuscripción obligatoria de los Managers para evitar fugas de memoria.
        /// </summary>
        private void frmABMPermisos_FormClosing(object sender, FormClosingEventArgs e)
        {
            _idiomaManager.Desuscribir(this);
            _sesionManager.Desuscribir(this);
        }

        #endregion

        #region "Implementación de Interfaces (Observer)"

        /// <summary>
        /// Traduce los controles del formulario y el título según el idioma actual.
        /// </summary>
        public void ActualizarIdioma()
        {
            Traductor.Traducir(this.Controls);

            if (this.Tag != null)
                this.Text = _idiomaManager.ObtenerTexto(this.Tag.ToString());
        }

        /// <summary>
        /// Valida en tiempo real si el usuario mantiene el permiso para gestionar patentes.
        /// </summary>
        public void ActualizarSesion()
        {
            // Verificamos el permiso específico para este módulo ABM
            if (!_sesionManager.TienePermiso("Seguridad_GestionPermisos"))
            {
                this.Close();
            }
        }

        #endregion

        #region "Eventos de Botones"

        /// <summary>
        /// Registra un nuevo permiso simple. La BL validará si el nombre ya existe.
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPermiso.Text))
            {
                string mensajeVacío = _idiomaManager.ObtenerTexto("msg_CampoVacio");
                string tituloAtencion = _idiomaManager.ObtenerTexto("cap_Atencion");
                MessageBox.Show(mensajeVacío ?? "El nombre del permiso no puede estar vacío.", tituloAtencion ?? "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PermisoBE nuevoPermiso = new PermisoBE();
                nuevoPermiso.Nombre = txtPermiso.Text.Trim();

                _permisoBL.GuardarPermisoSimple(nuevoPermiso);

                string mensajeExito = _idiomaManager.ObtenerTexto("msg_GuardadoExitoso");
                MessageBox.Show(mensajeExito ?? "Permiso guardado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FinalizarOperacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Permite eliminar registros, útil para limpiar duplicados de la base de datos.
        /// </summary>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvListaDePermisos.CurrentRow != null)
            {
                var permiso = (PermisoBE)dgvListaDePermisos.CurrentRow.DataBoundItem;

                string msgConfirmar = _idiomaManager.ObtenerTexto("msg_ConfirmarEliminar");
                string capConfirmar = _idiomaManager.ObtenerTexto("cap_Confirmacion");

                var rta = MessageBox.Show($"{msgConfirmar} '{permiso.Nombre}'?\n(ID: {permiso.Id})",
                                          capConfirmar ?? "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rta == DialogResult.Yes)
                {
                    try
                    {
                        _permisoBL.EliminarPermiso(permiso);
                        MessageBox.Show(_idiomaManager.ObtenerTexto("msg_EliminadoExitoso"), "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FinalizarOperacion();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se pudo eliminar: " + ex.Message, "Error de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(_idiomaManager.ObtenerTexto("msg_SeleccioneRegistro"), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FinalizarOperacion();
        }

        #endregion

        #region "Métodos de Soporte y UX"

        /// <summary>
        /// Configura el aspecto visual de la grilla para ocultar datos técnicos del Composite.
        /// </summary>
        private void ConfigurarGrilla()
        {
            dgvListaDePermisos.AutoGenerateColumns = false;
            dgvListaDePermisos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListaDePermisos.MultiSelect = false;
            dgvListaDePermisos.ReadOnly = true;
            dgvListaDePermisos.RowHeadersVisible = false;
            dgvListaDePermisos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.Beige;

            dgvListaDePermisos.Columns.Clear();

            // Columna ID
            dgvListaDePermisos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });

            // Columna Nombre
            dgvListaDePermisos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Descripción", // Se traducirá vía Traductor.Traducir
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        /// <summary>
        /// Configura la estética del formulario y prepara los tags para multi-idioma.
        /// </summary>
        private void ConfigurarEsteticaFormulario()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Aseguramos que el botón Volver tenga su Tag para traducción
            if (btnVolver != null) btnVolver.Tag = "btn_Volver";
            // Tag para el título del formulario (se usa en ActualizarIdioma)
            this.Tag = "frm_ABMPermisos";
        }

        private void CargarGrilla()
        {
            try
            {
                dgvListaDePermisos.DataSource = null;
                dgvListaDePermisos.DataSource = _permisoBL.ListarPermisosSimples();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista: " + ex.Message);
            }
        }

        private void FinalizarOperacion()
        {
            txtPermiso.Clear();
            CargarGrilla();
            txtPermiso.Focus();
        }

        #endregion
    }
}