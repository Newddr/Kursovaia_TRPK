using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Printing;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Kursovaia
{
    public partial class Form4 : Form
    {
        string connectionString = "Data Source=databaseCompany.db;";
        int id = 0;
        int idNotebook = 0;
        String[] s= null;
        DateTime dateEnd;
        int costPerDay = 0;
        public Form4(int id)
        {
            InitializeComponent();
            this.id = id;
            label2.Text = $"Договор № {id}";
            MakeContract();
            
        }
        private void MakeContract()
        {
            s = new string[6];
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string sql = $"SELECT startDate,endDate,cost,fioClient,idNotebook,status FROM contracts where id={id}";
            // Создание объекта SQLiteCommand
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                // Выполнение запроса
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Обработка результата запроса
                    while (reader.Read())
                    {
                        s[0] = reader.GetString(0);
                        s[1] = reader.GetString(3);
                        s[3] = (Convert.ToDateTime(reader.GetString(1))- Convert.ToDateTime(reader.GetString(0))).ToString();
                        s[4] = reader.GetInt32(2).ToString();
                        s[5] = s[3];
                        dateEnd = Convert.ToDateTime(reader.GetString(1));

                        idNotebook = reader.GetInt32(4);
                        using (SQLiteCommand command1 = new SQLiteCommand($"SELECT name,cost FROM notebooks WHERE id={idNotebook}", connection)) //получаем название ноутбука из таблицы Ноутбуки по его id
                        {
                            using (SQLiteDataReader reader1 = command1.ExecuteReader())
                            {
                                while (reader1.Read())
                                {
                                    s[2] = reader1.GetString(0);
                                    costPerDay = reader1.GetInt32(1);
                                }
                            }
                        }

                    }
                }
            }
            string filePath = "document1.txt";

            // Прочитайте текст из файла
            string text = File.ReadAllText(filePath);
            int replaceIndex = 0;
            int index = 0;

            while ((replaceIndex = text.IndexOf("?!?", replaceIndex)) != -1)
            {
                text = text.Remove(replaceIndex, 3);
                text = text.Insert(replaceIndex, s[index]);
                index++;
            }
            label1.Text = text;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(idNotebook);
            form.button_1.Visible = false;
            form.button_2.Visible = false;
            form.button_3.Visible = false;
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(DateTime.Today>dateEnd)
            {
                var days = DateTime.Today.Subtract(dateEnd).Days;
                var addCost = costPerDay * days;
                DialogResult result = MessageBox.Show($"Дата возврата не совпадает с датой указанной в договоре. Просрочка составляет {days} дней.Взимите дополнительную плату в размере {addCost} рублей", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    changeStatusContract();
                    Form1 form = new Form1(0);
                    form.Show();
                    this.Hide();
                }
                else
                {
                    // Ничего не делаем, если пользователь нажал "Нет" или закрыл диалоговое окно
                }
            }
           

        }
        private void changeStatusContract()
        {
            
            
            string query = $"UPDATE contracts SET status=1 WHERE id={id}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1(1);
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1(0);
            form.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
