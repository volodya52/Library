namespace Apteka
{
    partial class RegistrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose( );
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            label1 = new Label( );
            usernameTextBox = new TextBox( );
            passwordTextBox = new TextBox( );
            roleComboBox = new ComboBox( );
            RegisterButton = new Button( );
            SuspendLayout( );
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point,  204);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(153, 63);
            label1.Name = "label1";
            label1.Size = new Size(248, 50);
            label1.TabIndex = 0;
            label1.Text = "Регистрация";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(174, 173);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(210, 27);
            usernameTextBox.TabIndex = 1;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(174, 260);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(210, 27);
            passwordTextBox.TabIndex = 2;
            // 
            // roleComboBox
            // 
            roleComboBox.FormattingEnabled = true;
            roleComboBox.Items.AddRange(new object [ ] { "Пользователь", "Админ" });
            roleComboBox.Location = new Point(174, 338);
            roleComboBox.Name = "roleComboBox";
            roleComboBox.Size = new Size(210, 28);
            roleComboBox.TabIndex = 3;
            // 
            // RegisterButton
            // 
            RegisterButton.BackColor = SystemColors.ControlLightLight;
            RegisterButton.FlatStyle = FlatStyle.Flat;
            RegisterButton.Location = new Point(226, 456);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(94, 29);
            RegisterButton.TabIndex = 4;
            RegisterButton.Text = "Войти";
            RegisterButton.UseVisualStyleBackColor = false;
            RegisterButton.Click += RegisterButton_Click;
            // 
            // RegistrationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(581, 605);
            Controls.Add(RegisterButton);
            Controls.Add(roleComboBox);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(label1);
            Name = "RegistrationForm";
            Text = "RegistrationForm";
            ResumeLayout(false);
            PerformLayout( );
        }

        #endregion

        private Label label1;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private ComboBox roleComboBox;
        private Button RegisterButton;
    }
}