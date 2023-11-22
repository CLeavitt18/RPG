using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveButton : MonoBehaviour
{
    [SerializeField] private int index;


    public void SetNode(bool fromLoad = false)
    {
        bool result = PassiveTree.instance.SetNode(index, fromLoad);

        if(result)
        {
            PlayerUi.playerUi.CallSetPassiveTreeButtons();
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
        else if (index == 1) 
        {
            gameObject.GetComponent<Button>().enabled = true;
        }
    }

    public void AddToActiveListForSaving(List<int> activeNodes)
    {
        int state = PassiveTree.instance.GetNodeState(index);

        if (state == 2)
        {
            activeNodes.Add(index);
        }
    }
}
