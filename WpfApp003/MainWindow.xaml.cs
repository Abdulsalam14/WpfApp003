using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WpfApp003
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private Car car;

        public Car Carr
        {
            get { return car; }
            set
            {
                car = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Car> carss { get; set; } = new ObservableCollection<Car>();



        public ICommand AddCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            start();
            carss = getcars();
            AddCommand = new RelayCommand(AddCommandExecute, null);
            UpdateCommand = new RelayCommand(UpdateCommandExecute, UpdateCommandCanExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, DeleteCanExecute);

        }



        public void start()
        {
            using (var db = new CarContext())
            {
                List<Car> cars = new()
                {
                    new Car("BMW","X5",2022,"10-AA-111"),
                    new Car("Range","Rover",2021,"10-BB-222"),
                    new Car("Mercedes","C-Class",2023,"10-CC-333"),
                    new Car("Toyota","Prado",2019,"10-DD-444"),
                    new Car("Hyundai","Sonata",2022,"10-EE-555"),
                };

                bool b = true;
                foreach (Car item in cars)
                {
                    if (db.Cars.Any(c => c.StNumber == item.StNumber))
                    {
                        b = false;
                        break;
                    }

                }
                if (b)
                {
                    db.Cars.AddRange(cars);
                    db.SaveChanges();
                };
            }
        }

        public ObservableCollection<Car> getcars()
        {
            using (CarContext db = new())
            {
                carss.Clear();
                var dt = db.Cars.ToList();
                dt.ForEach(c => carss.Add(c));
            }
            return carss;
        }

        public void AddCommandExecute(object parameter)
        {
            Car temp = null;
            if (!string.IsNullOrEmpty(markabx.Text) && !string.IsNullOrEmpty(modelbx.Text) && !string.IsNullOrEmpty(yearbx.Text) && !string.IsNullOrEmpty(stnmbrbx.Text))
            {
                try
                {
                    temp = new Car(markabx.Text, modelbx.Text, int.Parse(yearbx.Text), stnmbrbx.Text);
                    Carr = null;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Duzgun doldurulmayib");
                }

            }
            if (temp != null)
            {
                using (CarContext db = new())
                {
                    try
                    {
                        db.Cars.Add(temp);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Bu nomre istifade olunub");
                    }
                }
            }
            carss = getcars();

        }



        public void DeleteExecute(object parameter)
        {
            using (CarContext db=new())
            {
                Car car = db.Cars.Where(c => c.Equals(Carr)).First();
                db.Cars.Remove(car);
                db.SaveChanges();
                MessageBox.Show("Deleted");
                carss = getcars();
            }
        }

        public bool DeleteCanExecute(object parameter)
        {
            return Carr != null;
        }



        public void UpdateCommandExecute(object parameter)
        {
            using (var db = new CarContext())
            {
                var car = db.Cars.Where(c => c.Equals(Carr)).First();
                if (Carr.StNumber != car.StNumber || car.Marka != Carr.Marka || car.Year != Carr.Year || Carr.Model != car.Model)
                {
                    car.StNumber = Carr.StNumber;
                    car.Marka = Carr.Marka;
                    car.Year = Carr.Year;
                    car.Model = car.Model;
                    db.Cars.Update(car);
                    db.SaveChanges();
                    carss = getcars();
                    MessageBox.Show("Updated");
                }
                else MessageBox.Show("UpdateError");
            }
        }
        public bool UpdateCommandCanExecute(object parameter)
        {
            return Carr != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
