using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavable
{
    public UiState Mode;

    public int MinWeapons;
    public int MaxWeapons;
    public int MinArmour;
    public int MaxArmour;
    public int MinRunes;
    public int MaxRunes;
    public int MinSpells;
    public int MaxSpells;
    public int MinPotions;
    public int MaxPotions;
    public int MinResources;
    public int MaxResources;
    public int MinMisc;
    public int MaxMisc;

    public int[] StartIds = new int[6];

    public int MaxCarryWeight;
    public int CurrentCarryWeight;

    public Transform InventroyHolder;

    public List<Item> AllItems;

    public void OnEnable()
    {
        if (AllItems.Count != 0)
        {
            List<Item> TempList = new List<Item>(AllItems);

            AllItems.Clear();
            
            StartIds[0] = 0;
            StartIds[1] = 0;
            StartIds[2] = 0;
            StartIds[3] = 0;
            StartIds[4] = 0;
            StartIds[5] = 0;

            for (int i = 0; i < TempList.Count; i++)
            {
                AddItem(TempList[i], false, TempList[i].GetComponent<Item>().Amount);
            }
        }
    }

    public void AddItem(Item Item, bool Stackable, int Amount, bool FromRoller = false)
    {
        //Debug.Log("Add Item called");

        if (Stackable && !FromRoller)
        {
            string Name = Item.name;
            Item = Instantiate(Item, InventroyHolder);
            Item.name = Name;
            Item.Amount = Amount;
        }

        Item itemObject = Item.GetComponent<Item>();

        int start_id;
        int end_id;

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                start_id = 0;
                end_id = StartIds[0];
                break;
            case GlobalValues.ArmourTag:
                start_id = StartIds[0];
                end_id = StartIds[1];
                break;
            case GlobalValues.SpellTag:
                start_id = StartIds[1];
                end_id = StartIds[2];
                break;
            case GlobalValues.RuneTag:
                start_id= StartIds[2];
                end_id= StartIds[3];
                break;
            case GlobalValues.PotionTag:
                start_id = StartIds[3];
                end_id = StartIds[4];

                Item.GetComponent<Consumable>().PotionHolder = gameObject.GetComponent<LivingEntities>();
                Item.transform.parent = InventroyHolder;

                break;
            case GlobalValues.ResourceTag:
                start_id = StartIds[4];
                end_id = StartIds[5];
                break;
            default:
                //Gold || Misc || Key
                start_id = StartIds[5];
                end_id = AllItems.Count;
                break;
        }

        if ((AllItems.Count == 0 || end_id == AllItems.Count) && !Stackable)
        {
            AllItems.Add(Item);

            CurrentCarryWeight += itemObject.Weight * Amount;
            Item.transform.parent = InventroyHolder;

            goto UpdateStartIDs;
        }

        if (Stackable && end_id != 0)
        {
            for (int i = start_id; i < end_id; i++)
            {
                Item item = AllItems[i].GetComponent<Item>();

                if (item is IEquipable && (item as IEquipable).IsEquiped)
                {
                    continue; 
                }

                if (itemObject.Equals(item))
                {
                    item.Amount += Amount;
                    CurrentCarryWeight += itemObject.Weight * Amount;

                    Destroy(Item.gameObject);

                    return;
                }
            }
        }

        AllItems.Insert(end_id, Item);

        CurrentCarryWeight += itemObject.Weight * Amount;
        Item.transform.parent = InventroyHolder;

    UpdateStartIDs:

        switch (Item.tag)
        {
            case GlobalValues.WeaponTag:
                StartIds[0]++;
                StartIds[1]++;
                StartIds[2]++;
                StartIds[3]++;
                StartIds[4]++;
                StartIds[5]++;
                break;
            case GlobalValues.ArmourTag:
                StartIds[1]++;
                StartIds[2]++;
                StartIds[3]++;
                StartIds[4]++;
                StartIds[5]++;
                break;
            case GlobalValues.SpellTag:
                StartIds[2]++;
                StartIds[3]++;
                StartIds[4]++;
                StartIds[5]++;
                break;
            case GlobalValues.RuneTag:
                StartIds[3]++;
                StartIds[4]++;
                StartIds[5]++;
                break;
            case GlobalValues.PotionTag:
                StartIds[4]++;
                StartIds[5]++;
                break;
            case GlobalValues.ResourceTag:
                StartIds[5]++;
                break;
            default:
                //Gold || Misc || Key
                break;
        }
    }

    public void RemoveItem(Item Item, int Amount, bool CanDestroy = true)
    {
        //Debug.Log("Remove item called");

        Item.Amount -= Amount;
        CurrentCarryWeight -= Item.Weight * Amount;

        if (Item.Amount == 0)
        {
            switch (Item.tag)
            {
                case GlobalValues.WeaponTag:
                    StartIds[0]--;
                    StartIds[1]--;
                    StartIds[2]--;
                    StartIds[3]--;
                    StartIds[4]--;
                    StartIds[5]--;
                    break;
                case GlobalValues.ArmourTag:
                    StartIds[1]--;
                    StartIds[2]--;
                    StartIds[3]--;
                    StartIds[4]--;
                    StartIds[5]--;
                    break;
                case GlobalValues.SpellTag:
                    StartIds[2]--;
                    StartIds[3]--;
                    StartIds[4]--;
                    StartIds[5]--;
                    break;
                case GlobalValues.RuneTag:
                    StartIds[3]--;
                    StartIds[4]--;
                    StartIds[5]--;
                    break;
                case GlobalValues.PotionTag:
                    StartIds[4]--;
                    StartIds[5]--;
                    break;
                case GlobalValues.ResourceTag:
                    StartIds[5]--;
                    break;
                default:
                    //Gold || Misc || Key
                    break;
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
            if (AllItems[i].GetComponent<IEquipable>() == null)
            {
                continue;
            }

            Item item = AllItems[i].GetComponent<Item>();

            if (AllItems[i].GetComponent<IEquipable>().IsEquiped && item.Amount > 1)
            {
                Item ItemClone = Instantiate(AllItems[i], InventroyHolder);
                ItemClone.name = AllItems[i].name;
                ItemClone.GetComponent<IEquipable>().IsEquiped = false;
                ItemClone.GetComponent<Item>().Amount--;

                item.Amount = 1;
                AddItem(ItemClone, false, 1);
            }
            else
            {
                for (int x = 0; x < AllItems.Count; x++)
                {
                    if (AllItems[x].GetComponent<IEquipable>() == null)
                    {
                        continue;
                    }

                    Item item2 = AllItems[x].GetComponent<Item>();

                    if (item.Equals(item2) && AllItems[i].GetComponent<IEquipable>().IsEquiped == false &&
                        AllItems[x].GetComponent<IEquipable>().IsEquiped == false && AllItems[i] != AllItems[x])
                    {
                        AddItem(AllItems[x], true, item2.Amount);
                        RemoveItem(AllItems[x], item2.Amount);
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
                BuyItem(Item, Amount);
                break;
            case UiState.Player:
                SellItem(Item, Amount);
                break;
            default:
                break;
        }
    }

    //Player selling to NPC
    public void SellItem(Item Item, int Amount)
    {
        //Debug.Log("Sell Item Called");

        Inventory NPCInventory = Player.player.GetHit().GetComponent<Inventory>();

        for (int i = StartIds[3]; i <= AllItems.Count; i++)
        {
            //Debug.Log("i = " + i);
            if (i == AllItems.Count)
            {
                //Debug.Log("Player Gold Not Found");
                Item goldH = Instantiate(PrefabIDs.prefabIDs.Gold, InventroyHolder).GetComponent<Item>();

                AddItem(goldH, false, 0);

                for (int x = NPCInventory.StartIds[3]; x < NPCInventory.AllItems.Count; x++)
                {
                    if (NPCInventory.AllItems[x].CompareTag(GlobalValues.GoldTag))
                    {
                        //Debug.Log("NPC Gold Found");
                        if (NPCInventory.AllItems[x].GetComponent<Item>().Amount >= Item.GetComponent<Item>().Value * Amount)
                        {
                            AllItems[i].GetComponent<Item>().Amount += Item.GetComponent<Item>().Value * Amount;
                            NPCInventory.AllItems[x].GetComponent<Item>().Amount -= Item.GetComponent<Item>().Value * Amount;
                            TransferItem(Item, Amount);
                            return;
                        }
                    }
                }
            }

            if (AllItems[i].CompareTag(GlobalValues.GoldTag))
            {
                //Debug.Log("Player Gold Found");
                for (int x = NPCInventory.StartIds[3]; x < NPCInventory.AllItems.Count; x++)
                {
                    if (NPCInventory.AllItems[x].CompareTag(GlobalValues.GoldTag))
                    {
                        //Debug.Log("NPC Gold Found");

                        if (NPCInventory.AllItems[x].GetComponent<Item>().Amount >= Item.GetComponent<Item>().Value * Amount)
                        {
                            AllItems[i].GetComponent<Item>().Amount += Item.GetComponent<Item>().Value * Amount;
                            NPCInventory.AllItems[x].GetComponent<Item>().Amount -= Item.GetComponent<Item>().Value * Amount;
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
    public void BuyItem(Item Item, int Amount)
    {
        //Debug.Log("BuyItem Called");
        List<Item> PlayerAllItems = Player.player.Inventory.AllItems;
        int[] PlayerStartIds = Player.player.Inventory.StartIds;

        for (int i = StartIds[3]; i <= AllItems.Count; i++)
        {
            if (i == AllItems.Count)
            {
                Item goldH = Instantiate(PrefabIDs.prefabIDs.Gold, InventroyHolder).GetComponent<Item>();

                AddItem(goldH, false, 0);

                for (int x = PlayerStartIds[3]; x < PlayerAllItems.Count; x++)
                {
                    if (AllItems[x].CompareTag(GlobalValues.GoldTag))
                    {
                        if (AllItems[x].GetComponent<Item>().Amount >= Item.GetComponent<Item>().Value * Amount)
                        {
                            PlayerAllItems[i].GetComponent<Item>().Amount += Item.GetComponent<Item>().Value * Amount;
                            AllItems[x].GetComponent<Item>().Amount -= Item.GetComponent<Item>().Value * Amount;
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

                        if (PlayerAllItems[x].GetComponent<Item>().Amount >= Item.GetComponent<Item>().Value * Amount)
                        {
                            PlayerAllItems[x].GetComponent<Item>().Amount -= Item.GetComponent<Item>().Value * Amount;
                            AllItems[i].GetComponent<Item>().Amount += Item.GetComponent<Item>().Value * Amount;
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
            if (Item.GetComponent<IEquipable>() != null && Item.GetComponent<IEquipable>().IsEquiped == true)
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
        int Chance;

        Chance = Random.Range(MinWeapons, MaxWeapons + 1);

        for (int i = 0; i < Chance; i++)
        {
            AddItem(Roller.roller.weaponRoller.RollWeapon(), true, 1, true);
        }

        Chance = Random.Range(MinArmour, MaxArmour + 1);

        for (int i = 0; i < Chance; i++)
        {
            AddItem(Roller.roller.armourRoller.RollArmour(), true, 1, true);
        }

        Chance = Random.Range(MinSpells, MaxSpells + 1);

        for (int i = 0; i < Chance; i++)
        {
            AddItem(Roller.roller.spellRoller.RollSpell(), true, 1, true);
        }

        Chance = Random.Range(MinRunes, MaxRunes + 1);

        for (int i = 0; i < Chance; i++)
        {
            AddItem(Roller.roller.runeRoller.RollRune(), true, 1, true);
        }

        Chance = Random.Range(MinPotions, MaxPotions + 1);

        for (int i = 0; i < Chance; i++)
        {
            CreatedItem item = Roller.roller.potionRolller.RollPotion();
            AddItem(item.Item, true, item.Amount, true);
        }

        Chance = Random.Range(MinResources, MaxResources + 1);

        for (int i = 0; i < Chance; i++)
        {
            CreatedItem item = Roller.roller.resourceRoller.RollResource();
            AddItem(item.Item, true, item.Amount, true);
        }

        Chance = Random.Range(MinMisc, MaxMisc + 1);

        for (int i = 0; i < Chance; i++)
        {
            CreatedItem item = Roller.roller.miscRoller.RollMisc();
            AddItem(item.Item, true, item.Amount, true);
        }
    }

    public Item this[int i]
    {
        get { return AllItems[i]; }
        private set { AllItems[i] = value; }
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
            StartIds = new int[6];
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

        ContainerData Data;

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

                WeaponStats WeaponRef = Data.Weapons[i];

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

            potion.Amount = Data.Potions[i].Amount;

            AddItem(potion, false, Data.Potions[i].Amount);
        }

        for (int i = 0; i < Data.NumOfResources; i++)
        {
            ResourceHolder resource = Instantiate(PrefabIDs.prefabIDs.CraftingMaterials[Data.Resources[i].ResourceId], InventroyHolder).GetComponent<ResourceHolder>();

            resource.Amount = Data.Resources[i].Amount;

            AddItem(resource, false, Data.Resources[i].Amount);
        }

        for (int i = 0; i < Data.NumOfMisc; i++)
        {
            Item misc = Instantiate(PrefabIDs.prefabIDs.Items[Data.Misc[i].ResourceId], InventroyHolder).GetComponent<Item>();

            misc.Amount = Data.Misc[i].Amount;

            AddItem(misc, false, Data.Misc[i].Amount);
        }

        return true;
    }
}
