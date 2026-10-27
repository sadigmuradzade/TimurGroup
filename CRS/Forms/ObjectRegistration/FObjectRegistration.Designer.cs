namespace CRS.Forms.ObjectRegistration
{
    partial class FObjectRegistration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FObjectRegistration));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.NewBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ConfirmBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.BurdenBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ObjectRegistrationRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.InfoRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.OutRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ContractsGridControl = new DevExpress.XtraGrid.GridControl();
            this.ContractsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Contract_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Address = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Area = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_StartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_ArrivalDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_RegistrationNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_StatusName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_URL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_BurdenRegistration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_BurdenUrl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Note = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_LastUpdateUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_UpdateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_UsedUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_ContractID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.ViewBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.MainUrlBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.BurdenUrlBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExcelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PdfBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RtfBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.HtmlBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.TxtBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.CsvBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.MhtBarButton = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.NewBarButton,
            this.RefreshBarButton,
            this.ConfirmBarButton,
            this.DeleteBarButton,
            this.PrintBarButton,
            this.ExportBarSubItem,
            this.EditBarButton,
            this.BurdenBarButton,
            this.ViewBarSubItem,
            this.MainUrlBarButton,
            this.BurdenUrlBarButton,
            this.ExcelBarButton,
            this.PdfBarButton,
            this.RtfBarButton,
            this.HtmlBarButton,
            this.TxtBarButton,
            this.CsvBarButton,
            this.MhtBarButton});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 22;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ObjectRegistrationRibbonPage});
            this.ribbon.ShowQatLocationSelector = false;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(1649, 143);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // NewBarButton
            // 
            this.NewBarButton.Caption = "Yeni";
            this.NewBarButton.Id = 1;
            this.NewBarButton.ImageOptions.Image = global::CRS.Properties.Resources.plus_32;
            this.NewBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.NewBarButton.Name = "NewBarButton";
            this.NewBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.NewBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.NewBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 2;
            this.RefreshBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.Image")));
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // ConfirmBarButton
            // 
            this.ConfirmBarButton.Caption = "Çıxarışı təsdiqlə";
            this.ConfirmBarButton.Id = 3;
            this.ConfirmBarButton.ImageOptions.Image = global::CRS.Properties.Resources.check_mark;
            this.ConfirmBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T));
            this.ConfirmBarButton.Name = "ConfirmBarButton";
            this.ConfirmBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.ConfirmBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ConfirmBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ConfirmBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Id = 4;
            this.DeleteBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("DeleteBarButton.ImageOptions.Image")));
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Delete);
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap";
            this.PrintBarButton.Id = 5;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // ExportBarSubItem
            // 
            this.ExportBarSubItem.Caption = "İxrac";
            this.ExportBarSubItem.Id = 6;
            this.ExportBarSubItem.ImageOptions.Image = global::CRS.Properties.Resources.table_export_32;
            this.ExportBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ExcelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PdfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RtfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.HtmlBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.TxtBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.CsvBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.MhtBarButton)});
            this.ExportBarSubItem.MultiColumn = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarSubItem.Name = "ExportBarSubItem";
            this.ExportBarSubItem.OptionsMultiColumn.ShowItemText = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarSubItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Dəyiş";
            this.EditBarButton.Id = 8;
            this.EditBarButton.ImageOptions.Image = global::CRS.Properties.Resources.edit_32;
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // BurdenBarButton
            // 
            this.BurdenBarButton.Caption = "Yüklülük";
            this.BurdenBarButton.Id = 9;
            this.BurdenBarButton.ImageOptions.Image = global::CRS.Properties.Resources.house;
            this.BurdenBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y));
            this.BurdenBarButton.Name = "BurdenBarButton";
            this.BurdenBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.BurdenBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.BurdenBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BurdenBarButton_ItemClick);
            // 
            // ObjectRegistrationRibbonPage
            // 
            this.ObjectRegistrationRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.InfoRibbonPageGroup,
            this.OutRibbonPageGroup});
            this.ObjectRegistrationRibbonPage.Name = "ObjectRegistrationRibbonPage";
            this.ObjectRegistrationRibbonPage.Text = "Çıxarışlara nəzarət";
            // 
            // InfoRibbonPageGroup
            // 
            this.InfoRibbonPageGroup.ItemLinks.Add(this.NewBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.EditBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.DeleteBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.ConfirmBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.RefreshBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.BurdenBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.ViewBarSubItem);
            this.InfoRibbonPageGroup.Name = "InfoRibbonPageGroup";
            this.InfoRibbonPageGroup.Text = "Çıxarış";
            // 
            // OutRibbonPageGroup
            // 
            this.OutRibbonPageGroup.ItemLinks.Add(this.PrintBarButton);
            this.OutRibbonPageGroup.ItemLinks.Add(this.ExportBarSubItem);
            this.OutRibbonPageGroup.Name = "OutRibbonPageGroup";
            this.OutRibbonPageGroup.Text = "Çıxış";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 587);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1649, 31);
            // 
            // ContractsGridControl
            // 
            this.ContractsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContractsGridControl.Location = new System.Drawing.Point(0, 143);
            this.ContractsGridControl.MainView = this.ContractsGridView;
            this.ContractsGridControl.Name = "ContractsGridControl";
            this.ContractsGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepositoryItemPictureEdit});
            this.ContractsGridControl.Size = new System.Drawing.Size(1649, 444);
            this.ContractsGridControl.TabIndex = 56;
            this.ContractsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ContractsGridView});
            // 
            // ContractsGridView
            // 
            this.ContractsGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.ContractsGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.ContractsGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.ContractsGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ContractsGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ContractsGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.ContractsGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ContractsGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ContractsGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.ContractsGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.ContractsGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ContractsGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ContractsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Contract_SS,
            this.Contract_Code,
            this.Contract_Address,
            this.Contract_Area,
            this.Contract_StartDate,
            this.Contract_ArrivalDate,
            this.Contract_RegistrationNumber,
            this.Contract_StatusName,
            this.Contract_URL,
            this.Contract_BurdenRegistration,
            this.Contract_BurdenUrl,
            this.Contract_Note,
            this.Contract_LastUpdateUser,
            this.Contract_UpdateDate,
            this.Contract_ID,
            this.Contract_UsedUserID,
            this.Contract_ContractID,
            this.gridColumn1});
            this.ContractsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.ContractsGridView.GridControl = this.ContractsGridControl;
            this.ContractsGridView.Name = "ContractsGridView";
            this.ContractsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.ContractsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.ContractsGridView.OptionsBehavior.Editable = false;
            this.ContractsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.ContractsGridView.OptionsFind.FindDelay = 100;
            this.ContractsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ContractsGridView.OptionsView.ColumnAutoWidth = false;
            this.ContractsGridView.OptionsView.ShowFooter = true;
            this.ContractsGridView.OptionsView.ShowGroupPanel = false;
            this.ContractsGridView.OptionsView.ShowIndicator = false;
            this.ContractsGridView.PaintStyleName = "Skin";
            this.ContractsGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.ContractsGridView.ViewCaption = "Müqavilələrin siyahısı";
            this.ContractsGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.ContractsGridView_RowCellStyle);
            this.ContractsGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.ContractsGridView_FocusedRowObjectChanged);
            this.ContractsGridView.ColumnFilterChanged += new System.EventHandler(this.ContractsGridView_ColumnFilterChanged);
            this.ContractsGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.ContractsGridView_CustomUnboundColumnData);
            this.ContractsGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ContractsGridView_MouseUp);
            this.ContractsGridView.DoubleClick += new System.EventHandler(this.ContractsGridView_DoubleClick);
            // 
            // Contract_SS
            // 
            this.Contract_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_SS.Caption = "S/s";
            this.Contract_SS.FieldName = "Contract_SS";
            this.Contract_SS.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Contract_SS.Name = "Contract_SS";
            this.Contract_SS.OptionsColumn.FixedWidth = true;
            this.Contract_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Contract_SS", "{0}")});
            this.Contract_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Contract_SS.Visible = true;
            this.Contract_SS.VisibleIndex = 0;
            this.Contract_SS.Width = 45;
            // 
            // Contract_Code
            // 
            this.Contract_Code.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_Code.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_Code.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_Code.Caption = "Müqavilə";
            this.Contract_Code.FieldName = "CONTRACT_CODE";
            this.Contract_Code.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Contract_Code.Name = "Contract_Code";
            this.Contract_Code.OptionsColumn.FixedWidth = true;
            this.Contract_Code.Visible = true;
            this.Contract_Code.VisibleIndex = 1;
            this.Contract_Code.Width = 70;
            // 
            // Contract_Address
            // 
            this.Contract_Address.Caption = "Əmlakın ünvanı";
            this.Contract_Address.FieldName = "ADDRESS";
            this.Contract_Address.Name = "Contract_Address";
            this.Contract_Address.OptionsColumn.FixedWidth = true;
            this.Contract_Address.ToolTip = "Daşınmaz  əmlakın ünvanı";
            this.Contract_Address.Visible = true;
            this.Contract_Address.VisibleIndex = 2;
            this.Contract_Address.Width = 500;
            // 
            // Contract_Area
            // 
            this.Contract_Area.Caption = "Sahəsi";
            this.Contract_Area.DisplayFormat.FormatString = "{0:n2} m²";
            this.Contract_Area.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Contract_Area.FieldName = "AREA";
            this.Contract_Area.Name = "Contract_Area";
            this.Contract_Area.OptionsColumn.FixedWidth = true;
            this.Contract_Area.Visible = true;
            this.Contract_Area.VisibleIndex = 3;
            // 
            // Contract_StartDate
            // 
            this.Contract_StartDate.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_StartDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_StartDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_StartDate.Caption = "Göndərilmə tarixi";
            this.Contract_StartDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Contract_StartDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Contract_StartDate.FieldName = "SENT_DATE";
            this.Contract_StartDate.Name = "Contract_StartDate";
            this.Contract_StartDate.OptionsColumn.FixedWidth = true;
            this.Contract_StartDate.Visible = true;
            this.Contract_StartDate.VisibleIndex = 4;
            this.Contract_StartDate.Width = 90;
            // 
            // Contract_ArrivalDate
            // 
            this.Contract_ArrivalDate.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_ArrivalDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_ArrivalDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_ArrivalDate.Caption = "Təxmini icra tarixi";
            this.Contract_ArrivalDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Contract_ArrivalDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Contract_ArrivalDate.FieldName = "ARRIVAL_DATE";
            this.Contract_ArrivalDate.Name = "Contract_ArrivalDate";
            this.Contract_ArrivalDate.OptionsColumn.FixedWidth = true;
            this.Contract_ArrivalDate.Visible = true;
            this.Contract_ArrivalDate.VisibleIndex = 5;
            this.Contract_ArrivalDate.Width = 95;
            // 
            // Contract_RegistrationNumber
            // 
            this.Contract_RegistrationNumber.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_RegistrationNumber.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_RegistrationNumber.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_RegistrationNumber.Caption = "Çıxarışın nömrəsi";
            this.Contract_RegistrationNumber.FieldName = "REGISTRATION_NUMBER";
            this.Contract_RegistrationNumber.Name = "Contract_RegistrationNumber";
            this.Contract_RegistrationNumber.OptionsColumn.FixedWidth = true;
            this.Contract_RegistrationNumber.Visible = true;
            this.Contract_RegistrationNumber.VisibleIndex = 6;
            this.Contract_RegistrationNumber.Width = 90;
            // 
            // Contract_StatusName
            // 
            this.Contract_StatusName.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_StatusName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_StatusName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_StatusName.Caption = "Çıxarışn tarixi";
            this.Contract_StatusName.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Contract_StatusName.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Contract_StatusName.FieldName = "REGISTRATION_DATE";
            this.Contract_StatusName.Name = "Contract_StatusName";
            this.Contract_StatusName.OptionsColumn.FixedWidth = true;
            this.Contract_StatusName.Visible = true;
            this.Contract_StatusName.VisibleIndex = 7;
            this.Contract_StatusName.Width = 90;
            // 
            // Contract_URL
            // 
            this.Contract_URL.Caption = "Çıxarışın linki";
            this.Contract_URL.FieldName = "URL";
            this.Contract_URL.Name = "Contract_URL";
            this.Contract_URL.OptionsColumn.FixedWidth = true;
            this.Contract_URL.Visible = true;
            this.Contract_URL.VisibleIndex = 8;
            this.Contract_URL.Width = 500;
            // 
            // Contract_BurdenRegistration
            // 
            this.Contract_BurdenRegistration.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Contract_BurdenRegistration.AppearanceCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Contract_BurdenRegistration.AppearanceCell.Options.UseBackColor = true;
            this.Contract_BurdenRegistration.Caption = "Yüklü çıxarışın nömrəsi";
            this.Contract_BurdenRegistration.FieldName = "BURDEN_REGISTRATION_NUMBER";
            this.Contract_BurdenRegistration.Name = "Contract_BurdenRegistration";
            this.Contract_BurdenRegistration.OptionsColumn.FixedWidth = true;
            this.Contract_BurdenRegistration.ToolTip = "Sonuncu yüklülüyün çıxarışının nömrəsi";
            this.Contract_BurdenRegistration.Visible = true;
            this.Contract_BurdenRegistration.VisibleIndex = 9;
            this.Contract_BurdenRegistration.Width = 120;
            // 
            // Contract_BurdenUrl
            // 
            this.Contract_BurdenUrl.Caption = "Yüklü çıxarışın linki";
            this.Contract_BurdenUrl.FieldName = "BURDEN_URL";
            this.Contract_BurdenUrl.Name = "Contract_BurdenUrl";
            this.Contract_BurdenUrl.OptionsColumn.FixedWidth = true;
            this.Contract_BurdenUrl.Visible = true;
            this.Contract_BurdenUrl.VisibleIndex = 10;
            this.Contract_BurdenUrl.Width = 500;
            // 
            // Contract_Note
            // 
            this.Contract_Note.Caption = "Qeyd";
            this.Contract_Note.FieldName = "NOTE";
            this.Contract_Note.Name = "Contract_Note";
            this.Contract_Note.Visible = true;
            this.Contract_Note.VisibleIndex = 11;
            this.Contract_Note.Width = 200;
            // 
            // Contract_LastUpdateUser
            // 
            this.Contract_LastUpdateUser.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_LastUpdateUser.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_LastUpdateUser.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_LastUpdateUser.Caption = "Sonuncu dəyişiklik edən";
            this.Contract_LastUpdateUser.FieldName = "LAST_UPDATE_USER";
            this.Contract_LastUpdateUser.Name = "Contract_LastUpdateUser";
            this.Contract_LastUpdateUser.OptionsColumn.FixedWidth = true;
            this.Contract_LastUpdateUser.Visible = true;
            this.Contract_LastUpdateUser.VisibleIndex = 12;
            this.Contract_LastUpdateUser.Width = 150;
            // 
            // Contract_UpdateDate
            // 
            this.Contract_UpdateDate.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_UpdateDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_UpdateDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_UpdateDate.Caption = "Dəyişilmə vaxtı";
            this.Contract_UpdateDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Contract_UpdateDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Contract_UpdateDate.FieldName = "UPDATE_DATE";
            this.Contract_UpdateDate.Name = "Contract_UpdateDate";
            this.Contract_UpdateDate.OptionsColumn.FixedWidth = true;
            this.Contract_UpdateDate.Visible = true;
            this.Contract_UpdateDate.VisibleIndex = 13;
            this.Contract_UpdateDate.Width = 110;
            // 
            // Contract_ID
            // 
            this.Contract_ID.Caption = "ID";
            this.Contract_ID.FieldName = "ID";
            this.Contract_ID.Name = "Contract_ID";
            // 
            // Contract_UsedUserID
            // 
            this.Contract_UsedUserID.Caption = "UsedUserID";
            this.Contract_UsedUserID.FieldName = "USED_USER_ID";
            this.Contract_UsedUserID.Name = "Contract_UsedUserID";
            // 
            // Contract_ContractID
            // 
            this.Contract_ContractID.Caption = "ContractID";
            this.Contract_ContractID.FieldName = "CONTRACT_ID";
            this.Contract_ContractID.Name = "Contract_ContractID";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Xəbərdarlıq";
            this.gridColumn1.FieldName = "WARNING";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowShowHide = false;
            // 
            // RepositoryItemPictureEdit
            // 
            this.RepositoryItemPictureEdit.InitialImage = global::CRS.Properties.Resources.notes_16;
            this.RepositoryItemPictureEdit.Name = "RepositoryItemPictureEdit";
            this.RepositoryItemPictureEdit.NullText = " ";
            this.RepositoryItemPictureEdit.ZoomAccelerationFactor = 1D;
            // 
            // PopupMenu
            // 
            this.PopupMenu.ItemLinks.Add(this.NewBarButton);
            this.PopupMenu.ItemLinks.Add(this.EditBarButton);
            this.PopupMenu.ItemLinks.Add(this.DeleteBarButton);
            this.PopupMenu.ItemLinks.Add(this.ConfirmBarButton);
            this.PopupMenu.ItemLinks.Add(this.RefreshBarButton);
            this.PopupMenu.ItemLinks.Add(this.BurdenBarButton);
            this.PopupMenu.ItemLinks.Add(this.ViewBarSubItem);
            this.PopupMenu.ItemLinks.Add(this.PrintBarButton, true);
            this.PopupMenu.ItemLinks.Add(this.ExportBarSubItem);
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Ribbon = this.ribbon;
            // 
            // ViewBarSubItem
            // 
            this.ViewBarSubItem.Caption = "Çıxarışa bax";
            this.ViewBarSubItem.Id = 12;
            this.ViewBarSubItem.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem1.ImageOptions.Image")));
            this.ViewBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MainUrlBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.BurdenUrlBarButton)});
            this.ViewBarSubItem.Name = "ViewBarSubItem";
            this.ViewBarSubItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // MainUrlBarButton
            // 
            this.MainUrlBarButton.Caption = "Əsas çıxarışa bax";
            this.MainUrlBarButton.Id = 13;
            this.MainUrlBarButton.Name = "MainUrlBarButton";
            this.MainUrlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MainUrlBarButton_ItemClick);
            // 
            // BurdenUrlBarButton
            // 
            this.BurdenUrlBarButton.Caption = "Sonuncu yüklülüyün çıxarışına bax";
            this.BurdenUrlBarButton.Id = 14;
            this.BurdenUrlBarButton.Name = "BurdenUrlBarButton";
            this.BurdenUrlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BurdenUrlBarButton_ItemClick);
            // 
            // ExcelBarButton
            // 
            this.ExcelBarButton.Caption = "Excel";
            this.ExcelBarButton.Id = 15;
            this.ExcelBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_32;
            this.ExcelBarButton.Name = "ExcelBarButton";
            this.ExcelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExcelBarButton_ItemClick);
            // 
            // PdfBarButton
            // 
            this.PdfBarButton.Caption = "Pdf";
            this.PdfBarButton.Id = 16;
            this.PdfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.pdf_32;
            this.PdfBarButton.Name = "PdfBarButton";
            this.PdfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PdfBarButton_ItemClick);
            // 
            // RtfBarButton
            // 
            this.RtfBarButton.Caption = "Rtf";
            this.RtfBarButton.Id = 17;
            this.RtfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.rtf_32;
            this.RtfBarButton.Name = "RtfBarButton";
            this.RtfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RtfBarButton_ItemClick);
            // 
            // HtmlBarButton
            // 
            this.HtmlBarButton.Caption = "Html";
            this.HtmlBarButton.Id = 18;
            this.HtmlBarButton.ImageOptions.Image = global::CRS.Properties.Resources.html_32;
            this.HtmlBarButton.Name = "HtmlBarButton";
            this.HtmlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HtmlBarButton_ItemClick);
            // 
            // TxtBarButton
            // 
            this.TxtBarButton.Caption = "Txt";
            this.TxtBarButton.Id = 19;
            this.TxtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.txt_32;
            this.TxtBarButton.Name = "TxtBarButton";
            this.TxtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TxtBarButton_ItemClick);
            // 
            // CsvBarButton
            // 
            this.CsvBarButton.Caption = "Csv";
            this.CsvBarButton.Id = 20;
            this.CsvBarButton.ImageOptions.Image = global::CRS.Properties.Resources.csv_32;
            this.CsvBarButton.Name = "CsvBarButton";
            this.CsvBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CsvBarButton_ItemClick);
            // 
            // MhtBarButton
            // 
            this.MhtBarButton.Caption = "Mht";
            this.MhtBarButton.Id = 21;
            this.MhtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.explorer_32;
            this.MhtBarButton.Name = "MhtBarButton";
            this.MhtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MhtBarButton_ItemClick);
            // 
            // FObjectRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1649, 618);
            this.Controls.Add(this.ContractsGridControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "FObjectRegistration";
            this.Ribbon = this.ribbon;
            this.ShowIcon = false;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Çıxarışlar nəzarət";
            this.Activated += new System.EventHandler(this.FObjectRegistration_Activated);
            this.Load += new System.EventHandler(this.FObjectRegistration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ObjectRegistrationRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup InfoRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraGrid.GridControl ContractsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ContractsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Code;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_StartDate;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_ArrivalDate;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_RegistrationNumber;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_StatusName;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit RepositoryItemPictureEdit;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_LastUpdateUser;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_UpdateDate;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Note;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_UsedUserID;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_ContractID;
        private DevExpress.XtraBars.BarButtonItem ConfirmBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup OutRibbonPageGroup;
        private DevExpress.XtraBars.BarSubItem ExportBarSubItem;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_URL;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Address;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Area;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraBars.BarButtonItem BurdenBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_BurdenRegistration;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_BurdenUrl;
        private DevExpress.XtraBars.BarSubItem ViewBarSubItem;
        private DevExpress.XtraBars.BarButtonItem MainUrlBarButton;
        private DevExpress.XtraBars.BarButtonItem BurdenUrlBarButton;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraBars.BarButtonItem PdfBarButton;
        private DevExpress.XtraBars.BarButtonItem RtfBarButton;
        private DevExpress.XtraBars.BarButtonItem HtmlBarButton;
        private DevExpress.XtraBars.BarButtonItem TxtBarButton;
        private DevExpress.XtraBars.BarButtonItem CsvBarButton;
        private DevExpress.XtraBars.BarButtonItem MhtBarButton;
    }
}