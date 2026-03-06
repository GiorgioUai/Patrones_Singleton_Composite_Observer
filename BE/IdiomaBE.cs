using System;

namespace BE
{
    /// <summary>
    /// Representa la entidad de Idioma registrada en el sistema.
    /// </summary>
    public class IdiomaBE
    {
        /// <summary>
        /// Identificador único del idioma (PK en la base de datos).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre o código del idioma (ej: "ES", "EN").
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Sobreescritura para mostrar el nombre en ComboBoxes o listados.
        /// </summary>
        public override string ToString()
        {
            return Nombre;
        }
    }
}