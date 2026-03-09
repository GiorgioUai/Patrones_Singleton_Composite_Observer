using System;
using System.Windows.Forms;
using BE;
using BL;

namespace UI
{
    /// <summary>
    /// Formulario para el Alta, Baja y Modificación de Permisos (Patentes simples).
    /// Permite la depuración de registros duplicados y gestión de seguridad atómica.
    /// </summary>
    public partial class frmABMPermisos : Form
    {
        #region "Atributos y Constructor"

        private readonly PermisoBL _permisoBL;

        public frmABMPermisos()
        {
            InitializeComponent();
            _permisoBL = new PermisoBL();
            ConfigurarEsteticaFormulario();
        }

        #endregion

        #region "Eventos de Formulario"

        /// <summary>
        /// Garantiza que al abrir la ventana, los datos ya estén cargados y la grilla configurada.
        /// </summary>
        private void frmABMPermisos_Load(object sender, EventArgs e)
        {
            ConfigurarGrilla();
            CargarGrilla();
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
                MessageBox.Show("El nombre del permiso no puede estar vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PermisoBE nuevoPermiso = new PermisoBE();
                nuevoPermiso.Nombre = txtPermiso.Text.Trim();

                _permisoBL.GuardarPermisoSimple(nuevoPermiso);

                MessageBox.Show("Permiso guardado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                var rta = MessageBox.Show($"¿Está seguro de eliminar el permiso '{permiso.Nombre}' (ID: {permiso.Id})?\nEsta acción es irreversible.",
                                          "Confirmación de Limpieza", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rta == DialogResult.Yes)
                {
                    try
                    {
                        _permisoBL.EliminarPermiso(permiso);
                        MessageBox.Show("Registro eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Por favor, seleccione un registro de la lista para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Cierra el formulario para retornar al menú principal.
        /// </summary>
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
                HeaderText = "Descripción del Permiso",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        /// <summary>
        /// Configura la estética del formulario y prepara los tags para multi-idioma.
        /// </summary>
        private void ConfigurarEsteticaFormulario()
        {
            this.Text = "Administración de Permisos (Patentes)";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Aseguramos que el botón Volver tenga su Tag para traducción
            if (btnVolver != null) btnVolver.Tag = "btn_Volver";
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