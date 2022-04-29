using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldStateTracker : MonoBehaviour
{
    public static WorldStateTracker Tracker;

    public float TimeOfDay;

    public int Day;
    public int Month;
    public int Year;

    public bool FirstOpen = true;
    public bool GoingToNewLevel = false;

    public string SceneName;
    public string PlayerName;
    public string SaveProfile;

    public void OnEnable()
    {
        if (Tracker == null)
        {
            DontDestroyOnLoad(gameObject);
            Tracker = this;
            //Debug.Log("World State Tracker " + Tracker.gameObject.name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTracker()
    {
        TimeOfDay = GameObject.Find("LightManager").GetComponent<LightingManager>().TimeOfDay;

        if (Day == 31)
        {
            Month++;
            Day = 1;

            if (Month == 13)
            {
                Year++;
                Month = 1;
            }
        }
    }

    public void CallSaveGame()
    {
        StartCoroutine(SaveGame());
    }

    public IEnumerator SaveGame()
    {
        Player.player.SavePlayer(true);
        UpdateTracker();
        SceneName = SceneManager.GetActiveScene().name;

        yield return new WaitForEndOfFrame();

        SceneManagerOwn.Manager.TempSaveScene();

        yield return new WaitForEndOfFrame();

        int tracker = SceneManagerOwn.Manager.Tracker;
        int target = SceneManagerOwn.Manager.Target;

        do
        {
            tracker = SceneManagerOwn.Manager.Tracker;
            yield return new WaitForEndOfFrame();

        } while (tracker < target);

        SaveSystem.SaveWorld(this);

        yield return new WaitForEndOfFrame();

        SaveSystem.DeleteAllTempFiles();

        Player.player.SetPlayerStateActive();

        PlayerUi.playerUi.StartPause(false);
    }

    public void LoadGame()
    {
        WorldData Data = SaveSystem.LoadWorld();

        Day = Data.Day;
        Month = Data.Month;
        Year = Data.Year;

        TimeOfDay = Data.TimeOfDay;
        SceneName = Data.SceneName;
        FirstOpen = Data.FirstOpen;

        GoingToNewLevel = false;

        SaveSystem.DeleteAllTempFiles();

        SceneManager.LoadScene(SceneName);
    }
}
