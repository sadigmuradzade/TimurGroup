namespace CRS.Forms.Total
{
    partial class FPenalty
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPenalty));
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem1 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCalculator = new DevExpress.XtraEditors.SimpleButton();
            this.BExchange = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.PenaltyGridControl = new DevExpress.XtraGrid.GridControl();
            this.PenaltyGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CalcPenalty_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_LastPaymentDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_StartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPercent_EndDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPercent_DayCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CaclPenalty_Debt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_Interest = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_PresentPenalty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_Payed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_Debt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_LastDebt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcPenalty_LastDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalcStandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.CalcBarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.CalcBar = new DevExpress.XtraBars.Bar();
            this.NewPenaltyBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditPenaltyBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshPenaltyBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintPenaltyBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportPenaltyBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.CalcPopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalcBarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalcPopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCalculator);
            this.PanelOption.Controls.Add(this.BExchange);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 692);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(1362, 62);
            this.PanelOption.TabIndex = 51;
            // 
            // BCalculator
            // 
            this.BCalculator.Appearance.Options.UseTextOptions = true;
            this.BCalculator.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.BCalculator.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.BCalculator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BCalculator.ImageOptions.Image = global::CRS.Properties.Resources.calculator_24;
            this.BCalculator.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BCalculator.Location = new System.Drawing.Point(14, 10);
            this.BCalculator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCalculator.Name = "BCalculator";
            this.BCalculator.Size = new System.Drawing.Size(146, 41);
            superToolTip1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem1.Text = "<color=255,0,0>Kalkulyator</color>";
            toolTipItem1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipItem1.Appearance.Options.UseImage = true;
            toolTipItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem1.Image")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Windows-un kalkulyatorunu açmaq üçün nəzərdə tutulub.";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.BCalculator.SuperTip = superToolTip1;
            this.BCalculator.TabIndex = 185;
            this.BCalculator.TabStop = false;
            this.BCalculator.Text = "Kalkulyator";
            this.BCalculator.Click += new System.EventHandler(this.BCalculator_Click);
            // 
            // BExchange
            // 
            this.BExchange.Appearance.Options.UseTextOptions = true;
            this.BExchange.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.BExchange.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.BExchange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BExchange.ImageOptions.Image = global::CRS.Properties.Resources.exchange_24;
            this.BExchange.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BExchange.Location = new System.Drawing.Point(167, 10);
            this.BExchange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BExchange.Name = "BExchange";
            this.BExchange.Size = new System.Drawing.Size(146, 41);
            superToolTip2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem2.Text = "<color=255,0,0>Valyuta çevirici</color>";
            toolTipItem2.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            toolTipItem2.Appearance.Options.UseImage = true;
            toolTipItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem2.Image")));
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Valyutanın digər valyutalara görə qarşılığını tapmaq üçün nəzərdə tutulub.";
            toolTipTitleItem3.LeftIndent = 6;
            toolTipTitleItem3.Text = "Qeyd: <i>Valyutaların qarşılığı tapılarkən baza valyuta kimi</i><b><color=104,6,6" +
    "> AZN</color></b><i> götürülür və ən son daxil etdiyiniz məzənnə ilə hesablanır." +
    "</i>";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            superToolTip2.Items.Add(toolTipSeparatorItem1);
            superToolTip2.Items.Add(toolTipTitleItem3);
            this.BExchange.SuperTip = superToolTip2;
            this.BExchange.TabIndex = 184;
            this.BExchange.TabStop = false;
            this.BExchange.Text = "Valyuta çevirici";
            this.BExchange.Click += new System.EventHandler(this.BExchange_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.Maroon;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.Controls.Add(this.PenaltyGridControl);
            this.groupControl1.Controls.Add(this.CalcStandaloneBarDockControl);
            this.groupControl1.Location = new System.Drawing.Point(14, 15);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1336, 669);
            this.groupControl1.TabIndex = 52;
            this.groupControl1.Text = "Hesablanmış cərimə faizləri";
            // 
            // PenaltyGridControl
            // 
            this.PenaltyGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PenaltyGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PenaltyGridControl.Location = new System.Drawing.Point(41, 26);
            this.PenaltyGridControl.MainView = this.PenaltyGridView;
            this.PenaltyGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PenaltyGridControl.Name = "PenaltyGridControl";
            this.PenaltyGridControl.Size = new System.Drawing.Size(1293, 641);
            this.PenaltyGridControl.TabIndex = 60;
            this.PenaltyGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PenaltyGridView});
            // 
            // PenaltyGridView
            // 
            this.PenaltyGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PenaltyGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PenaltyGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PenaltyGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PenaltyGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PenaltyGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PenaltyGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PenaltyGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PenaltyGridView.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PenaltyGridView.Appearance.ViewCaption.Options.UseBackColor = true;
            this.PenaltyGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PenaltyGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PenaltyGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PenaltyGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PenaltyGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.CalcPenalty_SS,
            this.CalcPenalty_LastPaymentDate,
            this.CalcPenalty_StartDate,
            this.CalcPercent_EndDate,
            this.CalcPercent_DayCount,
            this.CaclPenalty_Debt,
            this.CalcPenalty_Interest,
            this.CalcPenalty_Amount,
            this.CalcPenalty_PresentPenalty,
            this.CalcPenalty_Payed,
            this.CalcPenalty_Debt,
            this.CalcPenalty_LastDebt,
            this.CalcPenalty_LastDate});
            this.PenaltyGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.PenaltyGridView.GridControl = this.PenaltyGridControl;
            this.PenaltyGridView.Name = "PenaltyGridView";
            this.PenaltyGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.PenaltyGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PenaltyGridView.OptionsBehavior.Editable = false;
            this.PenaltyGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PenaltyGridView.OptionsFind.FindDelay = 100;
            this.PenaltyGridView.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            this.PenaltyGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PenaltyGridView.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.PenaltyGridView.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.PenaltyGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.PenaltyGridView.OptionsView.ShowFooter = true;
            this.PenaltyGridView.OptionsView.ShowGroupPanel = false;
            this.PenaltyGridView.OptionsView.ShowIndicator = false;
            this.PenaltyGridView.PaintStyleName = "Skin";
            this.PenaltyGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PenaltyGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.PenaltyGridView_CustomDrawFooterCell);
            this.PenaltyGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.PenaltyGridView_FocusedRowObjectChanged);
            this.PenaltyGridView.ColumnFilterChanged += new System.EventHandler(this.PenaltyGridView_ColumnFilterChanged);
            this.PenaltyGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.PenaltyGridView_CustomUnboundColumnData);
            this.PenaltyGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PenaltyGridView_MouseUp);
            this.PenaltyGridView.DoubleClick += new System.EventHandler(this.PenaltyGridView_DoubleClick);
            // 
            // CalcPenalty_SS
            // 
            this.CalcPenalty_SS.Caption = "S/s";
            this.CalcPenalty_SS.FieldName = "CalcPenalty_SS";
            this.CalcPenalty_SS.Name = "CalcPenalty_SS";
            this.CalcPenalty_SS.OptionsColumn.AllowShowHide = false;
            this.CalcPenalty_SS.OptionsColumn.FixedWidth = true;
            this.CalcPenalty_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.CalcPenalty_SS.Visible = true;
            this.CalcPenalty_SS.VisibleIndex = 0;
            this.CalcPenalty_SS.Width = 40;
            // 
            // CalcPenalty_LastPaymentDate
            // 
            this.CalcPenalty_LastPaymentDate.AppearanceCell.Options.UseTextOptions = true;
            this.CalcPenalty_LastPaymentDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CalcPenalty_LastPaymentDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CalcPenalty_LastPaymentDate.Caption = "Son ödənişin tarixi";
            this.CalcPenalty_LastPaymentDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.CalcPenalty_LastPaymentDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.CalcPenalty_LastPaymentDate.FieldName = "LAST_PAYMENT_DATE";
            this.CalcPenalty_LastPaymentDate.Name = "CalcPenalty_LastPaymentDate";
            this.CalcPenalty_LastPaymentDate.Visible = true;
            this.CalcPenalty_LastPaymentDate.VisibleIndex = 1;
            this.CalcPenalty_LastPaymentDate.Width = 170;
            // 
            // CalcPenalty_StartDate
            // 
            this.CalcPenalty_StartDate.AppearanceCell.Options.UseTextOptions = true;
            this.CalcPenalty_StartDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CalcPenalty_StartDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CalcPenalty_StartDate.Caption = "Cərimənin başlama tarixi";
            this.CalcPenalty_StartDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.CalcPenalty_StartDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.CalcPenalty_StartDate.FieldName = "START_DATE";
            this.CalcPenalty_StartDate.Name = "CalcPenalty_StartDate";
            this.CalcPenalty_StartDate.Visible = true;
            this.CalcPenalty_StartDate.VisibleIndex = 2;
            this.CalcPenalty_StartDate.Width = 170;
            // 
            // CalcPercent_EndDate
            // 
            this.CalcPercent_EndDate.AppearanceCell.Options.UseTextOptions = true;
            this.CalcPercent_EndDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CalcPercent_EndDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CalcPercent_EndDate.Caption = "Cərimənin bitmə tarixi";
            this.CalcPercent_EndDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.CalcPercent_EndDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.CalcPercent_EndDate.FieldName = "END_DATE";
            this.CalcPercent_EndDate.Name = "CalcPercent_EndDate";
            this.CalcPercent_EndDate.Visible = true;
            this.CalcPercent_EndDate.VisibleIndex = 3;
            this.CalcPercent_EndDate.Width = 172;
            // 
            // CalcPercent_DayCount
            // 
            this.CalcPercent_DayCount.AppearanceCell.Options.UseTextOptions = true;
            this.CalcPercent_DayCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CalcPercent_DayCount.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CalcPercent_DayCount.Caption = "Günlərin sayı";
            this.CalcPercent_DayCount.FieldName = "DAY_COUNT";
            this.CalcPercent_DayCount.Name = "CalcPercent_DayCount";
            this.CalcPercent_DayCount.Visible = true;
            this.CalcPercent_DayCount.VisibleIndex = 4;
            this.CalcPercent_DayCount.Width = 176;
            // 
            // CaclPenalty_Debt
            // 
            this.CaclPenalty_Debt.Caption = "Borc";
            this.CaclPenalty_Debt.DisplayFormat.FormatString = "n2";
            this.CaclPenalty_Debt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CaclPenalty_Debt.FieldName = "DEBT";
            this.CaclPenalty_Debt.Name = "CaclPenalty_Debt";
            this.CaclPenalty_Debt.OptionsColumn.FixedWidth = true;
            this.CaclPenalty_Debt.Visible = true;
            this.CaclPenalty_Debt.VisibleIndex = 5;
            this.CaclPenalty_Debt.Width = 110;
            // 
            // CalcPenalty_Interest
            // 
            this.CalcPenalty_Interest.Caption = "Faiz";
            this.CalcPenalty_Interest.DisplayFormat.FormatString = "n2";
            this.CalcPenalty_Interest.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcPenalty_Interest.FieldName = "INTEREST";
            this.CalcPenalty_Interest.Name = "CalcPenalty_Interest";
            this.CalcPenalty_Interest.OptionsColumn.FixedWidth = true;
            this.CalcPenalty_Interest.Visible = true;
            this.CalcPenalty_Interest.VisibleIndex = 6;
            this.CalcPenalty_Interest.Width = 50;
            // 
            // CalcPenalty_PresentPenalty
            // 
            this.CalcPenalty_PresentPenalty.Caption = "Bağışlanılan cərimə";
            this.CalcPenalty_PresentPenalty.DisplayFormat.FormatString = "n2";
            this.CalcPenalty_PresentPenalty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcPenalty_PresentPenalty.FieldName = "PRESENT_PENALTY";
            this.CalcPenalty_PresentPenalty.Name = "CalcPenalty_PresentPenalty";
            this.CalcPenalty_PresentPenalty.OptionsColumn.FixedWidth = true;
            this.CalcPenalty_PresentPenalty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PRESENT_PENALTY", "{0:n2}")});
            this.CalcPenalty_PresentPenalty.Visible = true;
            this.CalcPenalty_PresentPenalty.VisibleIndex = 8;
            this.CalcPenalty_PresentPenalty.Width = 110;
            // 
            // CalcPenalty_Amount
            // 
            this.CalcPenalty_Amount.Caption = "Hesablanmış";
            this.CalcPenalty_Amount.DisplayFormat.FormatString = "n2";
            this.CalcPenalty_Amount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcPenalty_Amount.FieldName = "PENALTY_AMOUNT";
            this.CalcPenalty_Amount.Name = "CalcPenalty_Amount";
            this.CalcPenalty_Amount.OptionsColumn.FixedWidth = true;
            this.CalcPenalty_Amount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PENALTY_AMOUNT", "{0:n2}")});
            this.CalcPenalty_Amount.ToolTip = "Hesablanmış cərimə";
            this.CalcPenalty_Amount.Visible = true;
            this.CalcPenalty_Amount.VisibleIndex = 7;
            this.CalcPenalty_Amount.Width = 110;
            // 
            // CalcPenalty_Payed
            // 
            this.CalcPenalty_Payed.Caption = "Ödənilən";
            this.CalcPenalty_Payed.DisplayFormat.FormatString = "n2";
            this.CalcPenalty_Payed.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcPenalty_Payed.FieldName = "PAYED_PENALTY";
            this.CalcPenalty_Payed.Name = "CalcPenalty_Payed";
            this.CalcPenalty_Payed.OptionsColumn.FixedWidth = true;
            this.CalcPenalty_Payed.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PAYED_PENALTY", "{0:n2}")});
            this.CalcPenalty_Payed.ToolTip = "Ödənilən cərimə";
            this.CalcPenalty_Payed.Visible = true;
            this.CalcPenalty_Payed.VisibleIndex = 9;
            this.CalcPenalty_Payed.Width = 110;
            // 
            // CalcPenalty_Debt
            // 
            this.CalcPenalty_Debt.Caption = "Qalıq";
            this.CalcPenalty_Debt.DisplayFormat.FormatString = "n2";
            this.CalcPenalty_Debt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcPenalty_Debt.FieldName = "PENALTY_DEBT";
            this.CalcPenalty_Debt.Name = "CalcPenalty_Debt";
            this.CalcPenalty_Debt.OptionsColumn.FixedWidth = true;
            this.CalcPenalty_Debt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PENALTY_DEBT", "{0:n2}")});
            this.CalcPenalty_Debt.ToolTip = "Qalıq cərimə";
            this.CalcPenalty_Debt.Visible = true;
            this.CalcPenalty_Debt.VisibleIndex = 10;
            this.CalcPenalty_Debt.Width = 110;
            // 
            // CalcPenalty_LastDebt
            // 
            this.CalcPenalty_LastDebt.Caption = "Sonuncu qalıq";
            this.CalcPenalty_LastDebt.DisplayFormat.FormatString = "n2";
            this.CalcPenalty_LastDebt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcPenalty_LastDebt.FieldName = "LAST_DEBT";
            this.CalcPenalty_LastDebt.Name = "CalcPenalty_LastDebt";
            // 
            // CalcPenalty_LastDate
            // 
            this.CalcPenalty_LastDate.Caption = "Sonuncu hesablanma tarixi";
            this.CalcPenalty_LastDate.FieldName = "LAST_END_DATE";
            this.CalcPenalty_LastDate.Name = "CalcPenalty_LastDate";
            // 
            // CalcStandaloneBarDockControl
            // 
            this.CalcStandaloneBarDockControl.CausesValidation = false;
            this.CalcStandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.CalcStandaloneBarDockControl.Location = new System.Drawing.Point(2, 26);
            this.CalcStandaloneBarDockControl.Manager = this.CalcBarManager;
            this.CalcStandaloneBarDockControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CalcStandaloneBarDockControl.Name = "CalcStandaloneBarDockControl";
            this.CalcStandaloneBarDockControl.Size = new System.Drawing.Size(39, 641);
            this.CalcStandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // CalcBarManager
            // 
            this.CalcBarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.CalcBar});
            this.CalcBarManager.DockControls.Add(this.barDockControlTop);
            this.CalcBarManager.DockControls.Add(this.barDockControlBottom);
            this.CalcBarManager.DockControls.Add(this.barDockControlLeft);
            this.CalcBarManager.DockControls.Add(this.barDockControlRight);
            this.CalcBarManager.DockControls.Add(this.CalcStandaloneBarDockControl);
            this.CalcBarManager.Form = this;
            this.CalcBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.EditPenaltyBarButton,
            this.RefreshPenaltyBarButton,
            this.PrintPenaltyBarButton,
            this.ExportPenaltyBarButton,
            this.NewPenaltyBarButton,
            this.DeleteBarButton});
            this.CalcBarManager.MainMenu = this.CalcBar;
            this.CalcBarManager.MaxItemId = 6;
            // 
            // CalcBar
            // 
            this.CalcBar.BarName = "Main menu";
            this.CalcBar.DockCol = 0;
            this.CalcBar.DockRow = 0;
            this.CalcBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.CalcBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewPenaltyBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditPenaltyBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshPenaltyBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintPenaltyBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportPenaltyBarButton)});
            this.CalcBar.OptionsBar.DrawBorder = false;
            this.CalcBar.OptionsBar.DrawDragBorder = false;
            this.CalcBar.OptionsBar.MultiLine = true;
            this.CalcBar.OptionsBar.UseWholeRow = true;
            this.CalcBar.StandaloneBarDockControl = this.CalcStandaloneBarDockControl;
            this.CalcBar.Text = "Main menu";
            // 
            // NewPenaltyBarButton
            // 
            this.NewPenaltyBarButton.Caption = "Yeni";
            this.NewPenaltyBarButton.Id = 4;
            this.NewPenaltyBarButton.ImageOptions.Image = global::CRS.Properties.Resources.plus_16;
            this.NewPenaltyBarButton.Name = "NewPenaltyBarButton";
            this.NewPenaltyBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewPenaltyBarButton_ItemClick);
            // 
            // EditPenaltyBarButton
            // 
            this.EditPenaltyBarButton.Caption = "Dəyiş";
            this.EditPenaltyBarButton.Id = 0;
            this.EditPenaltyBarButton.ImageOptions.Image = global::CRS.Properties.Resources.edit_16;
            this.EditPenaltyBarButton.Name = "EditPenaltyBarButton";
            this.EditPenaltyBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditPenaltyBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Id = 5;
            this.DeleteBarButton.ImageOptions.Image = global::CRS.Properties.Resources.minus_16;
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // RefreshPenaltyBarButton
            // 
            this.RefreshPenaltyBarButton.Caption = "Təzələ";
            this.RefreshPenaltyBarButton.Id = 1;
            this.RefreshPenaltyBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshPenaltyBarButton.ImageOptions.Image")));
            this.RefreshPenaltyBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("RefreshPenaltyBarButton.ImageOptions.LargeImage")));
            this.RefreshPenaltyBarButton.Name = "RefreshPenaltyBarButton";
            this.RefreshPenaltyBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshPenaltyBarButton_ItemClick);
            // 
            // PrintPenaltyBarButton
            // 
            this.PrintPenaltyBarButton.Caption = "Çap";
            this.PrintPenaltyBarButton.Id = 2;
            this.PrintPenaltyBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintPenaltyBarButton.ImageOptions.Image")));
            this.PrintPenaltyBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("PrintPenaltyBarButton.ImageOptions.LargeImage")));
            this.PrintPenaltyBarButton.Name = "PrintPenaltyBarButton";
            this.PrintPenaltyBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintPenaltyBarButton_ItemClick);
            // 
            // ExportPenaltyBarButton
            // 
            this.ExportPenaltyBarButton.Caption = "İxrac";
            this.ExportPenaltyBarButton.Id = 3;
            this.ExportPenaltyBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExportPenaltyBarButton.Name = "ExportPenaltyBarButton";
            this.ExportPenaltyBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportPenaltyBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.CalcBarManager;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(1362, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 754);
            this.barDockControlBottom.Manager = this.CalcBarManager;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(1362, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.CalcBarManager;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 754);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1362, 0);
            this.barDockControlRight.Manager = this.CalcBarManager;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 754);
            // 
            // CalcPopupMenu
            // 
            this.CalcPopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewPenaltyBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditPenaltyBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshPenaltyBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintPenaltyBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportPenaltyBarButton)});
            this.CalcPopupMenu.Manager = this.CalcBarManager;
            this.CalcPopupMenu.Name = "CalcPopupMenu";
            // 
            // FPenalty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 754);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1380, 801);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1380, 801);
            this.Name = "FPenalty";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FPenalty";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FPenalty_FormClosing);
            this.Load += new System.EventHandler(this.FPenalty_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalcBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalcPopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private DevExpress.XtraEditors.SimpleButton BCalculator;
        private DevExpress.XtraEditors.SimpleButton BExchange;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraBars.StandaloneBarDockControl CalcStandaloneBarDockControl;
        private DevExpress.XtraGrid.GridControl PenaltyGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PenaltyGridView;
        private DevExpress.XtraBars.BarManager CalcBarManager;
        private DevExpress.XtraBars.Bar CalcBar;
        private DevExpress.XtraBars.BarButtonItem EditPenaltyBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshPenaltyBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintPenaltyBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportPenaltyBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.PopupMenu CalcPopupMenu;
        private DevExpress.XtraBars.BarButtonItem NewPenaltyBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_SS;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_LastPaymentDate;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_StartDate;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPercent_EndDate;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPercent_DayCount;
        private DevExpress.XtraGrid.Columns.GridColumn CaclPenalty_Debt;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_Interest;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_Amount;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_Payed;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_Debt;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_LastDebt;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_LastDate;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn CalcPenalty_PresentPenalty;
    }
}