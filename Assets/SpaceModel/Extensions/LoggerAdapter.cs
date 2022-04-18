using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SpaceModel.Extensions
{
    /// <summary>
    /// Переходник (прослойка), через который модель может передавать 
    /// логи (сообщения) любому движку.
    /// </summary>
    public class LoggerAdapter
    {
        #region Singleton

        private LoggerAdapter() { }
        private static LoggerAdapter instancePrivate = null;
        public static LoggerAdapter instance
        {
            get
            {
                if (instancePrivate == null)
                {
                    instancePrivate = new LoggerAdapter();
                }

                return instancePrivate;
            }
        }

        #endregion Singleton

        #region Message methods delegates

        /// <summary>
        /// Метод для печати сообщения.
        /// </summary>
        private Action<String> onLogMessage;
        /// <summary>
        /// Метод для печати предупреждения.
        /// </summary>
        private Action<String> onWarningMessage;
        /// <summary>
        /// Метод для печати ошибки.
        /// </summary>
        private Action<String> onErrorMessage;

        #endregion Message methods delegates

        #region Set deleates

        /// <summary>
        /// Установить логеру метод для печати сообщения.
        /// </summary>
        /// <param name="onLogMessage">Метод для печати сообщения.</param>
        public void SetLogMessageMethod(Action<String> onLogMessage)
        {
            this.onLogMessage = onLogMessage;
        }
        /// <summary>
        /// Установить логеру метод для печати предупреждения
        /// </summary>
        /// <param name="onWarningMessage"> Метод для печати предупреждения.</param>
        public void SetWarningMessageMethod(Action<String> onWarningMessage)
        {
            this.onWarningMessage = onWarningMessage;
        }
        /// <summary>
        /// Установить логеру метод для печати ошибки.
        /// </summary>
        /// <param name="onErrorMessage">Метод для печати ошибки.</param>
        public void SetErrorMessageMethod(Action<String> onErrorMessage)
        {
            this.onErrorMessage = onErrorMessage;
        }

        #endregion Set deleates

        #region Message methods

        /// <summary>
        /// Сцепить в строку все строковые представления объектов.
        /// </summary>
        /// <param name="objs"></param>
        private String ConcatenateStrings(object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object obj in objs)
            {
                sb.Append(obj.ToString());  
            }
            return sb.ToString();
        }
        /// <summary>
        /// Напечатать сообщение.
        /// </summary>
        /// <param name="text"></param>
        public void LogMessage(String text)
        {
            this.onLogMessage?.Invoke(text);
        }
        /// <summary>
        /// Напечатать сообщение.
        /// </summary>
        /// <param name="text"></param>
        public void LogMessage(params object[] objs)
        {
            this.onLogMessage?.Invoke(ConcatenateStrings(objs));
        }
        /// <summary>
        /// Напечатать предупреждение.
        /// </summary>
        /// <param name="text"></param>
        public void WarningMessage(String text)
        {
            this.onWarningMessage?.Invoke(text);
        }
        /// <summary>
        /// Напечатать предупреждение.
        /// </summary>
        /// <param name="text"></param>
        public void WarningMessage(params object[] objs)
        {
            this.onWarningMessage?.Invoke(ConcatenateStrings(objs));
        }
        /// <summary>
        /// Напечатать ошибку.
        /// </summary>
        /// <param name="text"></param>
        public void ErrorMessage(String text)
        {
            this.onErrorMessage?.Invoke(text);
        }
        /// <summary>
        /// Напечатать ошибку.
        /// </summary>
        /// <param name="text"></param>
        public void ErrorMessage(params object[] objs)
        {
            this.onErrorMessage?.Invoke(ConcatenateStrings(objs));
        }

        #endregion Message methods
    }
}
