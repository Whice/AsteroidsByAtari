using Assets.SpaceModel.Extensions;
using System;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Класс модели для отправки сообщений.
    /// </summary>
    public abstract class ModelLogger
    {
        #region Logging

        /// <summary>
        /// Напечатать сообщение.
        /// </summary>
        /// <param name="text"></param>
        protected void LogMessage(String text)
        {
            LoggerAdapter.instance.LogMessage(text);
        }
        /// <summary>
        /// Напечатать предупреждение.
        /// </summary>
        /// <param name="text"></param>
        protected void WarningMessage(String text)
        {
            LoggerAdapter.instance.WarningMessage(text);
        }
        /// <summary>
        /// Напечатать ошибку.
        /// </summary>
        /// <param name="text"></param>
        protected void ErrorMessage(String text)
        {
            LoggerAdapter.instance.ErrorMessage(text);
        }

        #endregion Logging
    }
}
