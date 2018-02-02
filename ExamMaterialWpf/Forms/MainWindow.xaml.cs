using ExamMaterialWpf.ServiceReference1;
using ExamMaterialWpf.UserControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamMaterialWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window, IServiceCallback
    {
        User _currentUser;
        List<string> privatechat;
        ServiceClient _proxy;
        List<Client> clients;
        string _userName = String.Empty;

        public MainWindow(User currentUser)
        {
            InitializeComponent();
            _proxy = new ServiceClient(new InstanceContext(this));
            clients = new List<Client>();
            privatechat = new List<string>();
            _currentUser = currentUser;
            this.DataContext = _currentUser;

            _userName = _currentUser.FirstName + " " + _currentUser.LastName;
            _proxy.EnterClient(_currentUser);
            clients = _proxy.GetClientList().ToList();
            _currentUser.Friends = _proxy.GetFriendsList(_currentUser.Id);
            _currentUser.Plans = _proxy.GetPlansList(_currentUser.Id);
            _currentUser.Dates = _proxy.GetToDoDateList(_currentUser.Id);

            foreach (var item in _currentUser.Friends)
            {
                var addUser = _proxy.GetUser(item.Email);
                if (addUser != null)
                {
                    FriendCardTemplate friendCard = new FriendCardTemplate();
                    friendCard.tbFriendCardName.Text = addUser.FirstName + " " + addUser.LastName;
                    friendCard.tbFriendFacebook.Text = addUser.Facebook;
                    friendCard.tbFriendTwitter.Text = addUser.Twitter;
                    friendCard.tbFriendVk.Text = addUser.Vk;
                    friendCard.tbFriendGithub.Text = addUser.Github;
                    friendCard.bDelFriend.Click += BDelFriend_Click;
                    friendCard.tbFriendCardEmail.Text = addUser.Email;
                    ImageBrush myBrush = new ImageBrush();
                    myBrush.Stretch = Stretch.UniformToFill;
                    if (addUser.Image.Substring(0, 3) == "/Im")
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,," + addUser.Image, UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri(@addUser.Image, UriKind.RelativeOrAbsolute));
                    }
                    friendCard.imageFriendCard.Fill = myBrush;

                    lvFriends.Items.Add(friendCard);
                }
            }

            foreach (Client client in clients)
            {
                if (client.Name != _userName)
                {
                    ChatUserTemplate uc2 = new ChatUserTemplate();
                    uc2.tbUser.Text = client.Name;
                    ImageBrush myBrush = new ImageBrush();
                    if (client.User.Image.Substring(0, 3) == "/Im")
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,," + client.User.Image, UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri(@client.User.Image, UriKind.RelativeOrAbsolute));
                    }
                    uc2.chatUserImage.Fill = myBrush;
                    lbUsers.Items.Add(uc2);
                }
            }

            foreach (var item in _currentUser.Plans)
            {
                PlanCardTemplate planCardTemplate = new PlanCardTemplate();

                planCardTemplate.time.Text = item.Time;
                planCardTemplate.date.Text = item.Date;
                planCardTemplate.planName.Text = item.Name;
                planCardTemplate.notes.Text = item.Note;
                ImageBrush myBrush = new ImageBrush();

                myBrush.ImageSource = new BitmapImage(new Uri(item.Image, UriKind.RelativeOrAbsolute));

                planCardTemplate.planImage.Source = myBrush.ImageSource;

                planCardTemplate.rating.Value = item.Importance;

                lb.Items.Add(planCardTemplate);
            }

            foreach (var date in _currentUser.Dates)
            {
                date.ToDoItems = _proxy.GetToDoItemsList(date.Id);
                ToDoDayTemplate toDo = new ToDoDayTemplate(_proxy);
                toDo.tbToDoDate.Text = date.Date;
                toDo.tbToDoId.Text = date.Id.ToString();
                lvToDo.Items.Add(toDo);
                foreach (var item in date.ToDoItems)
                {
                    ToDoItemTemplate newItem = new ToDoItemTemplate(_proxy);
                    newItem.toDoItemName.Text = item.Name;
                    newItem.toggleBut.IsChecked = item.IsChecked;
                    toDo.FruitListBox.Items.Add(newItem);
                }
            }
        }

        #region ChatPage

        public void UpdateListClients(Client[] clients)
        {

            lbUsers.Items.Clear();
            foreach (Client client in clients)
            {
                if (client.Name != _userName)
                {
                    ChatUserTemplate uc2 = new ChatUserTemplate();
                    uc2.tbUser.Text = client.Name;
                    ImageBrush myBrush = new ImageBrush();
                    if (client.User.Image.Substring(0, 3) == "/Im")
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,," + client.User.Image, UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri(@client.User.Image, UriKind.RelativeOrAbsolute));
                    }
                    uc2.chatUserImage.Fill = myBrush;
                    lbUsers.Items.Add(uc2);
                }
            }
        }

        public void UpdateChat(string nameFrom, string message, string image)
        {
            try
            {
                GetMessageTemplate mesForm = new GetMessageTemplate();
                mesForm.HorizontalAlignment = HorizontalAlignment.Left;
                var date = DateTime.Now;
                mesForm.tbTime.Text = DateTime.Now.ToString("HH:mm");
                mesForm.tbUCMessage.Text = message;
                ImageBrush myBrush = new ImageBrush();
                if (image.Substring(0, 3) == "/Im")
                {
                    myBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,," + image, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    myBrush.ImageSource = new BitmapImage(new Uri(@image, UriKind.RelativeOrAbsolute));
                }
                mesForm.imageGetMsg.Fill = myBrush;

                //когда чел получает приватное сообщение, то ищем, есть ли вкладка с именем отправителя
                //если есть
                if (privatechat.Contains(nameFrom))
                {
                    TabItem currentitem = new TabItem();
                    foreach (TabItem item in chat.Items)
                    {
                        if (item.Header.ToString() == nameFrom)
                            currentitem = item;
                    }
                    chat.SelectedItem = currentitem;


                    ((ListBox)((StackPanel)((PrivateChatTemplate)((TabItem)chat.SelectedItem).Content).Content).Children[0]).Items.Add(mesForm);

                }
                //иначе создаем вкладку и добавляем сообщение
                else
                {
                    //список приватных чатов клиента
                    privatechat.Add((nameFrom));

                    //новая вкладка на чатах
                    TabItem privateTabItem = new TabItem();
                    privateTabItem.Header = (nameFrom);

                    //добавляем контролы в новую вкладку
                    PrivateChatTemplate uc3 = new PrivateChatTemplate();
                    uc3.tbMessage.KeyUp += TbMessage_KeyUp;
                    privateTabItem.Content = uc3;
                    ((ListBox)((StackPanel)((PrivateChatTemplate)privateTabItem.Content).Content).Children[0]).Items.Add(mesForm);
                    chat.Items.Add(privateTabItem);
                    chat.SelectedItem = privateTabItem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            ChatUserTemplate uc = (ChatUserTemplate)item.Content;
            try
            {
                //если мы кликнули не на свое имя, то создаем окно нового чата 
                if (_userName != uc.tbUser.Text)
                {
                    //если еще нет чата с выбранным 
                    if (privatechat.Contains(uc.tbUser.Text) == false)
                    {
                        //список приватных чатов клиента
                        privatechat.Add((uc.tbUser.Text));

                        //новая вкладка на чатах
                        TabItem privateTabItem = new TabItem();
                        privateTabItem.Header = (uc.tbUser.Text);

                        //добавляем контролы в новую вкладку
                        PrivateChatTemplate uc3 = new PrivateChatTemplate();
                        uc3.tbMessage.KeyUp += TbMessage_KeyUp;
                        privateTabItem.Content = uc3;

                        chat.Items.Add(privateTabItem);
                        chat.SelectedItem = privateTabItem;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TbMessage_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (_proxy.Send(_userName, ((TextBox)sender).Text, ((TabItem)chat.SelectedItem).Header.ToString()))
                {
                    SendMessageTemplate message = new SendMessageTemplate();
                    message.HorizontalAlignment = HorizontalAlignment.Right;
                    message.tbUCMessage.Text = ((TextBox)sender).Text;
                    ImageBrush myBrush = new ImageBrush();
                    if (_currentUser.Image.Substring(0, 3) == "/Im")
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,," + _currentUser.Image, UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri(@_currentUser.Image, UriKind.RelativeOrAbsolute));
                    }
                    message.imageSendMsg.Fill = myBrush;
                    var date = DateTime.Now;
                    message.tbTime.Text = DateTime.Now.ToString("HH:mm");
                    ((ListBox)((StackPanel)((PrivateChatTemplate)((TabItem)chat.SelectedItem).Content).Content).Children[0]).Items.Add(message);
                    ((TextBox)sender).Text = "";
                }
                else
                {
                    MessageBox.Show("User is offline now!");
                }
            }
        }
        #endregion

        #region Visibility

        private void bFriensList_Click(object sender, RoutedEventArgs e)
        {
            friendsListPage.Visibility = Visibility.Visible;
            settingsPage.Visibility = Visibility.Collapsed;
            mainPage.Visibility = Visibility.Collapsed;
            plannerPage.Visibility = Visibility.Collapsed;
            chatPage.Visibility = Visibility.Collapsed;
            toDoPage.Visibility = Visibility.Collapsed;
            faqPage.Visibility = Visibility.Collapsed;
        }

        private void bFaq_Click(object sender, RoutedEventArgs e)
        {
            faqPage.Visibility = Visibility.Visible;
            friendsListPage.Visibility = Visibility.Collapsed;
            settingsPage.Visibility = Visibility.Collapsed;
            mainPage.Visibility = Visibility.Collapsed;
            plannerPage.Visibility = Visibility.Collapsed;
            chatPage.Visibility = Visibility.Collapsed;
            toDoPage.Visibility = Visibility.Collapsed;
        }

        private void bToDo_Click(object sender, RoutedEventArgs e)
        {
            faqPage.Visibility = Visibility.Collapsed;
            friendsListPage.Visibility = Visibility.Collapsed;
            settingsPage.Visibility = Visibility.Collapsed;
            mainPage.Visibility = Visibility.Collapsed;
            plannerPage.Visibility = Visibility.Collapsed;
            chatPage.Visibility = Visibility.Collapsed;
            toDoPage.Visibility = Visibility.Visible;
        }

        private void bMain_Click(object sender, RoutedEventArgs e)
        {
            faqPage.Visibility = Visibility.Collapsed;
            friendsListPage.Visibility = Visibility.Collapsed;
            settingsPage.Visibility = Visibility.Collapsed;
            mainPage.Visibility = Visibility.Visible;
            plannerPage.Visibility = Visibility.Collapsed;
            toDoPage.Visibility = Visibility.Collapsed;
            chatPage.Visibility = Visibility.Collapsed;
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
            faqPage.Visibility = Visibility.Collapsed;
            friendsListPage.Visibility = Visibility.Collapsed;
            settingsPage.Visibility = Visibility.Visible;
            mainPage.Visibility = Visibility.Collapsed;
            toDoPage.Visibility = Visibility.Collapsed;
            plannerPage.Visibility = Visibility.Collapsed;
            chatPage.Visibility = Visibility.Collapsed;
        }

        private void bPlanner_Click(object sender, RoutedEventArgs e)
        {
            faqPage.Visibility = Visibility.Collapsed;
            friendsListPage.Visibility = Visibility.Collapsed;
            settingsPage.Visibility = Visibility.Collapsed;
            mainPage.Visibility = Visibility.Collapsed;
            toDoPage.Visibility = Visibility.Collapsed;
            plannerPage.Visibility = Visibility.Visible;
            chatPage.Visibility = Visibility.Collapsed;
        }

        private void bChat_Click(object sender, RoutedEventArgs e)
        {
            faqPage.Visibility = Visibility.Collapsed;
            friendsListPage.Visibility = Visibility.Collapsed;
            settingsPage.Visibility = Visibility.Collapsed;
            mainPage.Visibility = Visibility.Collapsed;
            toDoPage.Visibility = Visibility.Collapsed;
            plannerPage.Visibility = Visibility.Collapsed;
            chatPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region PlannerPage
        private void bDel_Click(object sender, RoutedEventArgs e)
        {
            if (lb.Items.Count > 0)
            {
                if (lb.SelectedItem != null)
                {
                    if (lb.SelectedItems.Count != 0)
                    {
                        _proxy.RemovePlan(((PlanCardTemplate)lb.SelectedItem).planImage.Source.ToString(), ((PlanCardTemplate)lb.SelectedItem).planName.Text, ((PlanCardTemplate)lb.SelectedItem).notes.Text);
                        lb.Items.RemoveAt(lb.SelectedIndex);

                    }
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MaterialDesignThemes.Wpf.Card copy = XamlReader.Parse(XamlWriter.Save(test)) as MaterialDesignThemes.Wpf.Card;
            //lb.Items.Add(copy);

            PlanCard plan = new PlanCard(_proxy, _currentUser.Id);
            plan.Owner = this;
            plan.ShowDialog();

        }
        #endregion

        #region MenuBar
        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            _proxy.ExitClient(_userName);
            Close();
        }


        private void ColorZone_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void bChangeUser_Click(object sender, RoutedEventArgs e)
        {
            _proxy.ExitClient(_userName);
            RegistrationLoginForm regForm = new RegistrationLoginForm();
            regForm.Show();
            Close();

        }

        private void bMain_MouseEnter(object sender, MouseEventArgs e)
        {
            Color color = (Color)ColorConverter.ConvertFromString("#3f51b5");
            bMain.Foreground = new System.Windows.Media.SolidColorBrush(color);


        }

        private void bMain_MouseLeave(object sender, MouseEventArgs e)
        {
            bMain.Foreground = new SolidColorBrush(Colors.White);
        }
        #endregion

        #region SettingsPage

        private void butSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _currentUser.Image = op.FileName;
            }

            _proxy.UpdateProfile(_currentUser);
        }

        private void bProfieSettings_Click(object sender, RoutedEventArgs e)
        {

            _currentUser.FirstName = tbFN.Text;
            _currentUser.LastName = tbLN.Text;
            _currentUser.Password = tbPassword.Text;
            _proxy.UpdateProfile(_currentUser);
        }
        private void bProfileSocial_Click(object sender, RoutedEventArgs e)
        {
            _currentUser.Facebook = tbFacebook.Text;
            _currentUser.Twitter = tbTwitter.Text;
            _currentUser.Vk = tbVk.Text;
            _proxy.UpdateProfile(_currentUser);
        }


        #endregion

        #region FriendsListPage

        private void bAddFriend_Click(object sender, RoutedEventArgs e)
        {
            var addUser = _proxy.GetUser(tbFriendEmail.Text);
            if (addUser != null)
            {

                FriendCardTemplate friendCard = new FriendCardTemplate();
                friendCard.tbFriendCardName.Text = addUser.FirstName + " " + addUser.LastName;
                friendCard.tbFriendFacebook.Text = addUser.Facebook;
                friendCard.tbFriendCardEmail.Text = addUser.Email;
                friendCard.tbFriendTwitter.Text = addUser.Twitter;
                friendCard.tbFriendVk.Text = addUser.Vk;
                friendCard.tbFriendGithub.Text = addUser.Github;
                friendCard.bDelFriend.Click += BDelFriend_Click;
                ImageBrush myBrush = new ImageBrush();
                myBrush.Stretch = Stretch.UniformToFill;
                if (addUser.Image.Substring(0, 3) == "/Im")
                {
                    myBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,," + addUser.Image, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    myBrush.ImageSource = new BitmapImage(new Uri(@addUser.Image, UriKind.RelativeOrAbsolute));
                }
                friendCard.imageFriendCard.Fill = myBrush;

                if (_currentUser.Email != friendCard.tbFriendCardEmail.Text)
                {
                    bool isContains = false;
                    foreach (var item in lvFriends.Items)
                    {
                        if (((FriendCardTemplate)item).tbFriendCardEmail.Text == friendCard.tbFriendCardEmail.Text)
                        {
                            isContains = true;
                            break;
                        }
                    }
                    if (!isContains)
                    {

                        lvFriends.Items.Add(friendCard);
                        _proxy.AddFriend(addUser.Email, _currentUser.Id);
                    }
                    else
                    {

                        MessageBox.Show("Friend was added before!");
                    }
                }
                else
                {
                    MessageBox.Show("It's you!");
                }

            }
        }


        private void BDelFriend_Click(object sender, RoutedEventArgs e)
        {

            if (lvFriends.Items.Count > 0)
            {
                if (lvFriends.SelectedItem != null)
                {
                    string email = ((FriendCardTemplate)lvFriends.SelectedItem).tbFriendCardEmail.Text;

                    var removeUser = _proxy.GetUser(email);
                    lvFriends.Items.Remove(lvFriends.SelectedItem);
                    _proxy.RemoveFriend(removeUser.Email, _currentUser.Id);
                }
            }
        }

        public void UpdateFriendsList()
        {
            _currentUser.Friends = _proxy.GetFriendsList(_currentUser.Id);
        }

        private void tbFriendEmail_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {

                bAddFriend_Click(sender, e);
                Keyboard.ClearFocus();
            }
        }

        #endregion

        #region ToDoPage

        private void bAddToDoDate_Click(object sender, RoutedEventArgs e)
        {
            ToDoDayTemplate toDo = new ToDoDayTemplate(_proxy);
            toDo.tbToDoDate.Text = tbToDoDate.Text;
            toDo.tbToDoDateName.Text = tbToDoDateName.Text;
            _proxy.AddToDoDate(_currentUser.Id, tbToDoDate.Text, tbToDoDateName.Text);
            int id = _proxy.GetToDoDateId(_currentUser.Id, tbToDoDate.Text);
            toDo.tbToDoId.Text = id.ToString();
            lvToDo.Items.Add(toDo);
        }

        private void bDeleteToDoDate_Click(object sender, RoutedEventArgs e)
        {
            if (lvToDo.Items.Count > 0)
            {
                if (lvToDo.SelectedItem != null)
                {
                    string date = ((ToDoDayTemplate)lvToDo.SelectedItem).tbToDoDate.Text;
                    lvToDo.Items.Remove(lvToDo.SelectedItem);
                    _proxy.RemoveToDoDate(_currentUser.Id, date);
                }
            }
        }
        #endregion
    }
}
