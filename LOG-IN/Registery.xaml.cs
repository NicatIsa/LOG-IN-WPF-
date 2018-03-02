using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;

namespace LOG_IN
{
    /// <summary>
    /// Interaction logic for Registery.xaml
    /// </summary>
    public partial class Registery : Window
    {
        
        static bool enter = true;
        public static List<Person> list = new List<Person>();
        public Registery()
        {
            InitializeComponent();
            comboboxCountry.Items.Add("Azerbaijan");
            comboboxCountry.Items.Add("Russian");
            comboboxCountry.Items.Add("England");
            comboboxGender.Items.Add("Male");
            comboboxGender.Items.Add("Female");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (! enter && comboboxCountry.SelectedItem != null && comboboxGender.SelectedItem != null)
            {
                Person reg = new Person();
                reg.Name = textboxName.Text;
                reg.Surname = textboxSurname.Text;
                reg.Gender = comboboxGender.SelectedItem.ToString();
                reg.Country = comboboxCountry.SelectedItem.ToString();
                reg.Username = textboxUsername.Text;
                reg.Password = textboxPassword.Password;
                reg.Email = textboxEmail.Text;
                list.Add(reg);
            }
            else
            {
                MessageBox.Show("Melumatlar tam daxil edilmemishdir");
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Person>));
            using (FileStream fs = new FileStream("persons.xml",FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, list);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ListPersons listPersons = new ListPersons(list);
            listPersons.Show();
        }

        private void textboxUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            if ((!txt.Text.All(x => char.IsLetter(x)) || string.IsNullOrWhiteSpace(txt.Text)) && txt.Name != textboxEmail.Name)
            {
                enter = true;
                txt.BorderBrush = Brushes.Red;
                txt.Text = null;
            }

            else
            {
                enter = false;
                txt.BorderBrush = SystemColors.ControlBrush;
            }

            if (txt.Name == textboxEmail.Name)
            {
                Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                     @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

                if (!regex.IsMatch(txt.Text))
                {
                    textboxEmail.Text = "Mail format incorrect";
                }
            }
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            StringBuilder sBuilder = new StringBuilder();
            
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            
            return sBuilder.ToString();
        }

        private void textboxPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            var pass = sender as PasswordBox;
            if (string.IsNullOrWhiteSpace(pass.Password))
            {
                enter = true;
                MessageBox.Show("Olmaz");
                pass.BorderBrush = Brushes.Red;
                pass.Password = null;
            }
            else
            {
                MD5 md5 = MD5.Create();
                pass.Password = GetMd5Hash(md5, pass.Password);

                MessageBox.Show(pass.Password);
            }
        }
        public void Deserialize()
        {
            XmlSerializer xmldeSerializer = new XmlSerializer(typeof(List<Person>));

            using (FileStream fs = new FileStream("person.xml",FileMode.Open))
            {
                list = (List<Person>)xmldeSerializer.Deserialize(fs);

                foreach (var item in list)
                {
                    MessageBox.Show(item.Password);
                }
            }
        }
    }
}