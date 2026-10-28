using Sap.Data.Hana;
using System.Data;
using System.Configuration;

namespace monitoreo.capaConexion
{
    public class ConectHana
    {
        string sCadenaConexion { get; set; }
        public HanaConnection HanaConn;
        public HanaDataAdapter HanadataAdapter;
        public HanaCommand HanaCommand;
        public HanaTransaction HanaTransaction;

        public bool IsConected;
        public bool bConexionValida;
        private string strCommand;
        public int TimeOutCommandDef = 0;
        string sMensaje = "";

        public ConectHana()
        {

            sCadenaConexion = ConfigurationManager.ConnectionStrings["HANA"].ConnectionString;         
            HanaConn = new HanaConnection(sCadenaConexion);
        }
        public bool Open()
        {
            if (HanaConn.State != ConnectionState.Open)
            {
                HanaConn.Open();
                IsConected = true;
                return true;
            }
            return false; // Ya estaba abierta
        }

        public void ProcConexionHanaAbrir()
        {
            if (!procBDComprobarConexionActivaHana(HanaConn))
            {
                HanaConn.Open();
            }
        }

        public void ProcConexionHanaCerrar()
        {
            if (procBDComprobarConexionActivaHana(HanaConn))
            {
                HanaConn.Close();
            }
        }

        private bool procBDComprobarConexionActivaHana(HanaConnection hanaConn)
        {
            bool bResultado = false;
            switch (hanaConn.State)
            {
                case ConnectionState.Closed:
                    bResultado = false;
                    break;
                case ConnectionState.Open:
                    bResultado = true;
                    break;
                case ConnectionState.Connecting:
                    bResultado = true;
                    break;
                case ConnectionState.Executing:
                    bResultado = true;
                    break;
                case ConnectionState.Fetching:
                    bResultado = true;
                    break;
                case ConnectionState.Broken:
                    bResultado = false;
                    break;
                default:
                    bResultado = false;
                    break;
            }
            return bResultado;
        }

        public bool procBDComprobarConectividad()
        {
            try
            {
                if (!procBDComprobarConexionActivaHana(HanaConn))
                    HanaConn.Open();
                this.bConexionValida = true;
            }
            catch (System.Exception sErr)
            {
                sMensaje = sErr.Message;
                this.bConexionValida = false;
            }
            finally
            {
                if (procBDComprobarConexionActivaHana(HanaConn))
                    HanaConn.Close();
            }
            return bConexionValida;
        }
        public bool Close()
        {
            if (HanaConn.State == ConnectionState.Open)
            {
                HanaConn.Close();
                IsConected = false;
                return true;
            }
            return false;
        }

        public bool setCommand(string sql, bool IsStoredProcedure)
        {
            strCommand = sql;
            if (!(HanaCommand == null))
            {
                HanaCommand.Dispose();
            }

            HanaCommand = new HanaCommand(strCommand, HanaConn);
            if (IsStoredProcedure) HanaCommand.CommandType = CommandType.StoredProcedure;
            if (!(HanaTransaction == null))
            {
                HanaCommand.Transaction = HanaTransaction;
            }
            if (TimeOutCommandDef > 0)
                HanaCommand.CommandTimeout = TimeOutCommandDef;

            return true;
        }
        public bool setCommandText(string sql, bool IsStoredProcedure)
        {
            strCommand = sql;
            if (!(HanaCommand == null))
            {
                HanaCommand.Dispose();
            }

            HanaCommand = new HanaCommand(strCommand, HanaConn);
            if (IsStoredProcedure) HanaCommand.CommandType = CommandType.Text;
            if (!(HanaTransaction == null))
            {
                HanaCommand.Transaction = HanaTransaction;
            }
            if (TimeOutCommandDef > 0)
                HanaCommand.CommandTimeout = TimeOutCommandDef;

            return true;
        }
        public DataSet getDataSet()
        {
            DataSet myDataSet = new DataSet();
            HanaCommand.CommandTimeout = 460;
            HanadataAdapter = new HanaDataAdapter(HanaCommand);
            HanadataAdapter.Fill(myDataSet, "0");
            HanadataAdapter.Dispose();
            return myDataSet;
        }
        public bool AddParameters(string ParameterName, object ParameterValue)
        {
            HanaCommand.Parameters.AddWithValue(ParameterName, ParameterValue);
            return true;
        }

        public void ProcComandoHanaGenerar(CommandType ctype, string sBaseDatos, string sHanaComando)
        {
            string sComando = "";
            switch (ctype)
            {
                case CommandType.Text:
                    sComando = sHanaComando;
                    break;
                case CommandType.StoredProcedure:
                    sComando = "\"" + sBaseDatos + "\"." + sHanaComando;
                    break;
                case CommandType.TableDirect:
                    sComando = "select * from \"" + sBaseDatos + "\".\"" + sHanaComando + "\";";
                    break;
                default:
                    break;
            }
            this.HanaCommand = this.HanaConn.CreateCommand();
            this.HanaCommand.Connection = this.HanaConn;
            this.HanaCommand.CommandType = ctype;
            this.HanaCommand.CommandText = sComando;
        }
    }


}