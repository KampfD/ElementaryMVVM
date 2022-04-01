using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElementaryMVVM;
using ElementaryMVVM.Services;

namespace ElementaryMVVM.Test.MVVM.ViewModels
{
    class SecondWindowViewModel : ObservableObject
    {
        private ServiceLocator services;

        private RelayCommand _windowLoaded;
        private RelayCommand _windowClosed;
        private string _text = "Текст по умолчанию";

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(() => Text); }
        }

        public RelayCommand WindowLoaded
        {
            get
            {
                return _windowLoaded ?? (_windowLoaded = new RelayCommand(obj =>
                {
                    services.Messenger.Register<string>(this, "token1", m => Text = m);
                }));
            }
        }

        public RelayCommand WindowClosed
        {
            get
            {
                return _windowClosed ?? (_windowClosed = new RelayCommand(obj =>
                {
                    services.Messenger.Unregister(this, "token1");
                }));
            }
        }

        public SecondWindowViewModel(ServiceLocator services)
        {
            this.services = services;
        }
    }
}
