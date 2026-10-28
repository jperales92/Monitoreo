using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using static monitoreo.MascaraGrid;

namespace monitoreo
{
    public class MascaraGrid
    {
        public enum EstatusFormulario
        {
            Consultando,
            Editando
        }
        List<GridView> VistasRegistradas = new List<GridView>();
        public DataSet dsCat { get; set; }
        public MascaraGrid(DataSet vDataSetCatalogo)
        {
            dsCat = vDataSetCatalogo;
        }
        public void TerminarEdicionGrids()
        {
            foreach (GridView gv in VistasRegistradas)
            {
                if (gv.GridControl != null)
                    gv.GridControl.EmbeddedNavigator.Buttons.DoClick(gv.GridControl.EmbeddedNavigator.Buttons.EndEdit);
            }
        }
        public void AplicarMascara(GridView vGridView)
        {
            AplicarMascara(vGridView, vGridView.GetDataRow(0).Table, EstatusFormulario.Consultando);
        }
        public void AplicarMascara(GridView vGridView, DataTable dt, EstatusFormulario vEstatusModulo)
        {
            vGridView.OptionsView.ColumnAutoWidth = false;
            ProcFormatColumns(vGridView, dt, vEstatusModulo);
        }
        public void ProcFormatColumns(GridView vGridView, DataTable dt, EstatusFormulario vEstatusModulo)
        {
            vGridView.Tag = dt.TableName;
            if (!VistasRegistradas.Contains(vGridView))
            {
                VistasRegistradas.Add(vGridView);
            }
            int visibleIndex = 0;
            foreach (GridColumn c in vGridView.Columns)
            {
                c.Visible = false;
                c.OptionsColumn.AllowEdit = false;
                c.OptionsColumn.ReadOnly = true;
                c.OptionsColumn.AllowFocus = false;
                if (dt.Columns[c.FieldName] != null)
                {
                    string Caption = dt.Columns[c.FieldName].Caption;

                    if (Caption.Contains("(v)"))
                    {
                        c.Visible = true;
                        c.OptionsColumn.AllowFocus = false;
                        
                        if (Caption.ToLower().Contains("(gs)"))
                        {
                            GridGroupSummaryItem item = new GridGroupSummaryItem();
                            item.FieldName = c.FieldName;
                            item.ShowInGroupColumnFooter = c;
                            item.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            if (Caption.ToLower().Contains("(p)"))
                                item.DisplayFormat = "{0:c2}";

                            vGridView.GroupSummary.Add(item);
                        }
                        if (Caption.ToLower().Contains("(n)"))
                        {
                            c.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            c.DisplayFormat.FormatString = "(n)";
                            if (!Caption.ToLower().Contains("(ns)"))
                                c.Summary.Add(DevExpress.Data.SummaryItemType.Sum);
                        }
                        if (Caption.ToLower().Contains("(num)"))
                        {
                            c.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            c.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            c.DisplayFormat.FormatString = "n2";
                        }
                        //if (Caption.ToLower().Contains("(sum)"))
                        //{
                        //    c.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        //    c.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        //    c.DisplayFormat.FormatString = "n2";
                        //    c.Summary.Add(DevExpress.Data.SummaryItemType.Sum);
                        //    c.SummaryItem.DisplayFormat = "{0:n2}";

                        //}

                        if (Caption.ToLower().Contains("(sort"))
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(Caption, @"\(sort(\d+)\)");
                            if (match.Success)
                            {
                                int sortIndex = int.Parse(match.Groups[1].Value);
                                c.SortOrder = ColumnSortOrder.Ascending;
                                c.SortIndex = sortIndex;
                            }
                        }

                        if (Caption.ToLower().Contains("(c)"))
                        {
                            c.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            c.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            c.DisplayFormat.FormatString = "C";
                            if (!Caption.ToLower().Contains("(ns)"))
                            {
                                c.Summary.Add(DevExpress.Data.SummaryItemType.Sum);
                                c.SummaryItem.DisplayFormat = "{0:C2}";
                            }
                        }
                        if (Caption.ToLower().Contains("(g)"))
                        {
                            c.Group();
                        }
                        if (Caption.ToLower().Contains("(n2)"))
                        {

                            c.DisplayFormat.FormatString = "{0:N2}";
                            c.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        }
                        if (c.FieldName.Contains("dtFecha"))
                        {
                            c.DisplayFormat.FormatType = FormatType.DateTime;
                            c.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
                        }
                        Caption = System.Text.RegularExpressions.Regex.Replace(Caption, @"\(sort\d+\)", "");

                        c.Caption = Caption.Replace("_", "").Replace("(sum)", "").Replace("(money)", "")
                                           .Replace("(fx)", "").Replace("(btn)", "").Replace("(unidad)", "")
                                           .Replace("(centro)", "").Replace("(TipoPieza)", "").Replace("(centros)", "")
                                           .Replace("(time)", "").Replace("(dttime)", "").Replace("(dtshort)", "")
                                           .Replace("(dtlong)", "").Replace("(v)", "").Replace("(sed)", "")
                                           .Replace("(img)", "").Replace("(gs)", "").Replace("(p)", "")
                                           .Replace("(ok)", "").Replace("(ed)", "").Replace("(n)", "")
                                           .Replace("(c)", "").Replace("(g)", "").Replace("(ns)", "")
                                           .Replace("(ctrl)", "").Replace("(cmd)", "").Replace("(+)", "")
                                           .Replace("(x)", "").Replace("_", " ").Replace("(tipoperiodo)", "")
                                           .Replace("(estatuscorte)", "").Replace("(iIdGrupo)", "")
                                           .Replace("(iIdLayOut)", "").Replace("(iIdMaquina)", "").Replace("(iTurno)", "")
                                           .Replace("(n2)", "").Replace("(tipoexplosion)", "").Replace("(centros)", "").Replace("(iIdSubtipoOperacion)", "")
                                           .Replace("(iDia)", "").Replace("(dtFecha)", ""); 
                    }
                    else
                    {
                        if (c.FieldName.Contains("dtFecha"))
                        {
                            c.DisplayFormat.FormatType = FormatType.DateTime;
                            c.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
                        }
                    }
                }
            }

