using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveTreeUi : IUi, ISavable
{
    [SerializeField] private GameObject Ui;
    [SerializeField] private PassiveButton[] nodeButtons;
    

    public override void Set()
    {
        /*if (isActive)
        {
            return;
        }*/

        Ui.SetActive(true);

        isActive = true;

        for(int i = 0; i < nodeButtons.Length; i++)
        {
            nodeButtons[i].SetColor();
        }
    }

    public override void Close()
    {
        Ui.SetActive(false);
        isActive = false;
    }

    public List<int> GetListOfActiveNodes()
    {
        List<int> nodes = new List<int>();

        for (int i = 0; i < nodes.Count; i++)
        {
            nodeButtons[i].AddToActiveListForSaving(nodes);
        }

        return nodes;
    }

    public bool Save(int id)
    {
        return SaveSystem.SavePassiveTree(this);
    }
    public bool Load(int id)
    {

        StringBuilder sb = new StringBuilder(Application.persistentDataPath);
        sb.Append(WorldStateTracker.Tracker.PlayerName);
        sb.Append('/');
        sb.Append(WorldStateTracker.Tracker.SaveProfile);
        sb.Append(GlobalValues.PlayerFolder);
        //sb.Append();
        return true;
    }
    public void SetDefaultState(bool priority)
    {
        
    }
}
