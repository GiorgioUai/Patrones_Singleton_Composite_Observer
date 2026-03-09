using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// Formulario para la gestión jerárquica de Roles y Permisos (Patrón Composite).
    /// Permite la edición dinámica en memoria antes de la persistencia transaccional.
    /// </summary>
    public partial class frmABMRoles : Form
    {
        #region "Atributos / Variables Privadas"

        private PermisoBL _permisoBL = new PermisoBL();
        private CompuestoBE _rolSeleccionado;

        #endregion

        #region "Constructor"

        public frmABMRoles()
        {
            InitializeComponent();
            ConfigurarEsteticaFormulario();
        }

        #endregion

        #region "Eventos del Formulario"

        private void frmABMRoles_Load(object sender, EventArgs e)
        {
            ConfigurarGrillasVisuales();
            ActualizarGrillas();
        }

        private void dgvListaDeRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Recuperamos el Rol para edición en RAM
                _rolSeleccionado = (CompuestoBE)dgvListaDeRoles.Rows[e.RowIndex].DataBoundItem;
                txtDatosDelRol.Text = _rolSeleccionado.Nombre;

                ActualizarEstructuraVisual(_rolSeleccionado);
            }
        }

        #endregion

        #region "Acciones de Botones (Lógica de Negocio UI)"

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogoDeComponentes.CurrentRow == null || _rolSeleccionado == null) return;

            ComponenteBE seleccionado = (ComponenteBE)dgvCatalogoDeComponentes.CurrentRow.DataBoundItem;

            // Validación de integridad: Evita que un rol se contenga a sí mismo o genere ciclos
            string contenedor = _permisoBL.ObtenerContenedorRecursivo(_rolSeleccionado, seleccionado.Id);

            if (contenedor != null)
            {
                MessageBox.Show($"Operación inválida: El componente ya es parte de '{contenedor}'.",
                                "Validación de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _rolSeleccionado.AgregarHijo(seleccionado);
            ActualizarEstructuraVisual(_rolSeleccionado);
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (tvEstructuraDelRol.SelectedNode != null && _rolSeleccionado != null)
            {
                ComponenteBE componenteAQuitar = (ComponenteBE)tvEstructuraDelRol.SelectedNode.Tag;

                // El objeto Composite gestiona su remoción lógica
                _rolSeleccionado.QuitarHijo(componenteAQuitar);
                ActualizarEstructuraVisual(_rolSeleccionado);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_rolSeleccionado != null)
                {
                    _permisoBL.GuardarRol(_rolSeleccionado);
                    MessageBox.Show("Estructura de seguridad persistida con éxito.", "Sincronización", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FinalizarEdicion();
                    ActualizarGrillas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de persistencia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FinalizarEdicion();
        }

        #endregion

        #region "Métodos de Soporte Visual (Recursividad y UX)"

        private void ConfigurarGrillasVisuales()
        {
            // Limpieza de dgvListaDeRoles
            dgvListaDeRoles.AutoGenerateColumns = false;
            dgvListaDeRoles.Columns.Clear();
            dgvListaDeRoles.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 40 });
            dgvListaDeRoles.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Rol", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            // Limpieza de dgvCatalogoDeComponentes
            dgvCatalogoDeComponentes.AutoGenerateColumns = false;
            dgvCatalogoDeComponentes.Columns.Clear();
            dgvCatalogoDeComponentes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 40 });
            dgvCatalogoDeComponentes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Componente Disponible", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        private void ConfigurarEsteticaFormulario()
        {
            this.Text = "Gestión de Roles y Estructura Jerárquica";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            if (btnVolver != null) btnVolver.Tag = "btn_Volver";
        }

        private void ActualizarGrillas()
        {
            try
            {
                var todosLosComponentes = _permisoBL.ListarTodo();

                dgvCatalogoDeComponentes.DataSource = null;
                dgvCatalogoDeComponentes.DataSource = todosLosComponentes;

                dgvListaDeRoles.DataSource = null;
                dgvListaDeRoles.DataSource = todosLosComponentes.Where(x => x is CompuestoBE).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de datos: {ex.Message}", "Conectividad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FinalizarEdicion()
        {
            _rolSeleccionado = null;
            txtDatosDelRol.Clear();
            tvEstructuraDelRol.Nodes.Clear();
        }

        private void ActualizarEstructuraVisual(CompuestoBE pRol)
        {
            tvEstructuraDelRol.Nodes.Clear();
            if (pRol == null) return;

            TreeNode root = new TreeNode(pRol.Nombre);
            root.Tag = pRol;
            tvEstructuraDelRol.Nodes.Add(root);

            GenerarNodosRecursivos(root, pRol);
            tvEstructuraDelRol.ExpandAll();
        }

        private void GenerarNodosRecursivos(TreeNode padreVisual, ComponenteBE padreLogico)
        {
            foreach (var hijo in padreLogico.ObtenerHijos())
            {
                TreeNode nuevoNodo = new TreeNode(hijo.Nombre);
                nuevoNodo.Tag = hijo;
                padreVisual.Nodes.Add(nuevoNodo);

                if (hijo is CompuestoBE)
                {
                    GenerarNodosRecursivos(nuevoNodo, hijo);
                }
            }
        }

        #endregion
    }
}