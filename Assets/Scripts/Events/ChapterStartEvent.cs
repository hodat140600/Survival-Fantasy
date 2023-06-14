public class ChapterStartEvent: IEvent
{
    public int CurrentChapter;
    public int WavesCount;
    public ChapterStartEvent(int currentChapter, int wavesCount)
    {
        this.CurrentChapter = currentChapter;
        this.WavesCount = wavesCount;
    }
}