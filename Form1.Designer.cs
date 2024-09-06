namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            label3 = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            button3 = new Button();
            openFileDialog1 = new OpenFileDialog();
            label5 = new Label();
            comboBox1 = new ComboBox();
            button4 = new Button();
            label1 = new Label();
            label6 = new Label();
            button5 = new Button();
            button6 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(410, 6);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(177, 59);
            button1.TabIndex = 0;
            button1.Text = "启动官服";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(410, 181);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(177, 59);
            button2.TabIndex = 1;
            button2.Text = "保存";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(36, 277);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(692, 31);
            label3.TabIndex = 11;
            label3.Text = "大概是在\\xxx\\ZenlessZoneZero Game\\ZenlessZoneZero.exe";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(162, 20);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(110, 31);
            label2.TabIndex = 13;
            label2.Text = "选择账号";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(281, 193);
            textBox2.Margin = new Padding(4);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(120, 31);
            textBox2.TabIndex = 14;
            textBox2.Click += textBox2_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(138, 193);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(134, 31);
            label4.TabIndex = 15;
            label4.Text = "账号保存为";
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ControlLightLight;
            button3.ForeColor = Color.CornflowerBlue;
            button3.Location = new Point(32, 360);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(104, 48);
            button3.TabIndex = 18;
            button3.Text = "第一次打开点我找到游戏本体";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(144, 376);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(0, 17);
            label5.TabIndex = 19;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(281, 23);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(120, 25);
            comboBox1.TabIndex = 20;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button4
            // 
            button4.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            button4.Location = new Point(410, 90);
            button4.Margin = new Padding(4);
            button4.Name = "button4";
            button4.Size = new Size(177, 59);
            button4.TabIndex = 21;
            button4.Text = "切换官服B服";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(643, 102);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(0, 31);
            label1.TabIndex = 22;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = SystemColors.ControlLightLight;
            label6.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.ForeColor = Color.CornflowerBlue;
            label6.Location = new Point(-1, 110);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(402, 21);
            label6.TabIndex = 23;
            label6.Text = "切换到B服后请打开游戏本体--快捷方式之类的都是官服";
            // 
            // button5
            // 
            button5.Location = new Point(624, 360);
            button5.Margin = new Padding(4);
            button5.Name = "button5";
            button5.Size = new Size(104, 48);
            button5.TabIndex = 25;
            button5.Text = "去B站关注我";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            button6.Location = new Point(643, 191);
            button6.Margin = new Padding(4);
            button6.Name = "button6";
            button6.Size = new Size(114, 38);
            button6.TabIndex = 26;
            button6.Text = "删除";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(765, 438);
            Controls.Add(button6);
            Controls.Add(label3);
            Controls.Add(button5);
            Controls.Add(label6);
            Controls.Add(label1);
            Controls.Add(button4);
            Controls.Add(comboBox1);
            Controls.Add(label5);
            Controls.Add(button3);
            Controls.Add(label4);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(4);
            Name = "Form1";
            Text = "梦魂启动器";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label3;
        private Label label2;
        private TextBox textBox2;
        private Label label4;
        private Button button3;
        private OpenFileDialog openFileDialog1;
        private Label label5;
        private ComboBox comboBox1;
        private Button button4;
        private Label label1;
        private Label label6;
        private Button button5;
        private Button button6;
    }
}