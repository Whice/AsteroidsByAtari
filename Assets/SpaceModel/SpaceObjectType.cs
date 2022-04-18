using System;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Тип космического объекта.
    /// </summary>
    public enum SpaceObjectType : Byte
    {
        /// <summary>
        /// Неопознано.
        /// </summary>
        unknow = 0,

        #region Опасные объекты.

        /// <summary>
        /// Большой астероид.
        /// </summary>
        bigAsteroid = unknow + 1,
        /// <summary>
        /// Маленький астероид - часть большого.
        /// </summary>
        asteroidShard = bigAsteroid + 1,
        /// <summary>
        /// Корабль пришельцев. Назвал стереотипно НЛО.
        /// </summary>
        nlo = asteroidShard + 1,

        #endregion Опасные объекты.

        #region Типы игрока.

        /// <summary>
        /// Игрок.
        /// </summary>
        player = nlo + 1,
        /// <summary>
        /// Обычная пуля игрока.
        /// </summary>
        simpleBullet = player + 1,
        /// <summary>
        /// Лазерный луч игрока.
        /// </summary>
        laser = simpleBullet + 1,

        #endregion Типы игрока.

        /// <summary>
        /// Это не тип, а номер, следующий за номером последнего типа.
        /// <br/>Например, если последний тип имеет номер 8, то <see cref="end"/> будет равен 9.
        /// </summary>
        end = laser + 1
    }
}
