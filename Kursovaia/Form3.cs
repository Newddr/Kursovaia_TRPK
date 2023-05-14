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
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Kursovaia
{
    public partial class Form3 : Form
    {
        private PrintDocument printDocument1 = new PrintDocument();
        string model = "";
        int cost = 0;
        int idNotebook = 0;
        int lastId = 0;
        String[] s = new string[6];
        public Form3(string model,int cost,int idNotebook)
        {
            InitializeComponent();
            this.model = model;
            this.cost = cost;
            this.idNotebook = idNotebook;
            // настройки печати
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage); // подписываемся на событие печати
            printDocument1.DocumentName = "Print Text"; // имя документа
            printDocument1.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50); // отступы страницы

            // настройки кнопки
            button1.Text = "Составить договор"; // название кнопки
            //MakeContract();
        }
        private String MakeContract(String[] s)
        {
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
            return text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || maskedTextBox1.Text == "" || maskedTextBox2.Text == "" || maskedTextBox3.Text == "" || maskedTextBox4.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Не оставляйте поля незаполненными", "Предупреждение", MessageBoxButtons.YesNo);
            }
            else
            {
                if (MessageBox.Show("Введенные данные верны и клиент согласен с условиями договора?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    PrintDialog printDialog1 = new PrintDialog();
                    printDialog1.Document = printDocument1;
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        // печатаем текст
                        printDocument1.Print();
                        MakeDocument();
                        // сохранение договора
                        //string path = $"C:\\Users\\Федор\\source\\repos\\Kursovaia\\Kursovaia\\bin\\Debug\\Documents\\Document{lastId}.txt";
                        //string content = MakeContract(s);
                        //File.WriteAllText(path, content);

                        Form1 form = new Form1(0);
                        form.Show();
                        this.Hide();
                    }
                }
            }
            
            
        }
        private void MakeDocument()
        {
            string connectionString = "Data Source=databaseCompany.db;";


            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            // Создаем команду для получения последнего id
            string query = "SELECT MAX(id) FROM contracts";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                // Выполняем запрос и получаем последний id
                lastId = Convert.ToInt32(command.ExecuteScalar());

                // Создаем команду для вставки новой строки с id+1
                query = "INSERT INTO contracts (id,startDate,endDate,cost,fioClient,passportClient,emailClient,phoneNumberClient,idNotebook,status) VALUES (@id,@startDate,@endDate,@cost,@fioClient,@passportClient,@emailClient,@phoneNumberClient,@idNotebook,@status)";
                command = new SQLiteCommand(query, connection);

                // Устанавливаем параметры для новой строки
                command.Parameters.AddWithValue("@id", lastId + 1);
                command.Parameters.AddWithValue("@startDate", s[0]);
                command.Parameters.AddWithValue("@endDate", s[3]);
                command.Parameters.AddWithValue("@cost", Convert.ToInt32(s[4])* Convert.ToInt32(s[5]));
                command.Parameters.AddWithValue("@fioClient", s[1]);
                command.Parameters.AddWithValue("@passportClient", maskedTextBox2.Text+maskedTextBox3.Text);
                command.Parameters.AddWithValue("@emailClient",textBox6.Text);
                command.Parameters.AddWithValue("@phoneNumberClient",maskedTextBox4.Text );
                command.Parameters.AddWithValue("@idNotebook", idNotebook);
                command.Parameters.AddWithValue("@status", 0);

                // Выполняем команду вставки новой строки
                command.ExecuteNonQuery();
            connection.Close();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int days = Convert.ToInt32(textBox2.Text);
            s= new string[6]
            {
                DateTime.Today.ToString("dd/MM/yyyy"),// сегодняшняя дата 
                textBox1.Text.ToString(),// фио клиента
                model, // модель ноутбука
                DateTime.Today.AddDays(days).ToString("dd/MM/yyyy"), // дата окончания
                cost.ToString(), // стоимость
                days.ToString() // количество дне аренды

            };
            // печатаем текст
            string text = MakeContract(s);
            Font font = new Font("Arial", 12); // шрифт текста
            e.Graphics.DrawString(text, font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top); // выводим текст на страницу
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8) // проверяем, является ли нажатая клавиша цифрой или клавишей Backspace
            {
                e.Handled = true; // если нет, то отменяем ее обработку
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8) // проверяем, является ли нажатая клавиша цифрой или клавишей Backspace
            {
                e.Handled = true; // если нет, то отменяем ее обработку
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8) // проверяем, является ли нажатая клавиша цифрой или клавишей Backspace
            {
                e.Handled = true; // если нет, то отменяем ее обработку
            }
        }

        private void Ljuj(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1(0);
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1(1);
            form.Show();
            this.Hide();
        }

        

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '+') // проверяем, является ли нажатая клавиша цифрой или клавишей Backspace
            {
                e.Handled = true; // если нет, то отменяем ее обработку
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {


        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // чтобы не было звука при нажатии Enter
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // чтобы не было звука при нажатии Enter
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void maskedTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // чтобы не было звука при нажатии Enter
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void maskedTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // чтобы не было звука при нажатии Enter
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void maskedTextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // чтобы не было звука при нажатии Enter
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // чтобы не было звука при нажатии Enter
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // чтобы не было звука при нажатии Enter
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
