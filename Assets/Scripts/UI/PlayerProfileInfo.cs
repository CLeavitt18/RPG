using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileInfo : DoubleClickAction
{
    public bool Mode;

    protected override void SingleAction()
    {
        if (Mode)
        {
            MainMenu[] MainMenu = Resources.FindObjectsOfTypeAll<MainMenu>();
            MainMenu[0].SetFocusedSaveFile(gameObject);
        }
        else
        {
            MainMenu[] MainMenu = Resources.FindObjectsOfTypeAll<MainMenu>();
            MainMenu[0].SetFocusedProfile(gameObject);
        }
    }

    protected override void DoubleAction()
    {
        if (Mode)
        {
            MainMenu[] MainMenu = Resources.FindObjectsOfTypeAll<MainMenu>();
            MainMenu[0].LoadProfile();
        }
        else
        {
            MainMenu[] MainMenu = Resources.FindObjectsOfTypeAll<MainMenu>();
            MainMenu[0].SetSaveFilesUI();
        }
    }
}
