using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveTreeUi : IUi
{
    [SerializeField] private GameObject Ui;

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
        PassiveTree.instance.SetNode(index);
    }
}
