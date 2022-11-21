using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResourceDeposit : Interactialbes, IInteractable, ISavable
{
    public Item Resource;

    public bool IsDead = false;

    public void OnEnable()
    {
        PlayerInstructionText = GameObject.Find("Player UI").transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        gameObject.name = Name;
    }

    public override void SetUiOpen()
    {
        PlayerInstructionText.text = "E: " + gameObject.name;

        PlayerInstructionText.gameObject.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }

    public void Interact(bool State)
    {
        Player.player.Inventory.AddItem(Resource, true, Resource.GetAmount());
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.GetMode());

        IsDead = true;

        if (GameObject.Find("SceneManager") != null)
        {
            SaveSystem.TempSaveResourceDeposit(this.gameObject, SceneManagerOwn.Manager.SavableObjects.IndexOf(this));
        }

        PlayerInstructionText.gameObject.SetActive(false);

        GameObject.FindGameObjectWithTag("Ground").GetComponent<Ground>().CallBakeNavMeshSurface();

        gameObject.SetActive(false);
    }

    public void SetDefaultState(bool priority)
    {

    }

    public bool Save(int id)
    {
        return SaveSystem.TempSaveResourceDeposit(this.gameObject, id);
    }

    public bool Load(int id)
    {
        return LoadResourceDeposit(id);
    }

    public bool LoadResourceDeposit(int id)
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);

        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);
        path.Append(GlobalValues.DepositFolder);
        path.Append('/');
        path.Append(Name);
        path.Append(id);

        StringBuilder tempPath = new StringBuilder(path.ToString());
        tempPath.Append(GlobalValues.TempExtension);

        path.Append(GlobalValues.SaveExtension);
        
        DepositData Data;

        if (File.Exists(tempPath.ToString()))
        {
            Data = SaveSystem.LoadDeposit(tempPath.ToString());
        }
        else
        {
            Data = SaveSystem.LoadDeposit(path.ToString());
        }

        if (Data.IsDead == true)
        {
            Destroy(gameObject);
        }

        return true;
    }
}
