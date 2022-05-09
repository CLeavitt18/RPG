using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public static class SaveSystem
{
    public static void SavePlayer(string savePath)
    {
        PlayerData Data = new PlayerData(Player.player);

        string Json = JsonUtility.ToJson(Data, true);

        StringBuilder profilePath = new StringBuilder(Application.persistentDataPath);
        profilePath.Append('/');
        profilePath.Append(WorldStateTracker.Tracker.PlayerName);

        if (!Directory.Exists(profilePath.ToString()))
        {
            Directory.CreateDirectory(profilePath.ToString());
        }

        profilePath.Append('/');
        profilePath.Append(WorldStateTracker.Tracker.SaveProfile);

        if (!Directory.Exists(profilePath.ToString()))
        {
            Directory.CreateDirectory(profilePath.ToString());

            StringBuilder Tempsb = new StringBuilder(profilePath.ToString());
            Tempsb.Append(GlobalValues.LevelFolder);

            Directory.CreateDirectory(Tempsb.ToString());
        }

        File.WriteAllText(savePath, Json);

        StringBuilder TempSB = new StringBuilder(profilePath.ToString());
        TempSB.Append("/Player.temp");

        profilePath.Append("/Player.save");

        if (savePath == profilePath.ToString())
        {
            File.Delete(TempSB.ToString());
        }
    }

    public static PlayerData LoadPlayer(string Path)
    {
        if (File.Exists(Path))
        {
            string SaveString = File.ReadAllText(Path);

            PlayerData Data = JsonUtility.FromJson<PlayerData>(SaveString);

            return Data;
        }
        else
        {
            //Debug.Log("Save file not found in " + Path);
            return null;
        }
    }

    public static void SaveWorld(WorldStateTracker World)
    {
        //Debug.Log("Saving World");

        WorldData Data = new WorldData(World);

        string Json = JsonUtility.ToJson(Data, true);

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(World.PlayerName);
        path.Append('/');
        path.Append(World.SaveProfile);

        StringBuilder TempSB = new StringBuilder(path.ToString());
        TempSB.Append("/World.save");

        File.WriteAllText(TempSB.ToString(), Json);
        //Hard save enemies, containers, and resourcedeposits
        string[] Path_temps = Directory.GetFiles(path.ToString(), "*.temp", SearchOption.AllDirectories);

        for (int i = 0; i < Path_temps.Length; i++)
        {
            string NewSavePath = Path.ChangeExtension(Path_temps[i], GlobalValues.SaveExtension);

            string Json_1 = File.ReadAllText(Path_temps[i]);

            File.WriteAllText(NewSavePath, Json_1);
        }
    }

    public static WorldData LoadWorld()
    {
        StringBuilder Path = new StringBuilder(Application.persistentDataPath);
        Path.Append('/');
        Path.Append(WorldStateTracker.Tracker.PlayerName);
        Path.Append('/');
        Path.Append(WorldStateTracker.Tracker.SaveProfile);
        Path.Append("/World.save");

        if (File.Exists(Path.ToString()))
        {
            string SaveString = File.ReadAllText(Path.ToString());
            WorldData Data = JsonUtility.FromJson<WorldData>(SaveString);
            return Data;
        }
        else
        {
            //Debug.Log("Save file not found in " + Path);
            return null;
        }
    }

    public static bool TempSaveScene(SceneManagerOwn Scene)
    {
        SceneData Data = new SceneData(Scene);

        string Json = JsonUtility.ToJson(Data, true);

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);
        path.Append(GlobalValues.TempExtension);

        File.WriteAllText(path.ToString(), Json);

        return true;
    }

    public static SceneData TempLoadScene()
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);
        path.Append(GlobalValues.TempExtension);

        if (File.Exists(path.ToString()))
        {
            string SaveString = File.ReadAllText(path.ToString());
            SceneData Data = JsonUtility.FromJson<SceneData>(SaveString);
            return Data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    public static SceneData LoadScene()
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);
        path.Append(GlobalValues.SaveExtension);

        if (File.Exists(path.ToString()))
        {
            string SaveString = File.ReadAllText(path.ToString());

            SceneData Data = JsonUtility.FromJson<SceneData>(SaveString);

            return Data;
        }
        else
        {
            //Debug.Log("Save file not found in " + Path);
            return null;
        }
    }

    public static bool SaveNPC(AIController NPC, int Id)
    {
        NPCData Data = new NPCData(NPC);

        string Json = JsonUtility.ToJson(Data, true);

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append(GlobalValues.NPCFolder);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append('/');
        path.Append(NPC.Name);
        path.Append(Id);
        path.Append(GlobalValues.TempExtension);

        File.WriteAllText(path.ToString(), Json);

        return true;
    }

    public static NPCData LoadNPC(string path)
    {
        if (File.Exists(path))
        {
            string SaveString = File.ReadAllText(path);

            NPCData data = JsonUtility.FromJson<NPCData>(SaveString);

            return data;
        }
        else
        {
            Debug.Log("File not found in " + path);
            return null;
        }
    }

    public static bool SaveEnemy(AIController Enemy, int Id)
    {
        EnemyData Data = new EnemyData(Enemy);

        string Json = JsonUtility.ToJson(Data, true);

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append(GlobalValues.EnemyFolder);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append('/');
        path.Append(Enemy.name);
        path.Append(Id);
        path.Append(GlobalValues.TempExtension);

        File.WriteAllText(path.ToString(), Json);

        return true;
    }

    public static EnemyData LoadEnemy(string Path)
    {
        if (File.Exists(Path))
        {
            string SaveString = File.ReadAllText(Path);

            EnemyData Data = JsonUtility.FromJson<EnemyData>(SaveString);

            return Data;
        }
        else
        {
            //Debug.Log("Save file not found in " + Path);
            return null;
        }
    }

    public static MinionData LoadMinion(string Path)
    {
        if (File.Exists(Path))
        {
            string SaveString = File.ReadAllText(Path);

            MinionData Data = JsonUtility.FromJson<MinionData>(SaveString);

            return Data;
        }
        return null;
    }

    public static bool SaveMinion(AIController minion, string path)
    {
        MinionData data = new MinionData(minion);

        string Json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, Json);

        return true;
    }

    public static bool SaveContainer(Inventory Container, int ID)
    {
        InventoryData Data = new InventoryData(Container);

        string Json = JsonUtility.ToJson(Data, true);

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append(GlobalValues.ContainerFolder);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append('/');
        path.Append(Container.name);
        path.Append(ID);
        path.Append(GlobalValues.TempExtension);

        File.WriteAllText(path.ToString(), Json);

        return true;
    }

    public static InventoryData LoadContainer(string Path)
    {
        if (File.Exists(Path))
        {
            string SaveString = File.ReadAllText(Path);

            InventoryData Data = JsonUtility.FromJson<InventoryData>(SaveString);

            return Data;
        }
        else
        {

            //Debug.Log("Save file not found in " + Path);
            return null;
        }
    }

    public static bool TempSaveResourceDeposit(GameObject Deposit, int Id)
    {
        DepositData Data = new DepositData(Deposit);

        string Json = JsonUtility.ToJson(Data, true);

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append(GlobalValues.DepositFolder);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append('/');
        path.Append(Deposit.name);
        path.Append(Id);
        path.Append(GlobalValues.TempExtension);

        File.WriteAllText(path.ToString(), Json);

        return true;
    }

    public static DepositData LoadDeposit(string Path)
    {
        if (File.Exists(Path))
        {
            string SaveString = File.ReadAllText(Path);

            DepositData Data = JsonUtility.FromJson<DepositData>(SaveString);

            return Data;
        }
        else
        {
            //Debug.Log("Save file not found in " + Path);
            return null;
        }
    }

    public static bool SaveDoor(Doors Door)
    {
        DoorData data = new DoorData(Door);

        string Json = JsonUtility.ToJson(data);

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append(GlobalValues.DoorFolder);

        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }

        path.Append('/');
        path.Append(Door.Name);
        path.Append(GlobalValues.TempExtension);

        File.WriteAllText(path.ToString(), Json);

        return true;
    }

    public static DoorData LoadDoor(string path)
    {
        if (File.Exists(path))
        {
            string saveString = File.ReadAllText(path);

            DoorData data = JsonUtility.FromJson<DoorData>(saveString);

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void DeleteAllTempFiles()
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);

        string[] Path_temps = Directory.GetFiles(path.ToString(), "*.temp", SearchOption.AllDirectories);

        for (int i = 0; i < Path_temps.Length; i++)
        {
            //Debug.Log("Path from Path_temps" + Path_temps[i]);
            File.Delete(Path_temps[i]);
        }
    }
}
