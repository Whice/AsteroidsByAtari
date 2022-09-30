using Assets.SpaceModel.DangerSpaceObjects;
using Assets.SpaceModel.Extensions;
using Assets.SpaceModel.PlayerClasses;
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
        /// Событие изменения счета. Передает аргументом новый счет.
        /// </summary>
        public event Action<Int32> onChangedScore;
        /// <summary>
        /// Информация о происходящем бое.
        /// </summary>
        private BattleInfo battleInfo;
        public BattleModel()
        {
        }

        #region Активные игровые объекты.

        /// <summary>
        /// Ссылка на игрока.
        /// </summary>
        private PlayerShip player;
        /// <summary>
        /// Список активных игровых предметов.
        /// <br/>Решил сделать список, т.к. будет часто выполняться обход всех
        /// активных объектов, а вот удаление намного реже.
        /// </summary>
        private List<SpaceObject> activeObects = new List<SpaceObject>(100);
        /// <summary>
        /// Событие начала игры.
        /// </summary>
        public event Action onStartedGame;
        /// <summary>
        /// Событие конца игры.
        /// Передает информацию о состоянии боя на момент окончания.
        /// </summary>
        public event Action<BattleInfo> onEndedGame;
        /// <summary>
        /// Произошло уничтожение игрока
        /// </summary>
        public event Action<SpaceObject> OnDestroedActiveObject;
        /// <summary>
        /// Выпонить действия при уничтожении игрового объекта.
        /// </summary>
        /// <param name="spaceObject"></param>
        private void OnDestroyActiveObject(SpaceObject spaceObject)
        {
            this.OnDestroedActiveObject?.Invoke(spaceObject);

            //Если был уничтожен опасный игроку объект, то нужно добавить счет.
            if (spaceObject.type.IsDangerObjectType())
            {
                DangerSpaceObject dangerSpaceObject = spaceObject as DangerSpaceObject;

                if (dangerSpaceObject == null)
                {
                    this.logger.ErrorMessage("Not danger spaceObject has been define as dangerous!");
                    return;
                }

                this.battleInfo.score += dangerSpaceObject.GetScore();
                this.onChangedScore?.Invoke(this.score);
            }

            //Уничтожение игрока означает конец игры.
            if (spaceObject.type == SpaceObjectType.player)
            {
                this.onEndedGame?.Invoke(this.battleInfo);
            }
        }
        /// <summary>
        /// Событие создания игрового объекта.
        /// </summary>
        public event Action<SpaceObject> onCreatedSpaceObject;
        /// <summary>
        /// Создать игровой объект заданного типа.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objectsCount">Количество создаваемых объектов.</param>
        private SpaceObject CreateSpaceObject(SpaceObjectType type, Int32 objectsCount = 1)
        {
            SpaceObject spaceObject = null;
            for (Int32 i = 0; i < objectsCount; i++)
            {
                spaceObject = this.poolKeeper.Pop(type);
                spaceObject.OnDestroed += OnDestroyActiveObject;
                this.activeObects.Add(spaceObject);
                this.onCreatedSpaceObject?.Invoke(spaceObject);


                switch ((Int32)type)
                {
                    case (Int32)SpaceObjectType.bigAsteroid:
                        {
                            ++this.dangerousObjectsCountsPrivate.bigAsteroids;
                            break;
                        }
                    case (Int32)SpaceObjectType.asteroidShard:
                        {
                            ++this.dangerousObjectsCountsPrivate.asteroidShards;
                            break;
                        }
                    case (Int32)SpaceObjectType.nlo:
                        {
                            ++this.dangerousObjectsCountsPrivate.NLOs;
                            break;
                        }
                }
            }

            if (spaceObject == null)
                this.logger.ErrorMessage("Space object does not created in create method in model!");

              return spaceObject;
        }

        #endregion Активные игровые объекты.

        #region Стрельба игрока.

        /// <summary>
        /// Игрок сделал выстрел пулей.
        /// <br/>Первым параметром передается игрок.
        /// <br/>Вторым параметром передается снаряд.
        /// <br/>Третьим параметром передается тип снаряда.
        /// </summary>
        public event Action<SpaceObject, SpaceObject, SpaceObjectType> onPlayerShoted;
        /// <summary>
        /// Обработать выстрел игрока.
        /// </summary>
        /// <param name="typeShell">Тип снаряда.</param>
        private void OnPlayerShot(SpaceObjectType typeShell)
        {
            SpaceObject shell = CreateSpaceObject(typeShell);
            this.onPlayerShoted?.Invoke(this.player, shell, typeShell);
        }
        /// <summary>
        /// Сделать выстрел пулей.
        /// </summary>
        public void PlayerBulletShot()
        {
            if(this.player.TryBulletShot())
                OnPlayerShot(SpaceObjectType.simpleBullet);
        }
        /// <summary>
        /// Сделать выстрел лазером.
        /// </summary>
        public void PlayerLazerShot()
        {
            if (this.player.TryLaserShot())
                OnPlayerShot(SpaceObjectType.laser);
        }

        #endregion Стрельба игрока.

        #region Количество астероидов и НЛО.

        /// <summary>
        /// Максимальное количество опасных объектов.
        /// <br/>Структра сделална на случай, если количество опасных объектов
        /// будет дополняться, так их будет легче передавать.
        /// </summary>
        public struct DangerousObjectsCounts
        {
            /// <summary>
            /// Максимум больших астероидов в игре.
            /// </summary>
            public Int32 bigAsteroids;
            /// <summary>
            /// Максимум частей астероидов в игре.
            /// </summary>
            public Int32 asteroidShards;
            /// <summary>
            /// Максимум НЛО в игре.
            /// </summary>
            public Int32 NLOs;

            /// <summary>
            /// Структура заполненая нулями.
            /// </summary>
            public static DangerousObjectsCounts zero
            {
                get => new DangerousObjectsCounts
                {
                    bigAsteroids = 0,
                    asteroidShards = 0,
                    NLOs = 0
                };
            }
        }

        /// <summary>
        /// Задать максимальное количество опасных объектов.
        /// </summary>
        public void SetMaxmumDangerousObjectsCounts(in DangerousObjectsCounts maxCounts)
        {
            this.maxmumDangerousObjectsCountsPrivate = maxCounts;
        }

        /// <summary>
        /// Количество опасных объектов.
        /// </summary>
        private DangerousObjectsCounts dangerousObjectsCountsPrivate = DangerousObjectsCounts.zero;
        /// <summary>
        /// Количество опасных объектов.
        /// </summary>
        public DangerousObjectsCounts dangerousObjectsCounts => this.dangerousObjectsCountsPrivate;
        /// <summary>
        /// Максимальное количество опасных объектов.
        /// </summary>
        private DangerousObjectsCounts maxmumDangerousObjectsCountsPrivate;
        /// <summary>
        /// Максимальное количество опасных объектов.
        /// </summary>
        public DangerousObjectsCounts maxmumDangerousObjectsCounts => this.maxmumDangerousObjectsCountsPrivate;

        /// <summary>
        /// Интенсивность пояления опасных объектов
        /// </summary>
        private const Single INTENSITY_OF_APPEARANCE_OF_DANGEROUS_OBJECTS = 1f;
        /// <summary>
        /// Прошло времени после появления опасного объекта.
        /// </summary>
        private Single leftTimeAfterAppearanceOfDangerousObject = 0;

        #endregion Количество астероидов и НЛО.

        /// <summary>
        /// Обновить внутренние данные.
        /// </summary>
        /// <param name="timeAfterLastTick">Время проедшее после последнего игрового тика.</param>
        public void Update(Single timeAfterLastTick)
        {
            //Обновить состояния активным объектам 
            for (int i = 0; i < this.activeObects.Count; i++)
            {
                this.activeObects[i].Update(timeAfterLastTick);
            }

            //Создавать раз в какое-то время противников, если их еще не максимальное количество.
            this.leftTimeAfterAppearanceOfDangerousObject += timeAfterLastTick;
            if (this.leftTimeAfterAppearanceOfDangerousObject > INTENSITY_OF_APPEARANCE_OF_DANGEROUS_OBJECTS)
            {
                this.leftTimeAfterAppearanceOfDangerousObject -= INTENSITY_OF_APPEARANCE_OF_DANGEROUS_OBJECTS;

                if (this.dangerousObjectsCounts.NLOs < this.maxmumDangerousObjectsCounts.NLOs)
                {
                    CreateSpaceObject(SpaceObjectType.nlo);
                }
                if (this.dangerousObjectsCounts.bigAsteroids < this.maxmumDangerousObjectsCounts.bigAsteroids)
                {
                    CreateSpaceObject(SpaceObjectType.bigAsteroid);
                }
                if (this.dangerousObjectsCounts.asteroidShards < this.maxmumDangerousObjectsCounts.asteroidShards)
                {
                    CreateSpaceObject(SpaceObjectType.asteroidShard);
                }
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

            this.maxmumDangerousObjectsCountsPrivate = new DangerousObjectsCounts
            {
                bigAsteroids = 5,
                asteroidShards = 10,
                NLOs = 1
            };

            PlayerShip ship = CreateSpaceObject(SpaceObjectType.player) as PlayerShip;
            if (ship == null)
                this.logger.ErrorMessage("The player was not created on demand!");
            this.player = ship;


            this.onStartedGame?.Invoke();
        }
    }
}
