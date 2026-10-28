using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Utils.Filtering.Internal;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using EvoToolCOM;
using monitoreo.capaConexion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.XtraEditors.RoundedSkinPanel;

namespace monitoreo
{
    public partial class frmMonitoreo : Form
    {
        #region Campos Privados
        dbConexion odb = null;
        //ConectHana HanaConexion = null;
        string logPath = "log.txt";
        MascaraGrid mascara;        
        int zoom = 0;
        int timem = 0;
        Dictionary<string, int> pendientesPrevios = new Dictionary<string, int>();
        Dictionary<string, int> contadorIntentosReinicio = new Dictionary<string, int>();
        readonly Dictionary<string, System.Threading.Timer> _timers = new Dictionary<string, System.Threading.Timer>();
        #endregion

        #region Propiedades 
        DataSet ds { get; set; }
        private string connectionStringCentral { get; set; }
        private string secondaryConnectionString { get; set; }
        private string sNodoConexion { get; set; }
        TabPage pag_Monitor { get; set; }
        //SplitContainer split { get; set; }
        Panel monitorPanel { get; set; }
        #endregion

        #region  Constructor 
        public frmMonitoreo()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.zoomTrackBarControl1.EditValueChanged += zoomTrackBarControl1_EditValueChanged;
            this.FormClosing += Form1_FormClosing1;
            this.Load += Form1_Load;
            this.AutoScroll = true;
            int zoomInicio, timee;
            
            if (int.TryParse(ConfigurationManager.AppSettings["ZoomInicio"], out zoomInicio))
            {
                zoom = zoomInicio;
            }
            else
            {
                zoomTrackBarControl1.Value = 0; // Valor por defecto si hay error
            }

            if (int.TryParse(ConfigurationManager.AppSettings["timer"], out timee))
            {
                timem = timee;
            }
            else
            {
                timem = 60000; // Valor por defecto si hay error
            }          
            this.KeyPreview = true;
            this.KeyDown += frm_Monitor_KeyDown;       
        }
        #endregion

