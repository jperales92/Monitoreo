using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace monitoreo.capaConexion
{
    internal class dbConexion
    {
        public string sCadenaConexion { get; set; }
        public SqlConnection Conn;
        private SqlDataAdapter dataAdapter;
        public SqlCommand dbCommand;
        private SqlTransaction MyTransaction;
        public bool IsConected;
        private string strCommand;
        public int TimeOutCommandDef = 0;     
        public dbConexion()
        {
        }
        public dbConexion(string scName)
        {
            sCadenaConexion = ConfigurationManager.ConnectionStrings[scName].ConnectionString;
            Conn = new SqlConnection(sCadenaConexion);
        }
        public dbConexion(string scName,bool bind)
        {
            if(bind)
             Conn = new SqlConnection(scName);
        }
        public string ValidaConexion()
        {
            string sNodo = "";
            sCadenaConexion = ConfigurationManager.ConnectionStrings["GLPI"].ConnectionString;
            Conn = new SqlConnection(sCadenaConexion);

        if (!Open())
        {
            sCadenaConexion = "";
            sCadenaConexion = ConfigurationManager.ConnectionStrings["CENTRAL"].ConnectionString;
            Conn = new SqlConnection(sCadenaConexion);
        }

        return sCadenaConexion;
        }

        public bool Open()
        {
            bool cone = false;

            try
            {
                Conn.Open();
                IsConected = true;
                cone = true;
            }
            catch (Exception)
            {
                cone = false;
            }

            return cone;
        }
        public bool Close()
        {
            Conn.Close();
            IsConected = false;
            return true;
        }
        public bool setCommand(string sql, bool IsStoredProcedure)
        {
            strCommand = sql;
            if (!(dbCommand == null))
            {
                dbCommand.Dispose();
            }

            dbCommand = new SqlCommand(strCommand, Conn);
            if (IsStoredProcedure) dbCommand.CommandType = CommandType.StoredProcedure;
            if (!(MyTransaction == null))
            {
                dbCommand.Transaction = MyTransaction;
            }
            if (TimeOutCommandDef > 0)
                dbCommand.CommandTimeout = TimeOutCommandDef;

            return true;
        }
        public DataSet getDataSet()
        {
            DataSet myDataSet = new DataSet();
            dbCommand.CommandTimeout = 460;
            dataAdapter = new SqlDataAdapter(dbCommand);
            dataAdapter.Fill(myDataSet, "0");
            dataAdapter.Dispose();
            return myDataSet;
        }
        public ConnectionState state()
        {
            ConnectionState satte = new ConnectionState();
            satte = dbCommand.Connection.State;
            return satte;   
        }

        public bool AddParameters(string ParameterName, object ParameterValue)
        {
            dbCommand.Parameters.AddWithValue(ParameterName, ParameterValue);
            return true;
        }

        public bool executeNomQuery()
        {
            dbCommand.ExecuteNonQuery();
            return true;
        }
    }
}