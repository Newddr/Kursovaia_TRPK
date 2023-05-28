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
            Program.OpenFormsCount++;
            this.id = id;
            label2.Text = $"Договор № {id}";
            MakeContract();
            
        }
        private void MakeContract()
        {
            ClassLogic classLogic = new ClassLogic();
            label1.Text = classLogic.makeContract(id);
        
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 class1 = new Class1();
            String[] s = class1.GetInfoForDocument(id);
            Form2 form = new Form2(Convert.ToInt32(s[8]));
            form.button_1.Visible = false;
            form.button_2.Visible = false;
            form.button_3.Visible = false;
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Class1 class1 = new Class1();
            String[] s = class1.GetInfoForDocument(id);
            if (DateTime.Today > Convert.ToDateTime(s[6]))
            {
                
                var days = DateTime.Today.Subtract(Convert.ToDateTime(s[6])).Days;
                var addCost = Convert.ToInt32(s[7]) * days;
                DialogResult result = MessageBox.Show($"Дата возврата не совпадает с датой указанной в договоре. Просрочка составляет {days} дней.Взимите дополнительную плату в размере {addCost} рублей", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    
                    class1.changeStatusContract(id);
                    Form1 form = new Form1(0);
                    form.Show();
                    this.Hide();
                }
                else
                {
                    // Ничего не делаем, если пользователь нажал "Нет" или закрыл диалоговое окно
                }
            }
            else
            {

               
                class1.changeStatusContract(id);
                Program.OpenFormsCount--;
                Form1 form = new Form1(0);
                    form.Show();
                    this.Hide();
                

            }


        }
       

        private void button4_Click(object sender, EventArgs e)
        {
            Program.OpenFormsCount--;
            Form1 form = new Form1(1);
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.OpenFormsCount--;
            Form1 form = new Form1(0);
            form.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
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
