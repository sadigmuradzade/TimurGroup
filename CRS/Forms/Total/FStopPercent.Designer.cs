namespace CRS.Forms.Total
{
    partial class FStopPercent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStopPercent));
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.NewBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.DateGridControl = new DevExpress.XtraGrid.GridControl();
            this.DateGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Date_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Date_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Date_Start = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Date_End = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Date_InsertDate = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton)});
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
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick_1);
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
            this.RefreshBarButton});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 4;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton)});
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
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(443, 31);
            this.StandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(443, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 506);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(443, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 506);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(443, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 506);
            // 
            // DateGridControl
            // 
            this.DateGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DateGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateGridControl.Location = new System.Drawing.Point(0, 31);
            this.DateGridControl.MainView = this.DateGridView;
            this.DateGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateGridControl.Name = "DateGridControl";
            this.DateGridControl.Size = new System.Drawing.Size(443, 475);
            this.DateGridControl.TabIndex = 219;
            this.DateGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DateGridView});
            // 
            // DateGridView
            // 
            this.DateGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.DateGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DateGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DateGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.DateGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DateGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DateGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Date_SS,
            this.Date_ID,
            this.Date_Start,
            this.Date_End,
            this.Date_InsertDate});
            this.DateGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.DateGridView.GridControl = this.DateGridControl;
            this.DateGridView.Name = "DateGridView";
            this.DateGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.DateGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.DateGridView.OptionsBehavior.Editable = false;
            this.DateGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.DateGridView.OptionsFind.FindDelay = 100;
            this.DateGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.DateGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.DateGridView.OptionsView.ShowFooter = true;
            this.DateGridView.OptionsView.ShowGroupPanel = false;
            this.DateGridView.OptionsView.ShowIndicator = false;
            this.DateGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.DateGridView_FocusedRowObjectChanged);
            this.DateGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.DateGridView_CustomUnboundColumnData);
            this.DateGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DateGridView_MouseUp);
            this.DateGridView.DoubleClick += new System.EventHandler(this.DateGridView_DoubleClick);
            // 
            // Date_SS
            // 
            this.Date_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Date_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Date_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Date_SS.Caption = "S/s";
            this.Date_SS.FieldName = "Date_SS";
            this.Date_SS.Name = "Date_SS";
            this.Date_SS.OptionsColumn.FixedWidth = true;
            this.Date_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Date_SS", "{0}")});
            this.Date_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Date_SS.Visible = true;
            this.Date_SS.VisibleIndex = 0;
            this.Date_SS.Width = 40;
            // 
            // Date_ID
            // 
            this.Date_ID.Caption = "ID";
            this.Date_ID.FieldName = "ID";
            this.Date_ID.Name = "Date_ID";
            this.Date_ID.OptionsColumn.AllowShowHide = false;
            // 
            // Date_Start
            // 
            this.Date_Start.AppearanceCell.Options.UseTextOptions = true;
            this.Date_Start.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Date_Start.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Date_Start.Caption = "Başlama tarixi";
            this.Date_Start.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Date_Start.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Date_Start.FieldName = "START_DATE";
            this.Date_Start.Name = "Date_Start";
            this.Date_Start.OptionsColumn.FixedWidth = true;
            this.Date_Start.Visible = true;
            this.Date_Start.VisibleIndex = 1;
            this.Date_Start.Width = 80;
            // 
            // Date_End
            // 
            this.Date_End.AppearanceCell.Options.UseTextOptions = true;
            this.Date_End.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Date_End.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Date_End.Caption = "Bitmə tarixi";
            this.Date_End.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Date_End.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Date_End.FieldName = "END_DATE";
            this.Date_End.Name = "Date_End";
            this.Date_End.OptionsColumn.FixedWidth = true;
            this.Date_End.Visible = true;
            this.Date_End.VisibleIndex = 2;
            this.Date_End.Width = 80;
            // 
            // Date_InsertDate
            // 
            this.Date_InsertDate.AppearanceCell.Options.UseTextOptions = true;
            this.Date_InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Date_InsertDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Date_InsertDate.Caption = "Yaradılma tarixi";
            this.Date_InsertDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Date_InsertDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Date_InsertDate.FieldName = "INSERT_DATE";
            this.Date_InsertDate.Name = "Date_InsertDate";
            this.Date_InsertDate.Visible = true;
            this.Date_InsertDate.VisibleIndex = 3;
            this.Date_InsertDate.Width = 309;
            // 
            // FStopPercent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 506);
            this.Controls.Add(this.DateGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FStopPercent";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Faizi dayandır";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FStopPercent_FormClosing);
            this.Load += new System.EventHandler(this.FStopPercent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl DateGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView DateGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Date_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Date_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Date_Start;
        private DevExpress.XtraGrid.Columns.GridColumn Date_End;
        private DevExpress.XtraGrid.Columns.GridColumn Date_InsertDate;
    }
}