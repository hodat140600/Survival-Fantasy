
public class PlaySoundEvent : IEvent
{
    private string _nameSound;
    public string NameSound => _nameSound;
    public PlaySoundEvent(string nameSound) { _nameSound = nameSound; }
}
public class PlaySoundEventWithTimeLife : IEvent
{
    private string _nameSound;
    public string NameSound => _nameSound;

    private float _timeLife;
    public float TimeLife => _timeLife;
    public PlaySoundEventWithTimeLife(string nameSound, float timeLife) { _nameSound = nameSound; _timeLife = timeLife; }
}