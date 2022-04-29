
[System.Serializable]
public class SceneData
{
    public int[] ResetDate;

    public string SceneName;

    public SceneData(SceneManagerOwn Scene)
    {
        SceneName = Scene.SceneName;

        ResetDate = new int[3];

        for (int i = 0; i < 3; i++)
        {
            ResetDate[i] = Scene.ResetDate[i];
        }
    }
}
