using Assets.SpaceModel.DangerSpaceObjects;
using Assets.SpaceModel.Extensions;
using System;
using System.Collections.Generic;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Модель боя в игре.
    /// </summary>
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


        private List<SpaceObject> activeObects;
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
        /// Обновить внутренние данные.
        /// </summary>
        /// <param name="timeAfterLastTick">Время проедшее после последнего игрового тика.</param>
        public void Update(DateTime timeAfterLastTick)
        { 
            for (int i = 0; i < this.activeObects.Count; i++)
            {
                this.activeObects[i].Update(timeAfterLastTick);
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
            this.activeObects = new List<SpaceObject>();

            this.onStartGame?.Invoke();
        }
        public event Action onStartGame;
        public event Action<BattleInfo> onEndGame;
    }
}
