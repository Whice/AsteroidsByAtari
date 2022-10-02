using Assets.SpaceModel;
using System;
using System.Collections.Generic;

namespace View
{
    public class SpaceObjectMovePool
    {
        private Borders battleFieldborders;
        public SpaceObjectMovePool(Borders battleFieldborders)
        {
            this.battleFieldborders = battleFieldborders;
        }

        /// <summary>
        /// Словарь наборов объектов для передвижения: тип->стэк наборов.
        /// </summary>
        private Dictionary<Int32, Stack<AbstractMove>> pool = new Dictionary<Int32, Stack<AbstractMove>>();
        /// <summary>
        /// Компонент передвижения игрока.
        /// </summary>
        private AbstractMove playerMove;
        /// <summary>
        /// Получить объект для передвижения.
        /// </summary>
        /// <param name="type">Тип SpaceObjectType</param>
        /// <returns></returns>
        public AbstractMove GetMoveComponent(Int32 type)
        {
            AbstractMove move = null;
            if (this.pool.ContainsKey(type))
            {
                if (this.pool[type].Count > 0)
                {
                    move = this.pool[type].Pop();
                }
            }
            if (move == null)
            {
                switch((SpaceObjectType)type)
                {
                    case SpaceObjectType.nlo:
                        {
                            move = new NLOMove();
                            break;
                        }
                    case SpaceObjectType.laser:
                        {
                            move = new LazerMove();
                            break;
                        }
                    case SpaceObjectType.bigAsteroid:
                        {
                            move = new BigAsteriodMove();
                            break;
                        }
                    case SpaceObjectType.asteroidShard:
                        {
                            move = new AsteroidShardMove();
                            break;
                        }
                    case SpaceObjectType.simpleBullet:
                        {
                            move = new BulletMove();
                            break;
                        }
                    case SpaceObjectType.player:
                        {
                            move = new PlayerMove();
                            break;
                        }
                }
            }

            if (type == (Int32)SpaceObjectType.player)
            {
                this.playerMove = move;
            }
            if (move is AbstractMoveWithTarget moveWithTarget)
            {
                moveWithTarget.SetTarget(this.playerMove);
            }

            move.InitPool(this);
            return move;
        }
        /// <summary>
        /// Вернуть объект передвижения в пулл.
        /// </summary>
        public void PushMoveComponent(AbstractMove move)
        {
            Int32 type = (Int32)move.type;
            if (this.pool.ContainsKey(type))
            {
                this.pool[type].Push(move);
            }
            else
            {
                Stack<AbstractMove> stack = new Stack<AbstractMove>();
                stack.Push(move);
                this.pool.Add(type, stack);
            }
        }
    }
}
