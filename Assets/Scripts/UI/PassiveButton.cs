using UnityEngine;
using UnityEngine.UI;

public class PassiveButton : MonoBehaviour
{
    [SerializeField] private int index;


    public void SetNode()
    {
        bool result = PassiveTree.instance.SetNode(index);

        if(result)
        {
            SetColor(2);
        }
    }
    
    public void SetColor()
    {
        SetColor(PassiveTree.instance.GetNodeState(index));
    }

    private void SetColor(int index)
    {
        gameObject.GetComponent<Image>().color = GlobalValues.buttonColors[index];

        if(index == 0)
        {
            gameObject.GetComponent<Button>().enabled = false;
        }
    }
}
