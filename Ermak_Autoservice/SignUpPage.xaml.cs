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

namespace Ermak_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private Service _currentService = new Service();
        private ClientService _currentClientService = new ClientService();

        public Page1( Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;
            DataContext = _currentService;

            var _currentClient = Ermak_autoserviceEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                Ermak_autoserviceEntities.GetContext().ClientService.Add(_currentClientService);
            try
            {
                Ermak_autoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            string filteredText = new string(s.Where(c => char.IsDigit(c) || c == ':').ToArray());
            if(s != filteredText)
            {
                TBEnd.Text = "";
                TBStart.Text = TBStart.Text.Substring(TBStart.Text.Length);
                return;

            }

            if (s.Length < 5 || !s.Contains(':'))
            { TBEnd.Text = ""; return; }

            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());

                if (startHour/60 > 23 || startMin > 59)
                {
                    MessageBox.Show("Введите действительное время");
                    TBStart.Clear();
                    return;
                }

                int sum = startHour + startMin + Convert.ToInt32(_currentService.Duration);

                int EndHour = sum / 60;
                int EndMin = sum % 60;
                s = EndHour.ToString() + ":" + EndMin.ToString("D2");
                TBEnd.Text = s;

            }
        }
    }
}
