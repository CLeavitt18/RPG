using System;
using UnityEngine;

[Serializable]
public class PassiveTreeNode : MonoBehaviour
{
    [SerializeField] private PassiveTreeNode[] previuos;
    [SerializeField] private PassiveTreeNode[] next;

    [SerializeField] private int divotion;

    [SerializeField] private bool active;


    public PassiveTreeNode GetPreviuos()
    {
        return previuos[0];
    }

    public PassiveTreeNode GetNext()
    {
        return next[0];
    }
}
