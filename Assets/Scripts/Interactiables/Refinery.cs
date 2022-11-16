using UnityEngine;
using UnityEngine.UI;

public class Refinery : Interactialbes, IInteractable
{
    public GameObject RefinaryUI;

    public void OnEnable()
    {
        PUIInsruction = GameObject.Find("Player UI").transform.GetChild(0).transform.GetChild(1).gameObject;
        RefinaryUI = GameObject.Find("RefinaryUi").transform.GetChild(0).gameObject;
        gameObject.name = Name;
    }

    public void Interact(bool State)
    {
        RefinaryUI.SetActive(State);
        RefinaryUI.transform.parent.gameObject.GetComponent<RefineryUi>().SetRefineryToDefault(true);
        SetPlayerState(State);
    }
    public override void SetUiOpen()
    {
        PUIInsruction.GetComponent<Text>().text = "E: Use " + Name;

        base.SetUiOpen();
    }

}
