namespace CRS.Forms.Bookkeeping
{
    partial class FPaymentList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPaymentList));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.PaymentsGridControl = new DevExpress.XtraGrid.GridControl();
            this.PaymentsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.InfoPayment_CustomerFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.InfoPayment_ContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.InfoPayment_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.InfoPayment_Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.InfoPayment_CurrencyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.InfoPayment_CreditTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.InfoPayment_Voen = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.ToolBar,
            this.bar3});
            this.BarManager.DockControls.Add(this.barDockControlTop);
            this.BarManager.DockControls.Add(this.barDockControlBottom);
            this.BarManager.DockControls.Add(this.barDockControlLeft);
            this.BarManager.DockControls.Add(this.barDockControlRight);
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarButton});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 3;
            this.BarManager.StatusBar = this.bar3;
            // 
            // ToolBar
            // 
            this.ToolBar.BarName = "Main menu";
            this.ToolBar.DockCol = 0;
            this.ToolBar.DockRow = 0;
            this.ToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.ToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.ToolBar.OptionsBar.DrawBorder = false;
            this.ToolBar.OptionsBar.DrawDragBorder = false;
            this.ToolBar.OptionsBar.MultiLine = true;
            this.ToolBar.OptionsBar.UseWholeRow = true;
            this.ToolBar.Text = "Main menu";
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 0;
            this.RefreshBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.Image")));
            this.RefreshBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.LargeImage")));
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 1;
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
            this.ExportBarButton.Id = 2;
            this.ExportBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(975, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 526);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlBottom.Size = new System.Drawing.Size(975, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 502);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(975, 24);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 502);
            // 
            // PaymentsGridControl
            // 
            this.PaymentsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaymentsGridControl.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.PaymentsGridControl.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.PaymentsGridControl.Location = new System.Drawing.Point(0, 24);
            this.PaymentsGridControl.MainView = this.PaymentsGridView;
            this.PaymentsGridControl.Name = "PaymentsGridControl";
            this.PaymentsGridControl.Size = new System.Drawing.Size(975, 502);
            this.PaymentsGridControl.TabIndex = 57;
            this.PaymentsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PaymentsGridView});
            // 
            // PaymentsGridView
            // 
            this.PaymentsGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentsGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PaymentsGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PaymentsGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PaymentsGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentsGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PaymentsGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PaymentsGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentsGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Navy;
            this.PaymentsGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.PaymentsGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PaymentsGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PaymentsGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentsGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PaymentsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.InfoPayment_CustomerFullName,
            this.InfoPayment_Voen,
            this.InfoPayment_ContractCode,
            this.InfoPayment_Date,
            this.InfoPayment_Amount,
            this.InfoPayment_CurrencyCode,
            this.InfoPayment_CreditTypeID});
            this.PaymentsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.PaymentsGridView.FormatRules.Add(gridFormatRule1);
            this.PaymentsGridView.GridControl = this.PaymentsGridControl;
            this.PaymentsGridView.GroupCount = 1;
            this.PaymentsGridView.Name = "PaymentsGridView";
            this.PaymentsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.PaymentsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PaymentsGridView.OptionsBehavior.Editable = false;
            this.PaymentsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PaymentsGridView.OptionsFind.FindDelay = 100;
            this.PaymentsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PaymentsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.PaymentsGridView.OptionsView.ShowFooter = true;
            this.PaymentsGridView.OptionsView.ShowGroupPanel = false;
            this.PaymentsGridView.OptionsView.ShowIndicator = false;
            this.PaymentsGridView.PaintStyleName = "Skin";
            this.PaymentsGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PaymentsGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.InfoPayment_CurrencyCode, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.PaymentsGridView.ViewCaption = "Ödənişlərin siyahısı";
            this.PaymentsGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PaymentsGridView_MouseUp);
            // 
            // InfoPayment_CustomerFullName
            // 
            this.InfoPayment_CustomerFullName.Caption = "Müştəri";
            this.InfoPayment_CustomerFullName.FieldName = "CUSTOMERFULLNAME";
            this.InfoPayment_CustomerFullName.Name = "InfoPayment_CustomerFullName";
            this.InfoPayment_CustomerFullName.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "CUSTOMERFULLNAME", "{0}")});
            this.InfoPayment_CustomerFullName.Visible = true;
            this.InfoPayment_CustomerFullName.VisibleIndex = 0;
            this.InfoPayment_CustomerFullName.Width = 396;
            // 
            // InfoPayment_ContractCode
            // 
            this.InfoPayment_ContractCode.AppearanceCell.Options.UseTextOptions = true;
            this.InfoPayment_ContractCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.InfoPayment_ContractCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InfoPayment_ContractCode.Caption = "Müqavilə";
            this.InfoPayment_ContractCode.FieldName = "CONTRACT_CODE";
            this.InfoPayment_ContractCode.Name = "InfoPayment_ContractCode";
            this.InfoPayment_ContractCode.OptionsColumn.FixedWidth = true;
            this.InfoPayment_ContractCode.Visible = true;
            this.InfoPayment_ContractCode.VisibleIndex = 2;
            this.InfoPayment_ContractCode.Width = 70;
            // 
            // InfoPayment_Date
            // 
            this.InfoPayment_Date.AppearanceCell.Options.UseTextOptions = true;
            this.InfoPayment_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.InfoPayment_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InfoPayment_Date.Caption = "Tarix";
            this.InfoPayment_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.InfoPayment_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.InfoPayment_Date.FieldName = "PDATE";
            this.InfoPayment_Date.Name = "InfoPayment_Date";
            this.InfoPayment_Date.OptionsColumn.FixedWidth = true;
            this.InfoPayment_Date.Visible = true;
            this.InfoPayment_Date.VisibleIndex = 3;
            this.InfoPayment_Date.Width = 70;
            // 
            // InfoPayment_Amount
            // 
            this.InfoPayment_Amount.Caption = "Məbləğ";
            this.InfoPayment_Amount.DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
            this.InfoPayment_Amount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.InfoPayment_Amount.FieldName = "AMOUNT";
            this.InfoPayment_Amount.Name = "InfoPayment_Amount";
            this.InfoPayment_Amount.OptionsColumn.FixedWidth = true;
            this.InfoPayment_Amount.Visible = true;
            this.InfoPayment_Amount.VisibleIndex = 4;
            this.InfoPayment_Amount.Width = 90;
            // 
            // InfoPayment_CurrencyCode
            // 
            this.InfoPayment_CurrencyCode.Caption = "Valyuta";
            this.InfoPayment_CurrencyCode.FieldName = "CUR_CODE";
            this.InfoPayment_CurrencyCode.Name = "InfoPayment_CurrencyCode";
            this.InfoPayment_CurrencyCode.Visible = true;
            this.InfoPayment_CurrencyCode.VisibleIndex = 4;
            // 
            // InfoPayment_CreditTypeID
            // 
            this.InfoPayment_CreditTypeID.Caption = "Type";
            this.InfoPayment_CreditTypeID.FieldName = "CREDIT_TYPE_ID";
            this.InfoPayment_CreditTypeID.Name = "InfoPayment_CreditTypeID";
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // InfoPayment_Voen
            // 
            this.InfoPayment_Voen.AppearanceCell.Options.UseTextOptions = true;
            this.InfoPayment_Voen.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.InfoPayment_Voen.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InfoPayment_Voen.Caption = "VÖEN";
            this.InfoPayment_Voen.FieldName = "VOEN";
            this.InfoPayment_Voen.Name = "InfoPayment_Voen";
            this.InfoPayment_Voen.OptionsColumn.FixedWidth = true;
            this.InfoPayment_Voen.Visible = true;
            this.InfoPayment_Voen.VisibleIndex = 1;
            // 
            // FPaymentList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 549);
            this.Controls.Add(this.PaymentsGridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPaymentList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FPaymentList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl PaymentsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PaymentsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn InfoPayment_CustomerFullName;
        private DevExpress.XtraGrid.Columns.GridColumn InfoPayment_ContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn InfoPayment_Date;
        private DevExpress.XtraGrid.Columns.GridColumn InfoPayment_Amount;
        private DevExpress.XtraGrid.Columns.GridColumn InfoPayment_CurrencyCode;
        private DevExpress.XtraGrid.Columns.GridColumn InfoPayment_CreditTypeID;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraGrid.Columns.GridColumn InfoPayment_Voen;
    }
}