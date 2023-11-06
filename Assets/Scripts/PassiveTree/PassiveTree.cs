using UnityEngine;

public class PassiveTree : MonoBehaviour
{
    public PassiveTree instance;
    
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

    public void SetNode(int index)
    {
        PassiveTreeNode node = GetNode(index);

        if(node.GetActive() == false && passivePoints > 0)
        {
            node.NegateActive();
            passivePoints--;
        }
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
