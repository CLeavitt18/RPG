
[System.Serializable]
public class WorldData
{
    public int Day;
    public int Month;
    public int Year;

    public float TimeOfDay;

    public bool FirstOpen;

    public string SceneName;
    public WorldData(WorldStateTracker Tracker)
    {
        Day = Tracker.Day;
        Month = Tracker.Month;
        Year = Tracker.Year;
        TimeOfDay = Tracker.TimeOfDay;
        SceneName = Tracker.SceneName;
        FirstOpen = Tracker.FirstOpen;
    }
}
