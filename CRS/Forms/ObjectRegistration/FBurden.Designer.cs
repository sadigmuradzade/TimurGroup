namespace CRS.Forms.ObjectRegistration
{
    partial class FBurden
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FBurden));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.NewBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ConfirmBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.ViewUrlBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ObjectRegistrationRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.InfoRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.OutRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ContractsGridControl = new DevExpress.XtraGrid.GridControl();
            this.ContractsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Contract_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Fullname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_StartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_ArrivalDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_RegistrationNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_StatusName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_URL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Note = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_LastUpdateUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_UpdateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Warning = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
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
            this.ViewUrlBarButton,
            this.EditBarButton,
            this.ExcelBarButton,
            this.PdfBarButton,
            this.RtfBarButton,
            this.HtmlBarButton,
            this.TxtBarButton,
            this.CsvBarButton,
            this.MhtBarButton});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 19;
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
            // ViewUrlBarButton
            // 
            this.ViewUrlBarButton.Caption = "Çıxarışa bax";
            this.ViewUrlBarButton.Id = 7;
            this.ViewUrlBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ViewUrlBarButton.ImageOptions.Image")));
            this.ViewUrlBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.ViewUrlBarButton.Name = "ViewUrlBarButton";
            this.ViewUrlBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.ViewUrlBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ViewUrlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ViewUrlBarButton_ItemClick);
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
            // ObjectRegistrationRibbonPage
            // 
            this.ObjectRegistrationRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.InfoRibbonPageGroup,
            this.OutRibbonPageGroup});
            this.ObjectRegistrationRibbonPage.Name = "ObjectRegistrationRibbonPage";
            this.ObjectRegistrationRibbonPage.Text = "Yüklülük";
            // 
            // InfoRibbonPageGroup
            // 
            this.InfoRibbonPageGroup.ItemLinks.Add(this.NewBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.EditBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.DeleteBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.ConfirmBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.RefreshBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.ViewUrlBarButton);
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
            this.Contract_Fullname,
            this.Contract_StartDate,
            this.Contract_ArrivalDate,
            this.Contract_RegistrationNumber,
            this.Contract_StatusName,
            this.Contract_URL,
            this.Contract_Note,
            this.Contract_LastUpdateUser,
            this.Contract_UpdateDate,
            this.Contract_Warning});
            this.ContractsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.ContractsGridView.GridControl = this.ContractsGridControl;
            this.ContractsGridView.Name = "ContractsGridView";
            this.ContractsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.ContractsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.ContractsGridView.OptionsBehavior.Editable = false;
            this.ContractsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.ContractsGridView.OptionsFind.FindDelay = 100;
            this.ContractsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
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
            // Contract_Fullname
            // 
            this.Contract_Fullname.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_Fullname.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_Fullname.Caption = "Soyadı, adı və atasının adı";
            this.Contract_Fullname.FieldName = "FULLNAME";
            this.Contract_Fullname.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Contract_Fullname.Name = "Contract_Fullname";
            this.Contract_Fullname.OptionsColumn.FixedWidth = true;
            this.Contract_Fullname.Visible = true;
            this.Contract_Fullname.VisibleIndex = 1;
            this.Contract_Fullname.Width = 230;
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
            this.Contract_StartDate.VisibleIndex = 2;
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
            this.Contract_ArrivalDate.VisibleIndex = 3;
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
            this.Contract_RegistrationNumber.VisibleIndex = 4;
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
            this.Contract_StatusName.VisibleIndex = 5;
            this.Contract_StatusName.Width = 90;
            // 
            // Contract_URL
            // 
            this.Contract_URL.Caption = "Çıxarışın linki";
            this.Contract_URL.FieldName = "URL";
            this.Contract_URL.Name = "Contract_URL";
            this.Contract_URL.OptionsColumn.FixedWidth = true;
            this.Contract_URL.Visible = true;
            this.Contract_URL.VisibleIndex = 6;
            this.Contract_URL.Width = 500;
            // 
            // Contract_Note
            // 
            this.Contract_Note.Caption = "Qeyd";
            this.Contract_Note.FieldName = "NOTE";
            this.Contract_Note.Name = "Contract_Note";
            this.Contract_Note.Visible = true;
            this.Contract_Note.VisibleIndex = 7;
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
            this.Contract_LastUpdateUser.VisibleIndex = 8;
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
            this.Contract_UpdateDate.VisibleIndex = 9;
            this.Contract_UpdateDate.Width = 110;
            // 
            // Contract_Warning
            // 
            this.Contract_Warning.Caption = "Xəbərdarlıq";
            this.Contract_Warning.FieldName = "WARNING";
            this.Contract_Warning.Name = "Contract_Warning";
            this.Contract_Warning.OptionsColumn.AllowShowHide = false;
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
            this.PopupMenu.ItemLinks.Add(this.ViewUrlBarButton);
            this.PopupMenu.ItemLinks.Add(this.PrintBarButton, true);
            this.PopupMenu.ItemLinks.Add(this.ExportBarSubItem);
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Ribbon = this.ribbon;
            // 
            // ExcelBarButton
            // 
            this.ExcelBarButton.Caption = "Excel";
            this.ExcelBarButton.Id = 12;
            this.ExcelBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_32;
            this.ExcelBarButton.Name = "ExcelBarButton";
            this.ExcelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExcelBarButton_ItemClick);
            // 
            // PdfBarButton
            // 
            this.PdfBarButton.Caption = "Pdf";
            this.PdfBarButton.Id = 13;
            this.PdfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.pdf_32;
            this.PdfBarButton.Name = "PdfBarButton";
            this.PdfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PdfBarButton_ItemClick);
            // 
            // RtfBarButton
            // 
            this.RtfBarButton.Caption = "Rtf";
            this.RtfBarButton.Id = 14;
            this.RtfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.rtf_32;
            this.RtfBarButton.Name = "RtfBarButton";
            this.RtfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RtfBarButton_ItemClick);
            // 
            // HtmlBarButton
            // 
            this.HtmlBarButton.Caption = "Html";
            this.HtmlBarButton.Id = 15;
            this.HtmlBarButton.ImageOptions.Image = global::CRS.Properties.Resources.html_32;
            this.HtmlBarButton.Name = "HtmlBarButton";
            this.HtmlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HtmlBarButton_ItemClick);
            // 
            // TxtBarButton
            // 
            this.TxtBarButton.Caption = "Txt";
            this.TxtBarButton.Id = 16;
            this.TxtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.txt_32;
            this.TxtBarButton.Name = "TxtBarButton";
            this.TxtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TxtBarButton_ItemClick);
            // 
            // CsvBarButton
            // 
            this.CsvBarButton.Caption = "Csv";
            this.CsvBarButton.Id = 17;
            this.CsvBarButton.ImageOptions.Image = global::CRS.Properties.Resources.csv_32;
            this.CsvBarButton.Name = "CsvBarButton";
            this.CsvBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CsvBarButton_ItemClick);
            // 
            // MhtBarButton
            // 
            this.MhtBarButton.Caption = "Mht";
            this.MhtBarButton.Id = 18;
            this.MhtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.explorer_32;
            this.MhtBarButton.Name = "MhtBarButton";
            this.MhtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MhtBarButton_ItemClick);
            // 
            // FBurden
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1649, 618);
            this.Controls.Add(this.ContractsGridControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.MinimizeBox = false;
            this.Name = "FBurden";
            this.Ribbon = this.ribbon;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Çıxarışlar nəzarət";
            this.Activated += new System.EventHandler(this.FObjectRegistration_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FBurden_FormClosing);
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
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Fullname;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_StartDate;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_ArrivalDate;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_RegistrationNumber;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_StatusName;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit RepositoryItemPictureEdit;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_LastUpdateUser;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_UpdateDate;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Note;
        private DevExpress.XtraBars.BarButtonItem ConfirmBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup OutRibbonPageGroup;
        private DevExpress.XtraBars.BarSubItem ExportBarSubItem;
        private DevExpress.XtraBars.BarButtonItem ViewUrlBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_URL;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Warning;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraBars.BarButtonItem PdfBarButton;
        private DevExpress.XtraBars.BarButtonItem RtfBarButton;
        private DevExpress.XtraBars.BarButtonItem HtmlBarButton;
        private DevExpress.XtraBars.BarButtonItem TxtBarButton;
        private DevExpress.XtraBars.BarButtonItem CsvBarButton;
        private DevExpress.XtraBars.BarButtonItem MhtBarButton;
    }
}