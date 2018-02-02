using ExamMaterialWpf.ServiceReference1;
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

namespace ExamMaterialWpf.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ToDoDayTemplate.xaml
    /// </summary>
    public partial class ToDoDayTemplate : UserControl
    {
        ServiceClient _proxy;
        public ToDoDayTemplate(ServiceClient proxy)
        {
            InitializeComponent();
            _proxy = proxy;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToDoItemTemplate item = new ToDoItemTemplate(_proxy);
            item.toDoItemName.Text = FruitTextBox.Text;
            FruitListBox.Items.Add(item);
            _proxy.AddToDoItem(Convert.ToInt32(tbToDoId.Text), false, FruitTextBox.Text);
           

        }

        private void bDelToDoItem_Click(object sender, RoutedEventArgs e)
        {
            if (FruitListBox.Items.Count > 0)
            {
                if (FruitListBox.SelectedItem != null)
                {
                    string name = ((ToDoItemTemplate)FruitListBox.SelectedItem).toDoItemName.Text;
                    FruitListBox.Items.Remove(FruitListBox.SelectedItem);
                    _proxy.RemoveToDoItem(Convert.ToInt32(tbToDoId.Text), name);
                }
            }
        }
    }
}
