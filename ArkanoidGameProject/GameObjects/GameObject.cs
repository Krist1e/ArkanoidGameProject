using ArkanoidGameProject.Collision;
using SFML.Graphics;
using SFML.System;
using System.Numerics;

namespace ArkanoidGameProject.GameObjects
{
    public abstract class GameObject
    {
        private Vector2f _position;
        public Bounds Bounds;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public Vector2f Position
        {
            get => _position;
            set
            {
                _position = value;
                Bounds.Position = _position;
            }
        }
        public bool IsStatic { get; set; } = false;
        public Action<GameObject>? Destroyed;

        public abstract void Draw(RenderWindow window);
        public abstract void OnWallCollision(CollisionArgs args);        
        public abstract void OnCollision(GameObject obj, CollisionArgs args);
        public bool CheckCollision(GameObject obj, out CollisionArgs args)
        {
            args = Bounds.CheckCollision(obj.Bounds);
            if (args is null)
                return false;
            return true;
        }
        public abstract void Move(float deltaTime);
        public void Destroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}
