namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Пуля игрока.
    /// </summary>
    internal class Bullet : PlayerAmmo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Инфо об игроке.</param>
        public Bullet(PlayerShip player, IModelLogger logger) : base(SpaceObjectType.simpleBullet, player, logger)
        { }
    }
}
