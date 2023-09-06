using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private UiState Mode;

    [SerializeField] private Range[] ranges;

    public int Count { get { return AllItems.Count; } private set { } }

    [SerializeField] private int[] StartIds = new int[6];

    [SerializeField] private int MaxCarryWeight;
    [SerializeField] private int CurrentCarryWeight;

    [SerializeField] private Transform InventroyHolder;

    [SerializeField] private List<Item> AllItems;

    public void OnEnable()
    {
        if (AllItems.Count != 0)
        {
            List<Item> TempList = new List<Item>(AllItems);

            Clear();

            for (int i = 0; i < TempList.Count; i++)
            {
                AddItem(TempList[i], false, TempList[i].GetAmount());
            }
        }
    }

    public void AddItem(Item Item, bool Stackable, int Amount, bool FromRoller = false)
    {
        //Debug.Log("Add Item called");

        if (Stackable && !FromRoller)
        {
            Item = Item.Clone(Amount);
        }

        int start_id;
        int end_id;

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                start_id = 0;
                end_id = StartIds[GlobalValues.ArmourStart];
                break;
            case GlobalValues.ArmourTag:
            case GlobalValues.ShieldTag:
                start_id = StartIds[GlobalValues.ArmourStart];
                end_id = StartIds[GlobalValues.SpellStart];
                break;
            case GlobalValues.SpellTag:
                start_id = StartIds[GlobalValues.SpellStart];
                end_id = StartIds[GlobalValues.RuneStart];
                break;
            case GlobalValues.RuneTag:
                start_id = StartIds[GlobalValues.RuneStart];
                end_id = StartIds[GlobalValues.PotionStart];
                break;
            case GlobalValues.PotionTag:
                start_id = StartIds[GlobalValues.PotionStart];
                end_id = StartIds[GlobalValues.ResourceStart];

                Item.GetComponent<Consumable>().PotionHolder = gameObject.GetComponent<LivingEntities>();
                Item.transform.parent = InventroyHolder;

                break;
            case GlobalValues.ResourceTag:
                start_id = StartIds[GlobalValues.ResourceStart];
                end_id = StartIds[GlobalValues.MiscStart];
                break;
            default:
                //Gold || Misc || Key || Torch
                start_id = StartIds[GlobalValues.MiscStart];
                end_id = AllItems.Count;
                break;
        }

        if ((AllItems.Count == 0 || end_id == AllItems.Count) && !Stackable)
        {
            AllItems.Add(Item);

            CurrentCarryWeight += Item.GetWeight() * Amount;
            Item.transform.parent = InventroyHolder;

            goto UpdateStartIDs;
        }

        if (Stackable && end_id != 0)
        {
            Item item;

            for (int i = start_id; i < end_id; i++)
            {
                item = AllItems[i];

                if (item.GetEquipable() && item.GetEquiped())
                {
                    continue;
                }

                if (Item.Equals(item))
                {
                    item += Item;
                    CurrentCarryWeight += Item.GetWeight() * Amount;

                    Destroy(Item.gameObject);

                    return;
                }
            }
        }

        AllItems.Insert(end_id, Item);

        CurrentCarryWeight += Item.GetWeight() * Amount;
        Item.transform.parent = InventroyHolder;

    UpdateStartIDs:

        int loopStart;

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                loopStart = GlobalValues.ArmourStart;
                break;
            case GlobalValues.ArmourTag:
            case GlobalValues.ShieldTag:
                loopStart = GlobalValues.SpellStart;
                break;
            case GlobalValues.SpellTag:
                loopStart = GlobalValues.RuneStart;
                break;
            case GlobalValues.RuneTag:
                loopStart = GlobalValues.PotionStart;
                break;
            case GlobalValues.PotionTag:
                loopStart = GlobalValues.ResourceStart;
                break;
            case GlobalValues.ResourceTag:
                loopStart = GlobalValues.MiscStart;
                break;
            default:
                //Gold || Misc || Key
                loopStart = GlobalValues.MiscStart + 1;
                break;
        }

        for (int i = loopStart; i < GlobalValues.MiscStart + 1; i++)
        {
            StartIds[i]++;
        }

        if (Mode == UiState.Player || Mode == UiState.Entity)
        {
            GetComponent<LivingEntities>().CalculateSpeed();
        }

        if (Item is QuestItemHolder holder)
        {
            QuestTracker.questTracker.UpdateQuest(Item.gameObject);
        }
    }

    public void RemoveItem(string itemName, int amount, string tag)
    {
        RemoveItem(Find(itemName, tag), amount);
    }

    public void RemoveItem(Item Item, int Amount, bool CanDestroy = true)
    {
        //Debug.Log("Remove item called");

        Item -= Amount;
        CurrentCarryWeight -= Item.GetWeight() * Amount;

        if (Item.GetAmount() == 0)
        {
            int loopStart;

            switch (Item.tag)
            {
                case GlobalValues.WeaponTag:
                    loopStart = GlobalValues.ArmourStart;
                    break;
                case GlobalValues.ArmourTag:
                case GlobalValues.ShieldTag:
                    loopStart = GlobalValues.SpellStart;
                    break;
                case GlobalValues.SpellTag:
                    loopStart = GlobalValues.RuneStart;
                    break;
                case GlobalValues.RuneTag:
                    loopStart = GlobalValues.PotionStart;
                    break;
                case GlobalValues.PotionTag:
                    loopStart = GlobalValues.ResourceStart;
                    break;
                case GlobalValues.ResourceTag:
                    loopStart = GlobalValues.MiscStart;
                    break;
                default:
                    //Gold || Misc || Key
                    loopStart = GlobalValues.MiscStart + 1;
                    break;
            }

            for (int i = loopStart; i < GlobalValues.MiscStart + 1; i++)
            {
                StartIds[i]--;
            }

            //Debug.Log("Item Removed");
            AllItems.Remove(Item);

            if (CanDestroy)
            {
                //Debug.Log("Destroying Item");
                Destroy(Item.gameObject);
            }
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < AllItems.Count; i++)
        {
            if (AllItems[i].GetEquipable() == false)
            {
                continue;
            }

            Item item = AllItems[i].GetComponent<Item>();

            if (AllItems[i].GetEquiped() && item.GetAmount() > 1)
            {
                Item ItemClone = Instantiate(AllItems[i], InventroyHolder);
                ItemClone.name = AllItems[i].name;
                ItemClone.SetEquiped(false);
                ItemClone--;

                item.SetAmount(1);
                AddItem(ItemClone, false, 1);
            }
            else
            {
                for (int x = 0; x < AllItems.Count; x++)
                {
                    if (AllItems[x].GetEquipable() == false)
                    {
                        continue;
                    }

                    Item item2 = AllItems[x].GetComponent<Item>();

                    if (item.Equals(item2) && AllItems[i].GetEquiped() == false &&
                        AllItems[x].GetEquiped() == false && AllItems[i] != AllItems[x])
                    {
                        AddItem(AllItems[x], true, item2.GetAmount());
                        RemoveItem(AllItems[x], item2.GetAmount());
                        break;
                    }
                }
            }
        }
    }

    public void Trade(Item Item, int Amount)
    {
        switch (Mode)
        {
            case UiState.Container:
                TransferItem(Item, Amount);
                break;
            case UiState.Store:
                BuyItem(Item, Amount, Item.GetValue());
                break;
            case UiState.Player:
                SellItem(Item, Amount, Item.GetValue());
                break;
            default:
                break;
        }
    }

    //Player selling to NPC
    public void SellItem(Item Item, int Amount, int value)
    {
        //Debug.Log("Sell Item Called");

        Inventory NPCInventory = Player.player.GetHit().GetComponent<Inventory>();

        for (int i = StartIds[GlobalValues.MiscStart]; i <= AllItems.Count; i++)
        {
            //Debug.Log("i = " + i);
            if (i == AllItems.Count)
            {
                //Debug.Log("Player Gold Not Found");
                Item goldH = Instantiate(PrefabIDs.prefabIDs.Gold, InventroyHolder).GetComponent<Item>();

                AddItem(goldH, false, 0);

                for (int x = NPCInventory.StartIds[GlobalValues.MiscStart]; x < NPCInventory.AllItems.Count; x++)
                {
                    if (NPCInventory.AllItems[x].CompareTag(GlobalValues.GoldTag))
                    {
                        //Debug.Log("NPC Gold Found");
                        if (NPCInventory.AllItems[x].GetAmount() >= value * Amount)
                        {
                            AllItems[i] += value * Amount;
                            NPCInventory.AllItems[x] -= value * Amount;
                            TransferItem(Item, Amount);
                            return;
                        }
                    }
                }
            }

            if (AllItems[i].CompareTag(GlobalValues.GoldTag))
            {
                //Debug.Log("Player Gold Found");
                for (int x = NPCInventory.StartIds[GlobalValues.MiscStart]; x < NPCInventory.AllItems.Count; x++)
                {
                    if (NPCInventory.AllItems[x].CompareTag(GlobalValues.GoldTag))
                    {
                        //Debug.Log("NPC Gold Found");

                        if (NPCInventory.AllItems[x].GetAmount() >= value * Amount)
                        {
                            AllItems[i] += value * Amount;
                            NPCInventory.AllItems[x] -= value * Amount;
                            TransferItem(Item, Amount);
                            return;
                        }
                    }
                }

                break;
            }

            //Debug.Log("Nothing happened");
        }

    }

    //Player buying from NPC
    public void BuyItem(Item Item, int Amount, int value)
    {
        //Debug.Log("BuyItem Called");
        List<Item> PlayerAllItems = Player.player.Inventory.AllItems;
        int[] PlayerStartIds = Player.player.Inventory.StartIds;

        for (int i = StartIds[GlobalValues.MiscStart]; i <= AllItems.Count; i++)
        {
            if (i == AllItems.Count)
            {
                Item goldH = Instantiate(PrefabIDs.prefabIDs.Gold, InventroyHolder).GetComponent<Item>();

                AddItem(goldH, false, 0);

                for (int x = PlayerStartIds[3]; x < PlayerAllItems.Count; x++)
                {
                    if (AllItems[x].CompareTag(GlobalValues.GoldTag))
                    {
                        if (AllItems[x].GetAmount() >= value * Amount)
                        {
                            PlayerAllItems[i] += value * Amount;
                            AllItems[x] -= value * Amount;
                            TransferItem(Item, Amount);
                            return;
                        }
                    }
                }
            }

            //Debug.Log("i = " + i);
            //Debug.Log(AllItems[i].name);
            if (AllItems[i].CompareTag(GlobalValues.GoldTag))
            {
                //Debug.Log($"NPC Gold found: {AllItems[i].GetComponent<IItem>().Amount}");

                for (int x = PlayerStartIds[3]; x < PlayerAllItems.Count; x++)
                {
                    if (PlayerAllItems[x].CompareTag(GlobalValues.GoldTag))
                    {
                        //Debug.Log($"Player Gold found: {PlayerAllItems[x].GetComponent<IItem>().Amount}");

                        if (PlayerAllItems[x].GetAmount() >= value * Amount)
                        {
                            PlayerAllItems[x] -= value * Amount;
                            AllItems[i] += value * Amount;
                            TransferItem(Item, Amount);
                            return;
                        }
                    }
                }

                break;
            }
        }
    }

    public void TransferItem(Item Item, int Amount)
    {
        //Debug.Log("Transfoer item Called");

        if (Mode != UiState.Player)
        {
            //Debug.Log("I'am a NPC/Chest");

            Player.player.Inventory.AddItem(Item, true, Amount);
            InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.GetMode());
            InventoryUi.playerUi.SetPlayerEquipedIndicators();

            RemoveItem(Item, Amount);
            InventoryUi.containerUi.CallSetInventory(InventoryUi.containerUi.GetMode());
        }
        else
        {
            if (Item.GetEquipable() && Item.GetEquiped())
            {
                return;
            }

            //Debug.Log("I'am a player");

            Inventory NPCInventory = Player.player.GetHit().GetComponent<Inventory>();

            NPCInventory.AddItem(Item, true, Amount);

            InventoryUi.containerUi.CallSetInventory(InventoryUi.containerUi.GetMode());

            RemoveItem(Item, Amount);

            InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.GetMode());
            InventoryUi.playerUi.SetPlayerEquipedIndicators();
        }
    }

    public void SetDefaultState(bool priority)
    {
        int num;

        Item item = null;

        for (InventoryState state = InventoryState.Weapons; state < InventoryState.AllItems; state++)
        {
            int index = (int)state;

            num = Random.Range(ranges[index].min, ranges[index].max + 1);

            for (int x = 0; x < num; x++)
            {
                item = Roller.roller.Roll(state);
                AddItem(item, true, item.GetAmount(), true);
            }
        }

        ranges = new Range[0];
    }

    public Item Find(string name, string tag)
    {
        int start = 0;
        int end = 0;

        switch (tag)
        {
            case GlobalValues.WeaponTag:
                start = 0;
                end = GetStart(GlobalValues.ArmourStart);
                break;
            case GlobalValues.ArmourTag:
            case GlobalValues.ShieldTag:
                start = GetStart(GlobalValues.Armourpieces);
                end = GetStart(GlobalValues.SpellStart);
                break;
            case GlobalValues.SpellTag:
                start = GetStart(GlobalValues.SpellStart);
                end = GetStart(GlobalValues.RuneStart);
                break;
            case GlobalValues.RuneTag:
                start = GetStart(GlobalValues.RuneStart);
                end = GetStart(GlobalValues.PotionStart);
                break;
            case GlobalValues.PotionTag:
                start = GetStart(GlobalValues.PotionStart);
                end = GetStart(GlobalValues.ResourceStart);
                break;
            case GlobalValues.ResourceTag:
                start = GetStart(GlobalValues.ResourceStart);
                end = GetStart(GlobalValues.MiscStart);
                break;
            default:// Gold | Misc | Key
                start = GetStart(GlobalValues.MiscStart);
                end = Count;
                break;
        }

        for (int i = start; i < end; i++)
        {
            if (AllItems[i].GetName() == name)
            {
                return AllItems[i];
            }
        }

        return null;
    }

    public int FindIndex(Item item)
    {
        if (item == null)
        {
            return -1;
        }

        int start = 0;
        int end = 0;

        string tag = item.tag;

        switch (tag)
        {
            case GlobalValues.WeaponTag:
                start = 0;
                end = GetStart(GlobalValues.ArmourStart);
                break;
            case GlobalValues.ArmourTag:
            case GlobalValues.ShieldTag:
                start = GetStart(GlobalValues.Armourpieces);
                end = GetStart(GlobalValues.SpellStart);
                break;
            case GlobalValues.SpellTag:
                start = GetStart(GlobalValues.SpellStart);
                end = GetStart(GlobalValues.RuneStart);
                break;
            case GlobalValues.RuneTag:
                start = GetStart(GlobalValues.RuneStart);
                end = GetStart(GlobalValues.PotionStart);
                break;
            case GlobalValues.PotionTag:
                start = GetStart(GlobalValues.PotionStart);
                end = GetStart(GlobalValues.ResourceStart);
                break;
            case GlobalValues.ResourceTag:
                start = GetStart(GlobalValues.ResourceStart);
                end = GetStart(GlobalValues.MiscStart);
                break;
            default:// Gold | Misc | Key
                start = GetStart(GlobalValues.MiscStart);
                end = Count;
                break;
        }

        for (int i = start; i < end; i++)
        {
            if (AllItems[i] == item)
            {
                return i;
            }
        }

        return -1;
    }

    public void Clear()
    {
        AllItems.Clear();

        for (int i = 0; i < GlobalValues.MiscStart + 1; i++)
        {
            StartIds[i] = 0;
        }
    }

    private void Sort()
    {

    }

    public void CalculateWeight(int strenght)
    {
        int tempWeight = 14500;

        int strenghtMulti = ((int)Mathf.Floor((float)strenght / 10f)) * 500;

        tempWeight += strenghtMulti;

        MaxCarryWeight = tempWeight;
    }

    public Item this[int i]
    {
        get { return AllItems[i]; }
        private set { AllItems[i] = value; }
    }

    public int GetStart(int id)
    {
        return StartIds[id];
    }

    public int GetCarryWeight()
    {
        return CurrentCarryWeight;
    }

    public int GetMaxCarryWeight()
    {
        return MaxCarryWeight;
    }

    public UiState GetMode()
    {
        return Mode;
    }

    public Transform GetHolder()
    {
        return InventroyHolder;
    }

    public void SetToContainer()
    {
        Mode = UiState.Container;
    }

    public void SetHolder(EntityType type)
    {
        switch (type)
        {
            case EntityType.Player:
                InventroyHolder = GameObject.Find("PlayerInventoryHolder").transform;
                break;
            case EntityType.Enemy:

                break;
            case EntityType.NPC:
                InventroyHolder = GameObject.Find("NPCInventoryHolder").transform;
                break;
        }
    }

    public bool Load(InventoryData data)
    {
        return LoadInventory(data);
    }

    private bool LoadInventory(InventoryData data)
    {
        if (AllItems.Count != 0)
        {
            Clear();
        }

        if (data == null)
        {
            Debug.Log("container's data equals null");
        }

        for (int i = 0; i < data.NumOfWeapons; i++)
        {
            WeaponHolder weapon = Instantiate(PrefabIDs.prefabIDs.WeaponHolder, InventroyHolder).GetComponent<WeaponHolder>();

            WeaponData WeaponRef = data.Weapons[i];

            LoadSystem.LoadItem(WeaponRef, weapon);

            AddItem(weapon, false, data.Weapons[i].Amount);
        }

        for (int i = 0; i < data.NumOfArmour; i++)
        {
            ArmourHolder armour;

            if (data.Armour[i].IsShield)
            {
                armour = Instantiate(PrefabIDs.prefabIDs.ShieldHolder, InventroyHolder).GetComponent<ArmourHolder>();
            }
            else
            {
                armour = Instantiate(PrefabIDs.prefabIDs.ArmourHolder, InventroyHolder).GetComponent<ArmourHolder>();
            }

            LoadSystem.LoadItem(data.Armour[i], armour.GetComponent<ArmourHolder>());

            AddItem(armour, false, data.Armour[i].Amount);
        }

        for (int i = 0; i < data.NumOfSpells; i++)
        {
            SpellHolder spell = Instantiate(PrefabIDs.prefabIDs.SpellHolder, InventroyHolder).GetComponent<SpellHolder>();

            LoadSystem.LoadItem(data.Spells[i], spell);

            AddItem(spell, false, data.Spells[i].Amount);
        }

        for (int i = 0; i < data.NumOfRunes; i++)
        {
            RuneHolder rune = Instantiate(PrefabIDs.prefabIDs.RuneHolder, InventroyHolder).GetComponent<RuneHolder>();

            LoadSystem.LoadItem(data.Runes[i], rune);

            AddItem(rune, false, data.Runes[i].Amount);
        }

        for (int i = 0; i < data.NumOfPotions; i++)
        {
            Consumable potion = Instantiate(PrefabIDs.prefabIDs.Potions[data.Potions[i].ResourceId], InventroyHolder).GetComponent<Consumable>();

            potion.SetAmount(data.Potions[i].Amount);

            AddItem(potion, false, data.Potions[i].Amount);
        }

        for (int i = 0; i < data.NumOfResources; i++)
        {
            ResourceHolder resource = Instantiate(PrefabIDs.prefabIDs.CraftingMaterials[data.Resources[i].ResourceId], InventroyHolder).GetComponent<ResourceHolder>();

            resource.SetAmount(data.Resources[i].Amount);

            AddItem(resource, false, data.Resources[i].Amount);
        }

        for (int i = 0; i < data.NumOfMisc; i++)
        {
            Item misc = Instantiate(PrefabIDs.prefabIDs.Items[data.Misc[i].ResourceId], InventroyHolder).GetComponent<Item>();

            misc.SetAmount(data.Misc[i].Amount);

            AddItem(misc, false, data.Misc[i].Amount);
        }

        ranges = new Range[0];

        return true;
    }
}
