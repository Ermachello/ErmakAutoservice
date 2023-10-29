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
    /// Логика взаимодействия для AddEdtPage.xaml
    /// </summary>
    public partial class AddEdtPage : Page
    {

        private Service _currentServise = new Service();
        bool IsEditing = false;
        public AddEdtPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
            {
                _currentServise = SelectedService;
                IsEditing = true;
            }
            
            DataContext = _currentServise;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentServise.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentServise.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (_currentServise.Discount < 0 || _currentServise.Discount > 100)
                errors.AppendLine("Укажите скидку от 0 до 100");

            if (string.IsNullOrWhiteSpace(_currentServise.Duration))
                errors.AppendLine("Укажите длительность услуги");

            if (Convert.ToInt32(_currentServise.Duration) > 720)
                errors.AppendLine("Длительность не может быть больше 720 минут");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = Ermak_autoserviceEntities.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentServise.Title).ToList();

            if (IsEditing)
            {
                if (allServices.Count == 2)
                {
                    MessageBox.Show("Уже существует такая услуга");
                    return;
                }
            }
            else
            {
                if (allServices.Count == 1)
                {
                    MessageBox.Show("Уже существует такая услуга");
                    return;
                }
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentServise.ID == 0)
            {
                Ermak_autoserviceEntities.GetContext().Service.Add(_currentServise);
            }
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
    }
}
