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
    /// ChoseExam.xaml 的交互逻辑
    /// </summary>
    public partial class ChoseExam : Page
    {
        private static String Create = "admin";
        private static String Update = "admin";
        private static String ExamTableName = "exam";
        private static String[] queryItem = { "ID", "NAME" };
        private static String[] col = { };
        private static String[] operation = { };
        private static String[] colvalue = { };
        private static SqLiteHelper sql;
        private SQLiteDataReader dataReader;
        private List<ExamInfoEntity> list = new List<ExamInfoEntity>();

        public ChoseExam()
        {
            InitializeComponent();
            sql = new SqLiteHelper("Data Source=examdata.sqlite;Version=3;");
            dataReader = sql.ReadTable(ExamTableName, queryItem, "0", col, operation, colvalue);
            int count = 0;

            while (dataReader.Read())
            {

                list.Add(new ExamInfoEntity { Id = dataReader.GetString(0), Name = dataReader.GetString(1) });
                count++;
            }
            Console.WriteLine(count);
            subjectTypeComboBox.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] args2 = new string[3];
           
            args2[0] = subjectTypeComboBox.SelectedValue.ToString();
            args2[1] = subjectTypeComboBox.Text.ToString();
            args2[2] = studentClassTextBox.Text.ToString();

            WrongSubject wrongSubject = new WrongSubject(args2);
            this.NavigationService.Navigate(wrongSubject);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
