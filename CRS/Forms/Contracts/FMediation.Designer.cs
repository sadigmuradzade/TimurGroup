namespace CRS.Forms.Contracts
{
    partial class FMediation
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMediation));
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.NewBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.StopBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ActiveBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.MediationGridControl = new DevExpress.XtraGrid.GridControl();
            this.MediationGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Mediation_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_ContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_Defendant = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_SendDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_ApplicationDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_SessionDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_Result = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_FirstPaymentDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_Representative = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_Note = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_StatusName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_UsedUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Mediation_StatusID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MediationGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MediationGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.StopBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ActiveBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // NewBarButton
            // 
            this.NewBarButton.Caption = "Yeni";
            this.NewBarButton.Id = 0;
            this.NewBarButton.ImageOptions.Image = global::CRS.Properties.Resources.plus_16;
            this.NewBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.NewBarButton.Name = "NewBarButton";
            this.NewBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.NewBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButton_ItemClick);
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Dəyiş";
            this.EditBarButton.Id = 1;
            this.EditBarButton.ImageOptions.Image = global::CRS.Properties.Resources.edit_16;
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Id = 2;
            this.DeleteBarButton.ImageOptions.Image = global::CRS.Properties.Resources.minus_16;
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete));
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 3;
            this.RefreshBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.Image")));
            this.RefreshBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.LargeImage")));
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // StopBarButton
            // 
            this.StopBarButton.Caption = "Mediasiyanı bitir";
            this.StopBarButton.Id = 6;
            this.StopBarButton.ImageOptions.Image = global::CRS.Properties.Resources.cancel_16;
            this.StopBarButton.Name = "StopBarButton";
            this.StopBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.StopBarButton_ItemClick);
            // 
            // ActiveBarButton
            // 
            this.ActiveBarButton.Caption = "Mediasiyanı yenidən aktiv et";
            this.ActiveBarButton.Id = 7;
            this.ActiveBarButton.ImageOptions.Image = global::CRS.Properties.Resources.lock_open_16;
            this.ActiveBarButton.Name = "ActiveBarButton";
            this.ActiveBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ActiveBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 4;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.LargeImage")));
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // ExportBarButton
            // 
            this.ExportBarButton.Caption = "İxrac et";
            this.ExportBarButton.Id = 5;
            this.ExportBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.ToolBar});
            this.BarManager.DockControls.Add(this.barDockControlTop);
            this.BarManager.DockControls.Add(this.barDockControlBottom);
            this.BarManager.DockControls.Add(this.barDockControlLeft);
            this.BarManager.DockControls.Add(this.barDockControlRight);
            this.BarManager.DockControls.Add(this.StandaloneBarDockControl);
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.NewBarButton,
            this.EditBarButton,
            this.DeleteBarButton,
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarButton,
            this.StopBarButton,
            this.ActiveBarButton});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 8;
            // 
            // ToolBar
            // 
            this.ToolBar.BarName = "Main menu";
            this.ToolBar.DockCol = 0;
            this.ToolBar.DockRow = 0;
            this.ToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.ToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.StopBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ActiveBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.ToolBar.OptionsBar.DrawBorder = false;
            this.ToolBar.OptionsBar.DrawDragBorder = false;
            this.ToolBar.OptionsBar.MultiLine = true;
            this.ToolBar.OptionsBar.UseWholeRow = true;
            this.ToolBar.StandaloneBarDockControl = this.StandaloneBarDockControl;
            this.ToolBar.Text = "Main menu";
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(32, 563);
            this.StandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1484, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 613);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1484, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 613);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1484, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 613);
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 563);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(1484, 50);
            this.PanelOption.TabIndex = 74;
            // 
            // BCancel
            // 
            this.BCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            office2010Red1.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Red1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Red1.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red1.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red1.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Red1.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Red1.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red1.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red1.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red1.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red1.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red1.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red1.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Red1.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Red1.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red1.TextColor = System.Drawing.Color.White;
            this.BCancel.ColorTable = office2010Red1;
            this.BCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BCancel.Location = new System.Drawing.Point(1395, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // MediationGridControl
            // 
            this.MediationGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MediationGridControl.Location = new System.Drawing.Point(32, 0);
            this.MediationGridControl.MainView = this.MediationGridView;
            this.MediationGridControl.Name = "MediationGridControl";
            this.MediationGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepositoryItemPictureEdit});
            this.MediationGridControl.Size = new System.Drawing.Size(1452, 563);
            this.MediationGridControl.TabIndex = 76;
            this.MediationGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.MediationGridView});
            // 
            // MediationGridView
            // 
            this.MediationGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.MediationGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.MediationGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.MediationGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MediationGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.MediationGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.MediationGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MediationGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.MediationGridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.MediationGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.MediationGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Indigo;
            this.MediationGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.MediationGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.MediationGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.MediationGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.MediationGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.MediationGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.MediationGridView.AppearancePrint.FooterPanel.Options.UseTextOptions = true;
            this.MediationGridView.AppearancePrint.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MediationGridView.AppearancePrint.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.MediationGridView.ColumnPanelRowHeight = 40;
            this.MediationGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Mediation_SS,
            this.Mediation_ID,
            this.Mediation_ContractCode,
            this.Mediation_Defendant,
            this.Mediation_SendDate,
            this.Mediation_ApplicationDate,
            this.Mediation_Name,
            this.Mediation_SessionDate,
            this.Mediation_Result,
            this.Mediation_FirstPaymentDate,
            this.Mediation_Representative,
            this.Mediation_Note,
            this.Mediation_StatusName,
            this.Mediation_UsedUserID,
            this.Mediation_StatusID});
            this.MediationGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.MediationGridView.FormatRules.Add(gridFormatRule1);
            this.MediationGridView.GridControl = this.MediationGridControl;
            this.MediationGridView.Name = "MediationGridView";
            this.MediationGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.MediationGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.MediationGridView.OptionsBehavior.Editable = false;
            this.MediationGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.MediationGridView.OptionsFind.FindDelay = 100;
            this.MediationGridView.OptionsPrint.RtfPageFooter = resources.GetString("MediationGridView.OptionsPrint.RtfPageFooter");
            this.MediationGridView.OptionsPrint.RtfReportHeader = resources.GetString("MediationGridView.OptionsPrint.RtfReportHeader");
            this.MediationGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.MediationGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.MediationGridView.OptionsView.ShowFooter = true;
            this.MediationGridView.OptionsView.ShowGroupPanel = false;
            this.MediationGridView.OptionsView.ShowIndicator = false;
            this.MediationGridView.PaintStyleName = "Skin";
            this.MediationGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.MediationGridView.ViewCaption = "Mediasiyalar";
            this.MediationGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.MediationGridView_RowCellStyle);
            this.MediationGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.MediationGridView_FocusedRowObjectChanged);
            this.MediationGridView.ColumnFilterChanged += new System.EventHandler(this.MediationGridView_ColumnFilterChanged);
            this.MediationGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.MediationGridView_CustomUnboundColumnData);
            this.MediationGridView.PrintInitialize += new DevExpress.XtraGrid.Views.Base.PrintInitializeEventHandler(this.MediationGridView_PrintInitialize);
            this.MediationGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MediationGridView_MouseUp);
            this.MediationGridView.DoubleClick += new System.EventHandler(this.MediationGridView_DoubleClick);
            // 
            // Mediation_SS
            // 
            this.Mediation_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Mediation_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Mediation_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Mediation_SS.Caption = "S/s";
            this.Mediation_SS.DisplayFormat.FormatString = "n0";
            this.Mediation_SS.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Mediation_SS.FieldName = "Mediation_SS";
            this.Mediation_SS.Name = "Mediation_SS";
            this.Mediation_SS.OptionsColumn.FixedWidth = true;
            this.Mediation_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Mediation_SS", "{0}")});
            this.Mediation_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Mediation_SS.Visible = true;
            this.Mediation_SS.VisibleIndex = 0;
            this.Mediation_SS.Width = 50;
            // 
            // Mediation_ID
            // 
            this.Mediation_ID.Caption = "ID";
            this.Mediation_ID.FieldName = "ID";
            this.Mediation_ID.Name = "Mediation_ID";
            this.Mediation_ID.OptionsColumn.AllowShowHide = false;
            // 
            // Mediation_ContractCode
            // 
            this.Mediation_ContractCode.AppearanceCell.Options.UseTextOptions = true;
            this.Mediation_ContractCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Mediation_ContractCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Mediation_ContractCode.Caption = "Müqavilə";
            this.Mediation_ContractCode.FieldName = "CONTRACT_CODE";
            this.Mediation_ContractCode.Name = "Mediation_ContractCode";
            this.Mediation_ContractCode.OptionsColumn.FixedWidth = true;
            this.Mediation_ContractCode.Visible = true;
            this.Mediation_ContractCode.VisibleIndex = 1;
            // 
            // Mediation_Defendant
            // 
            this.Mediation_Defendant.Caption = "Cavabdeh";
            this.Mediation_Defendant.FieldName = "DEFENDANT";
            this.Mediation_Defendant.Name = "Mediation_Defendant";
            this.Mediation_Defendant.OptionsColumn.FixedWidth = true;
            this.Mediation_Defendant.Visible = true;
            this.Mediation_Defendant.VisibleIndex = 2;
            this.Mediation_Defendant.Width = 200;
            // 
            // Mediation_SendDate
            // 
            this.Mediation_SendDate.AppearanceCell.Options.UseTextOptions = true;
            this.Mediation_SendDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Mediation_SendDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Mediation_SendDate.Caption = "Təklifin göndərilmə tarixi";
            this.Mediation_SendDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Mediation_SendDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Mediation_SendDate.FieldName = "SEND_DATE";
            this.Mediation_SendDate.Name = "Mediation_SendDate";
            this.Mediation_SendDate.OptionsColumn.FixedWidth = true;
            this.Mediation_SendDate.ToolTip = "Mediasiya təklifinin göndərilmə tarixi";
            this.Mediation_SendDate.Visible = true;
            this.Mediation_SendDate.VisibleIndex = 3;
            this.Mediation_SendDate.Width = 90;
            // 
            // Mediation_ApplicationDate
            // 
            this.Mediation_ApplicationDate.AppearanceCell.Options.UseTextOptions = true;
            this.Mediation_ApplicationDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Mediation_ApplicationDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Mediation_ApplicationDate.Caption = "Təşkilata müraciət olunduğu tarix";
            this.Mediation_ApplicationDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Mediation_ApplicationDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Mediation_ApplicationDate.FieldName = "DATE_OF_APPLICATION";
            this.Mediation_ApplicationDate.Name = "Mediation_ApplicationDate";
            this.Mediation_ApplicationDate.OptionsColumn.FixedWidth = true;
            this.Mediation_ApplicationDate.ToolTip = "Mediasiya təşkilatına müraciət olunduğu tarix";
            this.Mediation_ApplicationDate.Visible = true;
            this.Mediation_ApplicationDate.VisibleIndex = 4;
            this.Mediation_ApplicationDate.Width = 100;
            // 
            // Mediation_Name
            // 
            this.Mediation_Name.Caption = "Mediator";
            this.Mediation_Name.FieldName = "MEDIATOR";
            this.Mediation_Name.Name = "Mediation_Name";
            this.Mediation_Name.OptionsColumn.FixedWidth = true;
            this.Mediation_Name.Visible = true;
            this.Mediation_Name.VisibleIndex = 5;
            this.Mediation_Name.Width = 150;
            // 
            // Mediation_SessionDate
            // 
            this.Mediation_SessionDate.AppearanceCell.Options.UseTextOptions = true;
            this.Mediation_SessionDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Mediation_SessionDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Mediation_SessionDate.Caption = "Sessiyanın tarixi";
            this.Mediation_SessionDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Mediation_SessionDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Mediation_SessionDate.FieldName = "SESSION_DATE";
            this.Mediation_SessionDate.Name = "Mediation_SessionDate";
            this.Mediation_SessionDate.OptionsColumn.FixedWidth = true;
            this.Mediation_SessionDate.ToolTip = "Mediasiya sessiyasının tarixi";
            this.Mediation_SessionDate.Visible = true;
            this.Mediation_SessionDate.VisibleIndex = 6;
            this.Mediation_SessionDate.Width = 120;
            // 
            // Mediation_Result
            // 
            this.Mediation_Result.Caption = "Nəticə";
            this.Mediation_Result.FieldName = "RESULT";
            this.Mediation_Result.Name = "Mediation_Result";
            this.Mediation_Result.OptionsColumn.FixedWidth = true;
            this.Mediation_Result.Visible = true;
            this.Mediation_Result.VisibleIndex = 7;
            this.Mediation_Result.Width = 110;
            // 
            // Mediation_FirstPaymentDate
            // 
            this.Mediation_FirstPaymentDate.AppearanceCell.Options.UseTextOptions = true;
            this.Mediation_FirstPaymentDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Mediation_FirstPaymentDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Mediation_FirstPaymentDate.Caption = "İlkin ödəniş tarixi";
            this.Mediation_FirstPaymentDate.FieldName = "FIRST_PAYMENT_DATE";
            this.Mediation_FirstPaymentDate.Name = "Mediation_FirstPaymentDate";
            this.Mediation_FirstPaymentDate.OptionsColumn.FixedWidth = true;
            this.Mediation_FirstPaymentDate.Visible = true;
            this.Mediation_FirstPaymentDate.VisibleIndex = 9;
            // 
            // Mediation_Representative
            // 
            this.Mediation_Representative.Caption = "Nümayəndə";
            this.Mediation_Representative.FieldName = "REPRESENTATIVE";
            this.Mediation_Representative.Name = "Mediation_Representative";
            this.Mediation_Representative.OptionsColumn.FixedWidth = true;
            this.Mediation_Representative.Visible = true;
            this.Mediation_Representative.VisibleIndex = 8;
            this.Mediation_Representative.Width = 150;
            // 
            // Mediation_Note
            // 
            this.Mediation_Note.Caption = "Qeyd";
            this.Mediation_Note.FieldName = "NOTE";
            this.Mediation_Note.Name = "Mediation_Note";
            this.Mediation_Note.Visible = true;
            this.Mediation_Note.VisibleIndex = 10;
            this.Mediation_Note.Width = 472;
            // 
            // Mediation_StatusName
            // 
            this.Mediation_StatusName.Caption = "Statusu";
            this.Mediation_StatusName.FieldName = "STATUS_NAME";
            this.Mediation_StatusName.Name = "Mediation_StatusName";
            this.Mediation_StatusName.OptionsColumn.FixedWidth = true;
            this.Mediation_StatusName.Visible = true;
            this.Mediation_StatusName.VisibleIndex = 11;
            this.Mediation_StatusName.Width = 125;
            // 
            // Mediation_UsedUserID
            // 
            this.Mediation_UsedUserID.Caption = "UsedUserID";
            this.Mediation_UsedUserID.FieldName = "USED_USER_ID";
            this.Mediation_UsedUserID.Name = "Mediation_UsedUserID";
            this.Mediation_UsedUserID.OptionsColumn.AllowShowHide = false;
            // 
            // Mediation_StatusID
            // 
            this.Mediation_StatusID.Caption = "StatusID";
            this.Mediation_StatusID.FieldName = "MEDIATION_STATUS_ID";
            this.Mediation_StatusID.Name = "Mediation_StatusID";
            this.Mediation_StatusID.OptionsColumn.AllowShowHide = false;
            // 
            // RepositoryItemPictureEdit
            // 
            this.RepositoryItemPictureEdit.Name = "RepositoryItemPictureEdit";
            this.RepositoryItemPictureEdit.ZoomAccelerationFactor = 1D;
            // 
            // FMediation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(1484, 613);
            this.Controls.Add(this.MediationGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FMediation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mediasiyalar";
            this.Load += new System.EventHandler(this.FMediation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MediationGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MediationGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl MediationGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView MediationGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit RepositoryItemPictureEdit;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_Defendant;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_SendDate;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_ApplicationDate;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_Name;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_SessionDate;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_Result;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_Representative;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_Note;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_UsedUserID;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_ContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_FirstPaymentDate;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_StatusName;
        private DevExpress.XtraGrid.Columns.GridColumn Mediation_StatusID;
        private DevExpress.XtraBars.BarButtonItem StopBarButton;
        private DevExpress.XtraBars.BarButtonItem ActiveBarButton;
    }
}