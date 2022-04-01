using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;

namespace ElementaryMVVM.Services
{
    /// <summary>
    /// Реализует сервис управления окнами в приложении.
    /// </summary>
    public class WindowService : IWindowService
    {        
        /// <summary>
        /// Инициализирует новый экземпляр класса WindowService.
        /// </summary>
        public WindowService() { }

        public void ShowWindow(Modality modality, string windowName, object viewModel)
        {
            string callingAssemblyName = Assembly.GetCallingAssembly().FullName.Split(',').First();
            var window = CreateWindow(callingAssemblyName, windowName);
            window.DataContext = viewModel;
            if (modality == Modality.Modal)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        public void ShowWindow(Modality modality, string windowName, string windowOwnerName, object viewModel)
        {
            string callingAssemblyName = Assembly.GetCallingAssembly().FullName.Split(',').First();
            var window = CreateWindow(callingAssemblyName, windowName);
            window.DataContext = viewModel;
            var ownerWindow = GetWindow(windowOwnerName);
            if (ownerWindow != null)
            {
                window.Owner = ownerWindow;
                if (modality == Modality.Modal)
                {
                    window.ShowDialog();
                }
                else
                {
                    window.Show();
                }
            }
            else
            {
                throw new ArgumentException(
                    "Окна-владельца с таким именем не существует.", 
                    "windowOwnerName");
            }
        }

        public void ShowWindowWithActiveOwner(Modality modality, string windowName, object viewModel)
        {
            string callingAssemblyName = Assembly.GetCallingAssembly().FullName.Split(',').First();
            var window = CreateWindow(callingAssemblyName, windowName);
            var ownerWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.DataContext = viewModel;
            window.Owner = ownerWindow;
            if (modality == Modality.Modal)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        public void CloseWindow(string windowName)
        {
            var window = GetWindow(windowName);
            if (window != null)
            {
                window.Close();
            }
            else
            {
                throw new ArgumentException(
                    "Окно с таким именем не было открыто или не существует.", 
                    "windowName");
            }
        }

        public bool CheckWindowExistence(string windowName)
        {
            var callingAssemblyName = Assembly.GetCallingAssembly().FullName.Split(',').First();
            string fullName = $"{callingAssemblyName}.MVVM.Views.{windowName}";
            return Application.Current.Windows.OfType<Window>().Any(w => w.ToString() == fullName);
        }

        private Window GetWindow(string windowName)
        {
            try
            {
                return Application.Current.Windows.OfType<Window>().SingleOrDefault(w =>
                {
                    string[] fullNameSegments = w.ToString().Split('.');
                    string name = fullNameSegments[fullNameSegments.Length - 1];
                    if (name == windowName)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            // Найдено более одного окна.
            catch (InvalidOperationException) 
            { 
                throw; 
            }
        }

        private Window CreateWindow(string callingAssemblyName, string windowName)
        {
            if (CheckWindowExistence(windowName))
            {
                throw new ArgumentException("Такое окно уже открыто.", "windowName");
            }
            string strType = $"{callingAssemblyName}.MVVM.Views.{windowName}, {callingAssemblyName}";
            var type = Type.GetType(strType);
            if (type == null)
            {
                throw new ArgumentException("Указанное имя не является именем представления.", "windowName");
            }
            return Activator.CreateInstance(type) as Window;
        }
    }
}
