namespace CRS.Forms
{
    partial class FCreditCalculator
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
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue2 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions5 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions6 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCreditCalculator));
            this.Shedule_BasicAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Shedule_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.CreditAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.PeriodLabel = new DevExpress.XtraEditors.LabelControl();
            this.InterestLabel = new DevExpress.XtraEditors.LabelControl();
            this.MonthlyAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.CreditAmountValue = new DevExpress.XtraEditors.CalcEdit();
            this.PeriodValue = new DevExpress.XtraEditors.CalcEdit();
            this.InterestValue = new DevExpress.XtraEditors.CalcEdit();
            this.SumPaymentText = new DevExpress.XtraEditors.TextEdit();
            this.SumPaymentLabel = new DevExpress.XtraEditors.LabelControl();
            this.InterestAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.PaymentSchedulesGridControl = new DevExpress.XtraGrid.GridControl();
            this.PaymentSchedulesGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Shedule_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Shedule_MonthlyPayment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Shedule_InterestAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Shedule_Debt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MonthlyAmountValue = new DevExpress.XtraEditors.CalcEdit();
            this.AverageInterestPercentValue = new DevExpress.XtraEditors.CalcEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.InterestAmountValue = new DevExpress.XtraEditors.CalcEdit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CreditAmountValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SumPaymentText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthlyAmountValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AverageInterestPercentValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestAmountValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Shedule_BasicAmount
            // 
            this.Shedule_BasicAmount.Caption = "Əsas məbləğ";
            this.Shedule_BasicAmount.DisplayFormat.FormatString = "n2";
            this.Shedule_BasicAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Shedule_BasicAmount.FieldName = "BasicAmount";
            this.Shedule_BasicAmount.Name = "Shedule_BasicAmount";
            this.Shedule_BasicAmount.OptionsColumn.FixedWidth = true;
            this.Shedule_BasicAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "BasicAmount", "{0:n2}")});
            this.Shedule_BasicAmount.Visible = true;
            this.Shedule_BasicAmount.VisibleIndex = 3;
            // 
            // Shedule_Date
            // 
            this.Shedule_Date.AppearanceCell.Options.UseTextOptions = true;
            this.Shedule_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Shedule_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Shedule_Date.Caption = "Tarix";
            this.Shedule_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Shedule_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Shedule_Date.FieldName = "MonthDate";
            this.Shedule_Date.Name = "Shedule_Date";
            this.Shedule_Date.OptionsColumn.FixedWidth = true;
            this.Shedule_Date.Visible = true;
            this.Shedule_Date.VisibleIndex = 1;
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 570);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(868, 50);
            this.PanelOption.TabIndex = 50;
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
            this.BCancel.Location = new System.Drawing.Point(779, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // CreditAmountLabel
            // 
            this.CreditAmountLabel.Location = new System.Drawing.Point(12, 15);
            this.CreditAmountLabel.Name = "CreditAmountLabel";
            this.CreditAmountLabel.Size = new System.Drawing.Size(75, 13);
            this.CreditAmountLabel.TabIndex = 51;
            this.CreditAmountLabel.Text = "Kreditin məbləği";
            // 
            // PeriodLabel
            // 
            this.PeriodLabel.Location = new System.Drawing.Point(12, 41);
            this.PeriodLabel.Name = "PeriodLabel";
            this.PeriodLabel.Size = new System.Drawing.Size(61, 13);
            this.PeriodLabel.TabIndex = 52;
            this.PeriodLabel.Text = "Müddəti (ay)";
            // 
            // InterestLabel
            // 
            this.InterestLabel.Location = new System.Drawing.Point(12, 67);
            this.InterestLabel.Name = "InterestLabel";
            this.InterestLabel.Size = new System.Drawing.Size(59, 13);
            this.InterestLabel.TabIndex = 53;
            this.InterestLabel.Text = "İllik faizi (%)";
            // 
            // MonthlyAmountLabel
            // 
            this.MonthlyAmountLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.MonthlyAmountLabel.Appearance.Options.UseFont = true;
            this.MonthlyAmountLabel.Location = new System.Drawing.Point(12, 111);
            this.MonthlyAmountLabel.Name = "MonthlyAmountLabel";
            this.MonthlyAmountLabel.Size = new System.Drawing.Size(68, 13);
            this.MonthlyAmountLabel.TabIndex = 54;
            this.MonthlyAmountLabel.Text = "Aylıq ödəniş";
            // 
            // CreditAmountValue
            // 
            this.CreditAmountValue.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CreditAmountValue.Location = new System.Drawing.Point(145, 12);
            this.CreditAmountValue.Name = "CreditAmountValue";
            this.CreditAmountValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.CreditAmountValue.Properties.Mask.EditMask = "n2";
            this.CreditAmountValue.Properties.Precision = 2;
            this.CreditAmountValue.Size = new System.Drawing.Size(143, 20);
            this.CreditAmountValue.TabIndex = 0;
            this.CreditAmountValue.EditValueChanged += new System.EventHandler(this.CreditAmountValue_EditValueChanged);
            // 
            // PeriodValue
            // 
            this.PeriodValue.EditValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.PeriodValue.Location = new System.Drawing.Point(145, 38);
            this.PeriodValue.Name = "PeriodValue";
            this.PeriodValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.PeriodValue.Properties.Mask.EditMask = "n0";
            this.PeriodValue.Properties.Precision = 0;
            this.PeriodValue.Size = new System.Drawing.Size(143, 20);
            this.PeriodValue.TabIndex = 1;
            this.PeriodValue.EditValueChanged += new System.EventHandler(this.CreditAmountValue_EditValueChanged);
            // 
            // InterestValue
            // 
            this.InterestValue.EditValue = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.InterestValue.Location = new System.Drawing.Point(145, 64);
            this.InterestValue.Name = "InterestValue";
            this.InterestValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.InterestValue.Properties.Mask.EditMask = "n2";
            this.InterestValue.Properties.Precision = 2;
            this.InterestValue.Size = new System.Drawing.Size(143, 20);
            this.InterestValue.TabIndex = 2;
            this.InterestValue.EditValueChanged += new System.EventHandler(this.CreditAmountValue_EditValueChanged);
            // 
            // SumPaymentText
            // 
            this.SumPaymentText.Location = new System.Drawing.Point(145, 134);
            this.SumPaymentText.Name = "SumPaymentText";
            this.SumPaymentText.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SumPaymentText.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.SumPaymentText.Properties.Appearance.Options.UseFont = true;
            this.SumPaymentText.Properties.Appearance.Options.UseForeColor = true;
            this.SumPaymentText.Properties.ReadOnly = true;
            this.SumPaymentText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SumPaymentText.Size = new System.Drawing.Size(143, 20);
            this.SumPaymentText.TabIndex = 63;
            // 
            // SumPaymentLabel
            // 
            this.SumPaymentLabel.Location = new System.Drawing.Point(12, 138);
            this.SumPaymentLabel.Name = "SumPaymentLabel";
            this.SumPaymentLabel.Size = new System.Drawing.Size(57, 13);
            this.SumPaymentLabel.TabIndex = 62;
            this.SumPaymentLabel.Text = "Cəmi ödəniş";
            // 
            // InterestAmountLabel
            // 
            this.InterestAmountLabel.Location = new System.Drawing.Point(12, 164);
            this.InterestAmountLabel.Name = "InterestAmountLabel";
            this.InterestAmountLabel.Size = new System.Drawing.Size(58, 13);
            this.InterestAmountLabel.TabIndex = 64;
            this.InterestAmountLabel.Text = "Faiz məbləği";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.PaymentSchedulesGridControl);
            this.groupControl1.Location = new System.Drawing.Point(294, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(560, 552);
            this.groupControl1.TabIndex = 66;
            this.groupControl1.Text = "Ödəniş cədvəli";
            // 
            // PaymentSchedulesGridControl
            // 
            this.PaymentSchedulesGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaymentSchedulesGridControl.Location = new System.Drawing.Point(2, 20);
            this.PaymentSchedulesGridControl.MainView = this.PaymentSchedulesGridView;
            this.PaymentSchedulesGridControl.Name = "PaymentSchedulesGridControl";
            this.PaymentSchedulesGridControl.Size = new System.Drawing.Size(556, 530);
            this.PaymentSchedulesGridControl.TabIndex = 56;
            this.PaymentSchedulesGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PaymentSchedulesGridView});
            // 
            // PaymentSchedulesGridView
            // 
            this.PaymentSchedulesGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.GroupFooter.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentSchedulesGridView.Appearance.GroupFooter.Options.UseFont = true;
            this.PaymentSchedulesGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PaymentSchedulesGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Indigo;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PaymentSchedulesGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Shedule_SS,
            this.Shedule_Date,
            this.Shedule_MonthlyPayment,
            this.Shedule_BasicAmount,
            this.Shedule_InterestAmount,
            this.Shedule_Debt});
            this.PaymentSchedulesGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Column = this.Shedule_BasicAmount;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.PredefinedName = "Red Text";
            formatConditionRuleValue1.Value1 = new decimal(new int[] {
            0,
            0,
            0,
            0});
            gridFormatRule1.Rule = formatConditionRuleValue1;
            gridFormatRule2.ColumnApplyTo = this.Shedule_Date;
            gridFormatRule2.Name = "Format1";
            formatConditionRuleValue2.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue2.PredefinedName = "Red Text";
            formatConditionRuleValue2.Value1 = new decimal(new int[] {
            1,
            0,
            0,
            0});
            gridFormatRule2.Rule = formatConditionRuleValue2;
            this.PaymentSchedulesGridView.FormatRules.Add(gridFormatRule1);
            this.PaymentSchedulesGridView.FormatRules.Add(gridFormatRule2);
            this.PaymentSchedulesGridView.GridControl = this.PaymentSchedulesGridControl;
            this.PaymentSchedulesGridView.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MONTHLY_PAYMENT", this.Shedule_MonthlyPayment, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "BASIC_AMOUNT", this.Shedule_BasicAmount, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "INTEREST_AMOUNT", this.Shedule_InterestAmount, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", this.Shedule_SS, "{0}")});
            this.PaymentSchedulesGridView.Name = "PaymentSchedulesGridView";
            this.PaymentSchedulesGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PaymentSchedulesGridView.OptionsBehavior.Editable = false;
            this.PaymentSchedulesGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PaymentSchedulesGridView.OptionsFind.FindDelay = 100;
            this.PaymentSchedulesGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PaymentSchedulesGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.PaymentSchedulesGridView.OptionsView.ShowFooter = true;
            this.PaymentSchedulesGridView.OptionsView.ShowGroupPanel = false;
            this.PaymentSchedulesGridView.OptionsView.ShowIndicator = false;
            this.PaymentSchedulesGridView.PaintStyleName = "Skin";
            this.PaymentSchedulesGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PaymentSchedulesGridView.ViewCaption = "11300";
            this.PaymentSchedulesGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.PaymentSchedulesGridView_CustomDrawFooterCell);
            // 
            // Shedule_SS
            // 
            this.Shedule_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Shedule_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Shedule_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Shedule_SS.Caption = "S/s";
            this.Shedule_SS.FieldName = "SS";
            this.Shedule_SS.Name = "Shedule_SS";
            this.Shedule_SS.OptionsColumn.FixedWidth = true;
            this.Shedule_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", "{0}")});
            this.Shedule_SS.Visible = true;
            this.Shedule_SS.VisibleIndex = 0;
            this.Shedule_SS.Width = 30;
            // 
            // Shedule_MonthlyPayment
            // 
            this.Shedule_MonthlyPayment.Caption = "Aylıq ödəniş";
            this.Shedule_MonthlyPayment.DisplayFormat.FormatString = "n2";
            this.Shedule_MonthlyPayment.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Shedule_MonthlyPayment.FieldName = "MonthlyPayment";
            this.Shedule_MonthlyPayment.Name = "Shedule_MonthlyPayment";
            this.Shedule_MonthlyPayment.OptionsColumn.FixedWidth = true;
            this.Shedule_MonthlyPayment.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MonthlyPayment", "{0:n2}")});
            this.Shedule_MonthlyPayment.Visible = true;
            this.Shedule_MonthlyPayment.VisibleIndex = 2;
            // 
            // Shedule_InterestAmount
            // 
            this.Shedule_InterestAmount.Caption = "Faiz məbləği";
            this.Shedule_InterestAmount.DisplayFormat.FormatString = "n2";
            this.Shedule_InterestAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Shedule_InterestAmount.FieldName = "InterestAmount";
            this.Shedule_InterestAmount.Name = "Shedule_InterestAmount";
            this.Shedule_InterestAmount.OptionsColumn.FixedWidth = true;
            this.Shedule_InterestAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "InterestAmount", "{0:n2}")});
            this.Shedule_InterestAmount.Visible = true;
            this.Shedule_InterestAmount.VisibleIndex = 4;
            // 
            // Shedule_Debt
            // 
            this.Shedule_Debt.Caption = "Cari balans";
            this.Shedule_Debt.DisplayFormat.FormatString = "n2";
            this.Shedule_Debt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Shedule_Debt.FieldName = "Debt";
            this.Shedule_Debt.Name = "Shedule_Debt";
            this.Shedule_Debt.OptionsColumn.FixedWidth = true;
            this.Shedule_Debt.Visible = true;
            this.Shedule_Debt.VisibleIndex = 5;
            // 
            // MonthlyAmountValue
            // 
            this.MonthlyAmountValue.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.MonthlyAmountValue.Location = new System.Drawing.Point(145, 108);
            this.MonthlyAmountValue.Name = "MonthlyAmountValue";
            this.MonthlyAmountValue.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.MonthlyAmountValue.Properties.Appearance.Options.UseFont = true;
            this.MonthlyAmountValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.MonthlyAmountValue.Properties.Mask.EditMask = "n2";
            this.MonthlyAmountValue.Properties.Precision = 2;
            this.MonthlyAmountValue.Properties.ReadOnly = true;
            this.MonthlyAmountValue.Size = new System.Drawing.Size(143, 20);
            this.MonthlyAmountValue.TabIndex = 67;
            this.MonthlyAmountValue.EditValueChanged += new System.EventHandler(this.MonthlyAmountValue_EditValueChanged);
            // 
            // AverageInterestPercentValue
            // 
            this.AverageInterestPercentValue.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.AverageInterestPercentValue.Location = new System.Drawing.Point(145, 222);
            this.AverageInterestPercentValue.Name = "AverageInterestPercentValue";
            this.AverageInterestPercentValue.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.AverageInterestPercentValue.Properties.Appearance.Options.UseFont = true;
            this.AverageInterestPercentValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, editorButtonImageOptions5, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.AverageInterestPercentValue.Properties.Mask.EditMask = "n2";
            this.AverageInterestPercentValue.Properties.Precision = 2;
            this.AverageInterestPercentValue.Properties.ReadOnly = true;
            this.AverageInterestPercentValue.Size = new System.Drawing.Size(143, 20);
            this.AverageInterestPercentValue.TabIndex = 69;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 225);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(110, 13);
            this.labelControl1.TabIndex = 68;
            this.labelControl1.Text = "Orta aylıq faiz dərəcəsi";
            // 
            // InterestAmountValue
            // 
            this.InterestAmountValue.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.InterestAmountValue.Location = new System.Drawing.Point(145, 161);
            this.InterestAmountValue.Name = "InterestAmountValue";
            this.InterestAmountValue.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.InterestAmountValue.Properties.Appearance.Options.UseFont = true;
            this.InterestAmountValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, editorButtonImageOptions6, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.InterestAmountValue.Properties.Mask.EditMask = "n2";
            this.InterestAmountValue.Properties.Precision = 2;
            this.InterestAmountValue.Properties.ReadOnly = true;
            this.InterestAmountValue.Size = new System.Drawing.Size(143, 20);
            this.InterestAmountValue.TabIndex = 70;
            this.InterestAmountValue.EditValueChanged += new System.EventHandler(this.InterestAmountValue_EditValueChanged);
            // 
            // FCreditCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(868, 620);
            this.Controls.Add(this.InterestAmountValue);
            this.Controls.Add(this.AverageInterestPercentValue);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.MonthlyAmountValue);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.InterestAmountLabel);
            this.Controls.Add(this.SumPaymentText);
            this.Controls.Add(this.SumPaymentLabel);
            this.Controls.Add(this.InterestValue);
            this.Controls.Add(this.PeriodValue);
            this.Controls.Add(this.CreditAmountValue);
            this.Controls.Add(this.MonthlyAmountLabel);
            this.Controls.Add(this.InterestLabel);
            this.Controls.Add(this.PeriodLabel);
            this.Controls.Add(this.CreditAmountLabel);
            this.Controls.Add(this.PanelOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCreditCalculator";
            this.ShowInTaskbar = false;
            this.Text = "Kredit kalkulyatoru";
            this.Load += new System.EventHandler(this.FCreditCalculator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CreditAmountValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SumPaymentText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthlyAmountValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AverageInterestPercentValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestAmountValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.LabelControl CreditAmountLabel;
        private DevExpress.XtraEditors.LabelControl PeriodLabel;
        private DevExpress.XtraEditors.LabelControl InterestLabel;
        private DevExpress.XtraEditors.LabelControl MonthlyAmountLabel;
        private DevExpress.XtraEditors.CalcEdit CreditAmountValue;
        private DevExpress.XtraEditors.CalcEdit PeriodValue;
        private DevExpress.XtraEditors.CalcEdit InterestValue;
        private DevExpress.XtraEditors.TextEdit SumPaymentText;
        private DevExpress.XtraEditors.LabelControl SumPaymentLabel;
        private DevExpress.XtraEditors.LabelControl InterestAmountLabel;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl PaymentSchedulesGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PaymentSchedulesGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Shedule_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Shedule_Date;
        private DevExpress.XtraGrid.Columns.GridColumn Shedule_MonthlyPayment;
        private DevExpress.XtraGrid.Columns.GridColumn Shedule_BasicAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Shedule_InterestAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Shedule_Debt;
        private DevExpress.XtraEditors.CalcEdit MonthlyAmountValue;
        private DevExpress.XtraEditors.CalcEdit AverageInterestPercentValue;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CalcEdit InterestAmountValue;
    }
}