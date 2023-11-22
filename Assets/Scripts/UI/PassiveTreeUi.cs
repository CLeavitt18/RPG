using System.Collections.Generic;
using UnityEngine;

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

        for (int i = 0; i < nodeButtons.Length; i++)
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
        PassiveTreeData data = SaveSystem.LoadPassiveTree();

        if (data.nodes.Length > 0)
        {
            for (int i = 0; i < data.nodes.Length; i++)
            {
                PassiveTree.instance.SetNode(data.nodes[i], true);
            }
        }

        if (data.passivePoints > 0)
        {
            for (int i = 0; i < data.passivePoints; i++)
            {
                PassiveTree.instance.AddPassivePoint();
            }
        }

        return true;
    }

    public void SetDefaultState(bool priority)
    {
        
    }
}
