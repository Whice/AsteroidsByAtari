using System;

namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Общий класс оружия игрока.
    /// </summary>
    internal abstract class PlayerAmmo:SpaceObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="player">Инфо об игроке.</param>
        public PlayerAmmo(SpaceObjectType type, IModelLogger logger) : base(type, logger) 
        {
            this.hp = 1;
        }
    }
}
