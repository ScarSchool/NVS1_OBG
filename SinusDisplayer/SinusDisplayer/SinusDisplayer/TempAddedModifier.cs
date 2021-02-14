using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;

namespace SinusDisplayer
{
    public class ValueAddedNotifier : INotifyPropertyChanged
    {
        public SeriesCollection DataCollection { get; set; }

        public ObservableCollection<string> Labels { get; set; }


        public ValueAddedNotifier(string title)
        {
            DataCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = title,
                    PointGeometry = DefaultGeometries.Diamond,
                    PointGeometrySize = 10,
                    Values = new ChartValues<double>()
                }
            };
            Labels = new ObservableCollection<string>();
            Labels.CollectionChanged += Labels_CollectionChanged;
        }

        private void Labels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Labels));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
