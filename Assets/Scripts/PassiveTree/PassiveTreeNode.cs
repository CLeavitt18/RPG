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

    public PassiveTreeNode GetNext(int index)
    {
        return next[index];
    }
}
