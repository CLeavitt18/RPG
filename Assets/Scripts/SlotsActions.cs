using TMPro;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotsActions : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private SlotState Mode;

    [SerializeField] private Item _Item;

    [SerializeField] private GameObject EquipedIndicator;

    [SerializeField] private float WaitTime = .25f;

    [SerializeField] public InventoryUi UI;

    [SerializeField] public QuestUi QuestUi;

    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI miscText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI indicatorText;

    [SerializeField] private Image backGroundImage;

    [SerializeField] private bool Clicked = false;

    [SerializeField] private float NextTime;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("pointer Click");
        if (Mode == SlotState.Quest || UI.AmountUi.activeSelf == true)
        {
            return;
        }

        if (Time.realtimeSinceStartup >= NextTime)
        {
            Clicked = false;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Clicked)
            {
                UI.SetFocus(this, 0, _Item, Player.player.GetMode());
                Clicked = false;
            }
            else
            {
                NextTime = Time.realtimeSinceStartup + WaitTime;
                Clicked = true;
            }

            //Debug.Log("Left Mouse Button");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Clicked)
            {
                UI.SetFocus(this, 1, _Item, Player.player.GetMode());
                Clicked = false;
            }
            else
            {
                NextTime = Time.realtimeSinceStartup + WaitTime;
                Clicked = true;
            }

            //Debug.Log("Right Mouse Button");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnMouseEnter Called");
        if (Mode == SlotState.Item)
        {
            if (UI.GetFocus() == this || UI.AmountUi.activeSelf == true)
            {
                return;
            }

            UI.SetFocus(this, 0, _Item, Player.player.GetMode());
        }
        else
        {
            if (QuestUi.GetFocus() == this)
            {
                return;
            }

            QuestUi.SetQuestFocused(this, _Item);
        }
    }

    public void SetState(IUi ui, Item item)
    {
        if (ui as QuestUi)
        {
            QuestUi = ui as QuestUi;
            UI = null;
            Mode = SlotState.Quest;
        }
        else
        {
            UI = ui as InventoryUi;
            QuestUi = null;
            Mode = SlotState.Item;
        }

        _Item = item;

        StringBuilder sb = new StringBuilder(_Item.GetName());

        if (Mode == SlotState.Item)
        {
            if (_Item.GetAmount() > 1)
            {
                sb.Append(" (");
                sb.Append(_Item.GetAmount().ToString("n0"));
                sb.Append(")");
            }

            displayNameText.text = sb.ToString();

            sb.Clear();

            int ItemWeight = _Item.GetWeight();
            int BeforeDecimal = ItemWeight / 100;
            int AfterDecimal = ItemWeight - BeforeDecimal * 100;

            if (BeforeDecimal > 999)
            {
                int AfterComma = BeforeDecimal / 1000;

                BeforeDecimal = AfterComma * 1000 - BeforeDecimal;

                sb.Append(AfterComma);
                sb.Append(',');

                if (BeforeDecimal > 10)
                {
                    sb.Append("00");
                }
                else if (BeforeDecimal > 100)
                {
                    sb.Append('0');
                }
            }

            sb.Append(BeforeDecimal);

            if (AfterDecimal != 0)
            {
                sb.Append('.');

                if (AfterDecimal < 10)
                {
                    sb.Append('0');
                }

                sb.Append(AfterDecimal);
            }

            weightText.text = sb.ToString();

            sb.Clear();

            valueText.text = _Item.GetValue().ToString("n0");

            switch (_Item.tag)
            {
                case GlobalValues.WeaponTag:
                    WeaponHolder weapon = _Item as WeaponHolder;

                    sb.Append(weapon.GetDurability().ToString("n0"));
                    sb.Append('/');
                    sb.Append(weapon.GetMaxDurability().ToString("n0"));

                    break;
                case GlobalValues.ArmourTag:
                case GlobalValues.ShieldTag:
                        ArmourHolder armour = _Item.GetComponent<ArmourHolder>();

                        sb = new StringBuilder(armour.GetCurrentDurability().ToString("n0"));
                        sb.Append('/');
                        sb.Append(armour.GetMaxDurability().ToString("n0"));
                    break;
                default:
                    break;
            }

            miscText.text = sb.ToString();

            backGroundImage.color = _Item.GetRarity();
        }
        else
        {
            weightText.text = "";

            QuestHolder quest = item as QuestHolder;

            displayNameText.text = sb.ToString();

            sb.Clear();

            sb.Append(quest.GetCompleteType().ToString());
            weightText.text = sb.ToString();

            sb.Clear();

            sb.Append(quest.GetLocation());
            miscText.text = sb.ToString();

            sb.Clear();

            sb.Append(quest.GetSource());

            valueText.text = sb.ToString();
        }
    }

    public void SetIndicator(bool state, string text)
    {
        EquipedIndicator.SetActive(state);
        indicatorText.text = text;
    }

    public Item GetItem()
    {
        return _Item;
    }
}
