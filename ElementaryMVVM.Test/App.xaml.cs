using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ElementaryMVVM.Services;
using ElementaryMVVM.Test.MVVM.ViewModels;

namespace ElementaryMVVM.Test
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var sl = new ServiceLocator(
                new WindowService(),
                new DefaultDialogService(),
                new Messenger());
            sl.WindowService.ShowWindow(
                Modality.Parallel, 
                "MainWindow",
                new MainWindowViewModel(sl));
        }
    }
}
