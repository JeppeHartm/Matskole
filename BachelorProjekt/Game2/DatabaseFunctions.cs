using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Game2
{
    public static class DatabaseFunctions
    {
        private const String BASE_URL = "http://matskole.compute.dtu.dk/bscf131/scripts/";
        private static List<String> httpRequests = new List<string>();
        public static String httpResponse = "";
        
        public static async Task<int[]> login(String username,String password){
            var uri = new Uri(BASE_URL + "login.php?user="+username+"&pass="+password);

            var client = new WebClient();

            var results = await client.DownloadStringTaskAsync(uri);

            httpResponse = (String)results;

            if (((String)results).Equals("0"))
            {
                return null;
            }
            else
            {
                httpResponse = "1";
                char[] del = { '#' };
                String[] strs = ((String)results).Split(del);
                int[] levelids = new int[strs.Length-1];
                for (int i = 0; i < strs.Length-1; i++)
                {
                    levelids[i] = Convert.ToInt32(strs[i]);
                }
                return levelids;
            }
        }
        public class UserData
        {
            public String username;
            public String password;
            public bool isMale;
            public bool isTeacher;
            public String classid;
            public CompletedLevelData[] completedLevels;
        }
        public class CompletedLevelData{
            public String username;
            public int levelid;
            public int index;
            public int timespent;
            public List<UsedCompData> componentsUsed;

            public CompletedLevelData(String user, int id, int i, int time,List<UsedCompData> componentsUsed){
                this.username = user;
                this.levelid = id;
                this.index = i;
                this.timespent = time;
                this.componentsUsed = componentsUsed;
            }
            public class UsedCompData{
                public int compid;
                public int amount;
                public UsedCompData(int id, int n){
                    this.compid = id;
                    this.amount = n;
                }
            }
            
        }
        public static async Task<List<String>> getUsernamesByClassID(String ClassID)
        {
            Uri uri = new Uri(BASE_URL + "retrieveNames.php?class=" + ClassID);
            List<String> l = new List<string>();
            String[] input = (await (new WebClient().DownloadStringTaskAsync(uri))).Split('#');
            for (int i = 0; i < input.Length - 1; i++)
            {
                l.Add(input[i]);
            }
            return l;
        }
        public static async Task<int> getMaxIndex(String user)
        {
            Uri uri = new Uri(BASE_URL + "retrieveMaxIndex.php?user=" + user);
            return Convert.ToInt32(await (new WebClient().DownloadStringTaskAsync(uri)));
        }
        public static async Task<List<UserData>> getUsersByClassID(String ClassID, bool getLevelData, bool getCompData)
        {
            Uri uri = new Uri(BASE_URL + "retrieveNames.php?class="+ClassID);
            List<Task<UserData>> tasks = new List<Task<UserData>>();
            List<UserData> output = new List<UserData>();
            var client = new WebClient();
            string str = await client.DownloadStringTaskAsync(uri);
            string[] names = str.Split('#');
            foreach (string s in names)
            {
                if (s.Length == 0) break ;
                output.Add(await getUserData(s, getLevelData, getCompData));
            }

            return output;
        }
        public static async Task<string[]> getDistinctClassIDs()
        {
            Uri uri = new Uri(BASE_URL + "retrieveDistinctClassID.php");
            string[] cids = (await (new WebClient()).DownloadStringTaskAsync(uri)).Split('#');
            string[] output = new string[cids.Length - 1];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = cids[i];
            }
            return output;
        }
        public static async Task<UserData> getUserData(String name, bool getLevelData, bool getCompData)
        {
            Uri userUri = new Uri(BASE_URL + "retrieveUser.php?user=" + name);
            Uri lvlsUri = new Uri(BASE_URL + "retrieveLevels.php?user=" + name);
            
            var client = new WebClient();

            Task<String> t1 = client.DownloadStringTaskAsync(userUri);
            
            string userStr = await t1;
            string[] uFields = userStr.Split('#');
            UserData u = new UserData();
            u.username = uFields[0];
            u.password = uFields[1];
            u.isMale = uFields[2].Equals("1");
            u.isTeacher = uFields[3].Equals("1");
            u.classid = uFields[4];
            if (getLevelData)
            {
                string lvlsStr = await client.DownloadStringTaskAsync(lvlsUri);
                int i = 0;
                string[] levels = lvlsStr.Split('#');
                
                u.completedLevels = new CompletedLevelData[levels.Length-1];
                foreach (string s in levels)
                {
                    if (s.Length == 0) break;
                    string[] lFields = s.Split('%');
                    int lvlid = Convert.ToInt32(lFields[0]);
                    string user = lFields[1];
                    int index = Convert.ToInt32(lFields[2]);
                    int time = Convert.ToInt32(lFields[3]);
                    CompletedLevelData cld = new CompletedLevelData(user, lvlid, index, time, getCompData ? await getCompsByLevel(user, lvlid, index) : null);
                    u.completedLevels[i++] = cld;
                }
            }
            else
            {
                u.completedLevels = new CompletedLevelData[0];
            }
            return u;
            
        }

        private static async Task<List<CompletedLevelData.UsedCompData>> getCompsByLevel(string user, int lvlid, int index)
        {
            Uri uri = new Uri(BASE_URL + "retrieveComps.php?user=" + user + "&lvlid=" + lvlid + "&index="+ index);

            var client = new WebClient();

            string str = await client.DownloadStringTaskAsync(uri);

            List<CompletedLevelData.UsedCompData> l = new List<CompletedLevelData.UsedCompData>();
            string[] comps = str.Split('#');
            foreach (string s in comps)
            {
                if (s.Length == 0) break;
                string[] fields = s.Split('%');
                CompletedLevelData.UsedCompData ucd = new CompletedLevelData.UsedCompData(Convert.ToInt32(fields[0]), Convert.ToInt32(fields[1]));
                l.Add(ucd);
            }
            return l;
        }
        public static async Task uploadCompletedLevelData(String username, int levelid, int index, int timespent)
        {
            var uri = new Uri(BASE_URL + "levelcomplete.php?user=" + username + "&lvlid=" + levelid + "&index=" + index + "&timespent=" + timespent);

            var client = new WebClient();

            var results = client.DownloadStringTaskAsync(uri);
        }
        public static async Task uploadCompData(String username, int levelid, int index, int compid, int amount)
        {
            var uri = new Uri(BASE_URL + "registercompused.php?user=" + username + "&lvlid=" + levelid + "&index=" + index + "&compid=" + compid + "&amount=" + amount);

            var client = new WebClient();

            var results = client.DownloadStringTaskAsync(uri);
        }
        public static void registerCompletedLevel(String username, int levelid, int index, int timespent, List<CompletedLevelData.UsedCompData> usedComponents)
        {
            uploadCompletedLevelData(username, levelid, index, timespent);
            foreach (CompletedLevelData.UsedCompData c in usedComponents)
            {
                uploadCompData(username, levelid, index, c.compid, c.amount);
            }
        }
    }
}
