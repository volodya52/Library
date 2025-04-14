namespace Apteka
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            Search = new Label( );
            searchTextBox = new TextBox( );
            booksDataGridView = new DataGridView( );
            authorComboBox = new ComboBox( );
            label1 = new Label( );
            searchButton = new Button( );
            coverPictureBox = new PictureBox( );
            descriptionRichTextBox = new RichTextBox( );
            label2 = new Label( );
            genreComboBox = new ComboBox( );
            button1 = new Button( );
            ((System.ComponentModel.ISupportInitialize) booksDataGridView).BeginInit( );
            ((System.ComponentModel.ISupportInitialize) coverPictureBox).BeginInit( );
            SuspendLayout( );
            // 
            // Search
            // 
            Search.AutoSize = true;
            Search.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point,  204);
            Search.Location = new Point(742, 55);
            Search.Name = "Search";
            Search.Size = new Size(109, 41);
            Search.TabIndex = 0;
            Search.Text = "Поиск";
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(662, 100);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(292, 27);
            searchTextBox.TabIndex = 1;
            // 
            // booksDataGridView
            // 
            booksDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            booksDataGridView.Location = new Point(23, 228);
            booksDataGridView.Name = "booksDataGridView";
            booksDataGridView.RowHeadersWidth = 51;
            booksDataGridView.Size = new Size(581, 500);
            booksDataGridView.TabIndex = 2;
            booksDataGridView.CellContentClick += booksDataGridView_CellContentClick;
            // 
            // authorComboBox
            // 
            authorComboBox.FormattingEnabled = true;
            authorComboBox.Items.AddRange(new object [ ] { "Жидкий", "Твердый", "Мягкий", "Аэрозоль" });
            authorComboBox.Location = new Point(208, 148);
            authorComboBox.Name = "authorComboBox";
            authorComboBox.Size = new Size(335, 28);
            authorComboBox.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point,  204);
            label1.Location = new Point(23, 134);
            label1.Name = "label1";
            label1.Size = new Size(107, 41);
            label1.TabIndex = 4;
            label1.Text = "Автор";
            // 
            // searchButton
            // 
            searchButton.Location = new Point(747, 136);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(94, 29);
            searchButton.TabIndex = 5;
            searchButton.Text = "Найти";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += searchButton_Click_1;
            // 
            // coverPictureBox
            // 
            coverPictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            coverPictureBox.Location = new Point(1030, 228);
            coverPictureBox.Name = "coverPictureBox";
            coverPictureBox.Size = new Size(475, 500);
            coverPictureBox.TabIndex = 6;
            coverPictureBox.TabStop = false;
            // 
            // descriptionRichTextBox
            // 
            descriptionRichTextBox.Location = new Point(625, 228);
            descriptionRichTextBox.Name = "descriptionRichTextBox";
            descriptionRichTextBox.Size = new Size(379, 500);
            descriptionRichTextBox.TabIndex = 7;
            descriptionRichTextBox.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point,  204);
            label2.Location = new Point(23, 66);
            label2.Name = "label2";
            label2.Size = new Size(101, 41);
            label2.TabIndex = 9;
            label2.Text = "Жанр";
            // 
            // genreComboBox
            // 
            genreComboBox.FormattingEnabled = true;
            genreComboBox.Items.AddRange(new object [ ] { "Антибактериальные препараты", "Гормоны", "Диагностические средства", "Препараты, влияющие на иммунитет", "Препараты влияющие на метаболизм", "Препараты влияющие на психику", "Препараты, влияющие на свертываемость крови", "Препараты, влияющие на тонус сосудов", "Препараты, влияющие на функцию бронхов", "Препараты, влияющие на функции желудочно-кишечного тракта" });
            genreComboBox.Location = new Point(208, 79);
            genreComboBox.Name = "genreComboBox";
            genreComboBox.Size = new Size(335, 28);
            genreComboBox.TabIndex = 8;
            // 
            // button1
            // 
            button1.Location = new Point(1352, 27);
            button1.Name = "button1";
            button1.Size = new Size(153, 29);
            button1.TabIndex = 10;
            button1.Text = "Добавить книгу";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1557, 775);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(genreComboBox);
            Controls.Add(descriptionRichTextBox);
            Controls.Add(coverPictureBox);
            Controls.Add(searchButton);
            Controls.Add(label1);
            Controls.Add(authorComboBox);
            Controls.Add(booksDataGridView);
            Controls.Add(searchTextBox);
            Controls.Add(Search);
            Name = "Form1";
            Text = "Библиотека";
            ((System.ComponentModel.ISupportInitialize) booksDataGridView).EndInit( );
            ((System.ComponentModel.ISupportInitialize) coverPictureBox).EndInit( );
            ResumeLayout(false);
            PerformLayout( );
        }

        #endregion

        private Label Search;
        private TextBox searchTextBox;
        private DataGridView booksDataGridView;
        private ComboBox authorComboBox;
        private Label label1;
        private Button searchButton;
        private PictureBox coverPictureBox;
        private RichTextBox descriptionRichTextBox;
        private Label label2;
        private ComboBox genreComboBox;
        private Button button1;
    }
}
