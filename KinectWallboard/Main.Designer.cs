namespace KinectWallboard
{
    partial class Main
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.lblNextPageTimer = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNumSkeletons = new System.Windows.Forms.Label();
            this.lblGestureSkeletonState = new System.Windows.Forms.Label();
            this.lblKinectStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblState = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlNotification = new System.Windows.Forms.Panel();
            this.lblNotification = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.pnlNotification.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1382, 613);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Controls.Add(this.lblNextPageTimer);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblNumSkeletons);
            this.panel1.Controls.Add(this.lblGestureSkeletonState);
            this.panel1.Controls.Add(this.lblKinectStatus);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Controls.Add(this.lblState);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(970, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 607);
            this.panel1.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(277, 65);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(50, 50);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(221, 65);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(50, 50);
            this.btnPrevious.TabIndex = 11;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // lblNextPageTimer
            // 
            this.lblNextPageTimer.AutoSize = true;
            this.lblNextPageTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextPageTimer.Location = new System.Drawing.Point(64, 60);
            this.lblNextPageTimer.Name = "lblNextPageTimer";
            this.lblNextPageTimer.Size = new System.Drawing.Size(51, 55);
            this.lblNextPageTimer.TabIndex = 10;
            this.lblNextPageTimer.Text = "0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 31);
            this.label4.TabIndex = 9;
            this.label4.Text = "Number of people in view";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblNumSkeletons
            // 
            this.lblNumSkeletons.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumSkeletons.Location = new System.Drawing.Point(6, 269);
            this.lblNumSkeletons.Name = "lblNumSkeletons";
            this.lblNumSkeletons.Size = new System.Drawing.Size(81, 55);
            this.lblNumSkeletons.TabIndex = 8;
            this.lblNumSkeletons.Text = "0";
            this.lblNumSkeletons.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblGestureSkeletonState
            // 
            this.lblGestureSkeletonState.AutoSize = true;
            this.lblGestureSkeletonState.Location = new System.Drawing.Point(171, 285);
            this.lblGestureSkeletonState.Name = "lblGestureSkeletonState";
            this.lblGestureSkeletonState.Size = new System.Drawing.Size(102, 13);
            this.lblGestureSkeletonState.TabIndex = 7;
            this.lblGestureSkeletonState.Text = "No gesture skeleton";
            // 
            // lblKinectStatus
            // 
            this.lblKinectStatus.AutoSize = true;
            this.lblKinectStatus.Location = new System.Drawing.Point(170, 269);
            this.lblKinectStatus.Name = "lblKinectStatus";
            this.lblKinectStatus.Size = new System.Drawing.Size(116, 13);
            this.lblKinectStatus.TabIndex = 6;
            this.lblKinectStatus.Text = "No ready Kinect found!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 269);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Kinect status:";
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(6, 358);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(400, 240);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(71, 38);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(33, 13);
            this.lblState.TabIndex = 3;
            this.lblState.Text = "None";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "State:";
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Enabled = false;
            this.txtAddress.Location = new System.Drawing.Point(74, 9);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(332, 20);
            this.txtAddress.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Address:";
            // 
            // pnlNotification
            // 
            this.pnlNotification.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNotification.Controls.Add(this.lblNotification);
            this.pnlNotification.Location = new System.Drawing.Point(3, 3);
            this.pnlNotification.Name = "pnlNotification";
            this.pnlNotification.Size = new System.Drawing.Size(400, 75);
            this.pnlNotification.TabIndex = 1;
            this.pnlNotification.Visible = false;
            // 
            // lblNotification
            // 
            this.lblNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.Location = new System.Drawing.Point(0, 0);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(398, 73);
            this.lblNotification.TabIndex = 0;
            this.lblNotification.Text = "Notification";
            this.lblNotification.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1382, 613);
            this.Controls.Add(this.pnlNotification);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "Main";
            this.Text = "KinectWallboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.tableLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.pnlNotification.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblKinectStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label lblGestureSkeletonState;
        private System.Windows.Forms.Label lblNumSkeletons;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNextPageTimer;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Panel pnlNotification;
        private System.Windows.Forms.Label lblNotification;

    }
}

