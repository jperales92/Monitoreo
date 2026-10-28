namespace monitoreo
{
    partial class frmMonitoreo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMonitoreo));
            this.tab_Central = new System.Windows.Forms.TabControl();
            this.pag_monitor = new System.Windows.Forms.TabPage();
            this.pnl_Monitor = new System.Windows.Forms.Panel();
            this.pag_sucursales = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pl_Controles = new System.Windows.Forms.Panel();
            this.gc_Controles = new DevExpress.XtraEditors.GroupControl();
            this.btn_Editar = new System.Windows.Forms.Button();
            this.btn_Salir = new System.Windows.Forms.Button();
            this.btn_Guardar = new System.Windows.Forms.Button();
            this.pl_Sucursales = new System.Windows.Forms.Panel();
            this.gc_Sucursales = new DevExpress.XtraEditors.GroupControl();
            this.grd_Sucursales = new DevExpress.XtraGrid.GridControl();
            this.gv_Sucursales = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.zoomTrackBarControl1 = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.tab_Central.SuspendLayout();
            this.pag_monitor.SuspendLayout();
            this.pag_sucursales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pl_Controles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Controles)).BeginInit();
            this.pl_Sucursales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Sucursales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_Sucursales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Sucursales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tab_Central
            // 
            this.tab_Central.Controls.Add(this.pag_monitor);
            this.tab_Central.Controls.Add(this.pag_sucursales);
            this.tab_Central.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_Central.Location = new System.Drawing.Point(0, 0);
            this.tab_Central.Name = "tab_Central";
            this.tab_Central.SelectedIndex = 0;
            this.tab_Central.Size = new System.Drawing.Size(1378, 781);
            this.tab_Central.TabIndex = 1;
            // 
            // pag_monitor
            // 
            this.pag_monitor.BackColor = System.Drawing.Color.Black;
            this.pag_monitor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pag_monitor.Controls.Add(this.pnl_Monitor);
            this.pag_monitor.ForeColor = System.Drawing.Color.Lime;
            this.pag_monitor.Location = new System.Drawing.Point(4, 22);
            this.pag_monitor.Name = "pag_monitor";
            this.pag_monitor.Padding = new System.Windows.Forms.Padding(3);
            this.pag_monitor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pag_monitor.Size = new System.Drawing.Size(1370, 755);
            this.pag_monitor.TabIndex = 0;
            this.pag_monitor.Text = "Monitoreo";
            // 
            // pnl_Monitor
            // 
            this.pnl_Monitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Monitor.Location = new System.Drawing.Point(3, 3);
            this.pnl_Monitor.Name = "pnl_Monitor";
            this.pnl_Monitor.Size = new System.Drawing.Size(1360, 745);
            this.pnl_Monitor.TabIndex = 0;
            // 
            // pag_sucursales
            // 
            this.pag_sucursales.BackColor = System.Drawing.Color.Black;
            this.pag_sucursales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pag_sucursales.Controls.Add(this.splitContainer1);
            this.pag_sucursales.ForeColor = System.Drawing.Color.Lime;
            this.pag_sucursales.Location = new System.Drawing.Point(4, 22);
            this.pag_sucursales.Name = "pag_sucursales";
            this.pag_sucursales.Padding = new System.Windows.Forms.Padding(3);
            this.pag_sucursales.Size = new System.Drawing.Size(1370, 755);
            this.pag_sucursales.TabIndex = 1;
            this.pag_sucursales.Text = "Sucursales";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pl_Controles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pl_Sucursales);
            this.splitContainer1.Size = new System.Drawing.Size(1360, 745);
            this.splitContainer1.SplitterDistance = 86;
            this.splitContainer1.TabIndex = 0;
            // 
            // pl_Controles
            // 
            this.pl_Controles.Controls.Add(this.gc_Controles);
            this.pl_Controles.Dock = System.Windows.Forms.DockStyle.Top;
            this.pl_Controles.Location = new System.Drawing.Point(0, 0);
            this.pl_Controles.Name = "pl_Controles";
            this.pl_Controles.Size = new System.Drawing.Size(1360, 83);
            this.pl_Controles.TabIndex = 2;
            // 
            // gc_Controles
            // 
            this.gc_Controles.Controls.Add(this.btn_Editar);
            this.gc_Controles.Controls.Add(this.btn_Salir);
            this.gc_Controles.Controls.Add(this.btn_Guardar);
            this.gc_Controles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc_Controles.Location = new System.Drawing.Point(0, 0);
            this.gc_Controles.Name = "gc_Controles";
            this.gc_Controles.Size = new System.Drawing.Size(1360, 83);
            this.gc_Controles.TabIndex = 0;
            this.gc_Controles.Text = "Controles";
            // 
            // btn_Editar
            // 
            this.btn_Editar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Editar.BackgroundImage = global::monitoreo.Properties.Resources.Editar;
            this.btn_Editar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Editar.Location = new System.Drawing.Point(1225, 38);
            this.btn_Editar.Name = "btn_Editar";
            this.btn_Editar.Size = new System.Drawing.Size(40, 40);
            this.btn_Editar.TabIndex = 5;
            this.btn_Editar.UseVisualStyleBackColor = true;
            // 
            // btn_Salir
            // 
            this.btn_Salir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Salir.BackgroundImage = global::monitoreo.Properties.Resources.no_negro;
            this.btn_Salir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Salir.Location = new System.Drawing.Point(1317, 38);
            this.btn_Salir.Name = "btn_Salir";
            this.btn_Salir.Size = new System.Drawing.Size(40, 40);
            this.btn_Salir.TabIndex = 4;
            this.btn_Salir.UseVisualStyleBackColor = true;
            // 
            // btn_Guardar
            // 
            this.btn_Guardar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Guardar.BackgroundImage = global::monitoreo.Properties.Resources.save_512;
            this.btn_Guardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Guardar.Location = new System.Drawing.Point(1271, 38);
            this.btn_Guardar.Name = "btn_Guardar";
            this.btn_Guardar.Size = new System.Drawing.Size(40, 40);
            this.btn_Guardar.TabIndex = 3;
            this.btn_Guardar.UseVisualStyleBackColor = true;
            // 
            // pl_Sucursales
            // 
            this.pl_Sucursales.Controls.Add(this.gc_Sucursales);
            this.pl_Sucursales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pl_Sucursales.Location = new System.Drawing.Point(0, 0);
            this.pl_Sucursales.Name = "pl_Sucursales";
            this.pl_Sucursales.Size = new System.Drawing.Size(1360, 655);
            this.pl_Sucursales.TabIndex = 3;
            // 
            // gc_Sucursales
            // 
            this.gc_Sucursales.Controls.Add(this.grd_Sucursales);
            this.gc_Sucursales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc_Sucursales.Location = new System.Drawing.Point(0, 0);
            this.gc_Sucursales.Name = "gc_Sucursales";
            this.gc_Sucursales.Size = new System.Drawing.Size(1360, 655);
            this.gc_Sucursales.TabIndex = 0;
            this.gc_Sucursales.Text = "Sucursales";
            // 
            // grd_Sucursales
            // 
            this.grd_Sucursales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd_Sucursales.Location = new System.Drawing.Point(2, 23);
            this.grd_Sucursales.MainView = this.gv_Sucursales;
            this.grd_Sucursales.Name = "grd_Sucursales";
            this.grd_Sucursales.Size = new System.Drawing.Size(1356, 630);
            this.grd_Sucursales.TabIndex = 0;
            this.grd_Sucursales.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_Sucursales});
            // 
            // gv_Sucursales
            // 
            this.gv_Sucursales.GridControl = this.grd_Sucursales;
            this.gv_Sucursales.Name = "gv_Sucursales";
            // 
            // zoomTrackBarControl1
            // 
            this.zoomTrackBarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomTrackBarControl1.EditValue = null;
            this.zoomTrackBarControl1.Location = new System.Drawing.Point(1172, 762);
            this.zoomTrackBarControl1.Name = "zoomTrackBarControl1";
            this.zoomTrackBarControl1.Size = new System.Drawing.Size(204, 16);
            this.zoomTrackBarControl1.TabIndex = 0;
            // 
            // frmMonitoreo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1378, 781);
            this.Controls.Add(this.tab_Central);
            this.Controls.Add(this.zoomTrackBarControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMonitoreo";
            this.Text = "Monitoreo";
            this.tab_Central.ResumeLayout(false);
            this.pag_monitor.ResumeLayout(false);
            this.pag_sucursales.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pl_Controles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gc_Controles)).EndInit();
            this.pl_Sucursales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gc_Sucursales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_Sucursales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Sucursales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ZoomTrackBarControl zoomTrackBarControl1;
        private System.Windows.Forms.TabControl tab_Central;
        private System.Windows.Forms.TabPage pag_monitor;
        private System.Windows.Forms.TabPage pag_sucursales;
        private System.Windows.Forms.Panel pnl_Monitor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pl_Controles;
        private DevExpress.XtraEditors.GroupControl gc_Controles;
        private System.Windows.Forms.Panel pl_Sucursales;
        private DevExpress.XtraEditors.GroupControl gc_Sucursales;
        private System.Windows.Forms.Button btn_Editar;
        private System.Windows.Forms.Button btn_Salir;
        private System.Windows.Forms.Button btn_Guardar;
        private DevExpress.XtraGrid.GridControl grd_Sucursales;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_Sucursales;
    }
}

