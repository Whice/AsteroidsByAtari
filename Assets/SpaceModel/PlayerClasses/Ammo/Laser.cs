using System;

namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Класс лазера игрока.
    /// </summary>
    internal class Laser : PlayerAmmo
    {
        #region Количество зарядов.

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
        public void IncreaseChargeCount()
        {
            if (this.isNeedInsreasingChargeCount)
            {
                ++this.chargeCountPrivate;
            }
        }
        /// <summary>
        /// Вполнить выстрел лазером.
        /// </summary>
        public void PerformLaserShot()
        {
            if (this.chargeCountPrivate > 0)
                --this.chargeCountPrivate;
        }

        /// <summary>
        /// Время для обновления одного заряда лазера.
        /// </summary>
        private const Single TIME_FOR_INCREASE_CHARGE_COUNT = 5F;
        /// <summary>
        /// Прошло времени с послежнего пополнения зарядов.
        /// </summary>
        private Single leftTimeAfterLastIncreaseChargeCount;

        #endregion Количество зарядов.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Инфо об игроке.</param>
        public Laser(PlayerShip player, IModelLogger logger) : base(SpaceObjectType.laser, player, logger)
        {
            this.chargeCountPrivate = MAX_CHARGE_COUNT;
        }


        public override void Update(DateTime timeAfterLastTick)
        {
            base.Update(timeAfterLastTick);

            //Пополнить количество снарядов, если требуется.
            while (this.leftTimeAfterLastIncreaseChargeCount > TIME_FOR_INCREASE_CHARGE_COUNT)
            {
                if (this.chargeCountPrivate < MAX_CHARGE_COUNT)
                {
                    this.leftTimeAfterLastIncreaseChargeCount -= TIME_FOR_INCREASE_CHARGE_COUNT;
                    if (this.leftTimeAfterLastIncreaseChargeCount < 0)
                        this.leftTimeAfterLastIncreaseChargeCount = 0;

                    IncreaseChargeCount();
                }
            }
        }
    }
}
