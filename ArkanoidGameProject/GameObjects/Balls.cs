using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanoidGameProject.GameObjects
{
    internal class Balls
    {
        private readonly List<Ball> _balls;

        public event Action<Ball>? Added;
        public event Action? Removed;

        public void Add(Ball ball)
        {
            _balls.Add(ball);
            Added?.Invoke(ball);
        }

        public void Remove(Ball ball)
        {
            _balls.Remove(ball);
            Removed?.Invoke();
        }

        public void Clear()
        {
            _balls.Clear();
            Added = null;
            Removed = null;
        }

    }
}
