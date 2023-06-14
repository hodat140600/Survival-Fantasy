namespace Skills.Interfaces
{
    ///<summary> Only using for buying skill settings </summary>
    public interface IPercentSettings
    {
        void AddPercent(int addedPercent);
        void SetPercent(int percent);
        int GetAddedPercent();
    }
}