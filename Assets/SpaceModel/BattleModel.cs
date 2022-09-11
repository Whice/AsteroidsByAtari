using Assets.SpaceModel.DangerSpaceObjects;
using Assets.SpaceModel.Extensions;
using System;

namespace Assets.SpaceModel
{
    public class BattleModel
    {
        private IModelLogger logger;
        /// <summary>
        /// Пул объектов, откуда берутся объекты для игры.
        /// </summary>
        private SpaceObjectsPoolKeeper poolKeeper;
        /// <summary>
        /// Счет игры
        /// </summary>
        public Int32 score
        {
            get => this.battleInfo.score;
        }
        /// <summary>
        /// Информация о происходящем бое.
        /// </summary>
        private BattleInfo battleInfo;
        public BattleModel()
        {
        }

        /// <summary>
        /// Выпонить действия при уничтожении игрового объекта.
        /// </summary>
        /// <param name="spaceObject"></param>
        private void OnDestroyActiveObject(SpaceObject spaceObject)
        {
            if (spaceObject.type.IsDangerObjectType())
            {
                DangerSpaceObject dangerSpaceObject = spaceObject as DangerSpaceObject;

                if (dangerSpaceObject == null)
                {
                    this.logger.ErrorMessage("Not danger spaceObject has been define as dangerous!");
                    return;
                }

                this.battleInfo.score += dangerSpaceObject.GetScore();
            }
            if (spaceObject.type == SpaceObjectType.player)
            {
                this.onEndGame?.Invoke(this.battleInfo);
            }
        }

        /// <summary>
        /// Начать игру.
        /// Во время старта игры происходит инициализация всех внутренних классов.
        /// </summary>
        public void StartGame()
        {
            this.logger = new LoggerAdapter();
            this.poolKeeper = new SpaceObjectsPoolKeeper(this.logger);
            this.battleInfo = new BattleInfo();
        }
        public event Action onStartGame;
        public event Action<BattleInfo> onEndGame;
    }
}
