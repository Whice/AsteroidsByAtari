using System;
using Unity;

namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Класс лазера игрока.
    /// Он не уничтожается другими объектами, а по истечению отведенного времени.
    /// </summary>
    internal class Laser : PlayerAmmo
    {
        /// <summary>
        /// Время жизни лазера.
        /// </summary>
        private const float LIFE_TIME = 1.7f;
        /// <summary>
        /// Времени прошло после появления лазера.
        /// </summary>
        private float leftTimeAfterCreate = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Инфо об игроке.</param>
        public Laser(PlayerShip player, IModelLogger logger) : base(SpaceObjectType.laser, logger)
        {
        }
        public override void Update(float timeAfterLastTick)
        {
            base.Update(timeAfterLastTick);

            this.leftTimeAfterCreate += timeAfterLastTick;
            if (this.leftTimeAfterCreate > LIFE_TIME)
            {
                Destroy();
                this.leftTimeAfterCreate = 0;
            }
        }
        public override void SetMaxHP()
        {
            this.hp = Int32.MaxValue;
            this.leftTimeAfterCreate = 0;
        }
    }
}
