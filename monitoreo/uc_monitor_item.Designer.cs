namespace EvoToolCOM
{
    partial class uc_monitor_item
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pb_Marcador = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pb_Titulo = new System.Windows.Forms.PictureBox();
            this.pb_Valor = new System.Windows.Forms.PictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pb_Transaccion = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Marcador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Titulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Valor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Transaccion)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_Marcador
            // 
            this.pb_Marcador.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_Marcador.Location = new System.Drawing.Point(0, 0);
            this.pb_Marcador.Name = "pb_Marcador";
            this.pb_Marcador.Size = new System.Drawing.Size(25, 104);
            this.pb_Marcador.TabIndex = 0;
            this.pb_Marcador.TabStop = false;
            this.pb_Marcador.Click += new System.EventHandler(this.uc_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pb_Titulo);
            this.splitContainer1.Panel1MinSize = 10;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pb_Valor);
            this.splitContainer1.Panel2MinSize = 10;
            this.splitContainer1.Size = new System.Drawing.Size(350, 104);
            this.splitContainer1.SplitterDistance = 41;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // pb_Titulo
            // 
            this.pb_Titulo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pb_Titulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_Titulo.Location = new System.Drawing.Point(0, 0);
            this.pb_Titulo.Name = "pb_Titulo";
            this.pb_Titulo.Size = new System.Drawing.Size(350, 41);
            this.pb_Titulo.TabIndex = 4;
            this.pb_Titulo.TabStop = false;
            this.pb_Titulo.Click += new System.EventHandler(this.uc_Click);
            // 
            // pb_Valor
            // 
            this.pb_Valor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pb_Valor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_Valor.Location = new System.Drawing.Point(0, 0);
            this.pb_Valor.Name = "pb_Valor";
            this.pb_Valor.Size = new System.Drawing.Size(350, 58);
            this.pb_Valor.TabIndex = 5;
            this.pb_Valor.TabStop = false;
            this.pb_Valor.Click += new System.EventHandler(this.uc_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitContainer2.Location = new System.Drawing.Point(350, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pb_Marcador);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pb_Transaccion);
            this.splitContainer2.Size = new System.Drawing.Size(54, 104);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 5;
            // 
            // pb_Transaccion
            // 
            this.pb_Transaccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_Transaccion.Location = new System.Drawing.Point(0, 0);
            this.pb_Transaccion.Name = "pb_Transaccion";
            this.pb_Transaccion.Size = new System.Drawing.Size(25, 104);
            this.pb_Transaccion.TabIndex = 6;
            this.pb_Transaccion.TabStop = false;
            // 
            // uc_monitor_item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitContainer2);
            this.Name = "uc_monitor_item";
            this.Size = new System.Drawing.Size(404, 104);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Marcador)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Titulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Valor)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Transaccion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_Marcador;
        private System.Windows.Forms.PictureBox pb_Valor;
        public System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pb_Transaccion;
        public System.Windows.Forms.PictureBox pb_Titulo;
    }
}
