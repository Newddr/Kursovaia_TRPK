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
                        proc.Text= reader.GetString(5);
                        videocard.Text= reader.GetString(6);
                        oc.Text= reader.GetString(7);
                        screen.Text= reader.GetString(8);
                        opmemory.Text= reader.GetInt32(9).ToString();
                        storage.Text= reader.GetString(10);
                        batteryStorage.Text= reader.GetDouble(11).ToString();
                        touchpadAndKeyboard.Text= reader.GetString(12) + ", "+ reader.GetString(13);
                        ports.Text= reader.GetString(14);
                        producer.Text= reader.GetString(15);
                        size.Text= reader.GetString(16);
                        cost.Text= reader.GetInt32(17).ToString();
                        comments.Text= reader.GetString(18);
                        description.Text= reader.GetString(19);

                    }
                }
            }
            connection.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(0); // создание новой формы
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(1); // создание новой формы
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(name.Text,Convert.ToInt32(cost.Text),id); // создание новой формы
            form3.Show();
            this.Hide();
        }

        
    }
}
