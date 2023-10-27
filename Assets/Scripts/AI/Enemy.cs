using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : AI, ISavable
{
    public override void BehaviuorOnUpdate()
    {
        entity.BehaviuorOnUpdate();
    }

    public override void CallDeath(bool Animate)
    {
        if (entity.GetDead())
        {
            return;
        }

        SaveSystem.SaveEnemy(entity, SceneManagerOwn.Manager.SavableObjects.IndexOf(this));

        entity.CallDeath(Animate);
    }

    public void SetDefaultState(bool priority)
    {
        entity.SetStats(priority);
        GetComponent<Inventory>().SetDefaultState(priority);
    }

    public bool Save(int id)
    {
        return SaveSystem.SaveEnemy(entity, id);
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
        path.Append(GlobalValues.EnemyFolder);

        if (Directory.Exists(path.ToString()))
        {
            return LoadEnemy(id);
        }
        else
        {
            return entity.SetStats(false);
        }
    }

    private bool LoadEnemy(int id)
    {
        EnemyData Data;

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManager.GetActiveScene().name);
        path.Append(GlobalValues.EnemyFolder);
        path.Append('/');
        path.Append(entity.GetName());
        path.Append(id);

        StringBuilder TempPath = new StringBuilder(path.ToString());
        TempPath.Append(GlobalValues.TempExtension);

        path.Append(GlobalValues.SaveExtension);

        if (File.Exists(TempPath.ToString()))
        {
            Data = SaveSystem.LoadEnemy(TempPath.ToString());
        }
        else
        {
            Data = SaveSystem.LoadEnemy(path.ToString());
        }

        entity.LoadEntity(Data);

        transform.parent.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        Vector3 Position;
        Vector3 Rotation;

        Position.x = Data.Position[0];
        Position.y = Data.Position[1];
        Position.z = Data.Position[2];

        transform.parent.position = Position;

        Rotation.x = Data.Rotation[0];
        Rotation.y = Data.Rotation[1];
        Rotation.z = Data.Rotation[2];

        Rotation = Vector3.MoveTowards(transform.parent.transform.rotation.eulerAngles, Rotation, 360);
        transform.parent.rotation = Quaternion.Euler(Rotation);

        entity.SetStats(true);

        transform.parent.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;

        if (Data.IsDead)
        {
            CallDeath(false);
        }

        return true;
    }

}
