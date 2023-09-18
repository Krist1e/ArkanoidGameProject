namespace ArkanoidGameProject.Bonuses;

public class BonusesManager
{
    private readonly List<Bonus> _bonuses = new();
    private IPlayerProperties _playerProperties;

    public BonusesManager(IPlayerProperties playerProperties)
    {
        _playerProperties = playerProperties;
    }

    public IEnumerable<Bonus> Bonuses => _bonuses;

    public void Load(IPlayerProperties playerProperties, IEnumerable<Bonus> powerUps)
    {
        foreach (var powerUp in _bonuses) powerUp.RemoveEffect(_playerProperties);
        _playerProperties = playerProperties;
        _bonuses.Clear();
        foreach (var powerUp in powerUps) Add(powerUp);
    }

    public void Add(Bonus bonus)
    {
        _bonuses.Add(bonus);
        bonus.ApplyEffect(_playerProperties);
        Console.WriteLine($"Bonus {bonus.GetType().Name} added with duration {bonus.Duration}");
    }

    public void Remove(Bonus bonus)
    {
        _bonuses.Remove(bonus);
        bonus.RemoveEffect(_playerProperties);
        Console.WriteLine($"Bonus {bonus.GetType().Name} removed with duration {bonus.Duration}");
    }

    public void Clear()
    {
        foreach (var bonus in _bonuses) bonus.RemoveEffect(_playerProperties);
        _bonuses.Clear();
    }

    public void Update(float deltaTime)
    {
        foreach (var bonus in _bonuses) bonus.UpdateTime(deltaTime);
        _bonuses.Where(bonus => bonus.Duration <= TimeSpan.Zero).ToList().ForEach(Remove);
    }
}