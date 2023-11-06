using UnityEngine;

public class PassiveTree : MonoBehaviour
{
    public static PassiveTree instance;
    
    [SerializeField] private PassiveTreeNode start;

    [SerializeField] private int passivePoints = 0;


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

    public bool SetNode(int index)
    {
        PassiveTreeNode node = GetNode(index);

        if(node == null || passivePoints == 0)
        {
            return false;
        }

        if(node.GetActive() == false)
        {
            node.NegateActive();
            passivePoints--;
            return true;
        }

        return false;
    }

    private PassiveTreeNode GetNode(int index)
    {
        if(index != -1)
        {
            return start.GetNext(index);
        }

        return start;
    }
}
