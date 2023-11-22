using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveTreeData
{
    public int[] nodes;
    

    public PassiveTreeData(PassiveTreeUi tree)
    {
        List<int> activeNodes = tree.GetListOfActiveNodes();

        nodes = new int[activeNodes.Count];

        for (int i = 0; i < activeNodes.Count; i++)
        {
            nodes[i] = activeNodes[i];
        }
    }
}
