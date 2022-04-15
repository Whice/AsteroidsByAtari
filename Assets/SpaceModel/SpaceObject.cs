using System;

namespace SpaceModel
{
    /// <summary>
    /// Общий класс игровых объектов.
    /// </summary>
    /// <typeparam name="Vector">Тип данных, отвечающий за положение в пространстве.
    /// <br/>Это может быть Vector2 или Vector3 в Unity. А может быть Vec3 в Unigine, к примеру.</typeparam>
    public abstract class SpaceObject<Vector>
    {
        public SpaceObject(Vector direction, Vector position)
        {
            this.direction = direction;
            this.position = position;
        }

        #region Изменение свойств.

        /// <summary>
        /// Вызывается, когда проиходит изменение в свойстве с вызовом метода SetValueProperty.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="oldValue">Старое значение.</param>
        /// <param name="newValue">Новое значение.</param>
        protected virtual void OnChanged(String propertyName, object oldValue, object newValue)
        {
            switch (propertyName)
            {
                case nameof(this.hp):
                    {
                        if (this.hp <= 0)
                        {
                            this.OnDestroed?.Invoke();
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Установить новое значение для свойства.
        /// После ввода данных задействовать метод OnChanged.
        /// </summary>
        /// <typeparam name="T">Тип изменяемых данных.</typeparam>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="field">Поле, куда помещается значение.</param>
        /// <param name="newValue">Новое значение.</param>
        protected void SetValueProperty<T>(String propertyName, ref T field, T newValue)
        {
            //Если оба неназначены, то ничего не поменялось
            if (field is null && newValue is null)
            {
                return;
            }

            //Если поля не равны
            if (!(field is null) && !field.Equals(newValue))
            {
                T oldValue = field;
                field = newValue;
#pragma warning disable CS8604
                OnChanged(propertyName, oldValue, newValue);
#pragma warning restore CS8604
            }
        }

        #endregion Изменение свойств.

        #region Очки жизни.

        /// <summary>
        /// Количество очков жизни.
        /// </summary>
        protected Int32 hpProtected = 1;
        /// <summary>
        /// Количество очков жизни.
        /// </summary>
        public Int32 hp
        {
            get
            {
                return this.hpProtected;
            }
            set
            {
                SetValueProperty(nameof(this.hp), ref this.hpProtected, value);
            }
        }

        /// <summary>
        /// Событие, которое происходит, когда объект уничтожается.
        /// <br/>Объект уничтожется, когда его <see cref="hp"/> становиться меньше или равным 0.
        /// </summary>
        public event Action? OnDestroed;

        #endregion Очки жизни.

        #region Положение в пространстве.

        /// <summary>
        /// Направление полета.
        /// </summary>
        protected Vector direction;
        /// <summary>
        /// Местоположение в пространстве.
        /// </summary>
        protected Vector position;

        #endregion Положение в пространстве.
    }
}
