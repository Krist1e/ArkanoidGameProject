using ArkanoidGameProject.Collision;
using ArkanoidGameProject.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject
{
    internal class GameField
    {
        private List<GameObject> _objects;

        public float Width { get; set; }
        public float Height { get; set; }
        public GameField(float width, float height)
        {
            Width = width;
            Height = height;
            _objects = new();
        }

        public void CheckWallCollision()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].IsStatic)
                    continue;

                CheckWallCollision(_objects[i]);
            }

        }

        private void CheckWallCollision(GameObject obj)
        {
            Vector2f normal = new Vector2f(0.0f, 0.0f);
            float penetration = 0;
            if (obj.Bounds.LeftTop.X <= 0)
            {
                normal = new Vector2f(1.0f, 0.0f);
                penetration = -obj.Bounds.LeftTop.X;
            }
            else if (obj.Bounds.RightBottom.X >= Width)
            {
                normal = new Vector2f(-1.0f, 0.0f);
                penetration = obj.Bounds.RightBottom.X - Width;
            }
            else if (obj.Bounds.LeftTop.Y <= 0)
            {
                normal = new Vector2f(0.0f, 1.0f);
                penetration = -obj.Bounds.LeftTop.Y;
            }
            else if (obj.Bounds.RightBottom.Y >= Height)
            {
                normal = new Vector2f(0.0f, -1.0f);
                penetration = obj.Bounds.RightBottom.Y - Height;
            }

            if (normal == default)
            {
                return;
            }
            var args = new CollisionArgs(normal, penetration);
            obj.OnWallCollision(args);
        }


        public void CheckCollision()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                for (int j = i + 1; j < _objects.Count; j++)
                {
                    if (_objects[i].IsStatic && _objects[j].IsStatic)
                        continue;
                    if (_objects[i].CheckCollision(_objects[j], out var args))
                    {
                        _objects[i].OnCollision(_objects[j], args);
                        args.Reflect();
                        _objects[j].OnCollision(_objects[i], args);
                    }

                }
            }
            CheckWallCollision();
        }

        public void Move(float deltaTime)
        {
            foreach (var gameObject in _objects)
            {
                gameObject.Move(deltaTime);
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (var gameObject in _objects)
            {
                gameObject.Draw(window);
            }
        }

        public void Add(GameObject gameObject)
        {
            _objects.Add(gameObject);

        }
    }
}
