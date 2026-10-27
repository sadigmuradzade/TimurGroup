namespace CRS.Forms.Contracts
{
    partial class FPhoneBook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPhoneBook));
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
            this.PhoneGridControl = new DevExpress.XtraGrid.GridControl();
            this.PhoneGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Phone_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Phone_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Phone_Description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Phone_OwnerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Phone_Number = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneGridView)).BeginInit();
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
            this.RefreshBarButton.Caption = "Yenilə";
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
            this.barDockControlTop.Size = new System.Drawing.Size(1275, 30);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 642);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1275, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 612);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1275, 30);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 612);
            // 
            // PhoneGridControl
            // 
            this.PhoneGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PhoneGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PhoneGridControl.Location = new System.Drawing.Point(0, 30);
            this.PhoneGridControl.MainView = this.PhoneGridView;
            this.PhoneGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PhoneGridControl.Name = "PhoneGridControl";
            this.PhoneGridControl.Size = new System.Drawing.Size(1275, 612);
            this.PhoneGridControl.TabIndex = 60;
            this.PhoneGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PhoneGridView});
            // 
            // PhoneGridView
            // 
            this.PhoneGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PhoneGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PhoneGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PhoneGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PhoneGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PhoneGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PhoneGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PhoneGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PhoneGridView.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PhoneGridView.Appearance.ViewCaption.Options.UseBackColor = true;
            this.PhoneGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PhoneGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PhoneGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PhoneGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PhoneGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Phone_SS,
            this.Phone_ID,
            this.Phone_Description,
            this.Phone_OwnerName,
            this.Phone_Number});
            this.PhoneGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.PhoneGridView.GridControl = this.PhoneGridControl;
            this.PhoneGridView.GroupCount = 1;
            this.PhoneGridView.Name = "PhoneGridView";
            this.PhoneGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.PhoneGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PhoneGridView.OptionsBehavior.Editable = false;
            this.PhoneGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PhoneGridView.OptionsFind.FindDelay = 100;
            this.PhoneGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PhoneGridView.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.PhoneGridView.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.PhoneGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.PhoneGridView.OptionsView.ShowFooter = true;
            this.PhoneGridView.OptionsView.ShowGroupPanel = false;
            this.PhoneGridView.OptionsView.ShowIndicator = false;
            this.PhoneGridView.PaintStyleName = "Skin";
            this.PhoneGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PhoneGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.Phone_Description, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.PhoneGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.PhoneGridView_CustomUnboundColumnData);
            this.PhoneGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PhoneGridView_MouseUp);
            // 
            // Phone_SS
            // 
            this.Phone_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Phone_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Phone_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Phone_SS.Caption = "S/s";
            this.Phone_SS.FieldName = "Phone_SS";
            this.Phone_SS.Name = "Phone_SS";
            this.Phone_SS.OptionsColumn.FixedWidth = true;
            this.Phone_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", "{0}")});
            this.Phone_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Phone_SS.Visible = true;
            this.Phone_SS.VisibleIndex = 0;
            this.Phone_SS.Width = 70;
            // 
            // Phone_ID
            // 
            this.Phone_ID.Caption = "ID";
            this.Phone_ID.FieldName = "ID";
            this.Phone_ID.Name = "Phone_ID";
            // 
            // Phone_Description
            // 
            this.Phone_Description.Caption = "Təsvir";
            this.Phone_Description.FieldName = "OWNER_TYPE";
            this.Phone_Description.Name = "Phone_Description";
            this.Phone_Description.OptionsColumn.FixedWidth = true;
            this.Phone_Description.Visible = true;
            this.Phone_Description.VisibleIndex = 1;
            // 
            // Phone_OwnerName
            // 
            this.Phone_OwnerName.Caption = "Adı";
            this.Phone_OwnerName.FieldName = "OWNER_NAME";
            this.Phone_OwnerName.Name = "Phone_OwnerName";
            this.Phone_OwnerName.Visible = true;
            this.Phone_OwnerName.VisibleIndex = 1;
            this.Phone_OwnerName.Width = 263;
            // 
            // Phone_Number
            // 
            this.Phone_Number.AppearanceCell.Options.UseTextOptions = true;
            this.Phone_Number.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Phone_Number.Caption = "Nömrə";
            this.Phone_Number.FieldName = "PHONE";
            this.Phone_Number.Name = "Phone_Number";
            this.Phone_Number.Visible = true;
            this.Phone_Number.VisibleIndex = 2;
            this.Phone_Number.Width = 940;
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
            // FPhoneBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 667);
            this.Controls.Add(this.PhoneGridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FPhoneBook";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Telefon kitabçası";
            this.Load += new System.EventHandler(this.FPhoneBook_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraGrid.GridControl PhoneGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PhoneGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Phone_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Phone_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Phone_Description;
        private DevExpress.XtraGrid.Columns.GridColumn Phone_Number;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraGrid.Columns.GridColumn Phone_OwnerName;
    }
}