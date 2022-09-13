using Assets.SpaceModel.Extensions;
using System;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Общий класс игровых объектов.
    /// </summary>
    public abstract class SpaceObject
    {
        /// <summary>
        /// Тип этого объекта.
        /// </summary>
        public readonly SpaceObjectType type;

        protected IModelLogger logger;
        public SpaceObject(SpaceObjectType type, IModelLogger logger)
        {
            this.logger = logger;
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
                            this.OnDestroed?.Invoke(this);
                            UnsubscribeFromAllListenersForOnDestroy();
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
        /// <summary>
        /// Обновить внутренние данные.
        /// </summary>
        /// <param name="timeAfterLastTick">Время проедшее после последнего игрового тика.</param>
        public virtual void Update(Single timeAfterLastTick) { }

        #endregion Изменение свойств.

        #region Столкновение.

        /// <summary>
        /// Опасен ли тип этого объекта объекту указанного типа.
        /// <br/>Если тип этого объекта указан как союзник игрока (пуля, к примеру),
        /// а второй тип, как его противник (НЛО, к примеру), то да, они опасны друг другу
        /// и ответ будет положительным, иначе отрицательным.
        /// </summary>
        /// <param name="type">Тип другого объекта.</param>
        /// <returns></returns>
        protected Boolean IsDangerObjectWith(SpaceObjectType type)
        {
            return this.type.IsDangerObjectType() != type.IsDangerObjectType();
        }
        /// <summary>
        /// Столкнуть этот объект с указаным объектом.
        /// </summary>
        /// <param name="spaceObject"></param>
        /// <returns>true, если надо совершить какое-то дополнительное действие
        /// в зависимости от типа одного из объектов.
        /// <br/>Какие именно действия будут совершаться, будут решать правила игры.</returns>
        public virtual Boolean CollideWithObject(SpaceObject spaceObject)
        {
            SpaceObjectType type = spaceObject.type;
            if (IsDangerObjectWith(type))
            {
                --this.hp;
            }

            return false;
        }

        #endregion Столкновение.

        #region Очки жизни.

        /// <summary>
        /// Заставить всех подписчиков отписаться от события уничтожения,
        /// если подписчики есть.
        /// </summary>
        public void UnsubscribeFromAllListenersForOnDestroy()
        {
            if (this.OnDestroed != null)
                this.OnDestroed = null;
        }
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
        public void Destroy()
        {
            this.hp = 0;
        }

        /// <summary>
        /// Событие, которое происходит, когда объект уничтожается.
        /// <br/>Объект уничтожется, когда его <see cref="hp"/> становиться меньше или равным 0.
        /// </summary>
        public event Action<SpaceObject> OnDestroed;

        #endregion Очки жизни.

    }
}
