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

    public PassiveTreeNode GetPreviuos()
    {
        return previuos[0];
    }

    public PassiveTreeNode GetNext()
    {
        return next[0];
    }
}
