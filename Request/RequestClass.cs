using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request
{
    public class RequestClass
    {
        private static string conn = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=Doogee_X5_Max";
        private static MySqlConnection connection = new MySqlConnection(conn);

        public void addNewRequest(List<string> groupList, string requestName, string userToken, List<List<int>> analitycValues)
        {
            string date = DateTime.Now.ToString();
            string query = "INSERT INTO request (requestDate, requestName, user_id) VALUES " +
                "('" + date.Split(' ')[0].Split('.')[2] + "-" + date.Split('.')[1] + "-" + date.Split('.')[0] + "', '" + requestName + "', " + getUserId(userToken) + ")";
            runQuery(query);

            addGroups(groupList, requestName, analitycValues);
            addResult(requestName);
        }

        public void addResult(string requestName)
        {
            string query = "INSERT INTO result (resultName, request_id) VALUES ('" + requestName + "', " + getRequestId(requestName) + ")";
            runQuery(query);

        }

        public void addGroups(List<string> groupList, string requestName, List<List<int>> analitycValues)
        {
            string query = "INSERT INTO groupList (request_id) VALUES (" + getRequestId(requestName) + ")";
            runQuery(query);

            for (int i = 0; i < groupList.Count; i++)
            {
                addGroup(groupList[i], analitycValues[i], requestName);
            }
        }

        public void addGroup(string group, List<int> analysisValues, string requestName)
        {
            string query = "INSERT INTO mydb.`group` (urlGroup, positive, neutral, negative, groupList_id) VALUES " +
                "('" + group + "', " + analysisValues[0] + ", " + analysisValues[1] + ", " + analysisValues[2] + ", " + getGroupListId(requestName) + ")";
            runQuery(query);

        }

        public string getGroupListId(string requestName)
        {
            string query = "SELECT id FROM groupList WHERE request_id = " + getRequestId(requestName);
            return runScalar(query);
        }

        public string getRequestId(string requestName)
        {
            string query = "SELECT id FROM request WHERE requestName = '" + requestName + "'";
            return runScalar(query);
        }

        public string getUserId(string userToken)
        {
            string query = "SELECT id FROM user WHERE userToken = '" + userToken + "'";
            return runScalar(query);
        }

        public string runScalar(string query)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            string result = command.ExecuteScalar().ToString();
            connection.Close();
            return result;
        }

        public void runQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public class ResultClass
    {
        private static string conn = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=Doogee_X5_Max";
        private static MySqlConnection connection = new MySqlConnection(conn);

        public int userId = 0;
        public List<string> requestIdList = new List<string>();
        public List<string> requestNameList = new List<string>();
        public List<string> requestDateList = new List<string>();
        public List<List<string>> pagesList = new List<List<string>>();
        public List<List<string>> pagesPositiveList = new List<List<string>>();
        public List<List<string>> pagesNeutralList = new List<List<string>>();
        public List<List<string>> pagesNegativeList = new List<List<string>>();

        public ResultClass(string userToken)
        {
            getUserId(userToken);
            idList();
            nameList();
            dateList();
            getPagesList();
        }

        public void getUserId(string userToken)
        {
            string query = "SELECT id FROM user WHERE userToken = '" + userToken + "'";
            userId = Convert.ToInt32(runScalar(query));
        }

        public void idList()
        {
            string query = "SELECT id FROM mydb.request WHERE user_id = " + userId;
            requestIdList = runAsReader(query);
        }

        public void nameList()
        {
            string query = "SELECT requestName FROM mydb.request WHERE user_id = " + userId;
            requestNameList = runAsReader(query);
        }

        public void dateList()
        {
            string query = "SELECT requestDate FROM mydb.request WHERE user_id = " + userId;
            requestDateList = runAsReader(query);
        }

        public void getPagesList()
        {
            //List<List<string>> pagesListReturn = new List<List<string>>();
            List<string> pagesListInGroup = new List<string>();
            string query = "";

            foreach (string idRequest in requestIdList)
            {
                query = "SELECT id FROM mydb.grouplist WHERE request_id = " + idRequest;
                string idListPages = runScalar(query);
                
                query = "SELECT urlGroup FROM mydb.`group` WHERE grouplist_id = " + idListPages;
                pagesListInGroup = runAsReader(query);
                pagesList.Add(new List<string>(pagesListInGroup));

                query = "SELECT positive FROM mydb.`group` WHERE grouplist_id = " + idListPages;
                pagesListInGroup = runAsReader(query);
                pagesPositiveList.Add(new List<string>(pagesListInGroup));

                query = "SELECT neutral FROM mydb.`group` WHERE grouplist_id = " + idListPages;
                pagesListInGroup = runAsReader(query);
                pagesNeutralList.Add(new List<string>(pagesListInGroup));

                query = "SELECT negative FROM mydb.`group` WHERE grouplist_id = " + idListPages;
                pagesListInGroup = runAsReader(query);
                pagesNegativeList.Add(new List<string>(pagesListInGroup));

            }
        }

        public List<string> runAsReader(string query)
        {
            List<string> resultList = new List<string>();

            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader result = command.ExecuteReader();

            while (result.Read())
            {
                resultList.Add(result[0].ToString());
            }

            connection.Close();

            return resultList;

        }

        public string runScalar(string query)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            string result = command.ExecuteScalar().ToString();
            connection.Close();
            return result;
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
