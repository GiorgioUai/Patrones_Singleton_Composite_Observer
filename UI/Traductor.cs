using System;
using System.Windows.Forms;
using BL;

namespace UI
{
    public static class Traductor
    {
        /// <summary>
        /// Recorre recursivamente los controles de la UI y aplica las traducciones.
        /// </summary>
        public static void Traducir(Control.ControlCollection controles)
        {
            IdiomaManagerBL manager = IdiomaManagerBL.GetInstance();

            foreach (Control c in controles)
            {
                if (c.Tag != null)
                {
                    c.Text = manager.ObtenerTexto(c.Tag.ToString());
                }

                // Agregamos soporte para MenuStrip y ToolStrip manteniendo tu lógica original
                if (c is MenuStrip ms)
                {
                    TraducirMenuItems(ms.Items);
                }
                else if (c is ToolStrip ts)
                {
                    TraducirMenuItems(ts.Items);
                }

                if (c.HasChildren)
                {
                    Traducir(c.Controls);
                }
            }
        }

        /// <summary>
        /// Recorre los ítems de un menú o barra de herramientas de forma recursiva.
        /// </summary>
        private static void TraducirMenuItems(ToolStripItemCollection items)
        {
            IdiomaManagerBL manager = IdiomaManagerBL.GetInstance();

            foreach (ToolStripItem item in items)
            {
                if (item.Tag != null)
                {
                    item.Text = manager.ObtenerTexto(item.Tag.ToString());
                }

                if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                {
                    TraducirMenuItems(menuItem.DropDownItems);
                }
            }
        }
    }
}