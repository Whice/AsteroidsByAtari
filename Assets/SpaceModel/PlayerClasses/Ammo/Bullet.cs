namespace Assets.SpaceModel.PlayerClasses
{
    public class Bullet : PlayerAmmo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Инфо об игроке.</param>
        public Bullet(PlayerShip player) : base(SpaceObjectType.simpleBullet, player)
        { }
    }
}
