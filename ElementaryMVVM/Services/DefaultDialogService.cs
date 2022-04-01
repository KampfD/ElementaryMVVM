using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace ElementaryMVVM.Services
{
    public class DefaultDialogService : IDialogService
    {
        public string FileName { get; set; }
        public string FolderPath { get; set; }
        public string[] FileNames { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса DefaultDialogService.
        /// </summary>
        public DefaultDialogService() { }

        public bool OpenFileDialog(string title = "Открыть файл", string filter = "Все файлы (*.*)|*.*" )
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = title,
                InitialDirectory = FileName,
                Filter = filter
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool OpenFilesDialog(string title = "Открыть файл", string filter = "Все файлы (*.*)|*.*")
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = title,
                Multiselect = true,
                InitialDirectory = FileName,
                Filter = filter
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileNames = openFileDialog.FileNames;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog(string title = "Сохранить файл как", string filter = "Все файлы (*.*)|*.*")
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = title,
                InitialDirectory = FileName,
                Filter = filter
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                FileName = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool FolderSelectionDialog(string description = "Выбор директории.")
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog()
            {
                Description = description,
                SelectedPath = FolderPath
            };
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath = fbd.SelectedPath;
                return true;
            }
            else return false;
        }

        public UserResponse ShowMessage(MessageType type, string message, string caption)
        {
            MessageBoxResult result;
            switch (type)
            {
                case MessageType.Error:
                    result = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case MessageType.Information:
                    result = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case MessageType.Question:
                    result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    break;
                case MessageType.Warning:
                    result = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case MessageType.WarningQuestion:
                    result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    break;
                default:
                    throw new ArgumentException("Такого типа сообщения не существует");
            }
            return (UserResponse)result;
        }
    }
}