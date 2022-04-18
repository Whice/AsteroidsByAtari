namespace Assets.SpaceModel.PlayerClasses
{
    public class Laser : PlayerAmmo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Инфо об игроке.</param>
        public Laser(PlayerShip player) : base(SpaceObjectType.laser, player)
        { }
    }
}
