using UnityEngine;

public class PassiveTree : MonoBehaviour
{
    public static PassiveTree instance;
    
    [SerializeField] private PassiveTreeNode start;

    [SerializeField] private int passivePoints = 0;
    [SerializeField] private bool inRespec = false;


    private void OnEnable() 
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AddPassivePoint()
    {
        passivePoints++;
    }

    public bool SetNode(int index)
    {
        if(passivePoints == 0)
        {
            return false;
        }

        PassiveTreeNode node = GetNode(index);

        if((node == null && index != -1) || passivePoints == 0)
        {
            return false;
        }

        if((node != null && node.GetActive() == false) || node == null)
        {
            if (node == null)
            {
                node = start;
            }

            node.NegateActive();
            passivePoints--;
            return true;
        }

        return false;
    }

    public int GetNodeState(int index)
    {
        PassiveTreeNode node = GetNode(index);

        if (node != null && node.GetActive())
        {
            // node is actvice
            return 2;
        }

        if ((index == -1 || GetNodeUsable(index)) && passivePoints != 0 && inRespec == false)
        {
            // node can be activated
            return 1;
        }

        // node is neither actived nor ativatable
        return 0;
    }

    private bool GetNodeUsable(int index)
    {
        // gets the index of the previuos node
        // returns true if that node is active 
        // return false if the node is not active
        int baseTen = 10;

        if (index > baseTen)
        {
            while(index / baseTen > 10)
            {
                baseTen *= 10;
            }

            index -= baseTen * (index / baseTen);
        }
        else
        {
            index = -1;
        }
        
        PassiveTreeNode node = GetNode(index);

        if (node == null || node.GetActive() == false)
        {
            return false;
        }

        return true;
    }

    private PassiveTreeNode GetNode(int index)
    {
        if(index != -1)
        {
            return start.GetNext(index);
        }

        if (start.GetActive() == false)
        {
            return null;
        }

        return start;
    }
}
