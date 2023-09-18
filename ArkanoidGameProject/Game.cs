using System.Timers;
using ArkanoidGameProject.Bonuses;
using ArkanoidGameProject.GameObjects;
using ArkanoidGameProject.Input;
using ArkanoidGameProject.Serialization;
using ArkanoidGameProject.UI;
using ArkanoidGameProject.UI.Factory;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject
{
    public class Game
    {
        private readonly System.Timers.Timer _timer;
        private readonly RenderWindow _window;
        private readonly InputHandler _inputHandler;

        private GameField _field;
        private readonly GameSettings _settings;
        private readonly Player _player;

        private readonly UIFactory _uiFactory;
        private readonly StatsPanel _statsPanel;
        private readonly SettingsPanel _settingsPanel;
        private readonly MenuPanel _menuPanel;
        private readonly EndGamePanel _wonGamePanel;
        private readonly EndGamePanel _lostGamePanel;

        private IGameState _activeState = null!;

        private bool _isRunning = true;

        public Game(uint width, uint height, uint fps)
        {
            _settings = new GameSettings();
            _settings.SetWindowSize(new Vector2f(width, height));
            VideoMode mode = new VideoMode(width, height);
            RenderWindow window = new RenderWindow(mode, "Arkanoid Game");
            _window = window;
            FPS = fps;
            _timer = new System.Timers.Timer(DeltaTime);
            _inputHandler = new InputHandler(window);
            _field = new GameField(width, height);
            window.Closed += (sender, args) => window.Close();

            /*window.LostFocus += (sender, args) => Pause();
            window.GainedFocus += (sender, args) => Resume();*/
            
            var font = new Font(@"C:\Windows\Fonts\arial.ttf");
            _uiFactory = new UIFactory(width, height, new UIComponentsFactory(_inputHandler.EventManager, new TextInfo(font)));
            _statsPanel = _uiFactory.CreateStatsPanel();
            _settingsPanel = _uiFactory.CreateSettingsPanel();
            _menuPanel = _uiFactory.CreateMenuPanel();
            _wonGamePanel = _uiFactory.CreateWonGamePanel();
            _lostGamePanel = _uiFactory.CreateLostGamePanel();
            _settingsPanel.SetSettings(_settings);
            
            _player = new Player(_inputHandler, _settings, _field);
            _statsPanel.SetStats(_player.Stats);
            _statsPanel.SetGameSettings(_settings);
        }
        
        private IGameState ActiveState
        {
            get => _activeState;
            set
            {
                _activeState?.Exit();
                _activeState = value;
                _activeState.Enter();
            }
        }
        
        public uint FPS { get; set; }
        public float DeltaTime => 1.0f / FPS;

        private bool _isReady;

        public void Start()
        {
            InitializeField();
            _field.Destroyed += OnFieldDestroyed;
            _settings.WindowSizeChanged += OnRezizeWindow;
            _settings.DifficultyChanged += OnDifficultyChanged;
            _player.ScoreChanged += OnPlayerScoreChanged;
            _player.LivesChanged += OnPlayerLivesChanged;
            ActiveState = new InGameState(this);

            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
            while (_isRunning)
            {
                if (!_window.IsOpen) Exit();
                if (_isReady) GameLoopStep();
            }
        }
        private void OnFieldDestroyed(DisplayObject obj)
        {
            if (obj is not Block block) return;
            var powerUps = block.Bonuses.ToList();
            for (int i = 0; i < powerUps.Count; i++)
            {
                var powerUp = powerUps.ElementAt(i);
                _player.AddBonus(powerUp);
                _field.Add(_uiFactory.CreatePowerUpMessage(powerUp, block.Position with {Y = block.Position.Y + i * 30}), true);
            }
        }

        private void TimerOnElapsed(object? sender, ElapsedEventArgs e) => _isReady = true;

        private void GameLoopStep()
        {
            _inputHandler.HandleInput();
            _player.UpdateTime(DeltaTime);
            _field.Move(DeltaTime);
            _field.CheckCollision();
            _window.Clear(Color.White);
            _field.Draw(_window);
            _window.Display();
            _isReady = false;
        }

        private void OnDifficultyChanged(DifficultyStats obj)
        {
            _player.Load(_settings, _field, _player.Stats, _player.ActiveBonuses);
            _statsPanel.SetStats(_player.Stats);
        }

        private void OnPlayerScoreChanged(int score)
        {
            if (score == 10) ShowWonGame();
        }

        private void OnPlayerLivesChanged(int lives)
        {
            if (lives == 0) ShowLostGame();
        }

        private void OnRezizeWindow(Vector2f size)
        {
            _window.Size = (Vector2u)size;
            _window.Position = (Vector2i)(new Vector2u(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height) - _window.Size) / 2;
        }

        private void InitializeField()
        {
            var blocks = new List<Block>
            {
                new(new Vector2f(50f, 85f), new Vector2f(100, 50)),
                new(new Vector2f(160f, 85f), new Vector2f(100, 50)),
                new(new Vector2f(270f, 85f), new Vector2f(100, 50)),
                new(new Vector2f(380f, 85f), new Vector2f(100, 50)),
                new(new Vector2f(490f, 85f), new Vector2f(100, 50)),
                new(new Vector2f(600f, 85f), new Vector2f(100, 50)),
                new(new Vector2f(710f, 85f), new Vector2f(100, 50)),
                new(new Vector2f(820f, 85f), new Vector2f(100, 50)),

                new(new Vector2f(50f, 145f), new Vector2f(100, 50)),
                new(new Vector2f(160f, 145f), new Vector2f(100, 50)),
                new(new Vector2f(270f, 145f), new Vector2f(100, 50)),
                new(new Vector2f(380f, 145f), new Vector2f(100, 50)),
                new(new Vector2f(490f, 145f), new Vector2f(100, 50)),
                new(new Vector2f(600f, 145f), new Vector2f(100, 50)),
                new(new Vector2f(710f, 145f), new Vector2f(100, 50)),
                new(new Vector2f(820f, 145f), new Vector2f(100, 50)),
            };

            var powerUpTypes = new List<Type>
            {
                typeof(BallSizeIncreaseBonus),
                typeof(PaddleExpansionBonus),
                typeof(ExtraLifeBonus),
                typeof(ExtraScoreBonus),
            };

            foreach (var block in blocks)
            {
                var count = new Random().Next(1, 3);
                var powerUp1 = (Bonus)Activator.CreateInstance(powerUpTypes[new Random().Next(0, powerUpTypes.Count)]);
                var powerUp2 = (Bonus)Activator.CreateInstance(powerUpTypes[3]);
                var powerUp3 = (Bonus)Activator.CreateInstance(powerUpTypes[new Random().Next(0, powerUpTypes.Count)]);
                var powerUps = new List<Bonus> { powerUp1, powerUp2, powerUp3 };
                
                var powerUpsToActivate = new List<Bonus>(count);
                for (var i = 0; i < count; i++)
                {
                    powerUpsToActivate.Add(powerUps[new Random().Next(0, powerUps.Count)]);
                    powerUps.Remove(powerUpsToActivate[i]);
                }
                
                block.Bonuses = powerUpsToActivate;
            }

            _field.AddRange(blocks);
            _field.Add(_statsPanel, true);
        }

        public void SaveToJson()
        {
            var proxy = new Proxy(new JsonSerialization());
            var gameData = new GameData(this);
            proxy.Save(gameData);
        }
        
        public void SaveToTxt()
        {
            var proxy = new Proxy(new TxtSerialization());
            var gameData = new GameData(this);
            proxy.Save(gameData);
        }
        
        public void LoadFromJson()
        {
            var proxy = new Proxy(new JsonSerialization());
            var gameData = proxy.Load();
            if (gameData == null) return;
            Load(gameData.Value);
        }
        
        public void LoadFromTxt()
        {
            var proxy = new Proxy(new TxtSerialization());
            var gameData = proxy.Load();
            if (gameData == null) return;
            Load(gameData.Value);
        }
        
        private void Load(GameData gameData)
        {
            _field = gameData.GameField;
            _player.Load(_settings ,_field, gameData.GameStats, gameData.Bonuses);
            _statsPanel.SetStats(_player.Stats);
            _settings.SetPlayerName(gameData.GameSettings.PlayerName);
            _settings.SetWindowSize(gameData.GameSettings.WindowSize);
            _settings.SetDifficulty(gameData.GameSettings.Difficulty);
            _settingsPanel.SetSettings(_settings);
            _field.Add(_statsPanel, true);
            Resume();
        }
        
        public void Pause() => ActiveState = new PauseState(this);
        public void Resume() => ActiveState = new InGameState(this);

        public void Restart()
        {
            _field.Clear();
            InitializeField();
            _player.Load(_settings, _field, null, null);
            _statsPanel.SetStats(_player.Stats);
            
            Resume();
        }
        public void Exit() => _isRunning = false;
        public void ShowSettings() => ActiveState = new SettingsState(this);
        public void ShowWonGame() => ActiveState = new WonState(this);
        public void ShowLostGame() => ActiveState = new LostState(this);
        
        public struct GameData
        {
            public GameField GameField { get; set; }
            public GameStats GameStats { get; set; }
            public GameSettings GameSettings { get; set; }
            public IEnumerable<Bonus> Bonuses { get; set; }

            public GameData(Game game)
            {
                GameField = game._field;
                GameStats = game._player.Stats;
                GameSettings = game._settings;
                Bonuses = game._player.ActiveBonuses;
            }
        }

        private class InGameState : IGameState
        {
            private readonly Game _game;
            
            public InGameState(Game game)
            {
                _game = game;
            }
            
            public void Enter()
            {
                _game._inputHandler.EscapePressed += _game.Pause;
                _game._statsPanel.MenuButtonClicked += _game.Pause;
                _game._statsPanel.IsEnabled = true;
                _game._field.IsPaused = false;
                _game._player.IsPaused = false;
            }

            public void Exit()
            {
                _game._inputHandler.EscapePressed -= _game.Pause;
                _game._statsPanel.MenuButtonClicked -= _game.Pause;
                _game._statsPanel.IsEnabled = false;
                _game._field.IsPaused = true;
                _game._player.IsPaused = true;
            }
        }
        
        private class PauseState : IGameState
        {
            private readonly Game _game;

            public PauseState(Game game)
            {
                _game = game;
            }

            public void Enter()
            {
                _game._inputHandler.EscapePressed += _game.Resume;
                ShowMenu();
            }

            public void Exit()
            {
                _game._inputHandler.EscapePressed -= _game.Resume;
                HideMenu();
            }

            private void ShowMenu()
            {
                _game._menuPanel.IsEnabled = true;
                _game._menuPanel.Resume += _game.Resume;
                _game._menuPanel.Settings += _game.ShowSettings;
                _game._menuPanel.SaveToJson += _game.SaveToJson;
                _game._menuPanel.SaveToTxt += _game.SaveToTxt;
                _game._menuPanel.SaveToTxt += _game.SaveToJson2;
                _game._menuPanel.LoadFromJson += _game.LoadFromJson;
                _game._menuPanel.LoadFromTxt += _game.LoadFromJson2;
                _game._menuPanel.Restart += _game.Restart;
                _game._menuPanel.Exit += _game.Exit;

                _game._field.Add(_game._menuPanel, true);
            }

            private void HideMenu()
            {
                _game._menuPanel.Resume -= _game.Resume;
                _game._menuPanel.Settings -= _game.ShowSettings;
                _game._menuPanel.SaveToJson -= _game.SaveToJson;
                _game._menuPanel.SaveToTxt -= _game.SaveToTxt;
                _game._menuPanel.LoadFromJson -= _game.LoadFromJson;
                _game._menuPanel.LoadFromTxt -= _game.LoadFromTxt;
                _game._menuPanel.Restart -= _game.Restart;
                _game._menuPanel.Exit -= _game.Exit;

                _game._menuPanel.Destroy();
            }
        }
        
        private class SettingsState : IGameState
        {
            private readonly Game _game;

            public SettingsState(Game game)
            {
                _game = game;
            }

            public void Enter()
            {
                _game._inputHandler.EscapePressed += _game.Pause;
                _game._settingsPanel.IsEnabled = true;
                _game._settingsPanel.Return += _game.Pause;
                _game._settingsPanel.ResolutionChanged += _game._settings.SetWindowSize;
                _game._field.Add(_game._settingsPanel);
            }

            public void Exit()
            {
                _game._inputHandler.EscapePressed -= _game.Pause;
                _game._settingsPanel.Return -= _game.Pause;
                _game._settingsPanel.ResolutionChanged -= _game._settings.SetWindowSize;
                _game._settingsPanel.Destroy();
            }
        }
        
        private class LostState : IGameState
        {
            private readonly Game _game;

            public LostState(Game game)
            {
                _game = game;
            }

            public void Enter()
            {
                _game._inputHandler.EscapePressed += _game.Exit;
                _game._lostGamePanel.IsEnabled = true;
                _game._lostGamePanel.Restart += _game.Restart;
                _game._lostGamePanel.Exit += _game.Exit;
                _game._field.Add(_game._lostGamePanel, true);
            }

            public void Exit()
            {
                _game._inputHandler.EscapePressed -= _game.Exit;
                _game._lostGamePanel.Restart -= _game.Restart;
                _game._lostGamePanel.Exit -= _game.Exit;
                _game._lostGamePanel.Destroy();
            }
        }

        private class WonState : IGameState
        {
            private readonly Game _game;

            public WonState(Game game)
            {
                _game = game;
            }

            public void Enter()
            {
                _game._inputHandler.EscapePressed += _game.Exit;
                _game._wonGamePanel.IsEnabled = true;
                _game._wonGamePanel.Restart += _game.Restart;
                _game._wonGamePanel.Exit += _game.Exit;
                _game._field.Add(_game._wonGamePanel, true);
            }

            public void Exit()
            {
                _game._inputHandler.EscapePressed -= _game.Exit;
                _game._wonGamePanel.Restart -= _game.Restart;
                _game._wonGamePanel.Exit -= _game.Exit;
                _game._wonGamePanel.Destroy();
            }
        }

        private void SaveToJson2()
        {
            var proxy = new Proxy(new JsonSerialization())
            {
                Path = "save2"
            };
            var gameData = new GameData(this);
            proxy.Save(gameData);
        }

        private void LoadFromJson2()
        {
            var proxy = new Proxy(new JsonSerialization())
            {
                Path = "save2"
            };
            var gameData = proxy.Load();
            if (gameData == null) return;
            Load(gameData.Value);
        }
    }
}
