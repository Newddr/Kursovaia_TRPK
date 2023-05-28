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
using static System.Net.WebRequestMethods;

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
        int id = 0;
        public Form2(int id)
        {
            InitializeComponent();
            Program.OpenFormsCount++;
            this.id= id;
            LoadInfo();
            
        }
        

        private void LoadInfo()
        {
            Class1 class1 = new Class1();
            String[] s = class1.GetFullNotebookInfo(id);
            name.Text= s[0];
            image.Image = Image.FromFile(s[1]);
            image.SizeMode = PictureBoxSizeMode.Zoom;
            type.Text= s[2];
            color.Text= s[3];
            cost.Text = s[4];
            comments.Text = s[5];
            description.Text = s[6];
            proc.Text = s[7];
            videocard.Text = s[8];
            oc.Text = s[9];
            screen.Text = s[10];
            opmemory.Text = s[11];
            storage.Text = s[12];
            batteryStorage.Text = s[13];
            touchpadAndKeyboard.Text = s[14];
            ports.Text = s[15];
            producer.Text = s[16];
            size.Text = s[17];
                            

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

        private void comments_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
