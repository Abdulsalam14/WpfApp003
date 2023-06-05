using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp003
{
    public class Car
    {
        public Car(string marka, string model, int year, string stNumber)
        {
            Marka = marka;
            Model = model;
            Year = year;
            StNumber = stNumber;
        }

        public int Id { get; set; }

        public string Marka { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string StNumber { get; set; }
    }
}
