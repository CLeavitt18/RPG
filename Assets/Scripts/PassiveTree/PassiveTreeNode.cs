using System;
using UnityEngine;

[Serializable]
public class PassiveTreeNode : MonoBehaviour
{
    [SerializeField] private PassiveTreeNode[] previuos;
    [SerializeField] private PassiveTreeNode[] next;

    [SerializeField] private PassiveTreeNodeType type;

    [SerializeField] private int amount;
    [SerializeField] private int divotion;

    [SerializeField] private bool active;
    [SerializeField] private bool baseOrPercent;

    public int GetNextPassivesCount()
    {
        return next.Length;
    }

    public PassiveTreeNode GetPreviuos(int index)
    {
        return previuos[index];
    }

    public bool GetActive()
    {
        return active;
    }

    public PassiveTreeNode GetNext(int index)
    {
        int baseTen = 10;

        if (active == false)
        {
            return null;
        }

        if (baseTen > index)
        {
            return next[index - 1];
        }

        while(index / baseTen > 10)
        {
            baseTen *= 10;
        }

        index -= baseTen * (index / baseTen);
        return next[index- 1].GetNext(index);
    }

    public void NegateActive()
    {
        active = !active;
    }
}
