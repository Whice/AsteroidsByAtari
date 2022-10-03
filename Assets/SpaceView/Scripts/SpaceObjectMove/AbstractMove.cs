using Assets.SpaceModel;
using System;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Абстрактный класс родитель для классов передвижения объектов.
    /// </summary>
    public abstract class AbstractMove
    {
        #region Borders

        /// <summary>
        /// Информация о границах боевого поля.
        /// </summary>
        private Borders.BorderPosition spaceborders;
        /// <summary>
        /// Произошел выход за боевую зону.
        /// Если объект улетел слишком далеко, то его стоит удалить.
        /// </summary>
        public Action<Boolean> onOutFromBattleZone;
        /// <summary>
        /// Проверить выход в "открытый космос".
        /// Если объект улетел слишком далеко, то его стоит удалить.
        /// </summary>
        private void CheckIfOutSpaceBorders()
        {
            Vector2 position = this.position;
            Borders.BorderPosition borders = this.spaceborders;

            //x
            if (position.x > borders.right)
            {
                this.onOutFromBattleZone?.Invoke(true);
            }
            else if (position.x < borders.left)
            {
                this.onOutFromBattleZone?.Invoke(true);
            }

            //y
            else if (position.y > borders.up)
            {
                this.onOutFromBattleZone?.Invoke(true);
            }
            else if (position.y < borders.bottom)
            {
                this.onOutFromBattleZone?.Invoke(true);
            }
        }
        #endregion Borders

        /// <summary>
        /// Тип объекта, который должен двигать этот класс.
        /// </summary>
        public SpaceObjectType type { get; protected set; }
        /// <summary>
        /// Положение в пространстве передвигаемого объекта.
        /// </summary>
        public Transform spaceObjectTransform { get; protected set; }
        /// <summary>
        /// Инициализировать объект.
        /// </summary>
        /// <param name="spaceObjectTransform">Положение в пространстве передвигаемого объекта.</param>
        /// <param name="battleFieldborders">Начальные данные о положении. Если null, то инициализация не происходит.</param>
        public virtual void Init(in SpaceObjectMoveInfo info)
        {
            Transform spaceObjectTransform = info.spaceObjectTransform;
            Borders battleFieldborders = info.battleFieldborders;

            this.spaceObjectTransform = spaceObjectTransform;
            if (battleFieldborders != null)
            {
                var positionAndDirection = battleFieldborders.GetRandomPositionAndDirection();
                this.spaceborders = battleFieldborders.GetBorderPositionSpaceBorders();
                this.position = positionAndDirection.position;
                this.direction = positionAndDirection.direction;
            }
        }
        /// <summary>
        /// Позиция в 2d.
        /// </summary>
        public Vector2 position
        {
            get => new Vector2(this.spaceObjectTransform.localPosition.x, this.spaceObjectTransform.localPosition.y);
            protected set => this.spaceObjectTransform.localPosition = new Vector3(value.x, value.y, 0);
        }
        /// <summary>
        /// Направление в 2d.
        /// </summary>
        public Vector2 direction { get;protected set; }
        /// <summary>
        /// Поворот объекта, которому принадлежит компонен движения.
        /// </summary>
        public Quaternion rotation
        {
            get => this.spaceObjectTransform.rotation;
        }
        /// <summary>
        /// Скорость, с которой передвигается объект.
        /// </summary>
        protected float speed;

        /// <summary>
        /// Сдвинуть в заданном направлении объект с учетом времени.
        /// </summary>
        /// <param name="tick"></param>
        public virtual void Move(float tick)
        {
            this.position += this.direction * this.speed * tick;
            CheckIfOutSpaceBorders();
        }

        #region Pool

        /// <summary>
        /// Пулл объектов движения, куда вернуться этот объект после "уничтожения".
        /// </summary>
        private SpaceObjectMovePool pool;
        /// <summary>
        /// Инициализировать пулл объектов движения. 
        /// </summary>
        /// <param name="pool"></param>
        public void InitPool(SpaceObjectMovePool pool)
        {
            this.pool = pool;
        }
        /// <summary>
        /// Уничтожить объект и отправить его в его пулл.
        /// </summary>
        public virtual void DestroyMoveObject()
        {
            this.pool.PushMoveComponent(this);
            this.pool = null;
            this.spaceObjectTransform = null;
        }

        #endregion Pool

    }
}
