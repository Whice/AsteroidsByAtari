using System;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Общий класс игровых объектов.
    /// </summary>
    public abstract class SpaceObject : ModelLogger
    {
        /// <summary>
        /// Тип этого объекта.
        /// </summary>
        public readonly SpaceObjectType type;

        public SpaceObject(SpaceObjectType type)
        {
            this.type = type;
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

        #region Столкновение.

        /// <summary>
        /// Столкнуть этот объект с указаным объектом.
        /// </summary>
        /// <param name="spaceObject"></param>
        /// <returns>true, если надо совершить какое-то дополнительное действие
        /// в зависимости от типа одного из объектов.
        /// <br/>Какие именно действия будут совершаться, будут решать правила игры.</returns>
        public abstract Boolean CollideWithObject(SpaceObject spaceObject);

        #endregion Столкновение.

        #region Очки жизни.

        /// <summary>
        /// Количество очков жизни.
        /// </summary>
        private Int32 hpPrivate = 1;
        /// <summary>
        /// Количество очков жизни.
        /// </summary>
        public Int32 hp
        {
            get
            {
                return this.hpPrivate;
            }
            set
            {
                SetValueProperty(nameof(this.hp), ref this.hpPrivate, value);
            }
        }

        /// <summary>
        /// Событие, которое происходит, когда объект уничтожается.
        /// <br/>Объект уничтожется, когда его <see cref="hp"/> становиться меньше или равным 0.
        /// </summary>
        public event Action? OnDestroed;

        #endregion Очки жизни.
    }
}
