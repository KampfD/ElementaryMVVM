using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElementaryMVVM.Services
{
    /// <summary>
    /// Реализует сервис локатор для сервисов, обеспечивающих поддержку MVVM.
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// Сервис управления окнами в приложении.
        /// </summary>
        public IWindowService WindowService { get; private set; }

        /// <summary>
        /// Сервис, обеспечивающий диалог с пользователем через диалоговые окна.
        /// </summary>
        public IDialogService DialogService { get; private set; }

        /// <summary>
        /// Сервис обмена сообщениями между объектами по шине.
        /// </summary>
        public IMessenger Messenger { get; private set; }

        /// <summary>
        /// Инициализурует новый экземпляр класса ServiceLocator.
        /// </summary>
        /// <param name="windowService">Сервис управления окнами в приложении.</param>
        /// <param name="dialogService">Сервис, обеспечивающий диалог с пользователем через диалоговые окна.</param>
        /// <param name="messenger">Сервис обмена сообщениями между объектами по шине.</param>
        public ServiceLocator(IWindowService windowService, IDialogService dialogService, IMessenger messenger) 
        {
            WindowService = windowService;
            DialogService = dialogService;
            Messenger = messenger;
        }
    }
}
