using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Drawing;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Kursovaia
{
    internal class Class1
    { private string connectionString = "Data Source=databaseCompany.db;";
        public List<String[]> GetInfoFromBD(string table,string filter,string sort)
        {
            List<String[]> param = new List<String[]>();
           

            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string sql = $"SELECT * FROM {table} {filter} {sort}";
            // Создание объекта SQLiteCommand
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                
                
                // Выполнение запроса
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Обработка результата запроса
                    while (reader.Read())
                    {
                        String[] s = new String[10];
                        if (table == "contracts")
                        {
                            s[0] = reader.GetInt32(0).ToString();
                            s[1] = reader.GetString(1);
                            s[2] = reader.GetString(2);
                            s[3] = reader.GetInt32(3).ToString();
                            s[4] = reader.GetInt32(4).ToString();
                            
                            using (SQLiteCommand command1 = new SQLiteCommand($"SELECT name FROM notebooks WHERE id={reader.GetInt32(5)}", connection)) //получаем название ноутбука из таблицы Ноутбуки по его id
                            {
                                using (SQLiteDataReader reader1 = command1.ExecuteReader())
                                {
                                    while (reader1.Read())
                                    {
                                        s[5] = reader1.GetString(0);
                                    }
                                }
                            }
                            if (s[4] == "1") s[4] = "Завершен";
                            else s[4] = "Активен";
                            param.Add(s);
                        }
                        else
                        {
                            s[0] = reader.GetInt32(0).ToString();
                            s[1] = reader.GetString(1);
                            s[2] = reader.GetString(2);
                            s[3] = reader.GetString(5);
                            s[4] = reader.GetString(6);
                            s[5] = reader.GetInt32(7).ToString();
                            param.Add(s);
                        }

                        
                    }
                }
            }
            connection.Close();
            return param;

        }
        public String[] GetFullNotebookInfo(int id)
        {
            String[] s = new String[18];
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
                        
                        s[0] = reader.GetString(1);
                        s[1] = reader.GetString(2);
                        s[2] = reader.GetString(3);
                        s[3] = reader.GetString(4);
                        s[4] = reader.GetInt32(7).ToString();
                        s[5] = reader.GetString(5);
                        s[6] = reader.GetString(6);
                        string sql1 = $"SELECT * FROM model WHERE id={id}";
                        using (SQLiteCommand command1 = new SQLiteCommand(sql1, connection))
                        {
                            // Выполнение запроса
                            using (SQLiteDataReader reader1 = command1.ExecuteReader())
                            {
                                // Обработка результата запроса
                                while (reader1.Read())
                                {
                                    s[7] = reader1.GetString(1);
                                    s[8] = reader1.GetString(2);
                                    s[9] = reader1.GetString(3);
                                    s[10] = reader1.GetString(4);
                                    s[11] = reader1.GetInt32(5).ToString();
                                    s[12] = reader1.GetString(6);
                                    s[13] = reader1.GetDouble(7).ToString();
                                    s[14] = reader1.GetString(8) + ", " + reader1.GetString(9);
                                    s[15] = reader1.GetString(10);
                                    s[16] = reader1.GetString(11);
                                    s[17] = reader1.GetString(12);
                                }
                            }
                        }


                    }
                }
            }
            connection.Close();
            return s;
        }
        public String[] GetInfoForDocument(int id)
        {
            String[] s = new String[9];
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string sql = $"SELECT startDate,endDate,cost,idNotebook,status FROM contracts where id={id}";
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

                        s[3] = (Convert.ToDateTime(reader.GetString(1)) - Convert.ToDateTime(reader.GetString(0))).ToString();
                        s[4] = reader.GetInt32(2).ToString();
                        s[5] = s[3];
                        s[6] = reader.GetString(1);

                        string sql1 = $"SELECT fioClient FROM client where id={id}";
                        // Создание объекта SQLiteCommand
                        using (SQLiteCommand command1 = new SQLiteCommand(sql1, connection))
                        {
                            // Выполнение запроса
                            using (SQLiteDataReader reader1 = command1.ExecuteReader())
                            {
                                // Обработка результата запроса
                                while (reader1.Read())
                                {
                                    s[1] = reader1.GetString(0);
                                }
                            }
                        }
                        int idNotebook = reader.GetInt32(3);
                        using (SQLiteCommand command1 = new SQLiteCommand($"SELECT name,cost FROM notebooks WHERE id={idNotebook}", connection)) //получаем название ноутбука из таблицы Ноутбуки по его id
                        {
                            using (SQLiteDataReader reader1 = command1.ExecuteReader())
                            {
                                while (reader1.Read())
                                {
                                    s[2] = reader1.GetString(0);
                                    s[7] = reader1.GetInt32(1).ToString(); ;
                                }
                            }
                        }
                        s[8] = idNotebook.ToString();

                    }
                }
            }
            return s;
        }
        public void changeStatusContract(int id)
        {


            string query = $"UPDATE contracts SET status=1 WHERE id={id}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void MakeDocument(int lastId,int idNotebook, String[] s)
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
            query = "INSERT INTO contracts (id,startDate,endDate,cost,idNotebook,status) VALUES (@id,@startDate,@endDate,@cost,@idNotebook,@status)";
            string query1 = "INSERT INTO client (id,fioClient,passportClient,emailClient,phoneNumberClient) VALUES (@id,@fioClient,@passportClient,@emailClient,@phoneNumberClient)";

            command = new SQLiteCommand(query, connection);

            // Устанавливаем параметры для новой строки
            command.Parameters.AddWithValue("@id", lastId + 1);
            command.Parameters.AddWithValue("@startDate", s[0]);
            command.Parameters.AddWithValue("@endDate", s[3]);
            command.Parameters.AddWithValue("@cost", Convert.ToInt32(s[4]) * Convert.ToInt32(s[5]));
            command.Parameters.AddWithValue("@idNotebook", idNotebook);
            command.Parameters.AddWithValue("@status", 0);

            // Выполняем команду вставки новой строки
            command.ExecuteNonQuery();
            SQLiteCommand command1 = new SQLiteCommand(query1, connection);

            // Устанавливаем параметры для новой строки
            command1.Parameters.AddWithValue("@id", lastId + 1);
            command1.Parameters.AddWithValue("@fioClient", s[1]);
            command1.Parameters.AddWithValue("@passportClient", s[6] + s[7]);
            command1.Parameters.AddWithValue("@emailClient", s[8]);
            command1.Parameters.AddWithValue("@phoneNumberClient", s[9]);

            // Выполняем команду вставки новой строки
            command1.ExecuteNonQuery();
            connection.Close();
        }
    }
}
