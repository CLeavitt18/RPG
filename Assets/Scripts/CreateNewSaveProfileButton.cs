using UnityEngine;

public class CreateNewSaveProfileButton : DoubleClickAction
{
    public GameObject CreateProfileUI;

    protected override void DoubleAction()
    {
        CreateProfileUI.SetActive(true);
    }
}
