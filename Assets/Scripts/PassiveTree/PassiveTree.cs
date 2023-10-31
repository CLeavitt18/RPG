using UnityEngine;

public class PassiveTree : MonoBehaviour
{
    public PassiveTree instance;
    
    [SerializeField] private PassiveTreeNode start;


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
}
