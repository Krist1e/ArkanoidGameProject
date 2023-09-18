using ArkanoidGameProject.Bonuses;
using ArkanoidGameProject.GameObjects;
using ArkanoidGameProject.UI.Components;
using ArkanoidGameProject.UI.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.UI.Factory
{
    internal class UIFactory
    {
        private UIComponentsFactory _factory;
        private float _width;
        private float _height;
        public UIFactory(float width, float height, UIComponentsFactory factory)
        {
            _factory = factory;
            _width = width;
            _height = height;
        }

        public StatsPanel CreateStatsPanel()
        {
            return new StatsPanel(_factory)
            {
                Size = new Vector2f(_width, 50),
                Position = new Vector2f(_width / 2, 25),
                BackgroundColor = new Color(255, 249, 236),
                Padding = 5,
                IsEnabled = false,
                ZIndex = 2
            };
        }

        public EndGamePanel CreateWonGamePanel()
        {
            return new EndGamePanel(_factory, "You won!")
            {
                Size = new Vector2f(_width / 3, _height / 5),
                Position = new Vector2f(_width / 2, _height / 2),
                BackgroundColor = new Color(255, 249, 236),
                Padding = 15,
                Orientation = Orientation.Vertical,
                Spacing = 20,
                IsEnabled = false,
                ZIndex = 2
            };
        }

        public EndGamePanel CreateLostGamePanel()
        {
            return new EndGamePanel(_factory, "You lost!")
            {
                Size = new Vector2f(_width / 3, _height / 5),
                Position = new Vector2f(_width / 2, _height / 2),
                BackgroundColor = new Color(255, 249, 236),
                Padding = 15,
                Orientation = Orientation.Vertical,
                Spacing = 20,
                IsEnabled = false,
                ZIndex = 2
            };
        }

        public MenuPanel CreateMenuPanel()
        {
            return new MenuPanel(_factory)
            { 
                Size = new Vector2f(_width / 4, _height / 2 - 35),
                Position = new Vector2f(_width / 2, _height / 2),
                BackgroundColor = new Color(255, 249, 236),
                Padding = 10,
                Orientation = Orientation.Vertical,
                IsEnabled = false,
                ZIndex = 2
            };
        }

        public SettingsPanel CreateSettingsPanel()
        {
            return new SettingsPanel(_factory)
            {
                Size = new Vector2f(_width / 2.45f, _height / 1.325f),
                Position = new Vector2f(_width / 2, _height / 2),
                BackgroundColor = new Color(255, 249, 236),
                Padding = 10,
                Orientation = Orientation.Vertical,
                IsEnabled = false,
                ZIndex = 2
            };
        }
        
        public PowerUpMessage CreatePowerUpMessage(Bonus bonus, Vector2f position)
        {
            var text = bonus switch
            {
                ExtraLifeBonus => "Extra Life",
                PaddleExpansionBonus => "Paddle Expansion",
                BallSizeIncreaseBonus => "Ball Size Increase",
                ExtraScoreBonus extraScore => $"+{extraScore.Score} Score",
                _ => throw new ArgumentOutOfRangeException(nameof(bonus))
            };
            var textLabel = _factory.CreateTextLabel(text, bonus.Color, position);
            var powerUpMessage = new PowerUpMessage(textLabel);
            return powerUpMessage;
        }
    }
}
