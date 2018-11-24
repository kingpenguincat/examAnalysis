using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace examAnalysis
{
    /// <summary>
    /// WrongSubject.xaml 的交互逻辑
    /// </summary>
    public partial class WrongSubject : Page
    {
        private string examId;
        private string className;
        private static SqLiteHelper sql;
        private static String Create = "admin";
        private static String Update = "admin";
        private static String ExamTableName = "subject";
        private static string WrongTableName = "wrong";
        private SQLiteDataReader dataReader;
        private List<SubjectEntity> list = new List<SubjectEntity>();
        private List<StudentWrongSubject> studentWrongSubjects = new List<StudentWrongSubject>();
        public WrongSubject(String[] args)
        {
            InitializeComponent();
            sql = new SqLiteHelper("Data Source=examdata.sqlite;Version=3;");
            string examName = args[1];
            examId = args[0];
            className = args[2];
            dataReader = sql.ReadFullTableWithParam(ExamTableName, new string[] { "EXAM_ID" }, new string[] { "=" }, new string[] { examId });
            int count = 0;
            while (dataReader.Read())
            {
                string questionId = ToDBC(dataReader.GetString(7));
                list.Add(new SubjectEntity { Id = dataReader.GetString(4), QuestionId = questionId, Name = dataReader.GetString(5) });
                count++;
            }

        }

        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string wrongQuestionId = ToDBC(studentWrongSubjectTextBox.Text.ToString());
            string studentName = studentNameTextBox.Text.ToString();
            char[] separator = { ',' };
            string[] wrongQuestionIds = wrongQuestionId.Split(separator);
            for (int i = 0; i < wrongQuestionIds.Count(); i++)
            {
                Regex r = new Regex(wrongQuestionIds[i]); // 定义一个Regex对象实例

                for (int j = 0; j < list.Count(); j++)
                {
                    Match m = r.Match(list[j].QuestionId); // 在字符串中匹配
                    if (m.Success)
                    {
                        Console.WriteLine("Found match at type " + list[j].Name + "------Found match at position " + m.Index); //输入匹配字符的位置 
                        String guid = Guid.NewGuid().ToString();
                        String time = DateTime.Now.ToString();
                        sql.InsertValues(WrongTableName,
                    new string[] { Create, time, Update, time, guid, studentName, examId, list[j].Name , wrongQuestionIds[i], className });
                    }
                }
            }
            dataReader = sql.ReadFullTableWithParamAndGroupBy(WrongTableName, "SUBJECT_TYPE",new string[] { "NAME" }, new string[] { "=" }, new string[] { studentName });
            int count = 0;
            while (dataReader.Read())
            {
                studentWrongSubjects.Add(new StudentWrongSubject { SubjectType = dataReader.GetString(1), Count = dataReader.GetInt32(0).ToString() });
                count++;
            }
            wrongList.ItemsSource = studentWrongSubjects;

        }
    }
}
