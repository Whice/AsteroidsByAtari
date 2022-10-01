using UnityEngine;

namespace View
{
    public class NLOMove : AbstractMove
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.speed = 1.1f;
            this.type = Assets.SpaceModel.SpaceObjectType.nlo;
        }
        /// <summary>
        /// Цель, к которой будет стремиться нло.
        /// </summary>
        private AbstractMove target;
        /// <summary>
        /// Установить цель, к которой будет стремиться нло.
        /// </summary>
        /// <param name="target">Цель, к которой будет стремиться нло.</param>
        public void SetTarget(AbstractMove target)
        {
            this.target = target;
        }
        /// <summary>
        /// Частота смены направления у нло.
        /// </summary>
        private const float TIME_FOR_CHANGE_DIRECTION = 0.3f;
        /// <summary>
        /// Времени прошло после того, как направление поменялось.
        /// </summary>
        private float leftTickAfterChageDirection=0;
        public override void Move(float tick)
        {
            this.leftTickAfterChageDirection += tick;
            //Изменить направление на полоэение уели и только потом сдвинуться.
            if (this.leftTickAfterChageDirection > TIME_FOR_CHANGE_DIRECTION)//сдвигать пореже, т.к. нормализация тяжелый процесс.
            {
                this.direction = (this.target.position - this.position).normalized;
                this.leftTickAfterChageDirection -= TIME_FOR_CHANGE_DIRECTION;
            }
            base.Move(tick);
        }
    }
}