            var columnasOrdenadas = vGridView.Columns.Where(c => c.Visible).Select(c => new
             {
                 Columna = c,
                 SortIndex = ExtraerSortIndex(dt.Columns[c.FieldName].Caption)
             })
             .OrderBy(x => x.SortIndex)
             .ToList();

            
            foreach (var item in columnasOrdenadas)
            {
                item.Columna.VisibleIndex = visibleIndex++;
            }

            vGridView.BestFitColumns();
            vGridView.ExpandAllGroups();
            ProcHabilitarSegunEstatus(vEstatusModulo);
        }
        private int ExtraerSortIndex(string caption)
        {
            var match = System.Text.RegularExpressions.Regex.Match(caption, @"\(sort(\d+)\)");
            return match.Success ? int.Parse(match.Groups[1].Value) : int.MaxValue;
        }
        public void ProcHabilitarSegunEstatus(EstatusFormulario vEstatusModulo)
        {
            ProcHabilitarSegunEstatus(vEstatusModulo, string.Empty);
        }
        public void ProcHabilitarSegunEstatus(EstatusFormulario vEstatusModulo, string vNameGrid)
        {
            if (vNameGrid != string.Empty)
            {
                foreach (GridView gv in VistasRegistradas)
                {
                    if (gv.Name == vNameGrid)
                    {
                        HabilitarSegunEstatus(vEstatusModulo, gv);
                    }
                }
            }
            else
            {
                foreach (GridView gv in VistasRegistradas)
                {
                    HabilitarSegunEstatus(vEstatusModulo, gv);
                }
            }
        }
        public void HabilitarSegunEstatus(EstatusFormulario vEstatusModulo, GridView gv)
        {

            foreach (GridColumn c in gv.Columns)
            {
                if (c.Visible)
                {
                    string Caption = dsCat.Tables[gv.Tag.ToString()].Columns[c.FieldName].Caption;
                    if (Caption.ToLower().Contains("(sed)"))
                    {
                        c.OptionsColumn.AllowEdit = true;
                        c.OptionsColumn.ReadOnly = false;
                        c.OptionsColumn.AllowFocus = true;
                    }
                    else
                    {
                        if (Caption.ToLower().Contains("(ed)"))
                        {
                            c.OptionsColumn.AllowEdit = vEstatusModulo != EstatusFormulario.Consultando;
                            c.OptionsColumn.ReadOnly = vEstatusModulo == EstatusFormulario.Consultando;
                            c.OptionsColumn.AllowFocus = vEstatusModulo != EstatusFormulario.Consultando;
                            if (vEstatusModulo != EstatusFormulario.Consultando)
                                c.AppearanceCell.BackColor = Color.LemonChiffon;
                            else
                                c.AppearanceCell.BackColor = Color.WhiteSmoke;
                        }
                        else
                        {
                            c.OptionsColumn.AllowEdit = false;
                            c.OptionsColumn.ReadOnly = true;
                            c.OptionsColumn.AllowFocus = false;
                        }
                    }
                }
            }
            gv.ExpandAllGroups();
        }
        public void ProcExpandAllRows(GridView view)
        {
            view.BeginUpdate();
            try
            {
                int dataRowCount = view.DataRowCount;
                for (int rHandle = 0; rHandle < dataRowCount; rHandle++)
                    view.SetMasterRowExpanded(rHandle, true);
            }
            finally
            {
                view.EndUpdate();
            }
        }
        public void ProcExpandAllGroups(GridView view)
        {
            view.BeginUpdate();
            try
            {
                view.ExpandAllGroups();
            }
            finally
            {
                view.EndUpdate();
            }
        }
        public void ProcContraerGroups(GridView view)
        {
            view.BeginUpdate();
            try
            {
                view.CollapseAllGroups();
            }
            finally
            {
                view.EndUpdate();
            }
        }
        
    }
}