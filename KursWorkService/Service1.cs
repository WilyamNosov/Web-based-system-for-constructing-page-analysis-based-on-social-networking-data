using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SentAn;
using User;
using Request;

namespace KursWorkService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public SentimentAnalysis sentiment = new SentimentAnalysis();
        public List<string> PosNetNeg = new List<string>();
        public List<List<int>> sentAnalysisResults = new List<List<int>>();
        public List<List<string>> postsList = new List<List<string>>();
        public List<List<string>> analysisResult = new List<List<string>>();

        public ResultClass getHistory(string userToken)
        {
            ResultClass getResult = new ResultClass(userToken);

            return getResult;
        }

        public string randomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public List<List<int>> sendRequest(List<string> urlList, string requestName, string userID)
        {
            List<List<List<string>>> result = new List<List<List<string>>>();

            postsList = sentiment.generate(urlList);
            analysisResult = sentiment.getSetimenValue(postsList);
            result.Add(postsList);
            result.Add(analysisResult);
            sentAnalysisResults = analysisiPars();

            RequestClass requestVariable = new RequestClass();
            requestVariable.addNewRequest(urlList, requestName, userID, sentAnalysisResults);

            return sentAnalysisResults;
        }

        public List<List<int>> analysisiPars()
        {
            List<List<int>> resultAnalysis = new List<List<int>>();
            List<int> valuesAnalysis = new List<int>();

            int pos = 0;
            int neu = 0;
            int neg = 0;

            for (int i = 0; i < analysisResult.Count; i++)
            {
                foreach(string value in analysisResult[i])
                {
                    switch (value.Split(' ')[0])
                    {
                        case "positive":
                            pos++;
                            break;
                        case "neutral":
                            neu++;
                            break;
                        default:
                            neg++;
                            break;
                    }
                }
                valuesAnalysis.Add(pos);
                valuesAnalysis.Add(neu);
                valuesAnalysis.Add(neg);
                valuesAnalysis.Add(analysisResult[i].Count());
                resultAnalysis.Add(new List<int>(valuesAnalysis));

                pos = 0;
                neu = 0;
                neg = 0;

                valuesAnalysis.Clear();
            }

            return resultAnalysis;
        }

        public void addUserToDataBase(string email, string password)
        {
            UserDataBase addUser = new UserDataBase();
            addUser.insertInToDB(email, password);

        }

        public void updateUser(string email, string token)
        {
            UserDataBase updateUser = new UserDataBase();
            updateUser.updateUser(email, token);
        }

        public bool checkUser(string email, string token)
        {
            UserDataBase checkUser = new UserDataBase();
            return checkUser.getUserToken(email, token);
        }
    }
}
