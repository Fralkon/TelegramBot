﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class MySQL
    {
        private string connStr;
        public MySQL(string dbName)
        {
            connStr = "server=" + serverName +
               ";user=" + userName +
               ";database=" + dbName +
               ";port=" + port +
               ";password=" + password + ";";
        }
        public MySQL(string serverName, string userName, string dbName, string port, string password)
        {
            connStr = "server=" + serverName +
                 ";user=" + userName +
                 ";database=" + dbName +
                 ";port=" + port +
                 ";password=" + password + ";";
        }
        public DataTable GetDataTableSQL(string sql)
        {
            DataTable dt = new DataTable();
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                MySqlCommand sqlCom = new MySqlCommand(sql, connection);
                sqlCom.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
                dataAdapter.Fill(dt);
                connection.Close();
            }
            return dt;
        }
        public long InsertSQL(string SQL)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                MySqlCommand sqlCom = new MySqlCommand(SQL, connection);
                sqlCom.ExecuteNonQuery();
                connection.Close();
                return sqlCom.LastInsertedId;
            }
        }
        public void SendSQL(string sql)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                MySqlCommand sqlCom = new MySqlCommand(sql, connection);
                sqlCom.ExecuteNonQuery();
                connection.Close();
            }
        }
        private string serverName = "astf3-stp5"; // Адрес сервера (для локальной базы пишите "localhost")
        private string userName = "root"; // Имя пользователя
        private string port = "3306"; // Порт для подключения
        private string password = "Fralkon"; // Пароль для подключения
    }
}