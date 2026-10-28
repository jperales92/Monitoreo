using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace monitoreo
{
    public partial class frmMessageError : Form
    {
        public string Mensaje { get; set; }
        bool SupressCloseWithEnter { get; set; }

        public Font FontMessage
        {
            get { return txtMensaje.Font; }
        }

        public frmMessageError()
        {
            InitializeComponent();
            this.Load += frmMessageError_Load;
            btnNotificar.Enabled = false;
        }

        public frmMessageError(bool ShowNotifyButon)
        {
            InitializeComponent();
            this.Load += frmMessageError_Load;
            btnNotificar.Enabled = ShowNotifyButon;
        }

        public frmMessageError(bool ShowNotifyButon, bool vSupressCloseWithEnter)
        {
            InitializeComponent();
            this.Load += frmMessageError_Load;
            btnNotificar.Enabled = ShowNotifyButon;
            btnAceptar.Enabled = !vSupressCloseWithEnter;
            SupressCloseWithEnter = vSupressCloseWithEnter;
            //lbl_Cerrar.Visible = vSupressCloseWithEnter;
        }

        void frmMessageError_Load(object sender, EventArgs e)
        {
            txtMensaje.Text = Mensaje;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(!SupressCloseWithEnter)
                this.Close();
        }
    }
}
