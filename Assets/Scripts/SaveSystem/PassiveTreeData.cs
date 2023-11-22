using System;
using System.Collections.Generic;

[Serializable]
public class PassiveTreeData
{
    public int passivePoints;

    public int[] nodes;
    

    public PassiveTreeData(PassiveTreeUi tree)
    {
        passivePoints = PassiveTree.instance.GetPassivePoints();
        
        List<int> activeNodes = tree.GetListOfActiveNodes();

        nodes = new int[activeNodes.Count];

        for (int i = 0; i < activeNodes.Count; i++)
        {
            nodes[i] = activeNodes[i];
        }

    }
}
