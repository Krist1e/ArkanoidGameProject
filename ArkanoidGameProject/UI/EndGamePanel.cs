using ArkanoidGameProject.UI.Components;
using ArkanoidGameProject.UI.Factory;
using SFML.Graphics;
using SFML.System;
using System.Threading.Channels;

namespace ArkanoidGameProject.UI
{
    internal class EndGamePanel : StackPanel
    {
        private readonly TextLabel _messageLabel;
        private readonly Button _restartButton;
        private readonly Button _exitButton;

        public event Action? Restart;
        public event Action? Exit;
        
        public EndGamePanel(UIComponentsFactory factory, string message)
        {
            _messageLabel = factory.CreateTextLabel(message, Color.Black, new Vector2f(), new Vector2f(Size.X, 50));
            _messageLabel.FontSize = 40;
            _messageLabel.TextAlignment = TextAlignment.Center;

            var actionButtons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Size = new Vector2f(Size.X, 50),
                Spacing = 85,
                Padding = 0,
                BackgroundColor = Color.Transparent
            };
            _restartButton = factory.CreateButton("Restart", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(0, 50), new Vector2f(170, 50));
            _restartButton.Click += OnRestart;
            _exitButton = factory.CreateButton("Exit", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(0, 100), new Vector2f(170, 50));
            _exitButton.Click += OnExit;

            AddChild(_messageLabel);
            actionButtons.AddChild(_restartButton);
            actionButtons.AddChild(_exitButton);
            AddChild(actionButtons);
        }

        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            if (IsEnabled)
            {
                _restartButton.Click += OnRestart;
                _exitButton.Click += OnExit;                
            } else
            {
                _restartButton.Click -= OnRestart;
                _exitButton.Click -= OnExit;
            }
        }

        private void OnRestart(object? sender, EventArgs eventArgs)
        {
            Restart?.Invoke();
        }
        
        private void OnExit(object? sender, EventArgs eventArgs)
        {
            Exit?.Invoke();
        }
        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            _messageLabel.Size = new Vector2f(Size.X, 50);
        }
    }
}
