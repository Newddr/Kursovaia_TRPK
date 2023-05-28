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
            Program.OpenFormsCount++;

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
            this.Text = "Список ноутбуков";
            List<FlowLayoutPanel> elements = new List<FlowLayoutPanel>();
            
                    Class1 class1= new Class1();
                    List<String[]> param = class1.GetInfoFromBD("notebooks", filter, sort);
                    // Обработка результата запроса
                    foreach (String[] s in param)
                    {
                        
                        
                        PictureBox pictureBox = new PictureBox(); // Создание нового PictureBox
                        pictureBox.Image = Image.FromFile(s[2]);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        Label name = new Label();
                        name.Padding = new Padding(0, 20, 0, 0);
                        name.Height= pictureBox.Height;
                        name.Text = s[1];
                        Label description = new Label();

                        description.Width = 550;
                        description.Height = 140;
                        description.Text =s[4];
                        Label cost = new Label();
                        cost.Text = s[5] + " р/день";
                        FlowLayoutPanel element = new FlowLayoutPanel();
                        
                        element.Controls.Add(pictureBox);
                        element.Controls.Add(name);
                        element.Controls.Add(description);
                        element.Controls.Add(cost);
                        element.Dock = DockStyle.Top;

                        int id = Convert.ToInt32(s[0]);
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

        private void LoadContract()
        {
            this.Text = "Договоры";
            List<FlowLayoutPanel> elements = new List<FlowLayoutPanel>();
                    Class1 class1 = new Class1();
                    List<String[]> param = class1.GetInfoFromBD("contracts", filter, sort);
                    // Обработка результата запроса
                    foreach (String[] s in param)
                    {
                        Label contractId = new Label();
                        contractId.Text="№ "+ s[0];
                        Label name = new Label();
                        name.Text = s[5];
                        Label date = new Label();
                        date.AutoSize = true;
                        date.Text =s[1] +" - "+ s[2];
                        Label cost = new Label();
                        cost.Text = s[3] + " р.";
                        Label status = new Label();
                        status.Text = s[4];
                        
                        
                        FlowLayoutPanel element = new FlowLayoutPanel();
                        element.Controls.Add(contractId);
                        element.Controls.Add(name);
                        element.Controls.Add(date);
                        element.Controls.Add(cost);
                        element.Controls.Add(status);
                        element.Dock = DockStyle.Top;
                        panel1.Controls.Add(element);
                        elements.Add(element);
                        int id = Convert.ToInt32(s[0]);
                        cost.Click += (sender, e) =>
                        {
                            panel_Document_Click(sender, e, id);
                        };
                        contractId.Click += (sender, e) =>
                        {
                            panel_Document_Click(sender, e, id);
                        };
                        name.Click += (sender, e) =>
                        {
                            panel_Document_Click(sender, e, id);
                        };
                        date.Click += (sender, e) =>
                        {
                            panel_Document_Click(sender, e, id);
                        };
                        status.Click += (sender, e) =>
                        {
                            panel_Document_Click(sender, e, id);
                        };
                    }
        }
        private void panel_Notebook_Click(object sender, EventArgs e,int id)
        {
            Program.OpenFormsCount--;
            Form2 form2 = new Form2(id); // создание новой формы
            form2.Show();
            this.Hide();
        }
        private void panel_Document_Click(object sender, EventArgs e, int id)
        {
            Program.OpenFormsCount--;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.OpenFormsCount--;

            // Проверяем, если все формы закрыты, то завершаем работу приложения
            if (Program.OpenFormsCount == 0)
            {
                Application.Exit();
            }
        }
    }
}
