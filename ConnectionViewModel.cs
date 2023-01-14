using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace WPF_XpathBrowser
{
    public class ConnectionViewModel : INotifyPropertyChanged
    {

        private readonly CollectionView _years;
        private int _MyYear;
        private ObservableCollection<Registro> _Rows { get; set; }
        public ConnectionViewModel()
        {
            IList<int> l = new List<int>();
            foreach (var i in Enumerable.Range(2000, 81).ToList()) l.Add(i);
            _years = new CollectionView(l);
            _MyYear = DateTime.Now.Year;
        }
        public CollectionView years
        {
            get { return _years; }
        }
        public int MyYear
        {
            get { return _MyYear; }
            set
            {
                _MyYear = value;
                OnPropertyChanged("MyYear");
            }
        }
        public ObservableCollection<Registro> Rows
        {
            get
            {
                return _Rows;
            }

            set
            {
                _Rows = value;
                OnPropertyChanged("Rows");
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class Registro
    {
        public String fecha { get; set; }
        public String hora { get; set; }
        public String origen { get; set; }
        public String evento { get; set; }
    }
}
