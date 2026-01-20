namespace ImageProcessor;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();

        // Main layout
        this.mainTableLayout = new TableLayoutPanel();
        this.topPanel = new Panel();
        this.previewPanel = new Panel();
        this.validationPanel = new Panel();
        this.settingsPanel = new Panel();
        this.buttonPanel = new Panel();

        // Validation panel controls
        this.lblValidationTitle = new Label();
        this.lblResolutionStatus = new Label();
        this.lblAspectRatioStatus = new Label();
        this.lblFileSizeStatus = new Label();
        this.lblOverallStatus = new Label();

        // Top panel controls
        this.btnOpen = new Button();
        this.txtUrl = new TextBox();
        this.btnLoadUrl = new Button();
        this.btnSave = new Button();
        this.lblFileName = new Label();
        this.lblImageInfo = new Label();

        // Preview
        this.pictureBox = new PictureBox();

        // Settings controls - Resize
        this.grpResize = new GroupBox();
        this.lblWidth = new Label();
        this.txtWidth = new TextBox();
        this.lblHeight = new Label();
        this.txtHeight = new TextBox();
        this.chkMaintainRatio = new CheckBox();

        // Settings controls - Bleed
        this.grpBleed = new GroupBox();
        this.lblBleedWidth = new Label();
        this.numBleedWidth = new NumericUpDown();
        this.lblBleedHeight = new Label();
        this.numBleedHeight = new NumericUpDown();
        this.cmbBleedUnit = new ComboBox();
        this.lblDpi = new Label();
        this.numDpi = new NumericUpDown();
        this.chkMirrorBleed = new CheckBox();

        // Settings controls - Flip/Mirror
        this.grpFlip = new GroupBox();
        this.chkFlipHorizontal = new CheckBox();
        this.chkFlipVertical = new CheckBox();

        // Settings controls - Rotation
        this.grpRotation = new GroupBox();
        this.lblRotation = new Label();
        this.cmbRotation = new ComboBox();
        this.lblCustomAngle = new Label();
        this.numCustomAngle = new NumericUpDown();

        // Settings controls - Output
        this.grpOutput = new GroupBox();
        this.lblFormat = new Label();
        this.cmbFormat = new ComboBox();
        this.lblQuality = new Label();
        this.numQuality = new NumericUpDown();

        this.btnApply = new Button();
        this.btnReset = new Button();
        this.btnSaveExport = new Button();

        // Dialogs
        this.openFileDialog = new OpenFileDialog();
        this.saveFileDialog = new SaveFileDialog();

        // Suspend layout
        this.mainTableLayout.SuspendLayout();
        this.topPanel.SuspendLayout();
        this.previewPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
        this.settingsPanel.SuspendLayout();
        this.buttonPanel.SuspendLayout();
        this.grpResize.SuspendLayout();
        this.grpBleed.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.numBleedWidth).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.numBleedHeight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.numDpi).BeginInit();
        this.grpFlip.SuspendLayout();
        this.grpRotation.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.numCustomAngle).BeginInit();
        this.grpOutput.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.numQuality).BeginInit();
        this.SuspendLayout();

        // mainTableLayout
        this.mainTableLayout.ColumnCount = 1;
        this.mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        this.mainTableLayout.Controls.Add(this.topPanel, 0, 0);
        this.mainTableLayout.Controls.Add(this.previewPanel, 0, 1);
        this.mainTableLayout.Controls.Add(this.validationPanel, 0, 2);
        this.mainTableLayout.Controls.Add(this.settingsPanel, 0, 3);
        this.mainTableLayout.Controls.Add(this.buttonPanel, 0, 4);
        this.mainTableLayout.Dock = DockStyle.Fill;
        this.mainTableLayout.Location = new Point(0, 0);
        this.mainTableLayout.Name = "mainTableLayout";
        this.mainTableLayout.RowCount = 5;
        this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
        this.mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
        this.mainTableLayout.Size = new Size(900, 700);
        this.mainTableLayout.TabIndex = 0;

        // topPanel
        this.topPanel.BackColor = SystemColors.ControlLight;
        this.topPanel.Controls.Add(this.btnOpen);
        this.topPanel.Controls.Add(this.txtUrl);
        this.topPanel.Controls.Add(this.btnLoadUrl);
        this.topPanel.Controls.Add(this.lblFileName);
        this.topPanel.Controls.Add(this.lblImageInfo);
        this.topPanel.Controls.Add(this.btnSave);
        this.topPanel.Dock = DockStyle.Fill;
        this.topPanel.Name = "topPanel";
        this.topPanel.Padding = new Padding(10);
        this.topPanel.TabIndex = 0;

        // btnOpen
        this.btnOpen.Location = new Point(10, 12);
        this.btnOpen.Name = "btnOpen";
        this.btnOpen.Size = new Size(100, 28);
        this.btnOpen.TabIndex = 0;
        this.btnOpen.Text = "Open File";
        this.btnOpen.UseVisualStyleBackColor = true;
        this.btnOpen.Click += new EventHandler(this.btnOpen_Click);

        // txtUrl
        this.txtUrl.Location = new Point(120, 14);
        this.txtUrl.Name = "txtUrl";
        this.txtUrl.Size = new Size(350, 23);
        this.txtUrl.PlaceholderText = "Enter image URL...";
        this.txtUrl.TabIndex = 1;

        // btnLoadUrl
        this.btnLoadUrl.Location = new Point(480, 12);
        this.btnLoadUrl.Name = "btnLoadUrl";
        this.btnLoadUrl.Size = new Size(80, 28);
        this.btnLoadUrl.TabIndex = 2;
        this.btnLoadUrl.Text = "Load URL";
        this.btnLoadUrl.UseVisualStyleBackColor = true;
        this.btnLoadUrl.Click += new EventHandler(this.btnLoadUrl_Click);

        // lblFileName
        this.lblFileName.AutoSize = true;
        this.lblFileName.Location = new Point(570, 18);
        this.lblFileName.Name = "lblFileName";
        this.lblFileName.Size = new Size(100, 15);
        this.lblFileName.TabIndex = 3;
        this.lblFileName.Text = "No file loaded";

        // lblImageInfo
        this.lblImageInfo.AutoSize = true;
        this.lblImageInfo.Location = new Point(680, 18);
        this.lblImageInfo.Name = "lblImageInfo";
        this.lblImageInfo.Size = new Size(50, 15);
        this.lblImageInfo.TabIndex = 4;
        this.lblImageInfo.Text = "";

        // btnSave
        this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnSave.Enabled = false;
        this.btnSave.Location = new Point(790, 12);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 28);
        this.btnSave.TabIndex = 5;
        this.btnSave.Text = "Save / Export";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.btnSave_Click);

        // previewPanel
        this.previewPanel.AutoScroll = true;
        this.previewPanel.BackColor = SystemColors.AppWorkspace;
        this.previewPanel.BorderStyle = BorderStyle.Fixed3D;
        this.previewPanel.Controls.Add(this.pictureBox);
        this.previewPanel.Dock = DockStyle.Fill;
        this.previewPanel.Name = "previewPanel";
        this.previewPanel.TabIndex = 1;

        // pictureBox
        this.pictureBox.BackColor = SystemColors.AppWorkspace;
        this.pictureBox.Location = new Point(0, 0);
        this.pictureBox.Name = "pictureBox";
        this.pictureBox.Size = new Size(400, 300);
        this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        this.pictureBox.TabIndex = 0;
        this.pictureBox.TabStop = false;

        // validationPanel
        this.validationPanel.BackColor = SystemColors.ControlLight;
        this.validationPanel.BorderStyle = BorderStyle.FixedSingle;
        this.validationPanel.Controls.Add(this.lblValidationTitle);
        this.validationPanel.Controls.Add(this.lblResolutionStatus);
        this.validationPanel.Controls.Add(this.lblAspectRatioStatus);
        this.validationPanel.Controls.Add(this.lblFileSizeStatus);
        this.validationPanel.Controls.Add(this.lblOverallStatus);
        this.validationPanel.Dock = DockStyle.Fill;
        this.validationPanel.Name = "validationPanel";
        this.validationPanel.Padding = new Padding(10, 5, 10, 5);
        this.validationPanel.TabIndex = 2;

        // lblValidationTitle
        this.lblValidationTitle.AutoSize = true;
        this.lblValidationTitle.Font = new Font(this.Font.FontFamily, 9F, FontStyle.Bold);
        this.lblValidationTitle.Location = new Point(10, 8);
        this.lblValidationTitle.Name = "lblValidationTitle";
        this.lblValidationTitle.Size = new Size(120, 15);
        this.lblValidationTitle.Text = "Image Validation:";

        // lblResolutionStatus
        this.lblResolutionStatus.AutoSize = true;
        this.lblResolutionStatus.Location = new Point(10, 30);
        this.lblResolutionStatus.Name = "lblResolutionStatus";
        this.lblResolutionStatus.Size = new Size(200, 15);
        this.lblResolutionStatus.Text = "Resolution: --";

        // lblAspectRatioStatus
        this.lblAspectRatioStatus.AutoSize = true;
        this.lblAspectRatioStatus.Location = new Point(250, 30);
        this.lblAspectRatioStatus.Name = "lblAspectRatioStatus";
        this.lblAspectRatioStatus.Size = new Size(200, 15);
        this.lblAspectRatioStatus.Text = "Aspect Ratio: --";

        // lblFileSizeStatus
        this.lblFileSizeStatus.AutoSize = true;
        this.lblFileSizeStatus.Location = new Point(500, 30);
        this.lblFileSizeStatus.Name = "lblFileSizeStatus";
        this.lblFileSizeStatus.Size = new Size(150, 15);
        this.lblFileSizeStatus.Text = "File Size: --";

        // lblOverallStatus
        this.lblOverallStatus.AutoSize = true;
        this.lblOverallStatus.Font = new Font(this.Font.FontFamily, 10F, FontStyle.Bold);
        this.lblOverallStatus.Location = new Point(700, 25);
        this.lblOverallStatus.Name = "lblOverallStatus";
        this.lblOverallStatus.Size = new Size(150, 20);
        this.lblOverallStatus.Text = "";

        // settingsPanel
        this.settingsPanel.AutoScroll = true;
        this.settingsPanel.BackColor = SystemColors.Control;
        this.settingsPanel.Controls.Add(this.grpResize);
        this.settingsPanel.Controls.Add(this.grpBleed);
        this.settingsPanel.Controls.Add(this.grpFlip);
        this.settingsPanel.Controls.Add(this.grpRotation);
        this.settingsPanel.Controls.Add(this.grpOutput);
        this.settingsPanel.Dock = DockStyle.Fill;
        this.settingsPanel.Name = "settingsPanel";
        this.settingsPanel.Padding = new Padding(5);
        this.settingsPanel.TabIndex = 2;

        // buttonPanel - NEW panel for buttons at bottom
        this.buttonPanel.BackColor = SystemColors.ControlLight;
        this.buttonPanel.Controls.Add(this.btnApply);
        this.buttonPanel.Controls.Add(this.btnReset);
        this.buttonPanel.Controls.Add(this.btnSaveExport);
        this.buttonPanel.Dock = DockStyle.Fill;
        this.buttonPanel.Name = "buttonPanel";
        this.buttonPanel.Padding = new Padding(10);
        this.buttonPanel.TabIndex = 3;

        // grpResize
        this.grpResize.Controls.Add(this.lblWidth);
        this.grpResize.Controls.Add(this.txtWidth);
        this.grpResize.Controls.Add(this.lblHeight);
        this.grpResize.Controls.Add(this.txtHeight);
        this.grpResize.Controls.Add(this.chkMaintainRatio);
        this.grpResize.Location = new Point(5, 5);
        this.grpResize.Name = "grpResize";
        this.grpResize.Size = new Size(160, 130);
        this.grpResize.TabIndex = 0;
        this.grpResize.TabStop = false;
        this.grpResize.Text = "Resize (pixels)";

        // lblWidth
        this.lblWidth.AutoSize = true;
        this.lblWidth.Location = new Point(10, 25);
        this.lblWidth.Name = "lblWidth";
        this.lblWidth.Size = new Size(42, 15);
        this.lblWidth.TabIndex = 0;
        this.lblWidth.Text = "Width:";

        // txtWidth
        this.txtWidth.Location = new Point(65, 22);
        this.txtWidth.Name = "txtWidth";
        this.txtWidth.Size = new Size(80, 23);
        this.txtWidth.TabIndex = 1;
        this.txtWidth.TextChanged += new EventHandler(this.txtWidth_TextChanged);

        // lblHeight
        this.lblHeight.AutoSize = true;
        this.lblHeight.Location = new Point(10, 55);
        this.lblHeight.Name = "lblHeight";
        this.lblHeight.Size = new Size(46, 15);
        this.lblHeight.TabIndex = 2;
        this.lblHeight.Text = "Height:";

        // txtHeight
        this.txtHeight.Location = new Point(65, 52);
        this.txtHeight.Name = "txtHeight";
        this.txtHeight.Size = new Size(80, 23);
        this.txtHeight.TabIndex = 3;
        this.txtHeight.TextChanged += new EventHandler(this.txtHeight_TextChanged);

        // chkMaintainRatio
        this.chkMaintainRatio.AutoSize = true;
        this.chkMaintainRatio.Checked = true;
        this.chkMaintainRatio.CheckState = CheckState.Checked;
        this.chkMaintainRatio.Location = new Point(10, 85);
        this.chkMaintainRatio.Name = "chkMaintainRatio";
        this.chkMaintainRatio.Size = new Size(80, 19);
        this.chkMaintainRatio.TabIndex = 4;
        this.chkMaintainRatio.Text = "Lock ratio";
        this.chkMaintainRatio.UseVisualStyleBackColor = true;

        // grpBleed
        this.grpBleed.Controls.Add(this.lblBleedWidth);
        this.grpBleed.Controls.Add(this.numBleedWidth);
        this.grpBleed.Controls.Add(this.lblBleedHeight);
        this.grpBleed.Controls.Add(this.numBleedHeight);
        this.grpBleed.Controls.Add(this.cmbBleedUnit);
        this.grpBleed.Controls.Add(this.lblDpi);
        this.grpBleed.Controls.Add(this.numDpi);
        this.grpBleed.Controls.Add(this.chkMirrorBleed);
        this.grpBleed.Location = new Point(170, 5);
        this.grpBleed.Name = "grpBleed";
        this.grpBleed.Size = new Size(200, 130);
        this.grpBleed.TabIndex = 1;
        this.grpBleed.TabStop = false;
        this.grpBleed.Text = "Bleed (all 4 sides)";

        // lblBleedWidth
        this.lblBleedWidth.AutoSize = true;
        this.lblBleedWidth.Location = new Point(10, 25);
        this.lblBleedWidth.Name = "lblBleedWidth";
        this.lblBleedWidth.Size = new Size(55, 15);
        this.lblBleedWidth.TabIndex = 0;
        this.lblBleedWidth.Text = "L/R:";

        // numBleedWidth
        this.numBleedWidth.DecimalPlaces = 1;
        this.numBleedWidth.Location = new Point(45, 22);
        this.numBleedWidth.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        this.numBleedWidth.Name = "numBleedWidth";
        this.numBleedWidth.Size = new Size(55, 23);
        this.numBleedWidth.TabIndex = 1;

        // lblBleedHeight
        this.lblBleedHeight.AutoSize = true;
        this.lblBleedHeight.Location = new Point(105, 25);
        this.lblBleedHeight.Name = "lblBleedHeight";
        this.lblBleedHeight.Size = new Size(55, 15);
        this.lblBleedHeight.TabIndex = 2;
        this.lblBleedHeight.Text = "T/B:";

        // numBleedHeight
        this.numBleedHeight.DecimalPlaces = 1;
        this.numBleedHeight.Location = new Point(135, 22);
        this.numBleedHeight.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        this.numBleedHeight.Name = "numBleedHeight";
        this.numBleedHeight.Size = new Size(55, 23);
        this.numBleedHeight.TabIndex = 3;

        // cmbBleedUnit
        this.cmbBleedUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbBleedUnit.FormattingEnabled = true;
        this.cmbBleedUnit.Items.AddRange(new object[] { "mm", "px", "in" });
        this.cmbBleedUnit.Location = new Point(10, 52);
        this.cmbBleedUnit.Name = "cmbBleedUnit";
        this.cmbBleedUnit.Size = new Size(50, 23);
        this.cmbBleedUnit.TabIndex = 4;

        // lblDpi
        this.lblDpi.AutoSize = true;
        this.lblDpi.Location = new Point(70, 55);
        this.lblDpi.Name = "lblDpi";
        this.lblDpi.Size = new Size(28, 15);
        this.lblDpi.TabIndex = 5;
        this.lblDpi.Text = "DPI:";

        // numDpi
        this.numDpi.Location = new Point(100, 52);
        this.numDpi.Maximum = new decimal(new int[] { 1200, 0, 0, 0 });
        this.numDpi.Minimum = new decimal(new int[] { 72, 0, 0, 0 });
        this.numDpi.Name = "numDpi";
        this.numDpi.Size = new Size(70, 23);
        this.numDpi.TabIndex = 6;
        this.numDpi.Value = new decimal(new int[] { 300, 0, 0, 0 });

        // chkMirrorBleed
        this.chkMirrorBleed.AutoSize = true;
        this.chkMirrorBleed.Checked = true;
        this.chkMirrorBleed.CheckState = CheckState.Checked;
        this.chkMirrorBleed.Location = new Point(10, 85);
        this.chkMirrorBleed.Name = "chkMirrorBleed";
        this.chkMirrorBleed.Size = new Size(96, 19);
        this.chkMirrorBleed.TabIndex = 7;
        this.chkMirrorBleed.Text = "Mirror edges";
        this.chkMirrorBleed.UseVisualStyleBackColor = true;

        // grpFlip
        this.grpFlip.Controls.Add(this.chkFlipHorizontal);
        this.grpFlip.Controls.Add(this.chkFlipVertical);
        this.grpFlip.Location = new Point(375, 5);
        this.grpFlip.Name = "grpFlip";
        this.grpFlip.Size = new Size(120, 130);
        this.grpFlip.TabIndex = 2;
        this.grpFlip.TabStop = false;
        this.grpFlip.Text = "Flip / Mirror";

        // chkFlipHorizontal
        this.chkFlipHorizontal.AutoSize = true;
        this.chkFlipHorizontal.Location = new Point(10, 35);
        this.chkFlipHorizontal.Name = "chkFlipHorizontal";
        this.chkFlipHorizontal.Size = new Size(83, 19);
        this.chkFlipHorizontal.TabIndex = 0;
        this.chkFlipHorizontal.Text = "Horizontal";
        this.chkFlipHorizontal.UseVisualStyleBackColor = true;

        // chkFlipVertical
        this.chkFlipVertical.AutoSize = true;
        this.chkFlipVertical.Location = new Point(10, 65);
        this.chkFlipVertical.Name = "chkFlipVertical";
        this.chkFlipVertical.Size = new Size(66, 19);
        this.chkFlipVertical.TabIndex = 1;
        this.chkFlipVertical.Text = "Vertical";
        this.chkFlipVertical.UseVisualStyleBackColor = true;

        // grpRotation
        this.grpRotation.Controls.Add(this.lblRotation);
        this.grpRotation.Controls.Add(this.cmbRotation);
        this.grpRotation.Controls.Add(this.lblCustomAngle);
        this.grpRotation.Controls.Add(this.numCustomAngle);
        this.grpRotation.Location = new Point(500, 5);
        this.grpRotation.Name = "grpRotation";
        this.grpRotation.Size = new Size(160, 130);
        this.grpRotation.TabIndex = 3;
        this.grpRotation.TabStop = false;
        this.grpRotation.Text = "Rotation";

        // lblRotation
        this.lblRotation.AutoSize = true;
        this.lblRotation.Location = new Point(10, 25);
        this.lblRotation.Name = "lblRotation";
        this.lblRotation.Size = new Size(42, 15);
        this.lblRotation.TabIndex = 0;
        this.lblRotation.Text = "Preset:";

        // cmbRotation
        this.cmbRotation.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbRotation.FormattingEnabled = true;
        this.cmbRotation.Items.AddRange(new object[] { "0°", "90°", "180°", "270°", "Custom" });
        this.cmbRotation.Location = new Point(60, 22);
        this.cmbRotation.Name = "cmbRotation";
        this.cmbRotation.Size = new Size(90, 23);
        this.cmbRotation.TabIndex = 1;
        this.cmbRotation.SelectedIndexChanged += new EventHandler(this.cmbRotation_SelectedIndexChanged);

        // lblCustomAngle
        this.lblCustomAngle.AutoSize = true;
        this.lblCustomAngle.Location = new Point(10, 55);
        this.lblCustomAngle.Name = "lblCustomAngle";
        this.lblCustomAngle.Size = new Size(50, 15);
        this.lblCustomAngle.TabIndex = 2;
        this.lblCustomAngle.Text = "Custom:";

        // numCustomAngle
        this.numCustomAngle.DecimalPlaces = 1;
        this.numCustomAngle.Enabled = false;
        this.numCustomAngle.Location = new Point(60, 52);
        this.numCustomAngle.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
        this.numCustomAngle.Minimum = new decimal(new int[] { 360, 0, 0, int.MinValue });
        this.numCustomAngle.Name = "numCustomAngle";
        this.numCustomAngle.Size = new Size(80, 23);
        this.numCustomAngle.TabIndex = 3;

        // grpOutput
        this.grpOutput.Controls.Add(this.lblFormat);
        this.grpOutput.Controls.Add(this.cmbFormat);
        this.grpOutput.Controls.Add(this.lblQuality);
        this.grpOutput.Controls.Add(this.numQuality);
        this.grpOutput.Location = new Point(665, 5);
        this.grpOutput.Name = "grpOutput";
        this.grpOutput.Size = new Size(160, 130);
        this.grpOutput.TabIndex = 4;
        this.grpOutput.TabStop = false;
        this.grpOutput.Text = "Output";

        // lblFormat
        this.lblFormat.AutoSize = true;
        this.lblFormat.Location = new Point(10, 25);
        this.lblFormat.Name = "lblFormat";
        this.lblFormat.Size = new Size(48, 15);
        this.lblFormat.TabIndex = 0;
        this.lblFormat.Text = "Format:";

        // cmbFormat
        this.cmbFormat.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbFormat.FormattingEnabled = true;
        this.cmbFormat.Items.AddRange(new object[] { "PNG", "JPEG", "PDF" });
        this.cmbFormat.Location = new Point(65, 22);
        this.cmbFormat.Name = "cmbFormat";
        this.cmbFormat.Size = new Size(80, 23);
        this.cmbFormat.TabIndex = 1;
        this.cmbFormat.SelectedIndexChanged += new EventHandler(this.cmbFormat_SelectedIndexChanged);

        // lblQuality
        this.lblQuality.AutoSize = true;
        this.lblQuality.Location = new Point(10, 55);
        this.lblQuality.Name = "lblQuality";
        this.lblQuality.Size = new Size(48, 15);
        this.lblQuality.TabIndex = 2;
        this.lblQuality.Text = "Quality:";

        // numQuality
        this.numQuality.Enabled = false;
        this.numQuality.Location = new Point(65, 52);
        this.numQuality.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numQuality.Name = "numQuality";
        this.numQuality.Size = new Size(60, 23);
        this.numQuality.TabIndex = 3;
        this.numQuality.Value = new decimal(new int[] { 90, 0, 0, 0 });

        // btnApply - Now in buttonPanel
        this.btnApply.Enabled = false;
        this.btnApply.Location = new Point(10, 10);
        this.btnApply.Name = "btnApply";
        this.btnApply.Size = new Size(150, 40);
        this.btnApply.TabIndex = 0;
        this.btnApply.Text = "Apply Changes";
        this.btnApply.UseVisualStyleBackColor = true;
        this.btnApply.Click += new EventHandler(this.btnApply_Click);

        // btnReset - Now in buttonPanel
        this.btnReset.Enabled = false;
        this.btnReset.Location = new Point(170, 10);
        this.btnReset.Name = "btnReset";
        this.btnReset.Size = new Size(100, 40);
        this.btnReset.TabIndex = 1;
        this.btnReset.Text = "Reset";
        this.btnReset.UseVisualStyleBackColor = true;
        this.btnReset.Click += new EventHandler(this.btnReset_Click);

        // btnSaveExport - Prominent Save/Export button
        this.btnSaveExport.BackColor = Color.FromArgb(0, 120, 215);
        this.btnSaveExport.Enabled = false;
        this.btnSaveExport.FlatStyle = FlatStyle.Flat;
        this.btnSaveExport.Font = new Font(this.Font.FontFamily, 10F, FontStyle.Bold);
        this.btnSaveExport.ForeColor = Color.White;
        this.btnSaveExport.Location = new Point(300, 5);
        this.btnSaveExport.Name = "btnSaveExport";
        this.btnSaveExport.Size = new Size(180, 50);
        this.btnSaveExport.TabIndex = 2;
        this.btnSaveExport.Text = "💾 SAVE / EXPORT";
        this.btnSaveExport.UseVisualStyleBackColor = false;
        this.btnSaveExport.Click += new EventHandler(this.btnSave_Click);

        // openFileDialog
        this.openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|JPEG Files|*.jpg;*.jpeg|PNG Files|*.png|All Files|*.*";
        this.openFileDialog.Title = "Open Image File";

        // saveFileDialog
        this.saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|PDF Document|*.pdf";
        this.saveFileDialog.Title = "Save Image";

        // MainForm
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(900, 700);
        this.Controls.Add(this.mainTableLayout);
        this.MinimumSize = new Size(850, 600);
        this.Name = "MainForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Image Processor";
        this.Load += new EventHandler(this.MainForm_Load);

        // Resume layout
        this.mainTableLayout.ResumeLayout(false);
        this.topPanel.ResumeLayout(false);
        this.topPanel.PerformLayout();
        this.previewPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
        this.validationPanel.ResumeLayout(false);
        this.validationPanel.PerformLayout();
        this.settingsPanel.ResumeLayout(false);
        this.buttonPanel.ResumeLayout(false);
        this.grpResize.ResumeLayout(false);
        this.grpResize.PerformLayout();
        this.grpBleed.ResumeLayout(false);
        this.grpBleed.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.numBleedWidth).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.numBleedHeight).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.numDpi).EndInit();
        this.grpFlip.ResumeLayout(false);
        this.grpFlip.PerformLayout();
        this.grpRotation.ResumeLayout(false);
        this.grpRotation.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.numCustomAngle).EndInit();
        this.grpOutput.ResumeLayout(false);
        this.grpOutput.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.numQuality).EndInit();
        this.ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel mainTableLayout;
    private Panel topPanel;
    private Panel previewPanel;
    private Panel validationPanel;
    private Panel settingsPanel;
    private Panel buttonPanel;

    // Validation labels
    private Label lblValidationTitle;
    private Label lblResolutionStatus;
    private Label lblAspectRatioStatus;
    private Label lblFileSizeStatus;
    private Label lblOverallStatus;
    private Button btnOpen;
    private TextBox txtUrl;
    private Button btnLoadUrl;
    private Button btnSave;
    private Label lblFileName;
    private Label lblImageInfo;
    private PictureBox pictureBox;

    // Resize group
    private GroupBox grpResize;
    private Label lblWidth;
    private TextBox txtWidth;
    private Label lblHeight;
    private TextBox txtHeight;
    private CheckBox chkMaintainRatio;

    // Bleed group
    private GroupBox grpBleed;
    private Label lblBleedWidth;
    private NumericUpDown numBleedWidth;
    private Label lblBleedHeight;
    private NumericUpDown numBleedHeight;
    private ComboBox cmbBleedUnit;
    private Label lblDpi;
    private NumericUpDown numDpi;
    private CheckBox chkMirrorBleed;

    // Flip group
    private GroupBox grpFlip;
    private CheckBox chkFlipHorizontal;
    private CheckBox chkFlipVertical;

    // Rotation group
    private GroupBox grpRotation;
    private Label lblRotation;
    private ComboBox cmbRotation;
    private Label lblCustomAngle;
    private NumericUpDown numCustomAngle;

    // Output group
    private GroupBox grpOutput;
    private Label lblFormat;
    private ComboBox cmbFormat;
    private Label lblQuality;
    private NumericUpDown numQuality;

    private Button btnApply;
    private Button btnReset;
    private Button btnSaveExport;
    private OpenFileDialog openFileDialog;
    private SaveFileDialog saveFileDialog;
}
