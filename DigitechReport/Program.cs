using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Configuration;

namespace DigitechReport
{
    class Program
    {        
        static void Main(string[] args)
        {
            if(args.Length <= 0) { Console.WriteLine("No file to Process . File name with path not available for processing. ");  return; }
            string filePath = args[0];
            string flightNo = args[1];
            Console.WriteLine(filePath);
            Console.WriteLine(flightNo);

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("No file to Process . File name with path not available for processing. ");
                return;
            }
            StreamReader oReader;
            if (File.Exists(filePath))
            {
                oReader = new StreamReader(filePath);
                string cColl = oReader.ReadToEnd();
                List<DigitechData> lstDigitechData = new List<DigitechData>();
                foreach (var info in typeof(PathEnum).GetFields().Where(x => x.IsStatic && x.IsLiteral))
                {
                    var Name = info.Name;
                    var Value=info.GetValue(Name);
                    string cCriteria = Value.ToString();
                    System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(cCriteria, RegexOptions.IgnoreCase);                    
                    int count = oRegex.Matches(cColl).Count;
                    DigitechData objDigitechData = new DigitechData();
                    objDigitechData.Tile = Name;
                    objDigitechData.Count = count;
                    objDigitechData.ReportDateTime = Convert.ToDateTime(DateTime.Now);                   
                    objDigitechData.FlightNumber = flightNo;
                    lstDigitechData.Add(objDigitechData);
                    Console.WriteLine(count.ToString());
                }
                BulkToMySQL(lstDigitechData);
            }
            else
            {
                Console.WriteLine(filePath + "File not found.");
            }
            //Console.ReadLine();
        }

        public static void BulkToMySQL(List<DigitechData> lstDigitechData)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["AirHubConn"].ToString();  //"server=localhost;User Id=root;Password=Pavithra;Database=arincdb";
            StringBuilder sCommand = new StringBuilder("INSERT INTO digitechreport (Tile, Count, ReportDateTime,FlightNumber) VALUES ");
            using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
            {
                List<string> Rows = new List<string>();
                foreach(var item in lstDigitechData)
                {
                    Rows.Add(string.Format("('{0}','{1}','{2}','{3}')", MySqlHelper.EscapeString(item.Tile),item.Count,(item.ReportDateTime).ToString("yyyy-MM-dd HH:mm"),item.FlightNumber));
                }                
                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
