using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPC : AI, ISavable
{
    [SerializeField] private int CurrentSpeachBranch;
    [SerializeField] private int CurrentQuest;

    [SerializeField] private DialogueSet[] NPCSpeach;
    [SerializeField] private DialogueSet VendorSpeach;

    [SerializeField] private QuestHolder[] Quests;

    public int GetCurrentSpeachBranch()
    {
        return CurrentSpeachBranch;
    }

    public int GetCurrentQuest()
    {
        return CurrentQuest;
    }

    public DialogueSet[] GetDialogue()
    {
        return NPCSpeach;
    }

    public DialogueSet GetVendorSpeach()
    {
        return VendorSpeach;
    }

    public QuestHolder[] GetQuests()
    {
        return Quests;
    }

    public void IncrementSpeachBranch()
    {
        Debug.Log("Incrment called");
        CurrentSpeachBranch++;
    }

    public override void BehaviuorOnUpdate()
    {
        entity.BehaviuorOnUpdate();
    }

    public void SetDefaultState(bool priority)
    {
        entity.SetStats(priority);
        entity.Inventory.SetDefaultState(priority);
    }

    public override void CallDeath(bool Animate)
    {
        SaveSystem.SaveNPC(entity, SceneManagerOwn.Manager.SavableObjects.IndexOf(this));

        GetComponent<Containers>().Mode = UiState.Container;

        entity.Inventory.SetToContainer();
        entity.CallDeath(Animate);
    }

    public bool Save(int id)
    {
        return SaveSystem.SaveNPC(entity, id);
    }

    public bool Load(int id)
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);
        path.Append(GlobalValues.NPCFolder);

        if (Directory.Exists(path.ToString()))
        {
            return LoadNPC(id);
        }
        else
        {
            return entity.SetStats(false);
        }
    }

    private bool LoadNPC(int id)
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);
        path.Append(GlobalValues.NPCFolder);
        path.Append('/');
        path.Append(entity.GetName());
        path.Append(id);

        StringBuilder TempPath = new StringBuilder(path.ToString());
        TempPath.Append(GlobalValues.TempExtension);

        path.Append(GlobalValues.SaveExtension);

        NPCData Data;

        if (File.Exists(TempPath.ToString()))
        {
            Data = SaveSystem.LoadNPC(TempPath.ToString());
        }
        else
        {
            Data = SaveSystem.LoadNPC(path.ToString());
        }

        entity.LoadEntity(Data);

        transform.parent.gameObject.GetComponent<NavMeshAgent>().enabled = false;

        CurrentSpeachBranch = Data.CurrentSpeachBranch;
        CurrentQuest = Data.CurrentQuest;

        Vector3 Position;
        Vector3 Rotation;

        Position.x = Data.Position[0];
        Position.y = Data.Position[1];
        Position.z = Data.Position[2];

        transform.parent.position = Position;

        Rotation.x = Data.Rotation[0];
        Rotation.y = Data.Rotation[1];
        Rotation.z = Data.Rotation[2];

        Rotation = Vector3.MoveTowards(transform.parent.transform.rotation.eulerAngles, Rotation, 360f);
        transform.parent.rotation = Quaternion.Euler(Rotation);

        entity.SetStats(true);

        transform.parent.gameObject.GetComponent<NavMeshAgent>().enabled = true;

        if (Data.IsDead)
        {
            CallDeath(false);
        }

        return true;
    }
}
