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

            // �������� ���� ������������
            string role = GetUserRole(username);
            if (role == "������������")
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
        CREATE TABLE IF NOT EXISTS ����� (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            �������� TEXT NOT NULL,
            ���_������� INTEGER NOT NULL,
            ���� TEXT NOT NULL,
            ����� TEXT NOT NULL,
            �������� TEXT,
            ������� TEXT
        );

        CREATE TABLE IF NOT EXISTS ����� (
            �������� TEXT PRIMARY KEY
        );

        CREATE TABLE IF NOT EXISTS ������ (
            ��� TEXT PRIMARY KEY
        );

        CREATE TABLE IF NOT EXISTS ������������ (
            ��� TEXT PRIMARY KEY,
            ������ TEXT NOT NULL,
            ���� TEXT NOT NULL
        );
    ";

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery( );
        }

        private void LoadGenres ()
        {
            genreComboBox.Items.Clear( );
            string query = "SELECT �������� FROM ����� ORDER BY ��������";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            using (SQLiteDataReader reader = cmd.ExecuteReader( ))
            {
                while (reader.Read( ))
                {
                    genreComboBox.Items.Add(reader [ "��������" ].ToString( ));
                }
            }
        }

        private void LoadAuthors ()
        {
            authorComboBox.Items.Clear( );
            string query = "SELECT ��� FROM ������ ORDER BY ���";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            using (SQLiteDataReader reader = cmd.ExecuteReader( ))
            {
                while (reader.Read( ))
                {
                    authorComboBox.Items.Add(reader [ "���" ].ToString( ));
                }
            }
        }

        private void LoadData ()
        {
            string query = "SELECT ID, ��������, ���_�������, ����, �����, ��������, ������� FROM �����";
            adapter = new SQLiteDataAdapter(query, connection);
            dataTable = new DataTable( );
            adapter.Fill(dataTable);

            booksDataGridView.DataSource = dataTable;
            booksDataGridView.Columns [ "��������" ].Visible = false; // �������� ������
            booksDataGridView.Columns [ "�������" ].Visible = false;  // ������� ������

            // ����������� ����������� ��������
            booksDataGridView.Columns [ "��������" ].Width = 200;
            booksDataGridView.Columns [ "�����" ].Width = 150;
            booksDataGridView.Columns [ "����" ].Width = 120;
            booksDataGridView.Columns [ "���_�������" ].Width = 80;
        }

        private void booksDataGridView_SelectionChanged (object sender, EventArgs e)
        {
            // ���������, ��� ���� ���������� ������ � ��� �� ���������
            if (booksDataGridView.SelectedRows.Count > 0 && booksDataGridView.SelectedRows [ 0 ].Index >= 0)
            {
                DataGridViewRow selectedRow = booksDataGridView.SelectedRows [ 0 ];

                // ���������, ��� ������ � ��������� �� null
                if (selectedRow.Cells [ "��������" ].Value != null)
                {
                    descriptionRichTextBox.Text = selectedRow.Cells [ "��������" ].Value.ToString( );
                }
                else
                {
                    descriptionRichTextBox.Text = "�������� �����������";
                }

                // ���������, ��� ������ � �������� �� null
                if (selectedRow.Cells [ "�������" ].Value != null)
                {
                    string imagePath = selectedRow.Cells [ "�������" ].Value.ToString( );
                    LoadImage(imagePath);
                }
                else
                {
                    // ��������� ����������� �� ���������
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
                    // ������� ���������� �����������
                    if (coverPictureBox.Image != null)
                    {
                        coverPictureBox.Image.Dispose( );
                    }

                    // ��������� ����� �����������
                    coverPictureBox.Image = Image.FromFile(imagePath);
                }
                else
                {
                    // ���� � ����������� �� ���������
                    string defaultImagePath = Path.Combine(Application.StartupPath, "�����.jpg");

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
                MessageBox.Show($"������ �������� �����������: {ex.Message}");
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
                filter += $"���� = '{selectedGenre}'";
            }

            if (!string.IsNullOrEmpty(selectedAuthor))
            {
                selectedAuthor = selectedAuthor.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"����� = '{selectedAuthor}'";
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"�������� LIKE '%{searchText}%'";
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
            string checkQuery = "SELECT COUNT(*) FROM �����";
            SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection);
            long count = (long) checkCmd.ExecuteScalar( );

            if (count == 0)
            {
                string insertGenres = @"
            INSERT OR IGNORE INTO ����� (��������) VALUES
            ('����������'), ('�������'), ('��������'), ('�����'),
            ('������� ����������'), ('������������ �����'), ('���������'), ('������');
        ";
                new SQLiteCommand(insertGenres, connection).ExecuteNonQuery( );

                string insertAuthors = @"
            INSERT OR IGNORE INTO ������ (���) VALUES
            ('����� ������'), ('���� ������'), ('����� ������'), ('��� �������'),
            ('������ ������'), ('����� ������'), ('������ ��������'), ('���� ��������');
        ";
                new SQLiteCommand(insertAuthors, connection).ExecuteNonQuery( );

                string defaultImagePath = Path.Combine(Application.StartupPath, "�����.jpg");

                string insertBooks = $@"
            INSERT INTO ����� (��������, ���_�������, ����, �����, ��������, �������) VALUES
            ('���������', 1951, '����������', '����� ������', '�������� ������� ����������', '{defaultImagePath}'),
            ('��������� �����', 1954, '�������', '���� ������', '��������� ��������', '{defaultImagePath}'),
            ('�������� � ��������� ���������', 1934, '��������', '����� ������', '���������� ��������', '{defaultImagePath}'),
            ('����� � ���', 1869, '�����', '��� �������', '������� ��������', '{defaultImagePath}');
        ";
                new SQLiteCommand(insertBooks, connection).ExecuteNonQuery( );

                string insertUsers = @"
            INSERT OR IGNORE INTO ������������ (���, ������, ����) VALUES
            ('admin', 'admin', '�����'),
            ('user', 'user', '������������');
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
                filter += $"���� = '{selectedGenre}'";
            }

            if (!string.IsNullOrEmpty(selectedAuthor))
            {
                selectedAuthor = selectedAuthor.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"����� = '{selectedAuthor}'";
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.Replace("'", "''");
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"�������� LIKE '%{searchText}%'";
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
                MessageBox.Show("�������� ����� ��� ��������������");
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
                MessageBox.Show("�������� ����� ��� ��������");
                return;
            }

            DataGridViewRow selectedRow = booksDataGridView.SelectedRows [ 0 ];
            int bookId = Convert.ToInt32(selectedRow.Cells [ "ID" ].Value);
            string bookTitle = selectedRow.Cells [ "��������" ].Value.ToString( );

            if (MessageBox.Show($"�� �������, ��� ������ ������� ����� '{bookTitle}'?",
                "������������� ��������", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string deleteQuery = "DELETE FROM ����� WHERE ID = @id";
                    using (SQLiteCommand cmd = new SQLiteCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", bookId);
                        cmd.ExecuteNonQuery( );
                    }

                    LoadData( );
                    MessageBox.Show("����� ������� �������");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"������ ��� �������� �����: {ex.Message}");
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
            string query = "SELECT ���� FROM ������������ WHERE ��� = @username";
            using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                return cmd.ExecuteScalar( )?.ToString( );
            }
        }
    }


}
