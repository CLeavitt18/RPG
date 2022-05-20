using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavable
{
    public UiState Mode;

    [SerializeField] private int max;

    [SerializeField] private Range[] ranges;

    public int Count { get{ return AllItems.Count;} private set{} }

    [SerializeField] private int[] StartIds = new int[6];

    public int MaxCarryWeight;
    public int CurrentCarryWeight;

    public Transform InventroyHolder;

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
            string Name = Item.name;
            Color rarity = Item.GetRarity();
            Item = Instantiate(Item, InventroyHolder);
            Item.name = Name;
            Item.SetAmount(Amount);
            Item.SetRarity(rarity);
        }

        Item itemObject = Item.GetComponent<Item>();

        int start_id;
        int end_id;

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                start_id = 0;
                end_id = StartIds[GlobalValues.ArmourStart];
                break;
            case GlobalValues.ArmourTag:
                start_id = StartIds[GlobalValues.ArmourStart];
                end_id = StartIds[GlobalValues.SpellStart];
                break;
            case GlobalValues.SpellTag:
                start_id = StartIds[GlobalValues.SpellStart];
                end_id = StartIds[GlobalValues.RuneStart];
                break;
            case GlobalValues.RuneTag:
                start_id= StartIds[GlobalValues.RuneStart];
                end_id= StartIds[GlobalValues.PotionStart];
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

            CurrentCarryWeight += itemObject.GetWeight() * Amount;
            Item.transform.parent = InventroyHolder;

            goto UpdateStartIDs;
        }

        if (Stackable && end_id != 0)
        {
            for (int i = start_id; i < end_id; i++)
            {
                Item item = AllItems[i].GetComponent<Item>();

                if (item.GetEquipable() && item.GetEquiped())
                {
                    continue; 
                }

                if (itemObject.Equals(item))
                {
                    item += item;
                    CurrentCarryWeight += itemObject.GetWeight() * Amount;

                    Destroy(Item.gameObject);

                    return;
                }
            }
        }

        AllItems.Insert(end_id, Item);

        CurrentCarryWeight += itemObject.GetWeight() * Amount;
        Item.transform.parent = InventroyHolder;

    UpdateStartIDs:

        int loopStart;

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                loopStart = GlobalValues.ArmourStart;
                break;
            case GlobalValues.ArmourTag:
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

            for(int i = loopStart; i < GlobalValues.MiscStart + 1; i++)
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
            InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.Mode);
            InventoryUi.playerUi.SetPlayerEquipedIndicators();

            RemoveItem(Item, Amount);
            InventoryUi.containerUi.CallSetInventory(InventoryUi.containerUi.Mode);
            InventoryUi.containerUi.Focus = null;
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

            InventoryUi.containerUi.CallSetInventory(InventoryUi.containerUi.Mode);

            RemoveItem(Item, Amount);

            InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.Mode);
            InventoryUi.playerUi.SetPlayerEquipedIndicators();
            InventoryUi.playerUi.Focus = null;
        }
    }

    public void SetDefaultState(bool priority)
    {
        int num;
        int chance;

        Item item = null;

        for (InventoryState state = InventoryState.Weapons; state < InventoryState.AllItems; state++)
        {
            chance = Random.Range(0, max);

            if (chance == 0)
            {
                int index = (int)state;

                num = Random.Range(ranges[index].min, ranges[index].max + 1);

                for (int x = 0; x < num; x++)
                {
                    item = Roller.roller.Roll(state);
                    AddItem(item, true, item.GetAmount(), true);
                }
            }
        }

        ranges = new Range[0];
    }

    public void Clear()
    {
        AllItems.Clear();
        
        for(int i = 0; i < GlobalValues.MiscStart + 1; i++)
        {
            StartIds[i] = 0;
        }
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

    public bool Save(int id)
    {
        return SaveSystem.SaveContainer(this, id);
    }

    public bool Load(int id)
    {
        return LoadContainer(id);
    }

    public bool LoadContainer(int id)
    {
        if (AllItems.Count != 0)
        {
            int Count = AllItems.Count;

            for (int i = 0; i < Count; i++)
            {
                Destroy(AllItems[i]);
            }

            AllItems = new List<Item>();
            StartIds = new int[GlobalValues.MiscStart + 1];
        }

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);
        path.Append(GlobalValues.LevelFolder);
        path.Append(SceneManagerOwn.Manager.SceneName);
        path.Append(GlobalValues.ContainerFolder);
        path.Append('/');
        path.Append(GetComponent<Containers>().Name);
        path.Append(id);

        StringBuilder tempPath = new StringBuilder(path.ToString());
        tempPath.Append(GlobalValues.TempExtension);

        path.Append(GlobalValues.SaveExtension);

        InventoryData Data;

        if (File.Exists(tempPath.ToString()))
        {
            Data = SaveSystem.LoadContainer(tempPath.ToString());
        }
        else
        {
            Data = SaveSystem.LoadContainer(path.ToString());
        }

        if (Data == null)
        {
            Debug.Log("containers data equals null");
        }

        if (Data.NumOfWeapons > 0)
        {
            for (int i = 0; i < Data.NumOfWeapons; i++)
            {
                WeaponHolder weapon = Instantiate(PrefabIDs.prefabIDs.WeaponHolder, InventroyHolder).GetComponent<WeaponHolder>();

                WeaponStatsData WeaponRef = Data.Weapons[i];

                LoadSystem.LoadItem(WeaponRef, weapon);

                AddItem(weapon, false, Data.Weapons[i].Amount);
            }
        }

        if (Data.NumOfArmour > 0)
        {
            for (int i = 0; i < Data.NumOfArmour; i++)
            {
                ArmourHolder armour = Instantiate(PrefabIDs.prefabIDs.ArmourHolder, InventroyHolder).GetComponent<ArmourHolder>();

                LoadSystem.LoadItem(Data.Armour[i], armour.GetComponent<ArmourHolder>());

                AddItem(armour, false, Data.Armour[i].Amount);
            }
        }

        for (int i = 0; i < Data.NumOfSpells; i++)
        {
            SpellHolder spell = Instantiate(PrefabIDs.prefabIDs.SpellHolder, InventroyHolder).GetComponent<SpellHolder>();

            LoadSystem.LoadItem(Data.Spells[i], spell);

            AddItem(spell, false, Data.Spells[i].Amount);
        }

        for (int i = 0; i < Data.NumOfRunes; i++)
        {
            RuneHolder rune = Instantiate(PrefabIDs.prefabIDs.RuneHolder, InventroyHolder).GetComponent<RuneHolder>();

            LoadSystem.LoadItem(Data.Runes[i], rune);

            AddItem(rune, false, Data.Runes[i].Amount);
        }

        for (int i = 0; i < Data.NumOfPotions; i++)
        {
            Consumable potion = Instantiate(PrefabIDs.prefabIDs.Potions[Data.Potions[i].ResourceId], InventroyHolder).GetComponent<Consumable>();

            potion.SetAmount(Data.Potions[i].Amount);

            AddItem(potion, false, Data.Potions[i].Amount);
        }

        for (int i = 0; i < Data.NumOfResources; i++)
        {
            ResourceHolder resource = Instantiate(PrefabIDs.prefabIDs.CraftingMaterials[Data.Resources[i].ResourceId], InventroyHolder).GetComponent<ResourceHolder>();

            resource.SetAmount(Data.Resources[i].Amount);

            AddItem(resource, false, Data.Resources[i].Amount);
        }

        for (int i = 0; i < Data.NumOfMisc; i++)
        {
            Item misc = Instantiate(PrefabIDs.prefabIDs.Items[Data.Misc[i].ResourceId], InventroyHolder).GetComponent<Item>();

            misc.SetAmount(Data.Misc[i].Amount);

            AddItem(misc, false, Data.Misc[i].Amount);
        }

        ranges = new Range[0];

        return true;
    }
}
