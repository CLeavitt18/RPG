using UnityEngine;
using UnityEngine.UI;

public class CatogoryButton : MonoBehaviour
{
    public InventoryUi Parent;

    public InventoryState State;

    private Button Me;

    private void OnEnable()
    {
        Me = GetComponent<Button>();
        Me.onClick.AddListener(OnClickListener);
    }

    public void OnClickListener()
    {
        Parent.CallSetInventory(State);
    }
}
