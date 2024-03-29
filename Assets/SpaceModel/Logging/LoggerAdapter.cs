﻿using System;
using System.Text;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Переходник (прослойка), через который модель может передавать 
    /// логи (сообщения) любому движку.
    /// </summary>
    internal class LoggerAdapter : IModelLogger
    {
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
        /// Добавить логеру метод для печати сообщения.
        /// </summary>
        /// <param name="onLogMessage">Метод для печати сообщения.</param>
        public void AddLogMessageMethod(Action<String> onLogMessage)
        {
            this.onLogMessage += onLogMessage;
        }
        /// <summary>
        /// Удалить логеру метод для печати сообщения.
        /// </summary>
        /// <param name="onLogMessage">Метод для печати сообщения.</param>
        public void RemoveLogMessageMethod(Action<String> onLogMessage)
        {
            this.onLogMessage -= onLogMessage;
        }
        /// <summary>
        /// Добавить логеру метод для печати предупреждения
        /// </summary>
        /// <param name="onWarningMessage"> Метод для печати предупреждения.</param>
        public void AddWarningMessageMethod(Action<String> onWarningMessage)
        {
            this.onWarningMessage += onWarningMessage;
        }
        /// <summary>
        /// Удалить логеру метод для печати предупреждения
        /// </summary>
        /// <param name="onWarningMessage"> Метод для печати предупреждения.</param>
        public void RemoveWarningMessageMethod(Action<String> onWarningMessage)
        {
            this.onWarningMessage -= onWarningMessage;
        }
        /// <summary>
        /// Добавить логеру метод для печати ошибки.
        /// </summary>
        /// <param name="onErrorMessage">Метод для печати ошибки.</param>
        public void AddErrorMessageMethod(Action<String> onErrorMessage)
        {
            this.onErrorMessage += onErrorMessage;
        }
        /// <summary>
        /// Удалить логеру метод для печати ошибки.
        /// </summary>
        /// <param name="onErrorMessage">Метод для печати ошибки.</param>
        public void RemoveErrorMessageMethod(Action<String> onErrorMessage)
        {
            this.onErrorMessage -= onErrorMessage;
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
                sb.Append(" ");
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
