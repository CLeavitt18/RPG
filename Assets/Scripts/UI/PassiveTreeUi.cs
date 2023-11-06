using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveTreeUi : IUi
{
    [SerializeField] private GameObject Ui;
    [SerializeField] private Button[] nodeButtons;

    [SerializeField] private GameObject activeNodeUi;
    [SerializeField] private GameObject inactiveNodeUi;

    public override void Set()
    {
        if (isActive)
        {
            return;
        }

        Ui.SetActive(true);

        isActive = true;
    }

    public override void Close()
    {
        Ui.SetActive(false);
        isActive = false;
    }

    public void SetNode(int index)
    {
        bool result = PassiveTree.instance.SetNode(index);

        if(result)
        {

        }
    }
}
