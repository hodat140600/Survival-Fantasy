
public class StopSoundEvent : IEvent
{
    private string _nameSound;
    public string NameSound => _nameSound;

    public StopSoundEvent(string nameSound) { _nameSound = nameSound; }
}
