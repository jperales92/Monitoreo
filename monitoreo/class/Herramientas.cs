using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace monitoreo
{
    public class Herramientas
    {
        public Herramientas()
        {
            //
            // TODO: agregar aquí la lógica del constructor
            //
        }
        public static bool Confirma(string vMessage)
        {
            bool result;
            result = (MessageBox.Show(vMessage, "Ventana de confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
            return result;
        }

        public static bool Confirma(string vMessage, MessageBoxDefaultButton Def)
        {
            bool result;
            result = (MessageBox.Show(vMessage, "Ventana de confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Def) == DialogResult.Yes);
            return result;
        }

        public static bool ConfirmarMensaje(string Mensaje)
        {
            bool result;
            result = (MessageBox.Show(Mensaje, "Ventana de confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
            return result;
        }

        public static DialogResult ConfirmarAceptarCancelar(string Mensaje)
        {
            DialogResult result;
            result = (MessageBox.Show(Mensaje, "Ventana de confirmación", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1));
            return result;
        }
        public static void Avisa(string vMessage)
        {
            frmMessageError frmMensaje = new frmMessageError();
            frmMensaje.Mensaje = vMessage;
            Graphics g = frmMensaje.CreateGraphics();
            SizeF sf = g.MeasureString(vMessage, frmMensaje.FontMessage);
            if ((int)sf.Height + 130 > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height)
                frmMensaje.WindowState = FormWindowState.Maximized;
            else
            {
                if ((int)sf.Width + 150 > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width)
                    frmMensaje.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 50;
                else
                    frmMensaje.Width = (int)sf.Width + 150;

                frmMensaje.Height = (int)sf.Height + 130;
            }
            frmMensaje.ShowDialog();
        }
        public static void Avisa(string vMessage, bool ShowNotifyButon)
        {
            frmMessageError frmMensaje = new frmMessageError(ShowNotifyButon);
            frmMensaje.Mensaje = vMessage;
            frmMensaje.ShowDialog();
        }

        public static void Avisa(string vMessage, bool ShowNotifyButon, bool SupressClosWithEnter)
        {
            frmMessageError frmMensaje = new frmMessageError(ShowNotifyButon, SupressClosWithEnter);
            frmMensaje.Mensaje = vMessage;
            frmMensaje.ShowDialog();
        }
    }
}
