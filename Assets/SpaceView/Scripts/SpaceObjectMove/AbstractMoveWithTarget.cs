using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public abstract class AbstractMoveWithTarget : AbstractMove
    {
        /// <summary>
        /// Цель, к которой будет стремиться нло.
        /// </summary>
        protected AbstractMove target;
        /// <summary>
        /// Установить цель, к которой будет стремиться нло.
        /// </summary>
        /// <param name="target">Цель, к которой будет стремиться нло.</param>
        public void SetTarget(AbstractMove target)
        {
            this.target = target;
        }
        public override void DestroyMoveObject()
        {
            base.DestroyMoveObject();
            this.target = null;
        }
    }
}
