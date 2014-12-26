using Cirrious.MvvmCross.ViewModels;

namespace Kangou.Core.Helpers
{


    public class Location
        : MvxNotifyPropertyChanged
    {
        private double _lat = 0.0;
        public double Lat 
        {   
            get { return _lat; }
            set { _lat = value; RaisePropertyChanged(() => Lat); }
        }

        private double _lng = 0.0;
        public double Lng 
        {   
            get { return _lng; }
            set { _lng = value; RaisePropertyChanged(() => Lng); }
        }

        public override bool Equals(object obj)
        {
            var lRhs = obj as Location;
            if (lRhs == null)
                return false;

            return lRhs.Lat == Lat && lRhs.Lng == Lng;
        }

        public override int GetHashCode()
        {
            return Lat.GetHashCode() + Lng.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0:0.00000} {1:0.00000}", Lat, Lng);
        }
    }
}