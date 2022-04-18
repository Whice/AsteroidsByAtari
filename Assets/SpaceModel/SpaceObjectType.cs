namespace Assets.SpaceModel
{
    /// <summary>
    /// Тип космического объекта.
    /// </summary>
    public enum SpaceObjectType
    {
        /// <summary>
        /// Неопознано.
        /// </summary>
        unknow = 0,
        /// <summary>
        /// Большой астероид.
        /// </summary>
        bigAsteroid = 1,
        /// <summary>
        /// Маленький астероид - часть большого.
        /// </summary>
        asteroidShard = 2,
        /// <summary>
        /// Игрок.
        /// </summary>
        player = 3,
        /// <summary>
        /// Корабль пришельцев. Назвал стереотипно НЛО.
        /// </summary>
        nlo = 4,
        /// <summary>
        /// Обычная пуля игрока.
        /// </summary>
        simpleBullet = 5,
        /// <summary>
        /// Лазерный луч игрока.
        /// </summary>
        laser = 6
    }
}
