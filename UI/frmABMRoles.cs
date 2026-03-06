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
        }

        #endregion

        #region "Eventos del Formulario"

        private void frmABMRoles_Load(object sender, EventArgs e)
        {
            ActualizarGrillas();
        }

        private void dgvListaDeRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Se asigna la referencia del objeto seleccionado para edición en RAM
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

            // Validación de integridad: Evita recursividad infinita (que un rol se contenga a sí mismo)
            string contenedor = _permisoBL.ObtenerContenedorRecursivo(_rolSeleccionado, seleccionado.Id);

            if (contenedor != null)
            {
                MessageBox.Show($"Operación inválida: El componente ya está integrado en la estructura de '{contenedor}'.",
                                "Validación de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Aplicación del patrón Composite: El objeto gestiona su estado interno (Agregado)
            _rolSeleccionado.AgregarHijo(seleccionado);
            ActualizarEstructuraVisual(_rolSeleccionado);
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (tvEstructuraDelRol.SelectedNode != null && _rolSeleccionado != null)
            {
                // Recuperación del objeto de negocio asociado al nodo visual (Tag)
                ComponenteBE componenteAQuitar = (ComponenteBE)tvEstructuraDelRol.SelectedNode.Tag;

                // Remoción lógica: El objeto se marca para eliminación (Eliminado)
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
                    // Persistencia transaccional de los cambios realizados en memoria (RAM)
                    _permisoBL.GuardarRol(_rolSeleccionado);

                    MessageBox.Show("Estructura de seguridad actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FinalizarEdicion();
                    ActualizarGrillas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al persistir cambios: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FinalizarEdicion();
        }

        #endregion

        #region "Métodos de Soporte Visual (Recursividad)"

        private void ActualizarGrillas()
        {
            try
            {
                var todosLosComponentes = _permisoBL.ListarTodo();

                // Catálogo de componentes disponibles
                dgvCatalogoDeComponentes.DataSource = null;
                dgvCatalogoDeComponentes.DataSource = todosLosComponentes;

                // Lista de Roles (Compuestos) existentes
                dgvListaDeRoles.DataSource = null;
                dgvListaDeRoles.DataSource = todosLosComponentes.Where(x => x is CompuestoBE).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conectividad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FinalizarEdicion()
        {
            _rolSeleccionado = null;
            txtDatosDelRol.Clear();
            tvEstructuraDelRol.Nodes.Clear();
        }

        /// <summary>
        /// Sincroniza la visualización del TreeView con el estado del objeto Composite.
        /// </summary>
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

        /// <summary>
        /// Recorre la jerarquía Composite de forma recursiva para generar los nodos visuales.
        /// </summary>
        private void GenerarNodosRecursivos(TreeNode padreVisual, ComponenteBE padreLogico)
        {
            foreach (var hijo in padreLogico.ObtenerHijos())
            {
                TreeNode nuevoNodo = new TreeNode(hijo.Nombre);
                nuevoNodo.Tag = hijo; // Importante para recuperar el objeto al quitarlo
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