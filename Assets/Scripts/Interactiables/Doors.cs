using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doors : Interactialbes, IInteractable, ISavable
{
    public bool Locked;
    
    public GameObject LoadingScreen;

    public void OnEnable()
    {
        PUIInsruction = GameObject.Find("Player UI").transform.GetChild(2).transform.GetChild(1).gameObject;
        name = Name;
    }

    public IEnumerator ChangeScene()
    {
        Instantiate(LoadingScreen);

        Player.player.SavePlayer(false);

        PUIInsruction.SetActive(false);
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
            List<Item> list = Player.player.Inventory.AllItems;
            int[] StartIds = Player.player.Inventory.StartIds;

            StringBuilder KeyName = new StringBuilder("Key to ");
            KeyName.Append(Name);

            for (int i = StartIds[4]; i < list.Count; i++)
            {
                if (list[i].name == KeyName.ToString())
                {
                    Locked = false;
                    return;
                }
            }
        }
        else
        {
            StartCoroutine(ChangeScene());
        }
    }

    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder("E: To Enter ");
        sb.Append(Name);

        if (Locked)
        {
            sb.Append("(Locked)");
        }

        PUIInsruction.GetComponent<Text>().text = sb.ToString();
        //Debug.Log("Door Interaction Text Set");

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

        if (Directory.Exists(tempPath.ToString()))
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
