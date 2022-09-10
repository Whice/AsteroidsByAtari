namespace Assets.SpaceModel
{
    public class BattleModel
    {
        private IModelLogger logger;
        /// <summary>
        /// Пул объектов, откуда берутся объекты для игры.
        /// </summary>
        private SpaceObjectsPoolKeeper poolKeeper;
        public BattleModel()
        {
            this.logger = new LoggerAdapter();
            this.poolKeeper = new SpaceObjectsPoolKeeper(this.logger);
        }
    }
}
