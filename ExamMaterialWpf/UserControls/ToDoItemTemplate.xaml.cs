using ExamMaterialWpf.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для ToDoItemTemplate.xaml
    /// </summary>
    public partial class ToDoItemTemplate : UserControl
    {
        ServiceClient _proxy;
        public ToDoItemTemplate(ServiceClient proxy)
        {
            InitializeComponent();
            _proxy = proxy;
        }

        private void toggleBut_Click(object sender, RoutedEventArgs e)
        {
            _proxy.EditToDoItem(toDoItemName.Text);
        }
    }
}
