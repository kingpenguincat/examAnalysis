using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
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
    /// EditExamInfo.xaml 的交互逻辑
    /// </summary>
    public partial class EditExamInfo : Page
    {
        private static SqLiteHelper sql;
        private static String Create = "admin";
        private static String Update = "admin";
        private static String ExamTableName = "subject";
        private static String[] queryItem = { "ID","NAME","QUESTION_ID" };
        private static String[] col = { };
        private static String[] operation = { };
        private static String[] colvalue = { };
        private SQLiteDataReader dataReader;
        private List<SubjectEntity> list = new List<SubjectEntity>();
        private string examId;
        public EditExamInfo(String[] args)
        {
            InitializeComponent();
            
            sql = new SqLiteHelper("Data Source=examdata.sqlite;Version=3;");
            string examName = args[1];
            examId = args[0];
            examNameTextBlock.Text = examName;
            //String result =  JsonHelper.GetJson("examInfo.json", "subjectType");
            //String[] resultList = result.Split(',');
            //dataReader = sql.ReadTable(ExamTableName, queryItem, "0", col, operation, colvalue);
            dataReader = sql.ReadFullTableWithParam(ExamTableName, new string[] { "EXAM_ID" }, new string[] { "=" }, new string[] { examId });
            int count = 0;
            while (dataReader.Read())
            {
                string questionId = ToDBC(dataReader.GetString(7));
                list.Add(new SubjectEntity {Id = dataReader.GetString(4),QuestionId = questionId, Name = dataReader.GetString(5) });
                count++;
                
            }
            subjectDataGrid.ItemsSource = list;
            Console.WriteLine(count);
            


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
            try
            {
                list.RemoveRange(0,list.Count);
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
           
            this.NavigationService.GoBack();

        }

       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            
            String examName = examNameTextBlock.Text.ToString();
            if (list.Count > 0)
            {
                try
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        bool hasRows = sql.ReadFullTableWithParam(ExamTableName, new string[] {"ID", "EXAM_ID" }, new string[] { "=", "=" },new string[]{ list[i].Id,examId}).HasRows;
                        if (hasRows)
                        {
                            sql.UpdateValues(ExamTableName, new string[] { "NAME", "QUESTION_ID" }, new string[] { list[i].Name, list[i].QuestionId }, "ID", list[i].Id);
                            SnackbarSuccess.IsActive = true;
                            //progress.Visibility = Visibility.Hidden;
                        }
                        else {
                            String guid = Guid.NewGuid().ToString();
                            String time = DateTime.Now.ToString();
                            sql.InsertValues(ExamTableName,
                        new string[] { Create, time, Update, time, guid, list[i].Name, examId, list[i].QuestionId });
                            SnackbarSuccess.IsActive = true;
                            //progress.Visibility = Visibility.Hidden;
                        }

                    }



                    //sql.InsertValues(ExamTableName, new string[] { Create, time, Update, time, guid, examName, examName, "1" });
                    
                }
                catch (Exception ex)
                {

                }


            }
            else {
                Console.WriteLine("数据为空");
            }
           
        }

        private void SnackbarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Refresh();
            SnackbarSuccess.IsActive = false;
        }

       
    }
}
