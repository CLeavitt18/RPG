using System.Text;
using System.IO;
using System.Collections;
using UnityEngine;
using TMPro;

public class Doors : Interactialbes, IInteractable, ISavable
{
    public bool Locked;

    public GameObject LoadingScreen;

    public void OnEnable()
    {
        PUIInsruction = PlayerUi.playerUi.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        name = Name;
    }

    public IEnumerator ChangeScene()
    {
        Instantiate(LoadingScreen);

        Player.player.SavePlayer(false);

        PUIInsruction.gameObject.SetActive(false);
        WorldStateTracker.Tracker.UpdateTracker();
        SceneManagerOwn.Manager.TempSaveScene();

        int target = SceneManagerOwn.Manager.Target;
        int tracker = SceneManagerOwn.Manager.Tracker;

        yield return new WaitForSecondsRealtime(.1f);

        do
        {
            target = SceneManagerOwn.Manager.Target;
            tracker = SceneManagerOwn.Manager.Tracker;

            yield return new WaitForEndOfFrame();

        } while (tracker < target);

        WorldStateTracker.Tracker.GoingToNewLevel = true;

        UnityEngine.SceneManagement.SceneManager.LoadScene(Name);
    }

    public void Interact(bool State)
    {
        if (Locked)
        {
            Inventory inventory = Player.player.Inventory;

            StringBuilder KeyName = new StringBuilder("Key to ");
            KeyName.Append(Name);

            if (inventory.Find(KeyName.ToString(), GlobalValues.MiscTag) != null)
            {
                Locked = false;
                return;
            }

        }
        else
        {
            StartCoroutine(ChangeScene());
        }
    }

    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder("E: Enter ");
        sb.Append(Name);

        if (Locked)
        {
            sb.Append("(Locked)");
        }

        PUIInsruction.text = sb.ToString();

        base.SetUiOpen();
    }

    public bool Save(int id)
    {
        return SaveSystem.SaveDoor(this);
    }

    public bool Load(int id)
    {
        DoorData data;
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManagerOwn.Manager.SceneName);
        path.Append(GlobalValues.DoorFolder);
        path.Append('/');
        path.Append(Name);

        StringBuilder tempPath = new StringBuilder(path.ToString());
        tempPath.Append(GlobalValues.TempExtension);

        path.Append(GlobalValues.SaveExtension);

        if (File.Exists(tempPath.ToString()))
        {
            data = SaveSystem.LoadDoor(tempPath.ToString());
        }
        else
        {
            data = SaveSystem.LoadDoor(path.ToString());
        }

        Locked = data.Locked;

        return true;
    }

    public void SetDefaultState(bool priority)
    {

    }
}
