using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ElementaryMVVM;
using ElementaryMVVM.Services;

namespace ElementaryMVVM.Test.MVVM.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private ServiceLocator services;
        private int counter = 0;

        private RelayCommand _sendMessageToSecondWindow;
        private RelayCommand _openSecondWindow;
        private RelayCommand _openSecondWindowModal;
        private RelayCommand _changeProperty;
        private string _buttonText = "Кнопка со счётчиком";

        public RelayCommand SendMessageToSecondWindow
        {
            get
            {
                return _sendMessageToSecondWindow ?? (_sendMessageToSecondWindow = new RelayCommand(obj =>
                {
                    //messenger.BeginSend("Сообщение доставлено", "token1");
                    if (!services.Messenger.Send("Сообщение доставлено", "token1"))
                    {
                        services.DialogService.ShowMessage(
                            MessageType.Warning,
                            "Указанное сообщение не зарегистрировано в шине.",
                            "Сообщение не доставлено");
                    }
                }));
            }
        }

        public RelayCommand OpenSecondWindow
        {
            get
            {
                return _openSecondWindow ?? (_openSecondWindow = new RelayCommand(obj =>
                {
                    if (!services.WindowService.CheckWindowExistence("SecondWindow"))
                    {
                        services.WindowService.ShowWindow(
                            Modality.Parallel, 
                            "SecondWindow", 
                            new SecondWindowViewModel(services));
                    }
                    else
                    {
                        services.DialogService.ShowMessage(
                            MessageType.Warning,
                            "Это окно уже открыто и будет закрыто.",
                            "Предупреждение");
                        services.WindowService.CloseWindow("SecondWindow");
                    }
                }));
            }
        }

        public RelayCommand OpenSecondWindowModal
        {
            get
            {
                return _openSecondWindowModal ?? (_openSecondWindowModal = new RelayCommand(obj =>
                {
                    services.WindowService.ShowWindow(
                        Modality.Modal, 
                        "SecondWindow", 
                        "MainWindow",
                        new SecondWindowViewModel(services));
                }));
            }
        }

        public RelayCommand ChangeProperty
        {
            get
            {
                return _changeProperty ?? (_changeProperty = new RelayCommand(obj =>
                {
                    ButtonText = string.Format("Кнопка нажата {0} раз(а)", ++counter);
                }));
            }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel(ServiceLocator services)
        {
            this.services = services;
        }
    }
}
