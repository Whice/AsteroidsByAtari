using System;

namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Модельный класс игрока. 
    /// <br/>Обозван кораблем игрока, т.к. в основном будет
    /// восприниматься именно в качестве космического корабля.
    /// </summary>
    internal class PlayerShip : SpaceObject
    {
        public PlayerShip(IModelLogger logger) :base(SpaceObjectType.player, logger)
        {
            this.chargeCountPrivate = MAX_CHARGE_COUNT;
        }
        public override void SetMaxHP()
        {
            this.hp = 3;
        }
        public override Boolean CollideWithObject(SpaceObject spaceObject)
        {
            SpaceObjectType type = spaceObject.type;
            Boolean dangerType =
                type == SpaceObjectType.nlo ||
                type == SpaceObjectType.asteroidShard ||
                type == SpaceObjectType.bigAsteroid;

            //при столкновении с опасным объектом игрок проигрывает,
            //а значит надо выполнить соответсвующие действия.
            if(dangerType)
            {
                return true;
            }

            return false;
        }


        #region Выстрел.


        #region Количество зарядов лазером.

        /// <summary>
        /// Максимальное количество зарядов.
        /// </summary>
        private const Int32 MAX_CHARGE_COUNT = 3;
        /// <summary>
        /// Количество зарядов.
        /// </summary>
        private Int32 chargeCountPrivate;
        /// <summary>
        /// Количество зарядов.
        /// </summary>
        public Int32 chargeCount
        {
            get => chargeCountPrivate;
        }
        /// <summary>
        /// Нужно пополнение количества зарядов.
        /// </summary>
        public Boolean isNeedInsreasingChargeCount
        {
            get => this.chargeCountPrivate < MAX_CHARGE_COUNT;
        }

        /// <summary>
        /// Пополнение количества зарядов.
        /// Пополнение не происходит, если достигнут максимум.
        /// </summary>
        private void IncreaseChargeCount()
        {
            if (this.isNeedInsreasingChargeCount)
            {
                ++this.chargeCountPrivate;
            }
        }
        /// <summary>
        /// Попытаться сделать выстрел лазером.
        /// </summary>
        /// <returns>true, если зарядов хватило и выстрел сделать удалось.</returns>
        public Boolean TryLaserShot()
        {
            if (this.chargeCountPrivate > 0)
            {
                --this.chargeCountPrivate;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Время для обновления одного заряда лазера.
        /// </summary>
        private const Single TIME_FOR_INCREASE_CHARGE_COUNT = 5F;
        /// <summary>
        /// Прошло времени с послежнего пополнения зарядов.
        /// </summary>
        private Single leftTimeAfterLastIncreaseChargeCount = 0;

        #endregion Количество зарядов лазером.

        #region Стрельба пулями.

        /// <summary>
        /// Время для обновления одного выстрела пулей.
        /// </summary>
        private const Single TIME_FOR_NEXT_BULLET_SHOOT = 0.25f;
        /// <summary>
        /// Прошло времени с послежнего пополнения зарядов.
        /// </summary>
        private Single leftTimeAfterLastShoot = 0;
        /// <summary>
        /// Попытаться сделать выстрел пулей.
        /// </summary>
        /// <returns>true, если время перезарядки уже прошло и выстрел сделать удалось.</returns>
        public Boolean TryBulletShot()
        {
            if (this.leftTimeAfterLastShoot > TIME_FOR_NEXT_BULLET_SHOOT)
            {
                this.leftTimeAfterLastShoot = 0;
                return true;
            }
            return false;
        }

        #endregion Стрельба пулями.

        public override void Update(Single timeAfterLastTick)
        {
            base.Update(timeAfterLastTick);

            this.leftTimeAfterLastShoot += timeAfterLastTick;

            if (this.chargeCountPrivate < MAX_CHARGE_COUNT)
            {
                this.leftTimeAfterLastIncreaseChargeCount += timeAfterLastTick;
                //Пополнить количество снарядов, если требуется.
                while (this.leftTimeAfterLastIncreaseChargeCount > TIME_FOR_INCREASE_CHARGE_COUNT)
                {
                    IncreaseChargeCount();

                    this.leftTimeAfterLastIncreaseChargeCount -= TIME_FOR_INCREASE_CHARGE_COUNT;
                }
            }
        }

        #endregion Выстрел.
    }
}
