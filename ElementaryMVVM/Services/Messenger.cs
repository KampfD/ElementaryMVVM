using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ElementaryMVVM.Services
{
    /// <summary>
    /// Реализует шину обмена сообщениями между объектами.
    /// </summary>
    public class Messenger : IMessenger
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Messenger.
        /// </summary>
        public Messenger() { }

        /// <summary>
        /// Словарь, содержищий зарегистрированные в шине сообщения.
        /// Key - экземпляр структуры RecipientAndToken.
        /// Value - ссылка на делегат, ассоциированный с ключём.
        /// </summary>
        private readonly ConcurrentDictionary<RecipientAndToken, object> registeredMessages =
            new ConcurrentDictionary<RecipientAndToken, object>();

        /// <summary>
        /// Регистрирует получателя и токен для сообщения типа TMessage, 
        /// а также делегат получателя, который будет вызван при успешном приёме сообщения из шины.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="recipient">Объект-получатель сообщения.</param>
        /// <param name="token">Токен сообщения.</param>
        /// <param name="action">Делегат, который будет вызван в объекте-получателе при получении сообщения.</param>
        /// <returns>true - если сообщение успешно зарегистрировано, false - если нет.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается если один из аргументов равен null.</exception>
        public bool Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            if (recipient == null)
            {
                throw new ArgumentNullException("recipient", "Параметр recipient не может быть null.");
            }
            if (token == null)
            {
                throw new ArgumentNullException("token", "Параметр token не может быть null.");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action", "Параметр action не может быть null.");
            }
            var key = new RecipientAndToken(recipient, token);
            return registeredMessages.TryAdd(key, action);
        }

        /// <summary>
        /// Отправляет сообщение всем зарагистрированным получателям, 
        /// при условии совпадения типа сообщения и токена.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="message">Сообщение.</param>
        /// <param name="token">Токен сообщения.</param>
        /// <returns>true - если хотябы одно сообщение было отправлено, false - если нет.</returns>
        public bool Send<TMessage>(TMessage message, object token)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message", "Параметр message не может быть null.");
            }
            if (token == null)
            {
                throw new ArgumentNullException("token", "Параметр token не может быть null.");
            }
            bool wasSended = false;
            var neededMessages = registeredMessages.Where(r => r.Key.Token.Equals(token));
            foreach (var action in neededMessages.Select(x => x.Value).OfType<Action<TMessage>>())
            {
                action(message);
                wasSended = true;
            }
            return wasSended;
        }

        /// <summary>
        /// Выполняет отправку сообщения всем зарагистрированным получателям, 
        /// при условии совпадения типа сообщения и токена, в отдельном потоке.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="message">Сообщение.</param>
        /// <param name="token">Токен сообщения.</param>
        public void BeginSend<TMessage>(TMessage message, object token)
        {
            Task.Factory.StartNew(() => Send(message, token));
        }

        /// <summary>
        /// Отменяет регистрацию сообщения в шине.
        /// </summary>
        /// <param name="recipient">Объект-получатель сообщения.</param>
        /// <param name="token">Токен сообщения.</param>
        /// <returns>true - если регистрация сообщения успешно отменена, false - если нет.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается если один из аргументов равен null.</exception>
        public bool Unregister(object recipient, object token)
        {
            if (recipient == null)
            {
                throw new ArgumentNullException("recipient", "Параметр recipient не может быть null.");
            }
            if (token == null)
            {
                throw new ArgumentNullException("token", "Параметр token не может быть null.");
            }
            var key = new RecipientAndToken(recipient, token);
            return registeredMessages.TryRemove(key, out object action);
        }

        /// <summary>
        /// Представляет пару получатель-токен, харатеризующую сообщение в шине.
        /// </summary>
        private struct RecipientAndToken
        {
            /// <summary>
            /// Инициализирует новый экземпляр структуры RecipientAndToken.
            /// </summary>
            /// <param name="recipient">Объект-получатель сообщения.</param>
            /// <param name="token">Токен (идентификатор) сообщения.</param>
            public RecipientAndToken(object recipient, object token)
            {
                Recipient = recipient;
                Token = token;
            }

            /// <summary>
            /// Объект-получатель сообщения.
            /// </summary>
            public object Recipient;

            /// <summary>
            /// Токен (идентификатор) сообщения.
            /// </summary>
            public object Token;
        }
    }
}