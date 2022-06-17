using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Umsetzung_II
{
    public class ViewModel : INotifyPropertyChanged
    {

        // INotifyPropertyChanged notifies the View of property changes, so that Bindings are updated.

        private Model user;
        private int clickCounter;

        public string FirstName
        {
            get { return user.FirstName; }
            set
            {
                if (user.FirstName != value)
                {
                    user.FirstName = value;
                    OnPropertyChange("FirstName");

                }
            }
        }

        public string LastName
        {
            get { return user.LastName; }
            set
            {
                if (user.LastName != value)
                {
                    user.LastName = value;
                    OnPropertyChange("LastName");

                }
            }
        }

        // This property is an example of how model properties can be presented differently to the View.
        // In this case, we transform the birth date to the user's age, which is read only.
        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - user.BirthDate.Year;
                if (user.BirthDate > today.AddYears(-age)) age--;
                return age;
            }
        }

        public int ClickCounter
        {
            get
            {
                return clickCounter;
            }
            set
            {
                if (clickCounter != value)
                {
                    clickCounter = value;
                    OnPropertyChange("ClickCounter");

                }
            }
        }

        public ICommand KnopfCommand { get; }

        public ViewModel()
        {
            user = new Model
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddYears(-30)
            };
            clickCounter = 0;
            KnopfCommand = new KlickCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
