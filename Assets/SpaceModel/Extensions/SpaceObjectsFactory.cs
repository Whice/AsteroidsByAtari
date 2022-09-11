using Assets.SpaceModel.DangerSpaceObjects;
using Assets.SpaceModel.PlayerClasses;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Фабрика для создания космических объектов.
    /// </summary>
    internal class SpaceObjectsFactory
    {
        public SpaceObjectsFactory(IModelLogger logger)
        {
            this.logger = logger;
            this.player = new PlayerShip(logger);
        }
        private IModelLogger logger;
        /// <summary>
        /// Класс игрока создается сразу с фабрикой, чтобы был только один.
        /// Аля синглтон, но не он, мало ли что.
        /// </summary>
        private PlayerShip player;
        /// <summary>
        /// Создать космический объект по типу.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SpaceObject CreateSpaceObjects(SpaceObjectType type)
        {
            switch (type)
            {
                case SpaceObjectType.nlo: return new NLO(this.logger);
                case SpaceObjectType.asteroidShard: return new AsteroidShard(this.logger);
                case SpaceObjectType.bigAsteroid: return new BigAsteroid(this.logger);

                case SpaceObjectType.player: return this.player;
                case SpaceObjectType.laser: return new Laser(this.player, this.logger);
                case SpaceObjectType.simpleBullet: return new Bullet(this.player, this.logger);

                default:
                    {
                        logger.ErrorMessage("In method " + nameof(CreateSpaceObjects) 
                            + " was provided invalid " + nameof(SpaceObjectType) + "!");
                        return null;
                    }
            }
        }
    }
}
