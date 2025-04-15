using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Apteka
{
    public partial class Form1 :Form
    {
        private SQLiteConnection connection;
        private SQLiteDataAdapter adapter;
        private DataTable dataTable;

        public Form1 (SQLiteConnection conn, string user)
        {
            InitializeComponent( );
            connection = conn;
            string username = user;
            InitializeDatabase( );
            LoadGenres( );
            LoadAuthors( );
            LoadData( );
            booksDataGridView.SelectionChanged += booksDataGridView_SelectionChanged;

            // Проверка роли пользователя
            string role = GetUserRole(username);
            if (role == "Пользователь")
            {
                button1.Visible = false;
                editButton.Visible = false;
                deleteButton.Visible = false;
            }
        }


        private void InitializeDatabase ()
        {
            connection = new SQLiteConnection("Data Source=library.db;Version=3;");
            connection.Open( );
            CreateTables( );
            SeedDatabase( );
        }


        private void CreateTables ()
        {
            string query = @"
        CREATE TABLE IF NOT EXISTS Книги (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Название TEXT NOT NULL,
            Год_издания INTEGER NOT NULL,
            Жанр TEXT NOT NULL,
            Автор TEXT NOT NULL,
            Описание TEXT,
            Обложка TEXT
        );

        CREATE TABLE IF NOT EXISTS Жанры (
            Название TEXT PRIMARY KEY
        );

        CREATE TABLE IF NOT EXISTS Авторы (
            Имя TEXT PRIMARY KEY
        );

        CREATE TABLE IF NOT EXISTS Пользователи (
            Имя TEXT PRIMARY KEY,
            Пароль TEXT NOT NULL,
            Роль TEXT NOT NULL
        );
    ";

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery( );
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

        private void LoadData ()
        {
            string query = "SELECT ID, Название, Год_издания, Жанр, Автор, Описание, Обложка FROM Книги";
            adapter = new SQLiteDataAdapter(query, connection);
            dataTable = new DataTable( );
            adapter.Fill(dataTable);

            booksDataGridView.DataSource = dataTable;
            booksDataGridView.Columns [ "Описание" ].Visible = false; // Описание видимо
            booksDataGridView.Columns [ "Обложка" ].Visible = false;  // Обложка видима

            // Настраиваем отображение столбцов
            booksDataGridView.Columns [ "Название" ].Width = 200;
            booksDataGridView.Columns [ "Автор" ].Width = 150;
            booksDataGridView.Columns [ "Жанр" ].Width = 120;
            booksDataGridView.Columns [ "Год_издания" ].Width = 80;
        }

        private void booksDataGridView_SelectionChanged (object sender, EventArgs e)
        {
            // Проверяем, что есть выделенные строки и это не заголовок
            if (booksDataGridView.SelectedRows.Count > 0 && booksDataGridView.SelectedRows [ 0 ].Index >= 0)
            {
                DataGridViewRow selectedRow = booksDataGridView.SelectedRows [ 0 ];

                // Проверяем, что ячейка с описанием не null
                if (selectedRow.Cells [ "Описание" ].Value != null)
                {
                    descriptionRichTextBox.Text = selectedRow.Cells [ "Описание" ].Value.ToString( );
                }
                else
                {
                    descriptionRichTextBox.Text = "Описание отсутствует";
                }

                // Проверяем, что ячейка с обложкой не null
                if (selectedRow.Cells [ "Обложка" ].Value != null)
                {
                    string imagePath = selectedRow.Cells [ "Обложка" ].Value.ToString( );
                    LoadImage(imagePath);
                }
                else
                {
                    // Загружаем изображение по умолчанию
                    LoadImage(null);
                }
            }
        }

        private void LoadImage (string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    // Очищаем предыдущее изображение
                    if (coverPictureBox.Image != null)
                    {
                        coverPictureBox.Image.Dispose( );
                    }

                    // Загружаем новое изображение
                    coverPictureBox.Image = Image.FromFile(imagePath);
                }
                else
                {
                    // Путь к изображению по умолчанию
                    string defaultImagePath = Path.Combine(Application.StartupPath, "павел.jpg");

                    if (File.Exists(defaultImagePath))
                    {
                        coverPictureBox.Image = Image.FromFile(defaultImagePath);
                    }
                    else
                    {
                        coverPictureBox.Image = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                coverPictureBox.Image = null;
            }
        }

        private void searchButton_Click (object sender, EventArgs e)
        {
            string selectedGenre = genreComboBox.SelectedItem?.ToString( );
            string selectedAuthor = authorComboBox.SelectedItem?.ToString( );
            string searchText = searchTextBox.Text.Trim( );

            DataView dv = new DataView(dataTable);
            string filter = "";

            if (!string.IsNullOrEmpty(selectedGenre))
            {
                selectedGenre = selectedGenre.Replace("'", "''");
                filter += $"Жанр = '{selectedGenre}'";
            }

            if (!string.IsNullOrEmpty(selectedAuthor))
            {
                selectedAuthor = selectedAuthor.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"Автор = '{selectedAuthor}'";
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"Название LIKE '%{searchText}%'";
            }

            dv.RowFilter = filter;
            booksDataGridView.DataSource = dv;

            if (string.IsNullOrEmpty(filter))
            {
                booksDataGridView.DataSource = dataTable;
            }
        }

        private void SeedDatabase ()
        {
            string checkQuery = "SELECT COUNT(*) FROM Книги";
            SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection);
            long count = (long) checkCmd.ExecuteScalar( );

            if (count == 0)
            {
                string insertGenres = @"
            INSERT OR IGNORE INTO Жанры (Название) VALUES
            ('Фантастика'), ('Фэнтези'), ('Детектив'), ('Роман'),
            ('Научная литература'), ('Исторический роман'), ('Биография'), ('Поэзия');
        ";
                new SQLiteCommand(insertGenres, connection).ExecuteNonQuery( );

                string insertAuthors = @"
            INSERT OR IGNORE INTO Авторы (Имя) VALUES
            ('Айзек Азимов'), ('Джон Толкин'), ('Агата Кристи'), ('Лев Толстой'),
            ('Стивен Хокинг'), ('Борис Акунин'), ('Уолтер Айзексон'), ('Анна Ахматова');
        ";
                new SQLiteCommand(insertAuthors, connection).ExecuteNonQuery( );

                string defaultImagePath = Path.Combine(Application.StartupPath, "павел.jpg");

                string insertBooks = $@"
            INSERT INTO Книги (Название, Год_издания, Жанр, Автор, Описание, Обложка) VALUES
            ('Основание', 1951, 'Фантастика', 'Айзек Азимов', 'Классика научной фантастики', '{defaultImagePath}'),
            ('Властелин колец', 1954, 'Фэнтези', 'Джон Толкин', 'Эпическая трилогия', '{defaultImagePath}'),
            ('Убийство в Восточном экспрессе', 1934, 'Детектив', 'Агата Кристи', 'Знаменитый детектив', '{defaultImagePath}'),
            ('Война и мир', 1869, 'Роман', 'Лев Толстой', 'Русская классика', '{defaultImagePath}');
        ";
                new SQLiteCommand(insertBooks, connection).ExecuteNonQuery( );

                string insertUsers = @"
            INSERT OR IGNORE INTO Пользователи (Имя, Пароль, Роль) VALUES
            ('admin', 'admin', 'Админ'),
            ('user', 'user', 'Пользователь');
        ";
                new SQLiteCommand(insertUsers, connection).ExecuteNonQuery( );
            }
        }

        private void addBookButton_Click (object sender, EventArgs e)
        {

        }

        private void searchButton_Click_1 (object sender, EventArgs e)
        {
            string selectedGenre = genreComboBox.SelectedItem?.ToString( );
            string selectedAuthor = authorComboBox.SelectedItem?.ToString( );
            string searchText = searchTextBox.Text.Trim( );

            DataView dv = new DataView(dataTable);
            string filter = "";

            if (!string.IsNullOrEmpty(selectedGenre))
            {
                selectedGenre = selectedGenre.Replace("'", "''");
                filter += $"Жанр = '{selectedGenre}'";
            }

            if (!string.IsNullOrEmpty(selectedAuthor))
            {
                selectedAuthor = selectedAuthor.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"Автор = '{selectedAuthor}'";
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"Название LIKE '%{searchText}%'";
            }

            dv.RowFilter = filter;
            booksDataGridView.DataSource = dv;

            if (string.IsNullOrEmpty(filter))
            {
                booksDataGridView.DataSource = dataTable;
            }
        }

        private void booksDataGridView_CellContentClick (object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click (object sender, EventArgs e)
        {
            var form = new AddBookForm(connection);
            if (form.ShowDialog( ) == DialogResult.OK)
            {
                LoadGenres( );
                LoadAuthors( );
                LoadData( );
            }
        }
        private void EditBook ()
        {
            if (booksDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для редактирования");
                return;
            }

            DataGridViewRow selectedRow = booksDataGridView.SelectedRows [ 0 ];
            int bookId = Convert.ToInt32(selectedRow.Cells [ "ID" ].Value);

            var editForm = new AddBookForm(connection, bookId);
            if (editForm.ShowDialog( ) == DialogResult.OK)
            {
                LoadGenres( );
                LoadAuthors( );
                LoadData( );
            }
        }
        private void DeleteBook ()
        {
            if (booksDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для удаления");
                return;
            }

            DataGridViewRow selectedRow = booksDataGridView.SelectedRows [ 0 ];
            int bookId = Convert.ToInt32(selectedRow.Cells [ "ID" ].Value);
            string bookTitle = selectedRow.Cells [ "Название" ].Value.ToString( );

            if (MessageBox.Show($"Вы уверены, что хотите удалить книгу '{bookTitle}'?",
                "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string deleteQuery = "DELETE FROM Книги WHERE ID = @id";
                    using (SQLiteCommand cmd = new SQLiteCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", bookId);
                        cmd.ExecuteNonQuery( );
                    }

                    LoadData( );
                    MessageBox.Show("Книга успешно удалена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении книги: {ex.Message}");
                }
            }
        }

        private void editButton_Click (object sender, EventArgs e)
        {
            EditBook( );
        }

        private void deleteButton_Click (object sender, EventArgs e)
        {
            DeleteBook( );
        }
        private string GetUserRole (string username)
        {
            string query = "SELECT Роль FROM Пользователи WHERE Имя = @username";
            using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                return cmd.ExecuteScalar( )?.ToString( );
            }
        }
    }


}
