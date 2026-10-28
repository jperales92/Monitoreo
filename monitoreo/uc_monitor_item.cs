using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EvoToolCOM
{
    public partial class uc_monitor_item : UserControl
    {

        private string _Marcador;
        public string Titulo { get; set; }
        public string Valor { get; set; }
        public int Estatus { get; set; }

        private string _Transaccion;
        private bool _MostrarTitulo;
        private int _CantidadMinimaOptima;
        private decimal _PorcentajeZoom;

        Rectangle rThis { get; set; }
        Rectangle rMarcador { get; set; }
        Rectangle rTransaccion { get; set; }

        public uc_monitor_item()
        {
            InitializeComponent();
            this.Load += uc_monitor_item_Load;
            rThis = new Rectangle(0, 0, this.Width, this.Height);
            rMarcador = new Rectangle(0, 0, pb_Marcador.Width, pb_Marcador.Height);
            rTransaccion = new Rectangle(0, 0, pb_Transaccion.Width, pb_Transaccion.Height);
        }
        public bool MostrarTitulo { get { return _MostrarTitulo; } set { _MostrarTitulo = value; splitContainer1.Panel1Collapsed = !value; } }
        public int CantidadMinimaOptima { get { return _CantidadMinimaOptima; } set { _CantidadMinimaOptima = value; this.Refresh(); } }
        public string Marcador { get { return _Marcador; } set { _Marcador = value; this.Refresh(); } }

        public string Transaccion { get { return _Transaccion; } set { _Transaccion = value; this.Refresh(); } }

        public decimal PorcentajeZoom 
        {
            get { return _PorcentajeZoom; }
            set 
            {
                _PorcentajeZoom = value;
                ProcResize();
            }
        }
        void ProcResize()
        {
            if (_PorcentajeZoom != 0)
            {
                int tw, th, mw,mh, trw, trh;
                tw = (int)(Convert .ToDecimal(rThis.Width) * (Math.Abs((decimal)_PorcentajeZoom) / (decimal)100.00));
                th = (int)(Convert .ToDecimal(rThis.Height) * (Math.Abs((decimal)_PorcentajeZoom) / (decimal)100.00));
                mw = (int)(Convert.ToDecimal(rMarcador.Width) * (Math.Abs((decimal)_PorcentajeZoom) / (decimal)100.00));
                mh = (int)(Convert.ToDecimal(rMarcador.Height) * (Math.Abs((decimal)_PorcentajeZoom) / (decimal)100.00));
                trw = (int)(Convert.ToDecimal(rTransaccion.Width) * (Math.Abs((decimal)_PorcentajeZoom) / (decimal)100.00));
                trh = (int)(Convert.ToDecimal(rTransaccion.Height) * (Math.Abs((decimal)_PorcentajeZoom) / (decimal)100.00));
                if (_PorcentajeZoom > 0)
                {
                    tw = tw * -1;
                    th = th * -1;
                    mw = mw * -1;
                    mh = th * -1;
                    trw = trw * -1;
                    trh = trh * -1;
                }
                this.Height = rThis.Height - th;
                this.Width = rThis.Width - tw;
                
                pb_Marcador.Width = rMarcador.Width - mw;

                pb_Transaccion.Width = rTransaccion.Width - trw;
            }
        }
        void uc_monitor_item_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !MostrarTitulo;
            pb_Marcador.Paint += pictureBox1_Paint;
            pb_Titulo.Paint += pb_Titulo_Paint;
            pb_Valor.Paint += pb_Valor_Paint;
            pb_Transaccion.Paint += Pb_Transaccion_Paint;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
        }
        private void Pb_Transaccion_Paint(object sender, PaintEventArgs e)
        {
            if (Transaccion != null && Transaccion.Trim().Length > 0)
                ProcPintar(e, pb_Transaccion, Transaccion, true);
        }
        void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.Refresh();
        }
        void pb_Valor_Paint(object sender, PaintEventArgs e)
        {
            if (Valor != null && Valor.Trim().Length > 0)
            {
                ProcPintar(e, pb_Valor, Valor);
            }
        }
        void pb_Titulo_Paint(object sender, PaintEventArgs e)
        {
            if (Titulo != null && Titulo.Trim().Length > 0)
                ProcPintar(e, pb_Titulo, Titulo);
        }
        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if(Marcador!=null && Marcador.Trim().Length>0)
                ProcPintar(e, pb_Marcador, Marcador, true);
        }
        void ProcPintar(PaintEventArgs e, PictureBox pb, string vText, bool bIndSemaforo = false)
        {
            int iSF = 6;
            Font f = new Font("Tahoma", iSF, FontStyle.Bold);
            while (e.Graphics.MeasureString(vText, f).Width < pb.Width && e.Graphics.MeasureString(vText, f).Height < pb.Height)
            {
                iSF++;
                f = new Font("Tahoma", iSF, FontStyle.Bold);
            }
            iSF--;
            f = new Font("Tahoma", iSF, FontStyle.Bold);
            RectangleF rec = new RectangleF(0, 0, pb.Width, pb.Height);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            if (!bIndSemaforo)
            {
                f = new Font("Tahoma", iSF, FontStyle.Bold);
                rec = new RectangleF(0, 0, pb.Width, pb.Height);
                sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(vText.Trim(), f, Brushes.Black, rec, sf);
            }
            else
            {
                if (CantidadMinimaOptima == 0)
                {
                    e.Graphics.DrawString(vText.Trim(), f, Brushes.Black, rec, sf);
                }
                else
                {
                    int iValor = 0;
                    if (int.TryParse(vText, out iValor))
                    {
                        if (iValor < CantidadMinimaOptima)
                        {
                            e.Graphics.DrawString(vText.Trim(), f, Brushes.Green, rec, sf);
                        }
                        else if (iValor >= CantidadMinimaOptima && iValor < (CantidadMinimaOptima * 2))
                        {
                            e.Graphics.DrawString(vText.Trim(), f, Brushes.Orange, rec, sf);
                        }
                        else
                        {
                            e.Graphics.DrawString(vText.Trim(), f, Brushes.Red, rec, sf);
                        }
                    }
                    else
                    {
                        e.Graphics.DrawString(vText.Trim(), f, Brushes.Black, rec, sf);
                    }
                }
            }
        }
        private void uc_Click(object sender, EventArgs e)
        {
            this.InvokeOnClick(this, e);
        }
    }
}