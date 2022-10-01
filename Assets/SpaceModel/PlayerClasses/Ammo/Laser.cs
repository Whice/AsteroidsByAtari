using System;
using Unity;

namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Класс лазера игрока.
    /// </summary>
    internal class Laser : PlayerAmmo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Инфо об игроке.</param>
        public Laser(PlayerShip player, IModelLogger logger) : base(SpaceObjectType.laser, logger)
        {
        }
        public override void SetMaxHP()
        {
            this.hp = 1;
        }
    }
}
