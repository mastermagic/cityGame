using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using VkNet;
using VkNet.Categories;
using VkNet.Utils;
using VkNet.Model;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace WindowsFormsVk
{
    public partial class Form1 : Form
    {
        string login;
        string pass;
        ulong appId;
        public Form1()
        {
            InitializeComponent();
            
            try
            {
                string accDatapath = @"acc.data";
                using (StreamReader sr = new StreamReader(accDatapath, System.Text.Encoding.Default))
                {
                    login = sr.ReadLine();
                    pass = sr.ReadLine();
                    appId = UInt64.Parse(sr.ReadLine());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения файла данных об аккаунте");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var api = new VkApi();
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = appId,
                Login = login,
                Password = pass,
                Settings = Settings.All
            }); // авторизуемся

            //var group = api.Utils.ResolveScreenName("leningradka63"); // получаем id сущности с коротким именем habr

            // получаем данные пользователей из группы, макс. кол-во записей = 1000
            var parameters = new GroupsGetMembersParams();
            parameters.GroupId = "magicstoresamara";
            int signedCount = 0;
            //parameters.GroupId = "antiterror_russia";
            parameters.Fields = UsersFields.City;
            var userIds = api.Groups.GetMembers(parameters);
            Thread.Sleep(340);
            foreach (User id in userIds)
            {
                try
                {
                    if (api.Groups.IsMember("happybabymom", id.Id))
                        signedCount++;
                    label1.Text = Convert.ToString(signedCount + " / " + userIds.Count);
                    Thread.Sleep(340);
                    //listBox1.Items.Add(totalCount + " " + id.City.Title + " " + id.Sex);
                }
                catch (Exception) { }
            }
            MessageBox.Show(signedCount + " / " + userIds.Count);
            
            /*
            // Информация о группе
            var groups = api.Groups.GetById("magicstoresamara", GroupsFields.All);
            listBox1.Items.Add(groups.Contacts.First().UserId);
            listBox1.Items.Add(groups.Name);

            // Является ли участником группы
            label1.Text =  Convert.ToString(api.Groups.IsMember("magicstoresamara", 153355038));
            */
        }
    }
}
