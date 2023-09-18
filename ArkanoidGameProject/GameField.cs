using ArkanoidGameProject.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject
{
    public class GameField
    {
        private readonly Dictionary<int, List<DisplayObject>> _objectsByLayer = new();
        private readonly List<DisplayObject> _gameObjects = new();
        
        private readonly List<(DisplayObject, bool)> _objectsToAdd = new();
        private readonly List<DisplayObject> _objectsToRemove = new();

        public float Width { get; set; }
        public float Height { get; set; }
        public GameField(float width, float height)
        {
            Width = width;
            Height = height + 20;
        }
        
        public GameField()
        {
        }
        
        public event Action<DisplayObject>? Destroyed; 
        
        private IEnumerable<DisplayObject> movableObjects => _objectsByLayer.SelectMany(x => x.Value).Where(x => !x.IsStatic);

        public IEnumerable<DisplayObject> GameObjects
        {
            get => _gameObjects.Concat(_objectsToAdd.Where(pair => !pair.Item2).Select(pair => pair.Item1));
            set => AddRange(value);
        }
        
        public bool IsPaused { get; set; }

        public void CheckCollision()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                for (int j = i + 1; j < _gameObjects.Count; j++)
                {
                    if (_gameObjects[i].IsStatic && _gameObjects[j].IsStatic)
                        continue;
                    if (!_gameObjects[i].CheckCollision(_gameObjects[j], out var args)) continue;
                    
                    _gameObjects[i].OnCollision(_gameObjects[j], args);
                    _gameObjects[j].OnCollision(_gameObjects[i], args.Reflect());
                }
            }
            CheckWallCollision();
        }


        public void Move(float deltaTime)
        {
            UpdatePendings();
            if (IsPaused) return;
            foreach (var gameObject in movableObjects)
                gameObject.Move(deltaTime);
        }

        public void Draw(RenderWindow window)
        {
            foreach (var (_, objects) in _objectsByLayer.OrderBy(x => x.Key))
            foreach (var obj in objects)
                obj.Draw(window);
        }

        public void Add(DisplayObject displayObject, bool isUI = false)
        {
            _objectsToAdd.Add((displayObject, isUI));
            displayObject.Destroyed += Remove;
        }

        public void AddRange(IEnumerable<DisplayObject> displayObjects, bool isUI = false)
        {
            var objects = displayObjects.ToList();
            _objectsToAdd.AddRange(objects.Select(x => (x, isUI)));
            foreach (var displayObject in objects) 
                displayObject.Destroyed += Remove;
        }

        public void Remove(DisplayObject displayObject)
        {
            _objectsToRemove.Add(displayObject);
            displayObject.Destroyed -= Remove;
            Destroyed?.Invoke(displayObject);
        }
        
        public void Clear()
        {
            _objectsByLayer.Clear();
            _gameObjects.Clear();
            _objectsToAdd.Clear();
            _objectsToRemove.Clear();
        }

        private void UpdatePendings()
        {
            foreach (var (displayObject, isUI) in _objectsToAdd)
            {
                if (!isUI)
                    _gameObjects.Add(displayObject);
                AddToLayer(displayObject);
            }
            _objectsToAdd.Clear();

            foreach (var displayObject in _objectsToRemove)
            {
                _gameObjects.Remove(displayObject);
                RemoveFromLayer(displayObject);
            }
            _objectsToRemove.Clear();
        }

        private void AddToLayer(DisplayObject obj)
        {
            if (!_objectsByLayer.ContainsKey(obj.ZIndex))
                _objectsByLayer.Add(obj.ZIndex, new List<DisplayObject>());
            _objectsByLayer[obj.ZIndex].Add(obj);
        }
        
        private void RemoveFromLayer(DisplayObject obj)
        {
            if (!_objectsByLayer.ContainsKey(obj.ZIndex))
                return;
            _objectsByLayer[obj.ZIndex].Remove(obj);
        }

        private void CheckWallCollision()
        {
            foreach (var t in _gameObjects.Where(t => !t.IsStatic)) 
                CheckWallCollision(t);
        }

        private void CheckWallCollision(DisplayObject obj)
        {
            Vector2f normal = new Vector2f(0.0f, 0.0f);
            float penetration = 0;
            if (obj.LeftTop.X <= 0)
            {
                normal = new Vector2f(1.0f, 0.0f);
                penetration = -obj.LeftTop.X;
            }
            else if (obj.RightBottom.X >= Width)
            {
                normal = new Vector2f(-1.0f, 0.0f);
                penetration = obj.RightBottom.X - Width;
            }
            else if (obj.LeftTop.Y <= 0)
            {
                normal = new Vector2f(0.0f, 1.0f);
                penetration = -obj.LeftTop.Y;
            }
            else if (obj.RightBottom.Y >= Height)
            {
                normal = new Vector2f(0.0f, -1.0f);
                penetration = obj.RightBottom.Y - Height;
            }

            if (normal == default)
            {
                return;
            }
            var args = new CollisionArgs(normal, penetration);
            obj.OnWallCollision(args);
        }
    }
}
