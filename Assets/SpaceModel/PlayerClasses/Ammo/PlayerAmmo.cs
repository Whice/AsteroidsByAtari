using System;

namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Общий класс оружия игрока.
    /// </summary>
    internal abstract class PlayerAmmo:SpaceObject
    {
        /// <summary>
        /// Инфо об игроке.
        /// </summary>
        private PlayerShip player;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="player">Инфо об игроке.</param>
        public PlayerAmmo(SpaceObjectType type, PlayerShip player, IModelLogger logger) : base(type, logger) 
        {
            this.player = player;
        }

        public override Boolean CollideWithObject(SpaceObject spaceObject)
        {
            SpaceObjectType type = spaceObject.type;

            switch (type)
            {
                case SpaceObjectType.nlo:
                    {
                        this.player.score += 3;
                        break;
                    }
                case SpaceObjectType.asteroidShard:
                    {
                        this.player.score += 1;
                        break;
                    }
                case SpaceObjectType.bigAsteroid:
                    {
                        this.player.score += 2;
                        break;
                    }
            }

            return false;
        }
    }
}
