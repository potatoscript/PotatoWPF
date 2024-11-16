using PotatoWPF.ViewModels;

namespace PotatoWPF.Models
{
    public class PotatoModel: BaseViewModel
    {

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value, nameof(Id));
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        private string imagePath;
        public string ImageSource
        {
            get => imagePath;
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        }

        private int deleteable;
        public int Deleteable
        {
            get => deleteable;
            set
            {
                deleteable = value;
                OnPropertyChanged(nameof(Deleteable));
            }
        }

    }
}

