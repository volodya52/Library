using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apteka
{
    public partial class AddBookForm :Form
    {
        private SQLiteConnection connection;

        private TextBox titleTextBox;
        private NumericUpDown yearNumericUpDown;
        private ComboBox genreComboBox;
        private ComboBox authorComboBox;
        private TextBox descriptionTextBox;
        private PictureBox coverPictureBox;
        private Button selectCoverButton;
        private Button saveButton;
        private Button cancelButton;
        public AddBookForm (SQLiteConnection connection)
        {
            InitializeComponent( );
            this.connection = connection;
            InitializeComponents( );
            LoadGenres( );
            LoadAuthors( );
        }
        private void InitializeComponents ()
        {
            this.Text = "Добавить новую книгу";
            this.Size = new Size(400, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            // Название
            var titleLabel = new Label { Text = "Название:", Left = 20, Top = 20 };
            titleTextBox = new TextBox { Left = 120, Top = 20, Width = 250 };

            // Год издания
            var yearLabel = new Label { Text = "Год издания:", Left = 20, Top = 60 };
            yearNumericUpDown = new NumericUpDown { Left = 120, Top = 60, Width = 100, Minimum = 1000, Maximum = DateTime.Now.Year };

            // Жанр
            var genreLabel = new Label { Text = "Жанр:", Left = 20, Top = 100 };
            genreComboBox = new ComboBox { Left = 120, Top = 100, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            // Автор
            var authorLabel = new Label { Text = "Автор:", Left = 20, Top = 140 };
            authorComboBox = new ComboBox { Left = 120, Top = 140, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            // Описание
            var descriptionLabel = new Label { Text = "Описание:", Left = 20, Top = 180 };
            descriptionTextBox = new TextBox { Left = 120, Top = 180, Width = 250, Height = 60, Multiline = true };

            // Обложка
            var coverLabel = new Label { Text = "Обложка:", Left = 20, Top = 270 };
            coverPictureBox = new PictureBox { Left = 120, Top = 270, Width = 100, Height = 150, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            selectCoverButton = new Button { Text = "Выбрать...", Left = 230, Top = 270, Width = 80 };

            // Кнопки
            saveButton = new Button { Text = "Сохранить", Left = 320, Top = 370, Width = 80 };
            cancelButton = new Button { Text = "Отмена", Left = 220, Top = 370, Width = 80 };

            // Добавление элементов на форму
            this.Controls.AddRange(new Control [ ] {
                titleLabel, titleTextBox,
                yearLabel, yearNumericUpDown,
                genreLabel, genreComboBox,
                authorLabel, authorComboBox,
                descriptionLabel, descriptionTextBox,
                coverLabel, coverPictureBox, selectCoverButton,
                saveButton, cancelButton
            });

            // Обработчики событий
            selectCoverButton.Click += SelectCoverButton_Click;
            saveButton.Click += SaveButton_Click;
            cancelButton.Click += CancelButton_Click;
        }

        private void LoadGenres ()
        {
            genreComboBox.Items.Clear( );
            string query = "SELECT Название FROM Жанры ORDER BY Название";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            using (SQLiteDataReader reader = cmd.ExecuteReader( ))
            {
                while (reader.Read( ))
                {
                    genreComboBox.Items.Add(reader [ "Название" ].ToString( ));
                }
            }
        }

        private void LoadAuthors ()
        {
            authorComboBox.Items.Clear( );
            string query = "SELECT Имя FROM Авторы ORDER BY Имя";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            using (SQLiteDataReader reader = cmd.ExecuteReader( ))
            {
                while (reader.Read( ))
                {
                    authorComboBox.Items.Add(reader [ "Имя" ].ToString( ));
                }
            }
        }

        private void SelectCoverButton_Click (object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog( );
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog( ) == DialogResult.OK)
            {
                try
                {
                    coverPictureBox.Image = Image.FromFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                }
            }
        }

        private void SaveButton_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text) ||
                genreComboBox.SelectedItem == null ||
                authorComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните обязательные поля (Название, Жанр и Автор)");
                return;
            }

            string coverPath = "";
            if (coverPictureBox.Image != null)
            {
                // Здесь должна быть логика сохранения изображения
                // Для простоты просто запоминаем путь
                coverPath = "C:/Users/st310-05/Pictures/PhotoGallery/павел.jpg";
            }

            string query = @"
                INSERT INTO Книги (Название, Год_издания, Жанр, Автор, Описание, Обложка)
                VALUES (@title, @year, @genre, @author, @description, @cover)";

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@title", titleTextBox.Text);
            cmd.Parameters.AddWithValue("@year", (int) yearNumericUpDown.Value);
            cmd.Parameters.AddWithValue("@genre", genreComboBox.SelectedItem.ToString( ));
            cmd.Parameters.AddWithValue("@author", authorComboBox.SelectedItem.ToString( ));
            cmd.Parameters.AddWithValue("@description", descriptionTextBox.Text);
            cmd.Parameters.AddWithValue("@cover", coverPath);

            try
            {
                cmd.ExecuteNonQuery( );
                this.DialogResult = DialogResult.OK;
                this.Close( );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книги: {ex.Message}");
            }
        }

        private void CancelButton_Click (object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close( );
        }
    }
}
