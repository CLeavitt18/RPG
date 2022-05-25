using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : IUi
{
    public static InventoryUi containerUi;
    public static InventoryUi playerUi;

    public InventoryState Mode = InventoryState.AllItems;
    public UiState UiMode;

    public Item FocusedItem;

    public GameObject slot;
    public GameObject inventoryUi;
    public GameObject ActionBar;
    public GameObject AmountUi;
    public GameObject CategoryPrefab;
    public GameObject InventoryBar;
    public GameObject ContainerButton;
    public GameObject PlayerButton;
    public GameObject PlayerCanvas;

    public Transform ItemDetailsLocation;
    public Transform InventroyHolder;

    public Slider AmountBar;

    public List<SlotsActions> Slots;

    public string[] Instructions;

    public Text ItemNameText;
    public Text InstructionText;
    public Text ArmourText;
    public Text GoldText;
    public Text AmountText;
    public Text CarryWeigthText;

    public SlotsActions Focus;

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
        InventoryBar.SetActive(State);
        Cursor.visible = State;
        PlayerCanvas.SetActive(!State);
        ActionBar.SetActive(State);

        TurnItemDetailsOff();

        if (State == true)
        {
            CallSetInventory(InventoryState.AllItems);
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

        if (UiMode == UiState.Container)
        {
            if (!inventory.gameObject.CompareTag(GlobalValues.ContainerTag))
            {
                GoldText.text = "0";
            }
            else
            {
                GoldText.text = "";
            }
        }

        if (UiMode == UiState.Player)
        {
            GoldText.text = "0";
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

        StringBuilder sb;

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

            if (Item.CompareTag(GlobalValues.GoldTag))
            {
                if (UiMode == UiState.Container && Player.player.GetHit().CompareTag(GlobalValues.NPCTag))
                {
                    GoldText.text = Item.GetAmount().ToString("n0");
                }
                else if (UiMode == UiState.Player)
                {
                    GoldText.text = Item.GetAmount().ToString("n0");
                }

                if (Player.player.GetMode() == PlayerState.InStore)
                {
                    continue;
                }
            }

            if (Item.GetEquipable() &&
                Player.player.GetMode() == PlayerState.InStore)
            {
                if (Item.GetEquiped())
                {
                    continue;
                }
            }

            Text MiscText;
            int id;

            switch (Item.tag)
            {
                case GlobalValues.WeaponTag:
                    if (WeaponsBanner == null)
                    {
                        WeaponsBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        WeaponsBanner.GetComponent<Text>().text = "Weapons\n___________________________________________________";
                    }

                    if (Mode == InventoryState.AllItems || Mode == InventoryState.Weapons)
                    {
                        id = SpawnItemInventorySlot(Item);
                        MiscText = Slots[id].transform.GetChild(2).gameObject.GetComponent<Text>();

                        WeaponHolder Weapon = Item.GetComponent<WeaponHolder>();

                        sb = new StringBuilder(Weapon.GetDurability().ToString("n0"));
                        sb.Append('/');
                        sb.Append(Weapon.GetMaxDurability().ToString("n0"));

                        MiscText.text = sb.ToString();
                    }
                    break;
                case GlobalValues.ArmourTag:
                    if (ArmourBanner == null)
                    {
                        ArmourBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        ArmourBanner.GetComponent<Text>().text = "Armour\n___________________________________________________";
                    }

                    if (Mode == InventoryState.AllItems || Mode == InventoryState.Armour)
                    {
                        id = SpawnItemInventorySlot(Item);
                        MiscText = Slots[id].transform.GetChild(2).gameObject.GetComponent<Text>();

                        ArmourHolder armour = Item.GetComponent<ArmourHolder>();

                        sb = new StringBuilder(armour.CurrentDurability.ToString("n0"));
                        sb.Append('/');
                        sb.Append(armour.MaxDurability.ToString("n0"));

                        MiscText.text = sb.ToString();
                    }
                    break;
                case GlobalValues.SpellTag:
                    if (SpellsBanner == null)
                    {
                        SpellsBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        SpellsBanner.GetComponent<Text>().text = "Spells\n___________________________________________________";
                    }

                    if (Mode == InventoryState.AllItems || Mode == InventoryState.Spells)
                    {
                        id = SpawnItemInventorySlot(Item);
                        MiscText = Slots[id].transform.GetChild(2).gameObject.GetComponent<Text>();

                        SpellHolder Spell = Item.GetComponent<SpellHolder>();

                        //MiscText.text = Spell.ManaCost.ToString("n0");
                    }
                    break;
                case GlobalValues.RuneTag:
                    if (RuneBanner == null)
                    {
                        RuneBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        RuneBanner.GetComponent<Text>().text = "Runes\n___________________________________________________";
                    }

                    if (Mode == InventoryState.AllItems || Mode == InventoryState.Runes)
                    {
                        id = SpawnItemInventorySlot(Item);
                        MiscText = Slots[id].transform.GetChild(2).gameObject.GetComponent<Text>();
                        MiscText.text = "";
                    }

                    break;
                case GlobalValues.PotionTag:
                    if (PotionsBanner == null)
                    {
                        PotionsBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        PotionsBanner.GetComponent<Text>().text = "Potions\n___________________________________________________";
                    }

                    if (Mode == InventoryState.AllItems || Mode == InventoryState.Potions)
                    {
                        id = SpawnItemInventorySlot(Item);
                        MiscText = Slots[id].transform.GetChild(2).gameObject.GetComponent<Text>();
                        MiscText.text = "";
                    }

                    break;
                case GlobalValues.ResourceTag:
                    if (ResourcesBanner == null)
                    {
                        ResourcesBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        ResourcesBanner.GetComponent<Text>().text = "Resources\n___________________________________________________";
                    }

                    if (Mode == InventoryState.AllItems || Mode == InventoryState.Resources)
                    {
                        id = SpawnItemInventorySlot(Item);
                        MiscText = Slots[id].transform.GetChild(2).gameObject.GetComponent<Text>();
                        MiscText.text = "";
                    }
                    break;
                default://Gold | Misc | Keys
                    if (MiscBanner == null)
                    {
                        MiscBanner = Instantiate(CategoryPrefab, InventroyHolder);
                        MiscBanner.GetComponent<Text>().text = "Misc\n___________________________________________________";
                    }

                    if (Mode == InventoryState.AllItems || Mode == InventoryState.Misc)
                    {
                        id = SpawnItemInventorySlot(Item);
                        MiscText = Slots[id].transform.GetChild(2).gameObject.GetComponent<Text>();

                        MiscText.text = "";
                    }
                    break;
            }
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

        if (InventoryBar != null)
        {
            InventoryBar.SetActive(false);
        }
    }

    public override void Close()
    {
        SetInventroy(false);
    }

    private int SpawnItemInventorySlot(Item Item)
    {
        StringBuilder WeightString = new StringBuilder();

        int ItemWeight = Item.GetWeight();
        int BeforeDecimal = ItemWeight / 100;
        int AfterDecimal = ItemWeight - BeforeDecimal * 100;

        if (BeforeDecimal > 999)
        {
            int AfterComma = BeforeDecimal / 1000;

            BeforeDecimal = AfterComma * 1000 - BeforeDecimal;

            WeightString.Append(AfterComma);
            WeightString.Append(',');

            if (BeforeDecimal > 10)
            {
                WeightString.Append("00");
            }
            else if (BeforeDecimal > 100)
            {
                WeightString.Append('0');
            }
        }

        WeightString.Append(BeforeDecimal);

        if (AfterDecimal != 0)
        {
            WeightString.Append('.');

            if (AfterDecimal < 10)
            {
                WeightString.Append('0');
            }

            WeightString.Append(AfterDecimal);
        }

        GameObject NewSlot = Instantiate(this.slot, InventroyHolder);

        NewSlot.GetComponent<Image>().color = Item.GetRarity();

        SlotsActions slot = NewSlot.GetComponent<SlotsActions>();

        slot.SetState(this, Item);

        Slots.Add(slot);

        int id = Slots.Count - 1;

        Text NameText = Slots[id].transform.GetChild(0).GetComponent<Text>();
        Text WeightText = Slots[id].transform.GetChild(1).GetComponent<Text>();
        Text MiscText = Slots[id].transform.GetChild(2).GetComponent<Text>();
        Text ValueText = Slots[id].transform.GetChild(3).GetComponent<Text>();

        if (Item.GetAmount() > 1)
        {
            StringBuilder sb = new StringBuilder(Item.GetName());
            sb.Append(" (");
            sb.Append(Item.GetAmount().ToString("n0"));
            sb.Append(")");

            NameText.text = sb.ToString();
        }
        else
        {
            NameText.text = Item.GetName();
        }

        WeightText.text = WeightString.ToString();

        ValueText.text = Item.GetValue().ToString("n0");

        return id;
    }

    public void CallSetInventory(InventoryState state)
    {
        Mode = state;
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
            }
            else
            {
                Clear();
            }

            if (Player.player.GetMode() == PlayerState.InStore)
            {
                PlayerButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Sell";
                ContainerButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Buy";
            }
            else if (Player.player.GetMode() == PlayerState.InContainer)
            {
                PlayerButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = WorldStateTracker.Tracker.PlayerName;
                ContainerButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = inventory.name;
            }
        }

        inventoryUi.SetActive(State);
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
            if (inventory.Mode == UiState.Container)
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

            if (FocusedItem is WeaponHolder || FocusedItem is SpellHolder)
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
                int id = (int)holder.ArmourType;

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
            Item rightHand = Player.player.GetHeldItem(0);
            Item leftHand = Player.player.GetHeldItem(1);

            if (AllItems[i].CompareTag(GlobalValues.WeaponTag) || 
            AllItems[i].CompareTag(GlobalValues.SpellTag) || 
            AllItems[i].CompareTag(GlobalValues.TorchTag))
            {
                if (AllItems[i] == rightHand)
                {
                    if (rightHand == leftHand)
                    {
                        Slots[i].SetIndicator(true, "LR");
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

                int ArmourID = (int)armour.ArmourType;

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

        inventoryUi.SetActive(!Action);

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
                Weight = Player.player.Inventory.CurrentCarryWeight;
            }
            else
            {
                Weight = Player.player.Inventory.MaxCarryWeight;
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
}
