using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User
{
    public class UserDataBase
    {
        private static string conn = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=Doogee_X5_Max";
        private static MySqlConnection connection = new MySqlConnection(conn);

        public void insertInToDB(string email, string password)
        {
            string query = "INSERT INTO user (email, password) VALUES ('" + email + "', '" + password + "')";
            runQuery(query);
        }

        public bool getUserToken(string email, string token)
        {
            string query = "SELECT userToken FROM user WHERE email = '" + email + "'";

            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            string result = command.ExecuteScalar().ToString();
            connection.Close();

            return result == token ? true : false;
        }

        public void updateUser(string email, string token)
        {
            string query = "UPDATE user SET userToken = '" + token + "' WHERE email = '" + email +"'";
            runQuery(query);
        }

        public void runQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