        #region Metodos Privados
        void Form1_Load(object sender, EventArgs e)
        {
            pag_Monitor = this.tab_Central.TabPages["pag_monitor"];
            monitorPanel = pag_Monitor.Controls["pnl_monitor"] as Panel;
            ds = new DataSet();
            Estructura();
            odb = new dbConexion();
            sNodoConexion = odb.ValidaConexion();
            secondaryConnectionString = ConfigurationManager.ConnectionStrings["GLPI"].ConnectionString;
            connectionStringCentral = ConfigurationManager.ConnectionStrings["CENTRAL"].ConnectionString;

            grd_Sucursales.DataSource = ds;
            grd_Sucursales.DataMember = ds.Tables["dt_SucursalesSQL"].ToString();

            mascara = new MascaraGrid(ds);
            mascara.AplicarMascara(gv_Sucursales, ds.Tables["dt_SucursalesSQL"], MascaraGrid.EstatusFormulario.Consultando);
            Proc_ObtenerSucursalesSQL();
            gv_Sucursales.BestFitColumns();
            ProcCrearControles();
            InicializarMonitoreoSucursales();
            Eventos();
        }
        void PorcEstatusPantalla(MascaraGrid.EstatusFormulario vEstatusForm)
        {
            switch (vEstatusForm)
            {
                case MascaraGrid.EstatusFormulario.Consultando:
                    btn_Salir.Enabled = false;
                    btn_Guardar.Enabled = false;
                    btn_Editar.Enabled = true;
                    mascara.HabilitarSegunEstatus(MascaraGrid.EstatusFormulario.Consultando, gv_Sucursales);
                    break;
                case MascaraGrid.EstatusFormulario.Editando:
                    btn_Editar.Enabled = false;
                    btn_Salir.Enabled = true;
                    btn_Guardar.Enabled = true;
                    mascara.HabilitarSegunEstatus(MascaraGrid.EstatusFormulario.Editando, gv_Sucursales);
                    break;
            }
        }
        void Eventos()
        {
            btn_Editar.Click += Btn_Editar_Click;
            btn_Salir.Click += Btn_Salir_Click;
            btn_Guardar.Click += Btn_Guardar_Click;
            gv_Sucursales.ShowingEditor += Gv_Sucursales_ShowingEditor;
            // gv_Sucursales.ValidatingEditor += Gv_Sucursales_ValidatingEditor;
            gv_Sucursales.FocusedRowChanged += gv_Sucursales_FocusedRowChanged;
        }
        void Estructura()
        {
            DataTable dt = new DataTable("dt_SucursalesSQL");
            dt.Columns.Add("iIdSucursal", typeof(int));
            dt.Columns.Add("sSucursal", typeof(string));
            dt.Columns.Add("dtFechaAlta", typeof(DateTime));
            dt.Columns.Add("sUsuarioAlta", typeof(string));
            dt.Columns.Add("dtFechaModificacion", typeof(DateTime));
            dt.Columns.Add("sUsuarioModificacion", typeof(string));
            dt.Columns.Add("sServidorInstanciaIP", typeof(string));
            dt.Columns.Add("sBaseDatos", typeof(string));
            dt.Columns.Add("sUsuario", typeof(string));
            dt.Columns.Add("sContraseña", typeof(string));
            dt.Columns.Add("iPuerto", typeof(int));
            dt.Columns.Add("dtFechaUltimoIntentoActualizacion", typeof(DateTime));
            dt.Columns.Add("dtFechaUltimaActualizacion", typeof(DateTime));
            dt.Columns.Add("bActivo", typeof(bool));


            ds.Tables.Add(dt);

            ds.Tables["dt_SucursalesSQL"].Columns["iIdSucursal"].Caption = "ID(v)(sort0)";
            ds.Tables["dt_SucursalesSQL"].Columns["sSucursal"].Caption = "Sucursal(v)(sort1)";
            ds.Tables["dt_SucursalesSQL"].Columns["sServidorInstanciaIP"].Caption = "IP/Instancia(v)(ed)(sort2)";
            ds.Tables["dt_SucursalesSQL"].Columns["sBaseDatos"].Caption = "DB(v)(ed)(sort3)";
            ds.Tables["dt_SucursalesSQL"].Columns["sUsuario"].Caption = "Usuario DB(v)(ed)(sort4)";
            ds.Tables["dt_SucursalesSQL"].Columns["sContraseña"].Caption = "Password DB(v)(ed)(sort5)";
            ds.Tables["dt_SucursalesSQL"].Columns["iPuerto"].Caption = "Puerto DB(v)(ed)(sort6)";
            ds.Tables["dt_SucursalesSQL"].Columns["dtFechaAlta"].Caption = "Fecha Alta(v)(dtFecha)(sort7)";
            ds.Tables["dt_SucursalesSQL"].Columns["sUsuarioAlta"].Caption = "Usuario Alta(v)(sort8)";
            ds.Tables["dt_SucursalesSQL"].Columns["dtFechaModificacion"].Caption = "Fecha Modificación(v)(dtFecha)(sort9)";
            ds.Tables["dt_SucursalesSQL"].Columns["sUsuarioModificacion"].Caption = "Usuario.Modificación(v)(sort10)";
            ds.Tables["dt_SucursalesSQL"].Columns["dtFechaUltimoIntentoActualizacion"].Caption = "Ultimo intento de actualización(v)(dtFecha)(sort11)";
            ds.Tables["dt_SucursalesSQL"].Columns["dtFechaUltimaActualizacion"].Caption = "Última actualización(v)(dtFecha)(sort12)";
            ds.Tables["dt_SucursalesSQL"].Columns["bActivo"].Caption = "Activo(v)(ed)(sort12)";
        }
        void InicializarMonitoreoSucursales()
        {
            foreach (DataRow row in ds.Tables["dt_SucursalesSQL"].Rows)
            {
                if (Convert.ToBoolean(row["bActivo"]))
                {
                    string sucursalID = row["iIdSucursal"].ToString();
                    System.Threading.Timer timer = new System.Threading.Timer(CallbackSucursal, row, 0, timem);
                    _timers[sucursalID] = timer;
                }
            }
        }
        void CallbackSucursal(object state)
        {
            DataRow row = (DataRow)state;
            Task.Run(() => ProcCargaComunicacionsucursal(row));
        }
        void ProcCargaComunicacionsucursal(DataRow row)
        {
            string servidorInstanciaIP = row["sServidorInstanciaIP"].ToString();
            string baseDatos = row["sBaseDatos"].ToString();
            string usuario = row["sUsuario"].ToString();
            string contraseña = row["sContraseña"].ToString();
            string connectionString = $"Data Source={servidorInstanciaIP};Initial Catalog={baseDatos};Integrated Security=False;User ID={usuario};Password={contraseña}";
            string serviceName = ConfigurationManager.AppSettings["serviceName"];
            string processName = ConfigurationManager.AppSettings["processName"];
            string ipServidor = servidorInstanciaIP.Split('\\')[0];
            string controlName = "";
            int _iIdSucursal = Convert.ToInt32(row["iIdSucursal"]);
            string _sSucursal = row["sSucursal"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ObtenerResumenTransaccionesSucursal", conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            this.Invoke((Action)(() =>
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    uc_monitor_item i;
                                    controlName = "L" + dr["iIdSucursal"].ToString();
                                    string sucursal = dr["sSucursal"].ToString();
                                    string iIdSucursal = dr["iIdSucursal"].ToString();
                                    int pendientesActuales = Convert.ToInt32(dr["Pendientes"]);
                                    int SinFolio = Convert.ToInt32(dr["SinFolio"]);
                                    bool AutoSinfolio = false;

                                    WriteLog(ipServidor, sucursal, iIdSucursal, "Consultando estado de la sucursal...READMIN");

                                    if (monitorPanel.Controls.Find(controlName, false).Length == 0)
                                    {
                                        i = new uc_monitor_item
                                        {
                                            Name = controlName,
                                            Valor = dr["iIdSucursal"].ToString() + "-" + dr["sSucursal"].ToString(),
                                            Titulo = dr["sServidorInstanciaIP"].ToString(),
                                            Marcador = dr["Pendientes"].ToString(),
                                            Transaccion = "0",
                                            Visible = true,
                                            BackColor = SystemColors.ControlLight,
                                        };
                                        i.pb_Titulo.BackColor = Color.Orange;
                                        monitorPanel.Controls.Add(i);
                                    }
                                    else
                                    {
                                        i = (uc_monitor_item)monitorPanel.Controls.Find(controlName, false)[0];
                                        i.Marcador = dr["Pendientes"].ToString();
                                        i.Transaccion = dr["Transicion"].ToString();
                                        i.BackColor = SystemColors.ControlLight;
                                        i.ForeColor = Color.Black;
                                        //i.pb_Titulo.BackColor = Color.Green;

                                        bool necesitaReinicio = pendientesActuales >= i.CantidadMinimaOptima;

                                        // Si la sucursal ya está en el diccionario, verificamos el valor anterior
                                        if (pendientesPrevios.ContainsKey(iIdSucursal))
                                        {
                                            int pendientesPreviosValor = pendientesPrevios[iIdSucursal];
                                            using (SqlConnection connn = new SqlConnection(secondaryConnectionString))
                                            {
                                                connn.Open();
                                                using (SqlCommand lastCmd = new SqlCommand("sp_ObtenerUltimaCantidadPendiente", connn))
                                                {
                                                    lastCmd.CommandType = CommandType.StoredProcedure;
                                                    lastCmd.Parameters.AddWithValue("@idSucursal", iIdSucursal);
                                                    object lastResult = lastCmd.ExecuteScalar();

                                                    if (lastResult != null && Convert.ToInt32(lastResult) > 0)
                                                    {
                                                        if (pendientesActuales == 0 && pendientesPreviosValor > 0)
                                                        {
                                                            WriteLog(ipServidor, sucursal, iIdSucursal, $"✅ Alerta: {iIdSucursal}-{sucursal} ahora está en estado OK. Pendientes: 0.");
                                                            // 🔹 Insertar el estado OK en SQL
                                                            RegistrarEstadoOk(iIdSucursal, sucursal, ipServidor);
                                                        }
                                                    }
                                                }
                                            }
                                            // Solo reiniciar si los valores son iguales (se mantiene la misma cantidad de pendientes)
                                            if (necesitaReinicio && pendientesActuales == pendientesPreviosValor)
                                            {

                                                if (!CentralDisponible())
                                                {
                                                    WriteLog(ipServidor, sucursal, iIdSucursal, "🌐 Red central caída. Se suspenden acciones de reinicio.");
                                                    return;
                                                }

                                                if (!contadorIntentosReinicio.ContainsKey(iIdSucursal))
                                                {
                                                    contadorIntentosReinicio[iIdSucursal] = 1;
                                                }
                                                else
                                                {
                                                    contadorIntentosReinicio[iIdSucursal]++; // 🔹 Incrementar el contador
                                                }
                                                WriteLog(ipServidor, sucursal, iIdSucursal, $"🔄 Intento de reinicio #{contadorIntentosReinicio[iIdSucursal]} para la sucursal {sucursal}");

                                                if (contadorIntentosReinicio[iIdSucursal] >= 5 || contadorIntentosReinicio[iIdSucursal] == 1)
                                                {
                                                    WriteLog(ipServidor, sucursal, iIdSucursal, "Servicio requiere reinicio debido a demasiadas transacciones pendientes.");

                                                    if (SinFolio == 1)
                                                    {
                                                        using (SqlConnection connDelete = new SqlConnection(connectionString))
                                                        {
                                                            connDelete.Open();
                                                            using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM [@so1_01transpdtes] WHERE U_SO1_ESTADO='0' AND U_SO1_FOLIO=''", connDelete))
                                                            {
                                                                int rowsAffected = deleteCmd.ExecuteNonQuery();
                                                                WriteLog(ipServidor, sucursal, iIdSucursal, $"🧹 Se ejecutó DELETE automático. Registros eliminados: {rowsAffected}");
                                                                AutoSinfolio = rowsAffected > 0 ? true : false;
                                                                // Registrar estado OK si se resolvió automáticamente
                                                                //RegistrarEstadoOk(iIdSucursal, sucursal, ipServidor);
                                                                WriteLog(ipServidor, sucursal, iIdSucursal, $"✅ Estado OK registrado automáticamente tras limpieza de documentos sin folio.");
                                                            }
                                                        }
                                                    }

                                                    EnviarMensaje(iIdSucursal, sucursal, ipServidor, pendientesActuales, SinFolio, AutoSinfolio);
                                                    ServicioRemoto servicio = new ServicioRemoto(ipServidor, serviceName, processName, sucursal, iIdSucursal);
                                                    servicio.ReiniciarSistema();
                                                    WriteLog(ipServidor, sucursal, iIdSucursal, "Servicio reiniciado exitosamente.");
                                                    contadorIntentosReinicio[iIdSucursal] = 1;
                                                }
                                            }
                                            // TESTEO DIRECTO
                                            //if (Convert.ToInt32(iIdSucursal) == 245)
                                            //{
                                            //    ServicioRemoto servicio = new ServicioRemoto(ipServidor, serviceName, processName, sucursal, iIdSucursal);
                                            //    servicio.ReiniciarSistema();
                                            //}
                                        }
                                        // 🔹 Actualizar el diccionario con el nuevo valor de pendientes
                                        pendientesPrevios[iIdSucursal] = pendientesActuales;
                                    }
                                }
                            }));
                        }
                    }

                    try
                    {
                        ActualizarExistencias(_iIdSucursal, conn, controlName, _sSucursal, ipServidor);
                    }
                    catch (Exception exExistencia)
                    {
                        // 🔶 Si falla la actualización de existencias, solo se marca en naranja
                        var controles = monitorPanel.Controls.Find(controlName, false);
                        if (controles.Length > 0 && controles[0] is uc_monitor_item i)
                        {
                            i.pb_Titulo.BackColor = Color.Orange;
                        }

                        WriteLog(ipServidor, row["sSucursal"].ToString(), _iIdSucursal.ToString(),
                            "⚠️ No se pudo actualizar existencias para esta sucursal. Se marcó en naranja. Detalle: " + exExistencia.Message);
                    }
                }
                catch (Exception ex)
                {
                    string iIdSucursal = row["iIdSucursal"].ToString();
                    string sucursal = row["sSucursal"].ToString();
                    string servidorInstanciaIP_ = row["sServidorInstanciaIP"].ToString();
                    string ipServidor_ = servidorInstanciaIP.Split('\\')[0];
                    controlName = "L" + iIdSucursal;

                    using (SqlConnection secondaryConn = new SqlConnection(secondaryConnectionString))
                    {
                        try
                        {
                            secondaryConn.Open();

                            using (SqlCommand cmd = new SqlCommand("sp_ObtenerDetalleTransaccionesSucursal", secondaryConn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@iIdSucursal", Convert.ToInt32(iIdSucursal));

                                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                {
                                    DataTable dt = new DataTable();
                                    adapter.Fill(dt);

                                    this.Invoke((Action)(() =>
                                    {
                                        uc_monitor_item control = monitorPanel.Controls.Find(controlName, false)
                                            .FirstOrDefault() as uc_monitor_item;

                                        if (control == null)
                                        {
                                            control = new uc_monitor_item
                                            {
                                                Name = controlName,
                                                Titulo = sucursal,
                                                Visible = true
                                            };
                                            monitorPanel.Controls.Add(control);
                                        }

                                        if (dt.Rows.Count == 0)
                                        {
                                            // 🔴 Sin transmisión ni Radmin
                                            WriteLog("LOCAL", sucursal, iIdSucursal, "❌ Sin conexión ni transmisión detectada.");
                                            control.Marcador = "Sin línea";
                                            control.Transaccion = " ";
                                            control.BackColor = Color.Red;
                                            control.ForeColor = Color.White;
                                            control.pb_Titulo.BackColor = Color.Red;
                                        }
                                        else
                                        {
                                            DataRow dr = dt.Rows[0];
                                            DateTime ejecutado = dr["dtFechaEjecutado"] != DBNull.Value ? Convert.ToDateTime(dr["dtFechaEjecutado"]) : DateTime.MinValue;
                                            bool dentroDeTiempo = (DateTime.Now - ejecutado).TotalMinutes <= 30;

                                            if (dentroDeTiempo)
                                            {
                                                // 🟦 Nueva condición: Radmin caído pero sigue transmitiendo al HSMAPP
                                                WriteLog("LOCAL", sucursal, iIdSucursal,
                                                    "🌐 Radmin sin conexión, pero sucursal sigue transmitiendo al HSMAPP.");

                                                EnviarAlertaRadminCaido(sucursal, iIdSucursal, ejecutado, ipServidor_);

                                                control.Marcador = dr["Pendientes"].ToString();
                                                control.Transaccion = dr["transicion"].ToString();
                                                control.BackColor = Color.LightCyan;
                                                control.ForeColor = Color.Black;
                                                control.pb_Titulo.BackColor = Color.SkyBlue;
                                            }
                                            else
                                            {
                                                //EnviarAlertaRadminCaido(sucursal, iIdSucursal, ejecutado, ipServidor_);
                                                // 🔴 Transmisión desactualizada
                                                control.Marcador = "Sin línea";
                                                control.Transaccion = " ";
                                                control.BackColor = Color.Red;
                                                control.ForeColor = Color.White;
                                                control.pb_Titulo.BackColor = Color.Red;
                                            }
                                        }
                                    }));
                                }
                            }
                        }
                        catch (Exception ex2)
                        {
                            WriteLog("LOCAL", sucursal, iIdSucursal, $"⚠️ Error en fallback HSMAPP: {ex2.Message}");
                        }
                    }
                }
            }
        }
        void EnviarAlertaRadminCaido(string sucursal, string idSucursal, DateTime ultimaTransmision, string sIp)
        {
            string mensaje = $"⚠️ {sucursal} (ID {idSucursal}) sin conexión Radmin, pero transmitiendo correctamente.\r\nÚltima transmisión: {ultimaTransmision:G}";
            WriteLog("HSMAPP", sucursal, idSucursal, mensaje);
            EnviarAlertaRadminCaido(idSucursal, sucursal, sIp);
        }
        void EnviarAlertaRadminCaido(string iIdSucursal, string sucursal, string sIp)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(secondaryConnectionString))
                using (SqlCommand cmd = new SqlCommand("tbl_AlertasRadminCaido", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idSucursal", iIdSucursal);
                    cmd.Parameters.AddWithValue("@sSucursal", sucursal);
                    cmd.Parameters.AddWithValue("@sIp", sIp);
                    cmd.Parameters.AddWithValue("@bIndActivo", sIp);
                    cmd.Parameters.AddWithValue("@sDescripcion",
                        "Radmin apagado o sin conexión, pero la sucursal sigue transmitiendo datos al servidor HSMAPP.");

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        WriteLog(sIp, sucursal, iIdSucursal,
                            $"📡 Se registró alerta de Radmin caído para {sucursal} ({iIdSucursal}).");
                    }
                    else
                    {
                        WriteLog(sIp, sucursal, iIdSucursal,
                            $"⚠️ No se registró alerta (ya existía o no aplicaba) para {sucursal} ({iIdSucursal}).");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(sIp, sucursal, iIdSucursal,
                    $"❌ Error al registrar alerta de Radmin caído para {sucursal} ({iIdSucursal}): {ex.Message}");
            }
        }
        void RegistrarEstadoOk(string iIdSucursal, string sucursal, string sIp)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(secondaryConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_RegistrarAlertaEstadoOK", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idSucursal", iIdSucursal);
                        cmd.Parameters.AddWithValue("@sSucursal", sucursal);
                        cmd.Parameters.AddWithValue("@sIp", sIp);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        WriteLog(sIp, sucursal, iIdSucursal,
                                 rowsAffected > 0
                                 ? $"✅ Estado OK insertado: {sucursal} ahora tiene 0 pendientes."
                                 : $"⚠️ No se insertó ningún registro para {sucursal}. Verificar el SP sp_RegistrarAlertaEstadoOK.-- Void RegistrarEstadoOk");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(sIp, sucursal, iIdSucursal, $"❌ Error al insertar estado OK para {sucursal}. Verificar el SP sp_RegistrarAlertaEstadoOK. -- Void RegistrarEstadoOk: {ex.Message}");
            }
        }
        void EnviarMensaje(string iIdSucursal, string sucursal, string sIp, int dCantidadPendiente, int SinFolio, bool AutoSinfolio)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(secondaryConnectionString))
                using (SqlCommand cmd = new SqlCommand("sp_InsertarAlertaSiCorresponde", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idSucursal", iIdSucursal);
                    cmd.Parameters.AddWithValue("@sSucursal", sucursal);
                    cmd.Parameters.AddWithValue("@sIp", sIp);
                    cmd.Parameters.AddWithValue("@dCantidadPendiente", dCantidadPendiente);
                    cmd.Parameters.AddWithValue("@SinFolio", SinFolio);
                    cmd.Parameters.AddWithValue("@AutoSinfolio", AutoSinfolio);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        WriteLog(sIp, sucursal, iIdSucursal, $"✅ Se insertó alerta para {iIdSucursal} - {sucursal} con {dCantidadPendiente} pendiente(s).");
                    }
                    else
                    {
                        WriteLog(sIp, sucursal, iIdSucursal, $"⏳ No se insertó alerta para {iIdSucursal} - {sucursal} (pendiente anterior o tiempo insuficiente).");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(sIp, sucursal, iIdSucursal, $"❌ Error al ejecutar sp_InsertarAlertaSiCorresponde para {iIdSucursal} - {sucursal}: {ex.Message}");
            }
        }
        void WriteLog(string ipServidor, string sucursal, string message, string iIdSucursal)
        {
            // 🗂 Ruta base del ejecutable
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            // 📁 Carpeta LOG dentro del .exe
            string logDirectory = Path.Combine(basePath, "LOG");
            Directory.CreateDirectory(logDirectory); // Asegura que exista

            // 📅 Nombre del archivo por día
            string logFileName = $"log_{DateTime.Now:yyyyMMdd}.txt";
            string fullPath = Path.Combine(logDirectory, logFileName);

            // 📝 Escritura del log
            using (FileStream fs = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [IP: {ipServidor}, Sucursal:{iIdSucursal} - {sucursal}] {message}");
            }
        }
        void Proc_ObtenerSucursalesSQL()
        {
            SqlConnection conglpi = new SqlConnection(sNodoConexion);
            try
            {
                conglpi.Open();

                SqlCommand cmdglpi = new SqlCommand("sp_ObtenerSucursalesConConexion", conglpi);
                cmdglpi.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmdglpi);
                DataSet dts = new DataSet();
                adapter.Fill(dts);

                ds.Tables["dt_SucursalesSQL"].Clear();
                ds.Tables["dt_SucursalesSQL"].Merge(dts.Tables[0]);
                WriteLog("SERVER", "GLPI", "✅ Listado de sucursales Cargadas", "SERVER");
            }
            catch (Exception ex)
            {
                WriteLog("SERVER", "GLPI", $"❌ Error al obtener listado sucursales en sp_ObtenerSucursalesConConexion- void Proc_ObtenerSucursalesSQL: {ex.Message}", "SERVER");
            }
            finally
            {
                if (conglpi.State == ConnectionState.Open)
                {
                    conglpi.Close();
                }
            }
        }
        void ProcResizeControls(int iValue)
        {
            Point p = new Point(2, 2);
            foreach (Control c in monitorPanel.Controls)
            {
                if (c.GetType() == typeof(uc_monitor_item))
                {
                    uc_monitor_item i = (uc_monitor_item)c;
                    i.PorcentajeZoom = iValue;
                    i.Location = p;
                    if (p.Y + i.Height > this.Height - 100)
                    {
                        p.Y = 2;
                        p.X += i.Width + 2;
                    }
                    else
                    {
                        p.Y += i.Height + 2;
                    }

                    i.pb_Titulo.BackColor = Color.Orange;
                }
            }
        }
        #endregion

        #region Eventos
        void Btn_Guardar_Click(object sender, EventArgs e)
        {
            this.Validate();
            if (ds.HasChanges())
            {
                if (Guardado())
                {
                    Herramientas.Avisa("Cambios guardados");
                    this.PorcEstatusPantalla(MascaraGrid.EstatusFormulario.Consultando);
                }
            }
            else
            {
                Herramientas.Avisa("No existen cambios por guardar");
            }
        }
        void Btn_Salir_Click(object sender, EventArgs e)
        {
            if (ds.HasChanges())
            {
                if (Herramientas.Confirma("Desea cancelar los cambios?"))
                {
                    this.PorcEstatusPantalla(MascaraGrid.EstatusFormulario.Consultando);
                    ds.Tables["dt_SucursalesSQL"].RejectChanges();
                }
            }
            else
            {
                this.PorcEstatusPantalla(MascaraGrid.EstatusFormulario.Consultando);
            }
        }
        void Btn_Editar_Click(object sender, EventArgs e)
        {
            PorcEstatusPantalla(MascaraGrid.EstatusFormulario.Editando);
        }

        void gv_Sucursales_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.PrevFocusedRowHandle >= 0)
            {
                bool activo = Convert.ToBoolean(view.GetRowCellValue(e.PrevFocusedRowHandle, "bActivo"));

                if (activo)
                {
                    string[] columnasObligatorias = {"sServidorInstanciaIP", "sBaseDatos", "sUsuario", "sContraseña", "iPuerto"
                };

                    bool esiteerror = false;
                    StringBuilder errores = new StringBuilder();

                    // Encabezado con ID + Sucursal
                    string id = view.GetRowCellValue(e.PrevFocusedRowHandle, "iIdSucursal")?.ToString();
                    string sucursal = view.GetRowCellValue(e.PrevFocusedRowHandle, "sSucursal")?.ToString();
                    errores.AppendLine($"⛔ Validación para ID: {id} | Sucursal: {sucursal}");
                    errores.AppendLine(new string('-', 50));

                    // Validar campos vacíos
                    foreach (string col in columnasObligatorias)
                    {
                        var valor = view.GetRowCellValue(e.PrevFocusedRowHandle, col);
                        if (valor == null || string.IsNullOrWhiteSpace(valor.ToString()))
                        {
                            errores.AppendLine($"• El campo '{col}' no puede estar vacío.");
                            esiteerror = true;
                        }
                    }

                    // Validar IP/Instancia duplicada
                    string ipActual = view.GetRowCellValue(e.PrevFocusedRowHandle, "sServidorInstanciaIP")?.ToString();
                    for (int i = 0; i < view.RowCount; i++)
                    {
                        if (i == e.PrevFocusedRowHandle) continue;

                        string ipComparar = view.GetRowCellValue(i, "sServidorInstanciaIP")?.ToString();
                        if (ipActual == ipComparar)
                        {
                            errores.AppendLine("• Ya existe un registro con esa IP/Instancia.");
                            break;
                        }
                    }

                    // Mostrar errores si existen
                    if (errores.Length > 60 && esiteerror) // más de solo el encabezado
                    {
                        Herramientas.Avisa(errores.ToString());
                        view.FocusedRowHandle = e.PrevFocusedRowHandle; // regresa a la fila anterior
                    }
                }
            }
        }
        void Gv_Sucursales_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            GridView view = sender as GridView;

            if (view.FocusedColumn.FieldName == "sServidorInstanciaIP") // ajusta el nombre si es diferente
            {
                string nuevoValor = e.Value.ToString();

                for (int i = 0; i < view.RowCount; i++)
                {
                    if (i == view.FocusedRowHandle) continue; // omite la fila actual

                    string valorExistente = view.GetRowCellValue(i, view.FocusedColumn)?.ToString();
                    if (valorExistente == nuevoValor)
                    {
                        e.Valid = false;
                        e.ErrorText = "Ya existe un registro con esa IP/Instancia.";
                        break;
                    }
                }
            }

        }
        void Gv_Sucursales_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {

            GridView view = sender as GridView;
            if (view == null) return;

            string columnName = view.FocusedColumn.FieldName;

            if (columnName == null) return;

            if (columnName != "bActivo")
            {
                bool activo = Convert.ToBoolean(view.GetRowCellValue(view.FocusedRowHandle, "bActivo"));

                if (!activo)
                {
                    e.Cancel = true; // bloquea la edición
                }
            }
        }

        void Form1_FormClosing1(object sender, FormClosingEventArgs e)
        {
            foreach (var timer in _timers.Values)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite); // Detiene cada Timer
                timer.Dispose(); // Libera recursos
            }
            _timers.Clear(); // Limpia la lista de timers
        }
        void frm_Monitor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.F10)
            {
                zoomTrackBarControl1.BringToFront();
            }
        }
        void i_Click(object sender, EventArgs e)
        {
            zoomTrackBarControl1.SendToBack();
        }
        void zoomTrackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            ProcResizeControls(zoomTrackBarControl1.Value);
        }
        #endregion

        #region Metodos con retorno
        bool CentralDisponible()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStringCentral))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog("SERVER_SAP", "CENTRAL CEDIS",$"❌ No se pudo conectar al servidor central SAP. Verificar connectionStringCentral = ConfigurationManager.ConnectionStrings[\"CENTRAL\"].ConnectionString. -- VOID CentralDisponible(): {ex.Message}","SERVER_SAP");
                return false;
            }
        }
        bool Guardado()
        {
            bool resultado = true;

            using (SqlConnection conglpi = new SqlConnection(sNodoConexion))
            {
                conglpi.Open();
                SqlTransaction transaccion = conglpi.BeginTransaction();

                try
                {
                    foreach (DataRow row in ds.Tables["dt_SucursalesSQL"].Rows)
                    {
                        if (row.RowState == DataRowState.Modified)
                        {
                            using (SqlCommand cmd = new SqlCommand("PG_IN_UP_Sucursal_Conexion", conglpi, transaccion))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@iIdSucursal", Convert.ToInt32(row["iIdSucursal"]));
                                cmd.Parameters.AddWithValue("@sSucursal", row["sSucursal"].ToString());
                                cmd.Parameters.AddWithValue("@sServidorInstanciaIP", row["sServidorInstanciaIP"].ToString());
                                cmd.Parameters.AddWithValue("@sBaseDatos", row["sBaseDatos"].ToString());
                                cmd.Parameters.AddWithValue("@sUsuario", row["sUsuario"].ToString());
                                cmd.Parameters.AddWithValue("@sContraseña", row["sContraseña"].ToString());
                                cmd.Parameters.AddWithValue("@iPuerto", Convert.ToInt32(row["iPuerto"]));
                                cmd.Parameters.AddWithValue("@bActivo", Convert.ToBoolean(row["bActivo"]));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    Herramientas.Avisa("Error al guardar sucursales modificadas: " + ex.Message);
                    resultado = false;
                }
            }
            return resultado;
        }
        #endregion

        #region ActualizarExistencias
        void ActualizarExistencias(int _iIdSucursal, SqlConnection conn, string controlName, string sSucursal, string IP)
        {
            DateTime dtFechaYHora = DateTime.Now;
            DataTable dte = new DataTable("dt_Existencias");

            WriteLog("SERVER_SAP", "CENTRAL", $"📦 Iniciando validación de existencias para sucursal {_iIdSucursal.ToString()}-{sSucursal}", "-60.8-");

            if (ValidarUltimoIntento(_iIdSucursal, dtFechaYHora))
            {
                try
                {
                    WriteLog("SERVER_SAP", "CENTRAL", $"✔ Validación aprobada. Obteniendo existencias desde Sucursal {_iIdSucursal.ToString()}-{sSucursal}", "-60.8-");

                    using (SqlCommand segundoCmd = new SqlCommand("SELECT ItemCode, WhsCode, OnHand FROM OITW WHERE OnHand > 0 AND WhsCode = @WhsCode", conn))
                    {
                        segundoCmd.Parameters.AddWithValue("@WhsCode", _iIdSucursal);
                        using (SqlDataReader segundoReader = segundoCmd.ExecuteReader())
                        {
                            dte.Clear();
                            dte.Load(segundoReader);
                        }
                    }

                    WriteLog(IP, sSucursal, $"🔄 Datos obtenidos: {dte.Rows.Count} registros. Actualizando en SAP...", _iIdSucursal.ToString());

                    if (ActualizarExistenciaSap(dte, _iIdSucursal))
                    {
                        RegistrarActualizacion(_iIdSucursal, dtFechaYHora);

                        WriteLog("SERVER_SAP", "SAP", $"✅ Existencias  sincronizadas correctamente con SAP y SUcursal {_iIdSucursal.ToString()}-{sSucursal}", "-60.8-");

                        var controles = monitorPanel.Controls.Find(controlName, false);
                        if (controles.Length > 0 && controles[0] is uc_monitor_item i)
                        {
                            i.pb_Titulo.BackColor = Color.Green;
                            i.pb_Titulo.ForeColor = Color.White;
                        }
                    }
                    else
                    {
                        var controles = monitorPanel.Controls.Find(controlName, false);
                        if (controles.Length > 0 && controles[0] is uc_monitor_item i)
                        {
                            i.pb_Titulo.BackColor = Color.Orange;
                        }
                        WriteLog("SERVER_SAP", "SAP", $"❌ Existencias no sincronizadas correctamente con SAP y Sucursal {_iIdSucursal.ToString()}-{sSucursal}", "-60.8-");
                    }

                }
                catch (Exception ex)
                {
                    WriteLog("SISTEMA", "Sucursal", $"❌ Error durante la sincronización de existencias: {ex.Message}", _iIdSucursal.ToString());
                    throw;
                }
            }
            else
            {
                WriteLog("SISTEMA", "Sucursal", $"⏳ No se realiza actualización. Intervalo de 30 minutos no cumplido.", _iIdSucursal.ToString());
            }
        }
        bool ValidarUltimoIntento(int iIdSucursal, DateTime fechaIntento)
        {
            WriteLog("SISTEMA", "Sucursal", $"🕒 Validando intento de actualización para la sucursal {iIdSucursal} con fecha {fechaIntento}", iIdSucursal.ToString());

            using (SqlConnection conc = new SqlConnection(connectionStringCentral))
            {
                conc.Open();
                using (SqlCommand cmd = new SqlCommand("HSM_UP_Sucursales_ConexionesUltimoIntento", conc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@iIdSucursal", iIdSucursal);
                    cmd.Parameters.AddWithValue("@dtFechaUltimoIntentoActualizacion", fechaIntento);

                    var resultado = new SqlParameter("@bPuedeActualizar", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(resultado);

                    cmd.ExecuteNonQuery();
                    bool puedeActualizar = Convert.ToBoolean(resultado.Value);

                    WriteLog("SISTEMA", "Sucursal",
                        puedeActualizar
                        ? $"✅ Validación exitosa. Se permite la actualización."
                        : $"⛔ Validación fallida. No han pasado 30 minutos desde el último intento.",
                        iIdSucursal.ToString());

                    return puedeActualizar;
                }
            }
        }
        void RegistrarActualizacion(int iIdSucursal, DateTime fechaActualizacion)
        {
            WriteLog("SISTEMA", "Sucursal", $"📝 Registrando actualización final con fecha {fechaActualizacion}", iIdSucursal.ToString());

            using (SqlConnection conc = new SqlConnection(connectionStringCentral))
            {
                conc.Open();
                using (SqlCommand cmd = new SqlCommand("HSM_UP_Sucursales_ConexionesActualizacion", conc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@iIdSucursal", iIdSucursal);
                    cmd.Parameters.AddWithValue("@dtFechaUltimaActualizacion", fechaActualizacion);

                    cmd.ExecuteNonQuery();
                }
            }

            WriteLog("SISTEMA", "Sucursal", $"✅ Actualización final registrada correctamente en base de datos", iIdSucursal.ToString());
        }
        bool ActualizarExistenciaSap(DataTable dte, int _iIdSucursal)
        {
            bool Afectado = false;

            WriteLog("SISTEMA", "SAP", $"🚀 Iniciando sincronización de existencias SAP. Total registros: {dte.Rows.Count}", _iIdSucursal.ToString());
            ConectHana HanaConexion = new ConectHana();

            if (HanaConexion.Open())
            {
                using (HanaConexion.HanaTransaction = HanaConexion.HanaConn.BeginTransaction())
                {
                    try
                    {
                        HanaConexion.HanaCommand = HanaConexion.HanaConn.CreateCommand();
                        HanaConexion.HanaCommand.Transaction = HanaConexion.HanaTransaction;

                        string deleteSql = $"DELETE FROM \"PRUEBA_PROCESOS\".\"HSM_TBL_OITW\" WHERE \"WHSCODE\" = '{_iIdSucursal}'";
                        HanaConexion.ProcComandoHanaGenerar(CommandType.Text, "PRUEBA_PROCESOS", deleteSql);

                        HanaConexion.HanaCommand.Parameters.Clear();
                        HanaConexion.HanaCommand.ExecuteNonQuery();

                        foreach (DataRow fila in dte.Rows)
                        {
                            string insertSql = $"INSERT INTO \"PRUEBA_PROCESOS\".\"HSM_TBL_OITW\" (\"ITEMCODE\", \"WHSCODE\", \"ONHAND\") " +
                                               $"VALUES ('{fila["ItemCode"]}', '{fila["WhsCode"]}', {Convert.ToDecimal(fila["OnHand"])})";

                            HanaConexion.ProcComandoHanaGenerar(CommandType.Text, "PRUEBA_PROCESOS", insertSql);

                            HanaConexion.HanaCommand.Parameters.Clear();
                            HanaConexion.HanaCommand.ExecuteNonQuery();
                        }

                        HanaConexion.HanaTransaction.Commit();
                        Afectado = true;

                        WriteLog("SISTEMA", "SAP", "✅ Existencias actualizadas correctamente en SAP y transacción confirmada", _iIdSucursal.ToString());
                    }
                    catch (Exception ex)
                    {
                        HanaConexion.HanaTransaction.Rollback();
                        WriteLog("SISTEMA", "SAP", $"❌ Error al actualizar existencias en SAP. Se ejecutó rollback: {ex.Message}", _iIdSucursal.ToString());
                    }
                    finally
                    {
                        HanaConexion.Close();
                    }
                }
            }
            else
            {
                WriteLog("SISTEMA", "SAP", "⚠️ No se pudo abrir conexión con SAP", _iIdSucursal.ToString());
            }

            return Afectado;
        }
        #endregion

        #region Creacion de Controles
        void ProcCrearControles()
        {
            int cantidadMinimaOptima;
            if (int.TryParse(ConfigurationManager.AppSettings["dCanPendientes"], out cantidadMinimaOptima))
            {
                Console.WriteLine($"✅ Valor obtenido desde configuración: {cantidadMinimaOptima}");
            }
            else
            {
                cantidadMinimaOptima = 20; // Valor por defecto si falla la conversión
            }

            if (monitorPanel != null)
            {

                try
                {
                    Point p = new Point(4, 4);
                    foreach (DataRow roww in ds.Tables["dt_SucursalesSQL"].Rows)
                    {
                        if (Convert.ToBoolean(roww["bActivo"]))
                        {
                            uc_monitor_item i;
                            if (monitorPanel.Controls.Find("L" + roww["iIdSucursal"].ToString(), false).Length == 0)
                            {
                                i = new uc_monitor_item();
                                i.Click += i_Click;
                                i.PorcentajeZoom = zoom;
                                i.Name = "L" + roww["iIdSucursal"].ToString();
                                i.MostrarTitulo = true;
                                i.CantidadMinimaOptima = cantidadMinimaOptima;
                                monitorPanel.Controls.Add(i);
                                i.Location = p;
                                if (p.Y + i.Height > this.Height - 100)
                                {
                                    p.Y = 2;
                                    p.X += i.Width + 2;
                                }
                                else
                                {
                                    p.Y += i.Height + 2;
                                }
                                i.BackColor = SystemColors.ControlLight;
                                i.Visible = true;
                            }
                            else
                            {
                                i = (uc_monitor_item)monitorPanel.Controls.Find("L" + roww["iIdSucursal"].ToString(), false)[0];
                            }
                            i.Valor = roww["iIdSucursal"].ToString() + "-" + roww["sSucursal"].ToString();
                            i.Titulo = roww["sServidorInstanciaIP"].ToString();
                            i.Marcador = "0";
                            i.Transaccion = "0";
                            i.pb_Titulo.BackColor = Color.Orange;
                        }
                    }
                }
                catch
                {
                }
            }
        }
        #endregion
    }
}