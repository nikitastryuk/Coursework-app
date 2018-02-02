using ExamMaterialWpf.ServiceReference1;
using ExamMaterialWpf.UserControls;
using Microsoft.Win32;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExamMaterialWpf
{
    /// <summary>
    /// Логика взаимодействия для PlanCard.xaml
    /// </summary>
    public partial class PlanCard : Window
    {
        ServiceClient _proxy;
        int _userId;
        public PlanCard(ServiceClient proxy, int userId)
        {
            InitializeComponent();
            _proxy = proxy;
            _userId = userId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = this.Owner as MainWindow;
            PlanCardTemplate planCardTemplate = new PlanCardTemplate();
          
            planCardTemplate.time.Text = time.Text;
            planCardTemplate.date.Text = date.Text;
            planCardTemplate.planName.Text = planName.Text;
            planCardTemplate.notes.Text = notes.Text;
            planCardTemplate.planImage.Source = planImg.Source;
            
            planCardTemplate.rating.Value = rating.Value;

            //MaterialDesignThemes.Wpf.Card card = (MaterialDesignThemes.Wpf.Card)planCardTemplate.Content;
            //MaterialDesignThemes.Wpf.Card copy = new MaterialDesignThemes.Wpf.Card();
            //copy = XamlReader.Parse(XamlWriter.Save(card)) as MaterialDesignThemes.Wpf.Card;
            //copy.Visibility = Visibility.Visible;
            //mw.lb.Items.Add(copy);


            mw.lb.Items.Add(planCardTemplate);
            _proxy.AddPlan(planCardTemplate.planImage.Source.ToString(), planCardTemplate.planName.Text, planCardTemplate.rating.Value, planCardTemplate.notes.Text, planCardTemplate.time.Text, planCardTemplate.date.Text, _userId);

            Close();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void bPlanImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                planImg.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
