using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.IO;

namespace monitoreo
{
    public class ServicioRemoto
    {
        private readonly string _ipServidor;
        private readonly string _serviceName;
        private readonly string _processName;
        private readonly string _sucursal;
        private readonly string _iIdSucursal;
        public ServicioRemoto(string ipServidor, string serviceName, string processName, string sucursal, string iIdSucursal)
        {
            _ipServidor = ipServidor;
            _serviceName = serviceName;
            _processName = processName;
            _sucursal = sucursal;
            _iIdSucursal = iIdSucursal;
        }
        public void ReiniciarSistema()
        {
            WriteLog("🚨 Se requiere reinicio debido a un proceso bloqueado o servicio inactivo.");
            EnviarMensaje(0);

            if (MatarProcesoRemoto())
                WriteLog("✅ Proceso eliminado correctamente.");
            else
                WriteLog("⚠ Hubo un problema al intentar eliminar el proceso.");

            Thread.Sleep(2000);
            EjecutarComando($"net use \\\\{_ipServidor} /user:hsmadmin HSMrenew.");

            ReiniciarServicio();

            EjecutarComando($"net use \\\\{_ipServidor} /delete");
        }
        private void ReiniciarServicio()
        {
            WriteLog("🛑 Deteniendo servicio antes de reiniciar...");
            EjecutarComando($"sc \\\\{_ipServidor} stop {_serviceName}");
            Thread.Sleep(3000);

            if (ServicioEstaCorriendo())
            {
                WriteLog("⚠ Servicio sigue activo, forzando cierre...");
                EjecutarComando($"taskkill /F /FI \"SERVICES eq {_serviceName}\"");
                Thread.Sleep(3000);
            }

            if (!ServicioEstaCorriendo())
            {
                WriteLog("🔄 Iniciando servicio...");
                EjecutarComando($"sc \\\\{_ipServidor} start {_serviceName}");
            }
            else
            {
                WriteLog("⚠ Servicio ya estaba en ejecución, no es necesario reiniciarlo.");
            }

            WriteLog(ServicioEstaCorriendo() ? "✅ Servicio reiniciado correctamente." : "⚠ Advertencia: El servicio no se reinició correctamente.");
        }
        private bool MatarProcesoRemoto()
        {
            try
            {
                string checkCommand = $" tasklist /S {_ipServidor} /U hsmadmin /P HSMrenew. | findstr \"{_processName}\"";
                string output = EjecutarComandoConSalida(checkCommand);

                if (string.IsNullOrEmpty(output))
                {
                    WriteLog($"⚠ Proceso '{_processName}' no está activo en {_ipServidor}.");
                    return false;
                }

                foreach (string line in output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1 && int.TryParse(parts[1], out int pid))
                    {
                        WriteLog($"🛑 Finalizando proceso '{_processName}' en {_ipServidor} (PID: {pid})...");
                        string result = EjecutarComandoConSalida($"taskkill /S {_ipServidor} /U hsmadmin /P HSMrenew. /F /PID {pid}");

                        if (!string.IsNullOrEmpty(result) && result.Contains("SUCCESS"))
                        {
                            WriteLog($"✅ Proceso '{_processName}' (PID: {pid}) eliminado.");
                            return true;
                        }
                        WriteLog($"⚠ No se pudo eliminar el proceso '{_processName}' (PID: {pid}).");
                    }
                }

                WriteLog($"⚠ No se pudo extraer el PID correctamente para '{_processName}'.");
                return false;
            }
            catch (Exception ex)
            {
                WriteLog($"❌ Excepción al finalizar el proceso '{_processName}': {ex.Message}");
                return false;
            }
        }
        private bool ServicioEstaCorriendo()
        {
            try
            {
                using (ServiceController sc = new ServiceController(_serviceName, _ipServidor))
                {
                    return sc.Status == ServiceControllerStatus.Running;
                }
            }
            catch (Exception ex)
            {
                WriteLog($"❌ Error al verificar estado del servicio '{_serviceName}': {ex.Message}");
                return false;
            }
        }
        private void EjecutarComando(string comando)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {comando}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    WriteLog($"🔹 Comando ejecutado: {comando}");
                    if (!string.IsNullOrEmpty(output)) WriteLog($"📝 Salida: {output}");
                    if (!string.IsNullOrEmpty(error)) WriteLog($"⚠ Error: {error}");
                }
            }
            catch (Exception ex)
            {
                WriteLog($"❌ Excepción al ejecutar comando '{comando}': {ex.Message}");
            }
        }
        private string EjecutarComandoConSalida(string comando)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {comando}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    return process.StandardOutput.ReadToEnd().Trim();
                }
            }
            catch (Exception ex)
            {
                WriteLog($"❌ Error ejecutando comando '{comando}': {ex.Message}");
                return string.Empty;
            }
        }
        private void WriteLog(string message)
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string logDirectory = Path.Combine(basePath, "LOG");
                Directory.CreateDirectory(logDirectory); // 🗂 Crea la carpeta si no existe

                string logFileName = $"ServicioRemoto_{DateTime.Now:yyyyMMdd}.log";
                string logPath = Path.Combine(logDirectory, logFileName);

                // 📝 Mensaje con timestamp y contexto
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [IP: {_ipServidor}, Sucursal: {_iIdSucursal} - {_sucursal}] {message}";

                using (FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al escribir en el log: {ex.Message}");
            }
        }
        private void EnviarMensaje(int pendientes)
        {
            WriteLog($"📩 Notificación sobre {_sucursal} ({_iIdSucursal}) en {_ipServidor}. Pendientes: {pendientes}");
        }
    }
}