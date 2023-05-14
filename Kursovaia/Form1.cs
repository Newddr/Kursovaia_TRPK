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
    public partial class Form1 : Form
    {
        public Form1(int task)
        {

            InitializeComponent();
            

            this.task= task;
        }
        int task=0;
        string sort = "";
        string filter = "";
        string connectionString = "Data Source=databaseCompany.db;";
        private void Form1_Load(object sender, EventArgs e)
        {

            if (task == 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }
            else
            {
                comboBox3.SelectedIndex = 0;
                comboBox4.SelectedIndex = 0;
            }
            

        }
        private void LoadNotebooks()
        {
            List<FlowLayoutPanel> elements = new List<FlowLayoutPanel>();
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string sql = $"SELECT name,image,cost,description,id FROM notebooks {filter} {sort} ";
            // Создание объекта SQLiteCommand
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                // Выполнение запроса
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Обработка результата запроса
                    while (reader.Read())
                    {
                        
                        
                        PictureBox pictureBox = new PictureBox(); // Создание нового PictureBox
                        pictureBox.Image = Image.FromFile(reader.GetString(1));
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        Label name = new Label();
                        name.Padding = new Padding(0, 20, 0, 0);
                        name.Height= pictureBox.Height;
                        name.Text = reader.GetString(0);
                        Label description = new Label();

                        description.Width = 550;
                        description.Height = 140;
                        description.Text = reader.GetString(3);
                        Label cost = new Label();
                        cost.Text = reader.GetInt32(2).ToString() + " р/день";
                        FlowLayoutPanel element = new FlowLayoutPanel();
                        
                        element.Controls.Add(pictureBox);
                        element.Controls.Add(name);
                        element.Controls.Add(description);
                        element.Controls.Add(cost);
                        element.Dock = DockStyle.Top;

                        int id = reader.GetInt32(4);
                        name.Click += (sender, e) =>
                        {
                            panel_Notebook_Click(sender, e, id);
                        };
                        pictureBox.Click += (sender, e) =>
                        {
                            panel_Notebook_Click(sender, e, id);
                        };
                        description.Click += (sender, e) =>
                        {
                            panel_Notebook_Click(sender, e,id);
                        };
                        cost.Click += (sender, e) =>
                        {
                            panel_Notebook_Click(sender, e, id);
                        };
                        element.Click += (sender, e) =>
                        {
                            panel_Notebook_Click(sender, e, id);
                        };
                        panel1.Controls.Add(element);
                        elements.Add(element);
                    }
                }
            }
            connection.Close();
        }
        private void LoadContract()
        {
            List<FlowLayoutPanel> elements = new List<FlowLayoutPanel>();
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string sql = $"SELECT id,idNotebook,startDate,endDate,cost,status FROM contracts {filter} {sort}";
            // Создание объекта SQLiteCommand
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                // Выполнение запроса
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Обработка результата запроса
                    while (reader.Read())
                    {
                        Label contractId = new Label();
                        contractId.Text="№ "+ reader.GetInt32(0).ToString();
                        Label name = new Label();
                        using (SQLiteCommand command1 = new SQLiteCommand($"SELECT name FROM notebooks WHERE id={reader.GetInt32(1)}", connection)) //получаем название ноутбука из таблицы Ноутбуки по его id
                        {
                            using (SQLiteDataReader reader1 = command1.ExecuteReader())
                            {
                                while (reader1.Read())
                                {
                                    name.Text = reader1.GetString(0);
                                }
                            }
                        }
                        Label date = new Label();
                        date.Text = reader.GetString(2)+" - "+ reader.GetString(3);
                        Label cost = new Label();
                        cost.Text = reader.GetInt32(4).ToString() + " р.";
                        Label status = new Label();
                        if (reader.GetInt32(5) == 1) status.Text = "Выполнен";
                        else status.Text = "Не выполнен";
                        
                        FlowLayoutPanel element = new FlowLayoutPanel();
                        element.Controls.Add(contractId);
                        element.Controls.Add(name);
                        element.Controls.Add(date);
                        element.Controls.Add(cost);
                        element.Controls.Add(status);
                        element.Dock = DockStyle.Top;
                        panel1.Controls.Add(element);
                        elements.Add(element);
                        int id = reader.GetInt32(0);
                        cost.Click += (sender, e) =>
                        {
                            panel_Document_Click(sender, e, id);
                        };
                    }
                }
            }
            connection.Close();
        }
        private void panel_Notebook_Click(object sender, EventArgs e,int id)
        {
            
            Form2 form2 = new Form2(id); // создание новой формы
            form2.Show();
            this.Hide();
        }
        private void panel_Document_Click(object sender, EventArgs e, int id)
        {

            Form4 form4 = new Form4(id); // создание новой формы
            form4.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sort = "";
            filter = "";
            comboBox1.Visible = true;
            comboBox2.Visible = true;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            panel1.Controls.Clear();
            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sort = "";
            filter = "";
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = true;
            comboBox4.Visible = true;
            panel1.Controls.Clear();
            comboBox3.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
            comboBox2.Visible = true;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            if (comboBox1.SelectedItem == "Цена") sort = "ORDER BY cost DESC";
            else sort = "ORDER BY name DESC";
            while (panel1.Controls.Count > 0)
            {
                panel1.Controls[0].Dispose();
            }
            LoadNotebooks();
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox2.SelectedItem)
            {
                case "Все":
                    filter ="";
                    break;
                case "Игровой":
                    filter = "WHERE type = 'Игровой'";
                    break;
                case "Для офиса":
                    filter = "WHERE type = 'Офисный'";
                    break;
                case "Видео монтаж":
                    filter = "WHERE type = 'Видео'";
                    break;
                default: filter = "";
                    break;

            }
            while (panel1.Controls.Count > 0)
            {
                panel1.Controls[0].Dispose();
            }
            LoadNotebooks();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = true;
            comboBox4.Visible = true;
            switch (comboBox3.SelectedItem)
            {
                case "Все":
                    filter = "";
                    break;
                case "Завершенные":
                    filter = "WHERE status = 1";
                    break;
                case "Активные":
                    filter = "WHERE status =0";
                    break;
                default: filter = "";
                    break;

            }
            while (panel1.Controls.Count > 0)
            {
                panel1.Controls[0].Dispose();
            }
            LoadContract();

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.SelectedItem)
            {
                case "Номер договора":
                    sort = "ORDER BY id DESC";
                    break;
                case "Дата":
                    sort = "ORDER BY startDate DESC";
                    break;
                case "Цена":
                    sort = "ORDER BY cost";
                    break;
                default: sort = "ORDER BY id DESC";
                    break;
            }
            while (panel1.Controls.Count > 0)
            {
                panel1.Controls[0].Dispose();
            }
            LoadContract();
        }
    }
}
