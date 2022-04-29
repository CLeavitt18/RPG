using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerOwn : MonoBehaviour
{
    public static SceneManagerOwn Manager;

    public int DaysToReset = 14;

    public int[] ResetDate = new int[3];

    public List<ISavable> SavableObjects = new List<ISavable>();

    public string SceneName;

    public bool Priority;
    public bool CanReset = true;

    public bool[] ResetChecks = new bool[3];

    public int Tracker;
    public int Target;

    public void OnEnable()
    {
        Manager = this;
        Time.timeScale = 0;

        CollectSceneData();

        SceneName = SceneManager.GetActiveScene().name;

        Priority = true;

        SetScene();
    }

    public void TempSaveScene()
    {
        Time.timeScale = 0;

        CollectSceneData();

        Target = SavableObjects.Count;
        Tracker = 0;

        for (int i = 0; i < SavableObjects.Count; i++)
        {
            if (SavableObjects[i] != null && SavableObjects[i].Save(i))
            {
                Tracker++;
            }
        }

        SaveSystem.TempSaveScene(this);
    }

    public void CollectSceneData()
    {
        if (SavableObjects.Count != 0)
        {
            SavableObjects.Clear();
        }
        
        GameObject[] Savables = GameObject.FindGameObjectsWithTag(GlobalValues.NPCTag);

        for (int i = 0; i < Savables.Length; i++)
        {
            SavableObjects.Add(Savables[i].GetComponent<AI>() as ISavable);
        }

        Savables = GameObject.FindGameObjectsWithTag(GlobalValues.EnemyTag);

        for (int i = 0; i < Savables.Length; i++)
        {
            SavableObjects.Add(Savables[i].GetComponent<AI>() as ISavable);
        }

        Savables = GameObject.FindGameObjectsWithTag(GlobalValues.ContainerTag);

        for (int i = 0; i < Savables.Length; i++)
        {
            SavableObjects.Add(Savables[i].GetComponent<ISavable>());
        }

        Savables = GameObject.FindGameObjectsWithTag(GlobalValues.ResourceDepositTag);

        for (int i = 0; i < Savables.Length; i++)
        {
            SavableObjects.Add(Savables[i].GetComponent<ISavable>());
        }

        Savables = GameObject.FindGameObjectsWithTag(GlobalValues.DoorTag);

        for (int i = 0; i < Savables.Length; i++)
        {
            SavableObjects.Add(Savables[i].GetComponent<ISavable>());
        }
    }

    public void SetScene()
    {
        Target = SavableObjects.Count;
        Tracker = 0;

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneName);
        path.Append(GlobalValues.TempExtension);

        SceneData Data;

        if (File.Exists(path.ToString()))
        {
            Data = SaveSystem.TempLoadScene();
        }
        else
        {
            Data = SaveSystem.LoadScene();
        }

        if (Data == null)
        {
            ResetDate[0] = WorldStateTracker.Tracker.Day + DaysToReset;
            ResetDate[1] = WorldStateTracker.Tracker.Month;
            ResetDate[2] = WorldStateTracker.Tracker.Year;

            if (ResetDate[0] > 31)
            {
                ResetDate[0] = 1;
                ResetDate[1]++;

                if (ResetDate[1] > 13)
                {
                    ResetDate[1] = 1;
                    ResetDate[2]++;
                }
            }

            Priority = false;

            //Start all funciuotns that are in OnEnable for all gameobjects;
            for (int i = 0; i < SavableObjects.Count; i++)
            {
                SavableObjects[i].SetDefaultState(Priority);
                Tracker++;
            }

            return;
        }

        for (int i = 0; i < 3; i++)
        {
            ResetDate[i] = Data.ResetDate[i];
        }

        if (ResetDate[0] <= WorldStateTracker.Tracker.Day && CanReset)
        {
            ResetChecks[0] = true;

            if (ResetDate[1] <= WorldStateTracker.Tracker.Month)
            {
                ResetChecks[1] = true;

                if (ResetDate[2] <= WorldStateTracker.Tracker.Year)
                {
                    ResetChecks[2] = true;
                }
            }
        }

        if (ResetChecks[0] == true && ResetChecks[1] == true && ResetChecks[2] == true)
        {
            ResetDate[0] = WorldStateTracker.Tracker.Day + DaysToReset;

            if (ResetDate[0] > 31)
            {
                ResetDate[0] -= 31;
                ResetDate[1]++;

                if (ResetDate[1] > 12)
                {
                    ResetDate[1] -= 12;
                    ResetDate[2]++;
                }
            }

            Priority = false;

            //Start all funciuotns that are in OnEnable for all gameobjects;
            for (int i = 0; i < SavableObjects.Count; i++)
            {
                if (SavableObjects[i] is ResourceDeposit)
                {
                    Tracker++;
                    continue;
                }
                else
                {
                    Tracker++;
                }

                SavableObjects[i].SetDefaultState(Priority);
                Debug.Log("Set Default State call on " + SavableObjects[i].ToString());
            }

            TempSaveScene();

            return;
        }

        Tracker = 0;

        for (int i = 0; i < SavableObjects.Count; i++)
        {
            SavableObjects[i].Load(i);

            Tracker++;
        }
    }
}
