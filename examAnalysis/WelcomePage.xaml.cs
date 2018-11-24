using System;
using System.Collections.Generic;
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
    /// WelcomePage.xaml 的交互逻辑
    /// </summary>
    public partial class WelcomePage : Page
    {

        private string[] args = { };
        public WelcomePage()
        {
           InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

                Exam exam = new Exam(args);
                this.NavigationService.Navigate(exam);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChoseExam choseExam = new ChoseExam();
            this.NavigationService.Navigate(choseExam);
        }
    }
}
