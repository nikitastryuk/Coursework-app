using ConsoleServer.Classes;
using ExamMaterialWpf.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer
{
    [ServiceContract(CallbackContract = typeof(IClientCallback), SessionMode = SessionMode.Required)]
    public interface IService
    {
        [OperationContract]
        void AddToDoItem(int listId, bool isCheked, string name);

        [OperationContract]
        void RemoveToDoItem(int listId, string name);

        [OperationContract]
        void EditToDoItem(string name);

        [OperationContract]
        int GetToDoDateId(int userId, string date);

        [OperationContract]
        List<ToDoItem> GetToDoItemsList(int listId);

        [OperationContract]
        void AddFriend(string email, int userId);

        [OperationContract]
        void AddToDoDate(int userId, string date, string name);

        [OperationContract]
        void RemoveToDoDate(int userId, string date);

        [OperationContract]
        void RemoveFriend(string email, int userId);

        [OperationContract]
        void AddPlan(string image, string name, int importance, string note, string time, string date, int userId);

        [OperationContract]
        void RemovePlan(string image, string name, string note);

        [OperationContract]
        bool EnterClient(User user);

        [OperationContract]
        List<Client> GetClientList();

        [OperationContract]
        List<Friend> GetFriendsList(int userId);

        [OperationContract]
        List<Plan> GetPlansList(int userId);

        [OperationContract]
        List<ToDoDate> GetToDoDateList(int userId);

        [OperationContract]
        bool Send(string nameFrom, string message, string nameTo);

        [OperationContract]
        void ExitClient(string name);

        [OperationContract]
        void UpdateProfile(User user);

        [OperationContract]
        User SignIn(string email, string password);

        [OperationContract]
        User GetUser(string email);

        [OperationContract]
        bool RegisterUser(string firstName, string lastName, string email, string password);
    }
    public interface IClientCallback
    {
        [OperationContract]
        void UpdateListClients(List<Client> clients);

        [OperationContract]
        void UpdateChat(string nameFrom, string message, string image);

    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IService
    {
        List<Client> _clients = new List<Client>();

        #region RegistrationForm
        public bool EnterClient(User user)
        {
            foreach (Client client in _clients)
            {
                if (client.Name == user.FirstName + " " + user.LastName)
                    return false;
            }
            Client newClient = new Client { Id = ++Client.Counter, User = user, Name = user.FirstName + " " + user.LastName, Callback = OperationContext.Current.GetCallbackChannel<IClientCallback>() };
            _clients.Add(newClient);

            foreach (Client client in _clients)
            {
                if (client.Id != newClient.Id)
                {
                    client.Callback.UpdateListClients(_clients);
                }
            }
            Console.WriteLine(user.FirstName + " " + user.LastName + " entered chat");
            return true;

        }

        public User SignIn(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ToString()))
            {
                connection.Open();
                SqlCommand commandCheck = connection.CreateCommand();
                commandCheck.CommandText = String.Format("Select Count(*) FROM Users WHERE Email ='{0}' AND Password = '{1}'", email, password);
                int a = (int)commandCheck.ExecuteScalar();
                if (a > 0)
                {
                    using (UserContext db = new UserContext())
                    {
                        User signInUser = db.Users.First(user => user.Email == email && user.Password == password);

                        return signInUser;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public bool RegisterUser(string firstName, string lastName, string email, string password)
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ToString()))
            {
                connection.Open();
                bool isExist = false;
                SqlCommand commandCheckEmail = connection.CreateCommand();
                commandCheckEmail.CommandText = String.Format("Select Count(*) FROM Users WHERE Email =N'{0}'", email);
                int a = (int)commandCheckEmail.ExecuteScalar();
                if (a > 0)
                {
                    isExist = true;
                }
                if (!isExist)
                {
                    SqlCommand commandCheckUser = connection.CreateCommand();
                    commandCheckUser.CommandText = String.Format("Select Count(*) FROM Users WHERE Email =N'{0}' AND Password = N'{1}'", email, password);
                    int b = (int)commandCheckUser.ExecuteScalar();
                    if (b == 0)
                    {

                        using (UserContext db = new UserContext())
                        {
                            User newUser = new User { FirstName = firstName, LastName = lastName, Password = password, Email = email };

                            db.Users.Add(newUser);

                            db.SaveChanges();
                        }
                        Console.WriteLine("Пользователь зарегистрирован");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Пользователь с таким email уже зарегистрирован");
                    return false;
                }
            }
        }

        #endregion

        public void ExitClient(string name)
        {
            Client currentClient = _clients.First(a => a.Name == name);
            _clients.Remove(currentClient);
            foreach (Client client in _clients)
            {
                if (client.Id != currentClient.Id)
                {
                    client.Callback.UpdateListClients(_clients);
                }
            }
        }

        public void UpdateProfile(User user)
        {

            using (UserContext db = new UserContext())
            {
                var dbUser = db.Users.First(u => u.Email == user.Email);
                //var currentClient = _clients.First(a => a.User == dbUser).User;
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Password = user.Password;
                dbUser.Facebook = user.Facebook;
                dbUser.Github = user.Github;
                dbUser.Twitter = user.Twitter;
                dbUser.Vk = user.Vk;
                dbUser.Image = user.Image;
                db.SaveChanges();

                //foreach (Client client in _clients)
                //{
                //    if (client.User != currentClient)
                //    {
                //        client.Callback.UpdateListClients(_clients);
                //    }
                //}

            }



        }

        public List<Client> GetClientList()
        {
            return _clients;
        }

        public bool Send(string nameFrom, string message, string nameTo)
        {
            try
            {
                Client ClientT = _clients.First(a => a.Name == nameTo);
                Client ClientF = _clients.First(a => a.Name == nameFrom);
                ClientT.Callback.UpdateChat(nameFrom, message, ClientF.User.Image);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User GetUser(string email)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    var resUser = db.Users.First(user => user.Email == email);
                    return resUser;
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<Friend> GetFriendsList(int userId)
        {
            List<Friend> tmp;
            List<string> res = new List<string>();
            using (UserContext db = new UserContext())
            {
                tmp = db.Friends.Where(a => a.UserId == userId).ToList();
            }
            return tmp;
        }

        public void AddFriend(string email, int userId)
        {

            using (UserContext db = new UserContext())
            {
                Friend newFriend = new Friend() { UserId = userId, Email = email };
                db.Friends.Add(newFriend);
                db.SaveChanges();
            }
        }

        public void RemoveFriend(string email, int userId)
        {
            using (UserContext db = new UserContext())
            {
                var removeFriend = db.Friends.First(a => a.UserId == userId && a.Email==email);
                db.Friends.Remove(removeFriend);
                db.SaveChanges();
            }

        }

        public void AddPlan(string image, string name, int importance, string note, string time, string date, int userId)
        {
            using (UserContext db = new UserContext())
            {
                Plan newPlan = new Plan() { Image = image, Name = name, Importance = importance, Note = note, Time = time, Date = date, UserId = userId };
                db.Plans.Add(newPlan);
                db.SaveChanges();
            }
        }

        public void RemovePlan(string image, string name, string note)
        {
            using (UserContext db = new UserContext())
            {
                var removePlan = db.Plans.First(a => a.Image == image && a.Name == name && a.Note == note);
                db.Plans.Remove(removePlan);
                db.SaveChanges();
            }
        }

        public List<Plan> GetPlansList(int userId)
        {
            List<Plan> tmp;

            using (UserContext db = new UserContext())
            {
                tmp = db.Plans.Where(a => a.UserId == userId).ToList();
            }
            return tmp;
        }

        public List<ToDoDate> GetToDoDateList(int userId)
        {
            List<ToDoDate> tmp;

            using (UserContext db = new UserContext())
            {
                tmp = db.Dates.Where(a => a.UserId == userId).ToList();
            }
            return tmp;
        }

        public void AddToDoDate(int userId, string date, string name)
        {
            using (UserContext db = new UserContext())
            {
                ToDoDate newToDoDate = new ToDoDate() { UserId = userId, Date = date, Name = name };
                db.Dates.Add(newToDoDate);
                db.SaveChanges();
            }
        }

        public void RemoveToDoDate(int userId, string date)
        {
            using (UserContext db = new UserContext())
            {
                var removeDate = db.Dates.First(a => a.UserId == userId && a.Date == date);
                foreach (var item in db.ToDoItems)
                {
                    if (item.ToDoDateId == removeDate.Id)
                    {
                        db.ToDoItems.Remove(item);
                    }
                }

                db.Dates.Remove(removeDate);
                db.SaveChanges();
            }
        }

        public void AddToDoItem(int listId, bool isCheked, string name)
        {
            using (UserContext db = new UserContext())
            {
                ToDoItem newToDoItem = new ToDoItem() { IsChecked = isCheked, ToDoDateId = listId, Name = name };
                db.ToDoItems.Add(newToDoItem);
                db.SaveChanges();
            }
        }

        public void RemoveToDoItem(int listId, string name)
        {
            using (UserContext db = new UserContext())
            {
                var removeItem = db.ToDoItems.First(a => a.ToDoDateId == listId && a.Name == name);
                db.ToDoItems.Remove(removeItem);
                db.SaveChanges();
            }
        }

        public List<ToDoItem> GetToDoItemsList(int listId)
        {
            List<ToDoItem> tmp;

            using (UserContext db = new UserContext())
            {
                tmp = db.ToDoItems.Where(a => a.ToDoDateId == listId).ToList();
            }
            return tmp;
        }

        public int GetToDoDateId(int userId, string date)
        {
            List<ToDoDate> tmp;


            using (UserContext db = new UserContext())
            {
                tmp = db.Dates.Where(a => a.UserId == userId && a.Date == date).ToList();

            }
            return tmp[0].Id;
        }

        public void EditToDoItem(string name)
        {
            using (UserContext db = new UserContext())
            {
                var editItem = db.ToDoItems.First(a => a.Name == name);
                editItem.IsChecked = !editItem.IsChecked;
                db.SaveChanges();
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Service), new Uri("http://localhost/server")))
            {
                host.AddServiceEndpoint(typeof(IService), new WSDualHttpBinding(), "");
                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                behavior.HttpGetEnabled = true;
                host.Description.Behaviors.Add(behavior);
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
                host.Open();

                //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ToString()))
                //{
                //    connection.Open();

                //    using (UserContext db = new UserContext())
                //    {
                //        User newUser = new User { FirstName = "1", LastName = "2", Password = "1", Email = "test1" };

                //        db.Users.Add(newUser);
                //        db.SaveChanges();
                //    }
                //}


                Console.WriteLine("Служба запущена. Для остановки нажмите любую клавишу...");
                Console.ReadKey(true);
                Console.WriteLine("Служба остановлена");
            }
        }
    }
}
