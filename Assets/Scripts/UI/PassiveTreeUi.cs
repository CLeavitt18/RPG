using UnityEngine;
using UnityEngine.UI;

public class PassiveTreeUi : IUi
{
    [SerializeField] private GameObject Ui;
    [SerializeField] private PassiveButton[] nodeButtons;
    

    public override void Set()
    {
        if (isActive)
        {
            return;
        }

        Ui.SetActive(true);

        isActive = true;

        for(int i = 0; i < nodeButtons.Length; i++)
        {
            nodeButtons[i].SetColor();
        }
    }

    public override void Close()
    {
        Ui.SetActive(false);
        isActive = false;
    }
}
