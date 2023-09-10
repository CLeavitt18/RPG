using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUi : IUi
{
    public static InventoryUi containerUi;
    public static InventoryUi playerUi;

    [SerializeField] private InventoryState Mode = InventoryState.AllItems;
    [SerializeField] private UiState UiMode;

    [SerializeField] private Item FocusedItem;

    [SerializeField] private GameObject slot;
    [SerializeField] private GameObject inventoryUi;
    [SerializeField] private GameObject ActionBar;
    [SerializeField] public GameObject AmountUi;
    [SerializeField] public GameObject sortOptionsUi;
    [SerializeField] private GameObject CategoryPrefab;
    [SerializeField] private GameObject InventoryBar;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject ContainerButton;
    [SerializeField] private GameObject PlayerButton;
    [SerializeField] public GameObject PlayerCanvas;

    [SerializeField] private Transform ItemDetailsLocation;
    [SerializeField] private Transform InventroyHolder;

    [SerializeField] private Slider AmountBar;

    [SerializeField] private List<SlotsActions> Slots;

    [SerializeField] private string[] Instructions;

    [SerializeField] private TextMeshProUGUI InstructionText;
    [SerializeField] private TextMeshProUGUI ArmourText;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI AmountText;
    [SerializeField] private TextMeshProUGUI CarryWeigthText;

    [SerializeField] private SlotsActions Focus;

    [SerializeField] private Inventory inventory;

    protected GameObject WeaponsBanner;
    protected GameObject ArmourBanner;
    protected GameObject SpellsBanner;
    protected GameObject RuneBanner;
    protected GameObject PotionsBanner;
    protected GameObject ResourcesBanner;
    protected GameObject MiscBanner;


    private void OnEnable()
    {
        if (UiMode == UiState.Player)
        {
            playerUi = this;
        }
        else
        {
            containerUi = this;
        }
    }

    private void Update()
    {
        if (UiMode == UiState.Player)
        {
            PlayerState playerMode = Player.player.GetMode();

            if (Input.GetButtonDown("E") && Focus != null)
            {
                if (playerMode == PlayerState.InInventoy)
                {
                    StartCoroutine(SetEquipment('E', 0));
                }
                else if (playerMode == PlayerState.InContainer)
                {
                    Player.player.Inventory.TransferItem(FocusedItem, (int)AmountBar.value);
                }
            }

            if (Input.GetButtonDown("R") && Focus != null && playerMode == PlayerState.InInventoy)
            {
                StartCoroutine(SetEquipment('R', 0));
            }
        }
        else
        {
            //needs to be updated for if in a store
            //check gold amount vs item value
            /*if (Input.GetButtonDown("E") && Focus != null)
            {
                inventory.TransferItem(FocusedItem, 1);
            }*/
            //logic for moving items from a container or npc when pressing e
            //needs updating
        }
    }

    public void SetInventroy(bool State)
    {
        inventoryUi.SetActive(State);
        
        if (UiMode == UiState.Player)
        {
            InventoryBar.SetActive(State);
            PlayerCanvas.SetActive(!State);
        }
        
        InventoryPanel.SetActive(State);
        Cursor.visible = State;
        ActionBar.SetActive(State);

        TurnItemDetailsOff();

        if (State == true)
        {
            CallSetInventory(InventoryState.AllItems);
        }
        else
        {
            if (AmountUi.activeSelf)
            {
                AmountUi.SetActive(false);
            }
        }

        if (Focus != null)
        {
            Focus = null;
        }
    }

    public override void Set()
    {
        if (UiMode == UiState.Player)
        {
            inventory = Player.player.Inventory;
        }
        else
        {
            inventory = Player.player.GetHit().GetComponent<Inventory>();
        }

        if (InventoryBar != null)
        {
            InventoryBar.SetActive(true);
        }

        isActive = true;

        int LoopStart = 0;
        int LoopEnd = 0;

        if (InventroyHolder.childCount > 0)
        {
            int children = InventroyHolder.childCount;

            for (int i = 0; i < children; i++)
            {
                Destroy(InventroyHolder.GetChild(i).gameObject);
            }

            Slots.Clear();
        }

        Item gold = inventory.Find("Gold", GlobalValues.MiscTag);

        if (gold != null)
        {
            if (UiMode == UiState.Container)
            {
                if (Player.player.GetMode() == PlayerState.InStore)
                {
                    GoldText.text = gold.GetAmount().ToString("n0");
                }
                else
                {
                    GoldText.text = "";
                }
            }
            else if (UiMode == UiState.Player)
            {
                GoldText.text = gold.GetAmount().ToString("n0");
            }
        }
        else
        {
            if (UiMode == UiState.Container)
            {
                if (Player.player.GetMode() == PlayerState.InStore)
                {
                    GoldText.text = "0";
                }
                else
                {
                    GoldText.text = "";
                }
            }
            else if (UiMode == UiState.Player)
            {
                GoldText.text = "0";
            }
        }

        InstructionText.text = "";

        if (WeaponsBanner != null)
        {
            Destroy(WeaponsBanner);
            WeaponsBanner = null;
        }

        if (ArmourBanner != null)
        {
            Destroy(ArmourBanner);
            ArmourBanner = null;
        }

        if (SpellsBanner != null)
        {
            Destroy(SpellsBanner);
            SpellsBanner = null;
        }

        if (RuneBanner != null)
        {
            Destroy(RuneBanner);
            RuneBanner = null;
        }

        if (PotionsBanner != null)
        {
            Destroy(PotionsBanner);
            PotionsBanner = null;
        }

        if (ResourcesBanner != null)
        {
            Destroy(ResourcesBanner);
            ResourcesBanner = null;
        }

        if (MiscBanner != null)
        {
            Destroy(MiscBanner);
            MiscBanner = null;
        }

        switch (Mode)
        {
            case InventoryState.AllItems:
                LoopStart = 0;
                LoopEnd = inventory.Count;
                break;
            case InventoryState.Weapons:
                LoopStart = 0;
                LoopEnd = inventory.GetStart(GlobalValues.ArmourStart);
                break;
            case InventoryState.Armour:
                LoopStart = inventory.GetStart(GlobalValues.ArmourStart);
                LoopEnd = inventory.GetStart(GlobalValues.SpellStart);
                break;
            case InventoryState.Spells:
                LoopStart = inventory.GetStart(GlobalValues.SpellStart);
                LoopEnd = inventory.GetStart(GlobalValues.RuneStart);
                break;
            case InventoryState.Runes:
                LoopStart = inventory.GetStart(GlobalValues.RuneStart);
                LoopEnd = inventory.GetStart(GlobalValues.PotionStart);
                break;
            case InventoryState.Potions:
                LoopStart = inventory.GetStart(GlobalValues.PotionStart);
                LoopEnd = inventory.GetStart(GlobalValues.ResourceStart);
                break;
            case InventoryState.Resources:
                LoopStart = inventory.GetStart(GlobalValues.ResourceStart);
                LoopEnd = inventory.GetStart(GlobalValues.MiscStart);
                break;
            case InventoryState.Misc:
                LoopStart = inventory.GetStart(GlobalValues.MiscStart);
                LoopEnd = inventory.Count;
                break;
        }

        for (int i = LoopStart; i < LoopEnd; i++)
        {
            Item Item = inventory[i].GetComponent<Item>();

            if (Player.player.GetMode() == PlayerState.InStore && ((Item.GetEquipable() && Item.GetEquiped()) || Item == gold))
            {
                continue;
            }

            switch (Item.tag)
            {
                case GlobalValues.WeaponTag:
                    if (WeaponsBanner == null)
                    {
                        WeaponsBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        WeaponsBanner.GetComponent<TextMeshProUGUI>().text = "Weapons\n___________________________________________________";
                    }
                    break;
                case GlobalValues.ArmourTag:
                case GlobalValues.ShieldTag:
                    if (ArmourBanner == null)
                    {
                        ArmourBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        ArmourBanner.GetComponent<TextMeshProUGUI>().text = "Armour\n___________________________________________________";
                    }
                    break;
                case GlobalValues.SpellTag:
                    if (SpellsBanner == null)
                    {
                        SpellsBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        SpellsBanner.GetComponent<TextMeshProUGUI>().text = "Spells\n___________________________________________________";
                    }
                    break;
                case GlobalValues.RuneTag:
                    if (RuneBanner == null)
                    {
                        RuneBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        RuneBanner.GetComponent<TextMeshProUGUI>().text = "Runes\n___________________________________________________";
                    }
                    break;
                case GlobalValues.PotionTag:
                    if (PotionsBanner == null)
                    {
                        PotionsBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        PotionsBanner.GetComponent<TextMeshProUGUI>().text = "Potions\n___________________________________________________";
                    }
                    break;
                case GlobalValues.ResourceTag:
                    if (ResourcesBanner == null)
                    {
                        ResourcesBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        ResourcesBanner.GetComponent<TextMeshProUGUI>().text = "Resources\n___________________________________________________";
                    }
                    break;
                default://Gold | Misc | Keys
                    if (MiscBanner == null)
                    {
                        MiscBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        MiscBanner.GetComponent<TextMeshProUGUI>().text = "Misc\n___________________________________________________";
                    }
                    break;
            }

            SpawnItemInventorySlot(Item);
        }
    }

    public override void Clear()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        int children = InventroyHolder.childCount;

        if (!inventoryUi.activeSelf)
        {
            inventoryUi.SetActive(true);
            InventoryPanel.SetActive(true);
        }

        for (int i = 0; i < children; i++)
        {
            Destroy(InventroyHolder.GetChild(i).gameObject);
        }

        Slots.Clear();

        if (WeaponsBanner != null)
        {
            Destroy(WeaponsBanner);
        }

        if (ArmourBanner != null)
        {
            Destroy(ArmourBanner);
        }

        if (SpellsBanner != null)
        {
            Destroy(SpellsBanner);
        }

        if (ResourcesBanner != null)
        {
            Destroy(ResourcesBanner);
        }

        if (MiscBanner != null)
        {
            Destroy(MiscBanner);
        }

        TurnItemDetailsOff();

        InstructionText.text = "";

        FocusedItem = null;
        Focus = null;
    }

    public override void Close()
    {
        SetInventroy(false);
    }

    private void SpawnItemInventorySlot(Item Item)
    {
        GameObject NewSlot = Instantiate(this.slot, InventroyHolder);

        SlotsActions slot = NewSlot.GetComponent<SlotsActions>();

        slot.SetState(this, Item);

        Slots.Add(slot);
    }

    public void CallSetInventory(InventoryState state)
    {
        Mode = state;
        Focus = null;
        Set();

        if (UiMode == UiState.Player)
        {
            if (Player.player.GetMode() != PlayerState.InStore)
            {
                playerUi.SetPlayerEquipedIndicators();
            }

            playerUi.UpDateWeight();
        }
    }

    public void OpenCloseInventory(bool State)
    {
        if (UiMode == UiState.Container)
        {
            if (State)
            {
                inventory = Player.player.GetHit().GetComponent<Inventory>();

                CallSetInventory(InventoryState.AllItems);
            
                if (Player.player.GetMode() == PlayerState.InStore)
                {
                    PlayerButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Sell";
                    ContainerButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Buy";
                }
                else if (Player.player.GetMode() == PlayerState.InContainer)
                {
                    PlayerButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = WorldStateTracker.Tracker.PlayerName;
                    ContainerButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = inventory.name;
                }
            }
            else
            {
                Clear();
                Close();
            }

        }
        else if (UiMode == UiState.Player)
        {
            UpDateWeight();
        }

        if (ItemDetailsLocation.transform.childCount != 0)
        {
            Destroy(ItemDetailsLocation.transform.GetChild(0).gameObject);
        }

        inventoryUi.SetActive(State);
        InventoryPanel.SetActive(State);
        ActionBar.SetActive(State);
    }

    public void SetFocus(SlotsActions Slot, int ClickSource, Item item, PlayerState playerMode)
    {
        if (Focus != null && Focus == Slot)
        {
            if (FocusedItem.GetAmount() >= 4 && playerMode != PlayerState.InInventoy)
            {
                AmountUi.SetActive(true);
                AmountBar.maxValue = FocusedItem.GetAmount();
            }
            else
            {
                CallAddItem(ClickSource, 1);
            }

            return;
        }

        //QuestInfoUi.SetActive(false);

        Focus = Slot;
        FocusedItem = item;

        if (UiMode == UiState.Player)
        {
            if (playerMode == PlayerState.InContainer)
            {
                InstructionText.text = Instructions[3];
            }
            else if (playerMode == PlayerState.InStore)
            {
                InstructionText.text = Instructions[5];
            }
            else if (playerMode == PlayerState.InInventoy)
            {
                if (item.GetEquipable())
                {
                    if (item.GetEquiped())
                    {
                        InstructionText.text = Instructions[1];
                    }
                    else
                    {
                        InstructionText.text = Instructions[0];
                    }
                }
                else if (FocusedItem.CompareTag(GlobalValues.PotionTag))
                {
                    InstructionText.text = Instructions[7];
                }
                else
                {
                    InstructionText.text = Instructions[2];
                }
            }
        }
        else if (UiMode == UiState.Container)
        {
            InstructionText.text = Instructions[4];
        }
        else
        {
            //Store
            InstructionText.text = Instructions[6];
        }

        if (ItemDetailsLocation.childCount != 0)
        {
            Destroy(ItemDetailsLocation.GetChild(0).gameObject);
        }

        Helper.helper.CreateItemDetails(FocusedItem, ItemDetailsLocation);
    }

    public void TurnItemDetailsOff()
    {
        if (ItemDetailsLocation.childCount != 0)
        {
            Destroy(ItemDetailsLocation.GetChild(0).gameObject);
        }
    }

    public void AddAmount()
    {
        AmountBar.value++;
        SetAmountText();
    }

    public void SubAmount()
    {
        AmountBar.value--;
        SetAmountText();
    }

    public void AllAmount()
    {
        AmountBar.value = AmountBar.maxValue;
        ConfirmAmount();
    }

    public void SetAmountText()
    {
        StringBuilder sb = new StringBuilder("How many?");
        sb.Append("\n");
        sb.Append(AmountBar.value);

        AmountText.text = sb.ToString();
    }

    public void ConfirmAmount()
    {
        CallAddItem(0, (int)AmountBar.value);
        CancelAmount();
    }

    public void CancelAmount()
    {
        AmountUi.SetActive(false);
        AmountBar.value = 1;
    }

    public void CallAddItem(int ClickSource, int amount)
    {
        if (UiMode == UiState.Player)
        {
            PlayerState playerMode = Player.player.GetMode();

            if (playerMode == PlayerState.InStore)
            {
                Player.player.Inventory.SellItem(FocusedItem, amount, FocusedItem.GetValue());
                Player.player.CalculateSpeed();
            }
            else if (playerMode == PlayerState.InInventoy)
            {
                if (FocusedItem.GetEquipable())
                {
                    StartCoroutine(SetEquipment('E', ClickSource));
                }
                else if (FocusedItem.CompareTag(GlobalValues.PotionTag))
                {
                    FocusedItem.GetComponent<Consumable>().Action();
                }
            }
            else if (playerMode == PlayerState.InContainer)
            {
                Player.player.Inventory.TransferItem(FocusedItem, amount);
                Player.player.CalculateSpeed();
            }
        }
        else
        {
            if (inventory.GetMode() == UiState.Container)
            {
                inventory.TransferItem(FocusedItem, amount);
                return;
            }
            else //if (Container.Mode == UiState.Store)
            {
                inventory.Trade(FocusedItem, amount);
                return;
            }
        }
    }

    public IEnumerator SetEquipment(Char KeyTracker, int ClickSource)
    {
        if (KeyTracker == 'E')
        {
            Item heldItem;

            if (FocusedItem is WeaponHolder || FocusedItem is SpellHolder || FocusedItem is ShieldHolder)
            {
                for (int i = 0; i < 2; i++)
                {
                    heldItem = Player.player.GetHeldItem(i);

                    if (i == ClickSource && heldItem != null &&
                        heldItem != FocusedItem)
                    {
                        Player.player.UnequipItem(heldItem);
                    }
                    else if (i != ClickSource && heldItem == FocusedItem &&
                        heldItem != Player.player.GetHeldItem(1))
                    {
                        Player.player.UnequipItem(FocusedItem);
                    }
                }
            }
            else if (FocusedItem is ArmourHolder holder)
            {
                int id = (int)holder.GetArmourType();

                ArmourHolder armour = Player.player.GetEquipedArmour(id);

                if (armour != null &&
                    FocusedItem != armour.gameObject)
                {
                    Player.player.UnequipItem(armour);
                }
            }

            if (FocusedItem.GetEquiped())
            {
                Player.player.UnequipItem(FocusedItem);
                goto CleanUpUi;
            }

            Player.player.EquipItem(FocusedItem, ClickSource);

        CleanUpUi:
            yield return new WaitForEndOfFrame();

            Player.player.Inventory.UpdateInventory();

            CallSetInventory(Mode);
            SetPlayerEquipedIndicators();

            Focus = null;

            yield break;
        }
        else if (KeyTracker == 'R')
        {
            Item item = FocusedItem.GetComponent<Item>();

            item.SpawnItem();

            Player.player.Inventory.RemoveItem(FocusedItem, item.GetAmount(), false);
            SetPlayerEquipedIndicators();
            Focus = null;

            yield break;
        }
        else
        {
            Debug.Log("Invalid key " + KeyTracker);
        }
    }

    public void SetPlayerEquipedIndicators()
    {
        Inventory AllItems = Player.player.Inventory;

        if (AllItems.Count == 0 || Slots.Count == 0)
        {
            return;
        }

        for (int i = 0; i < AllItems.Count; i++)
        {
            if (i >= Slots.Count)
            {
                break;
            }

            Item rightHand = Player.player.GetHeldItem(0);
            Item leftHand = Player.player.GetHeldItem(1);

            if (AllItems[i].CompareTag(GlobalValues.WeaponTag) ||
            AllItems[i].CompareTag(GlobalValues.SpellTag) ||
            AllItems[i].CompareTag(GlobalValues.TorchTag) ||
            AllItems[i].CompareTag(GlobalValues.ShieldTag))
            {
                if (AllItems[i] == rightHand)
                {
                    if (rightHand == leftHand)
                    {
                        Slots[i].SetIndicator(true, "LR");
                        break;
                    }
                    else
                    {
                        Slots[i].SetIndicator(true, "R");
                    }
                }
                else if (AllItems[i] == leftHand)
                {
                    Slots[i].SetIndicator(true, "L");
                }
                else
                {
                    Slots[i].SetIndicator(false, "");
                }
            }

            if (AllItems[i].CompareTag(GlobalValues.ArmourTag))
            {
                ArmourHolder armour = AllItems[i].GetComponent<ArmourHolder>();

                int ArmourID = (int)armour.GetArmourType();

                if (Player.player.GetEquipedArmour(ArmourID) == null)
                {
                    continue;
                }

                if (armour == Player.player.GetEquipedArmour(ArmourID))
                {
                    Slots[i].GetComponent<SlotsActions>().SetIndicator(true, "");
                }
                else
                {
                    Slots[i].GetComponent<SlotsActions>().SetIndicator(false, "");
                }
            }
        }
    }

    public void ChangeView(bool Action)
    {
        if (playerUi.inventoryUi.activeSelf == Action)
        {
            return;
        }

        if (Action == true)
        {
            playerUi.Set();
        }

        playerUi.inventoryUi.SetActive(Action);
        playerUi.InventoryBar.SetActive(Action);
        playerUi.InventoryPanel.SetActive(Action);

        inventoryUi.SetActive(!Action);
        InventoryPanel.SetActive(!Action);

        TurnItemDetailsOff();

        playerUi.TurnItemDetailsOff();

        playerUi.Focus = null;

        Focus = null;
    }

    public void UpDateWeight()
    {
        StringBuilder WeightText = new StringBuilder();

        StringBuilder[] WeightString = new StringBuilder[2];

        for (int i = 0; i < 2; i++)
        {
            int Weight;

            WeightString[i] = new StringBuilder();

            if (i == 0)
            {
                Weight = Player.player.Inventory.GetCarryWeight();
            }
            else
            {
                Weight = Player.player.Inventory.GetMaxCarryWeight();
            }

            int BeforeDecimal = Weight / 100;
            int AfterDecimal = Weight - BeforeDecimal * 100;

            if (BeforeDecimal > 999)
            {
                int AfterComma = BeforeDecimal / 1000;

                BeforeDecimal = AfterComma * 1000 - BeforeDecimal;

                WeightString[i].Append(AfterComma);
                WeightString[i].Append(',');

                if (BeforeDecimal > 10)
                {
                    WeightString[i].Append("00");
                }
                else if (BeforeDecimal > 100)
                {
                    WeightString[i].Append('0');
                }
            }

            WeightString[i].Append(BeforeDecimal);

            if (AfterDecimal != 0)
            {
                WeightString[i].Append('.');

                if (AfterDecimal < 10)
                {
                    WeightString[i].Append("0");
                }

                WeightString[i].Append(AfterDecimal);
            }

            WeightText.Append(WeightString[i].ToString());

            if (i == 0)
            {
                WeightText.Append(" / ");
            }
        }

        CarryWeigthText.text = WeightText.ToString();
    }

    public InventoryState GetMode()
    {
        return Mode;
    }

    public SlotsActions GetFocus()
    {
        return Focus;
    }
}
