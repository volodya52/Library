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
    public partial class RegistrationForm :Form
    {
        private SQLiteConnection connection;

        public RegistrationForm ()
        {
            InitializeComponent( );
            InitializeDatabase( );
            
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

        private void RegisterButton_Click (object sender, EventArgs e)
        {
            string username = usernameTextBox.Text.Trim( );
            string password = passwordTextBox.Text.Trim( );
            string role = roleComboBox.SelectedItem?.ToString( );

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }

            try
            {
                string insertQuery = "INSERT INTO Пользователи (Имя, Пароль, Роль) VALUES (@username, @password, @role)";
                using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery( );
                }

                MessageBox.Show("Регистрация успешна");
                OpenMainForm(username);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }
        private void OpenMainForm (string username)
        {
            this.Hide( );
            var mainForm = new Form1(connection, username);
            mainForm.ShowDialog( );
            this.Close( );
        }

    }
}
