using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Odontologia.Views.Conexion
{
    public class Conexion
    {

        string cadena = "localhost;Initial Catalog=Odontologia Integrated Segurity=True";
        public SqlConnection conn = new SqlConnection();


        public Conexion()
        {
            conn.ConnectionString = cadena;
        }

        public void abril()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Conexion Abierta");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error"+ ex.Message);
            }
        }

    }
}