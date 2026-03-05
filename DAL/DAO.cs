using System;
using System.Configuration;

namespace DAL
{
    public abstract class DAO
    {
        protected string _connectionString;

        public DAO()
        {            
            _connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
    }
}