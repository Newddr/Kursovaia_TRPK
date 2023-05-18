using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Kursovaia
{
    public partial class Form2 : Form
    {
        public System.Windows.Forms.Button button_1
        {
            get { return button1; }
        }
        public System.Windows.Forms.Button button_2
        {
            get { return button2; }
        }
        public System.Windows.Forms.Button button_3
        {
            get { return button3; }
        }
        public Form2(int id)
        {
            InitializeComponent();
            Program.OpenFormsCount++;
            this.id= id;
            LoadInfo();
            
        }
        int id=0;

        private void LoadInfo()
        {
            string connectionString = "Data Source=databaseCompany.db;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string sql = $"SELECT * FROM notebooks WHERE id={id}";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                // Выполнение запроса
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Обработка результата запроса
                    while (reader.Read())
                    {
                        name.Text= reader.GetString(1);
                        image.Image = Image.FromFile(reader.GetString(2));
                        image.SizeMode = PictureBoxSizeMode.Zoom;
                        type.Text= reader.GetString(3);
                        color.Text= reader.GetString(4);
                        cost.Text = reader.GetInt32(7).ToString();
                        comments.Text = reader.GetString(5);
                        description.Text = reader.GetString(6);
                        string sql1 = $"SELECT * FROM model WHERE id={id}";
                        using (SQLiteCommand command1 = new SQLiteCommand(sql1, connection))
                        {
                            // Выполнение запроса
                            using (SQLiteDataReader reader1 = command1.ExecuteReader())
                            {
                                // Обработка результата запроса
                                while (reader1.Read())
                                {
                                    proc.Text = reader1.GetString(1);
                                    videocard.Text = reader1.GetString(2);
                                    oc.Text = reader1.GetString(3);
                                    screen.Text = reader1.GetString(4);
                                    opmemory.Text = reader1.GetInt32(5).ToString();
                                    storage.Text = reader1.GetString(6);
                                    batteryStorage.Text = reader1.GetDouble(7).ToString();
                                    touchpadAndKeyboard.Text = reader1.GetString(8) + ", " + reader1.GetString(9);
                                    ports.Text = reader1.GetString(10);
                                    producer.Text = reader1.GetString(11);
                                    size.Text = reader1.GetString(12);
                                }
                            }
                        }
                        

                    }
                }
            }
            connection.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Program.OpenFormsCount--;
            Form1 form1 = new Form1(0); // создание новой формы
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.OpenFormsCount--;
            Form1 form1 = new Form1(1); // создание новой формы
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.OpenFormsCount--;
            Form3 form3 = new Form3(name.Text,Convert.ToInt32(cost.Text),id); // создание новой формы
            form3.Show();
            this.Hide();
        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.OpenFormsCount--;

            // Проверяем, если все формы закрыты, то завершаем работу приложения
            if (Program.OpenFormsCount == 0)
            {
                Application.Exit();
            }
        }

        private void comments_Leave(object sender, EventArgs e)
        {
            string connectionString = "Data Source=databaseCompany.db;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string text = comments.Text;
            string query = $"UPDATE Notebooks SET comments = '{text}' WHERE id={id};";

            SQLiteCommand command = new SQLiteCommand(query, connection);
            

            // Выполняем команду вставки новой строки
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
