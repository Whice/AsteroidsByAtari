using System;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Интерфейс модели для отправки сообщений.
    /// </summary>
    public interface IModelLogger
    {
        /// <summary>
        /// Напечатать сообщение.
        /// </summary>
        /// <param name="text"></param>
        public void LogMessage(String text);
        /// <summary>
        /// Напечатать сообщение.
        /// </summary>
        /// <param name="text"></param>
        public void LogMessage(params object[] objs);
        /// <summary>
        /// Напечатать предупреждение.
        /// </summary>
        /// <param name="text"></param>
        public void WarningMessage(String text);
        /// <summary>
        /// Напечатать предупреждение.
        /// </summary>
        /// <param name="text"></param>
        public void WarningMessage(params object[] objs);
        /// <summary>
        /// Напечатать ошибку.
        /// </summary>
        /// <param name="text"></param>
        public void ErrorMessage(String text);
        /// <summary>
        /// Напечатать ошибку.
        /// </summary>
        /// <param name="text"></param>
        public void ErrorMessage(params object[] objs);
    }
}
