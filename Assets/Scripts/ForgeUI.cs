using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeUI : MonoBehaviour
{
    public static ForgeUI forgeUi;

    [Range(0, 1)] public int ItemType;
    public int ItemCatogoryType = 0;
    public int mat_Id = 0;
    public int sec_Id = 0;
    public int ter_Id = 0;
    [Range(0, 23)] public int Cat_ID = 0;

    [SerializeField] private bool canCraft = false;

    public GameObject CreateWeaponUi;
    public GameObject CreateArmourUi;
    public GameObject ResourceCostUi;
    public GameObject ResourceSlot;
    public GameObject ConfirmCreateWeaponUi;

    public GameObject[] BaseItems;

    [SerializeField] private Transform itemDetailsLocation;
    public Transform resourceCostRetailsLocation;

    public BaseWeapon[] WeaponBase;

    public BaseArmour[] ArmourBase;

    public BaseRecipes RecipesCatalyst;

    public BaseRecipes[] RecipesPrimary;
    public BaseRecipes[] RecipesSecoundary;
    public BaseRecipes[] RecipesTeritiary;

    [SerializeField] private DictionaryOfStringAndInt RequiredItems;

    public WeaponHolder weapon;

    public void OnEnable()
    {
        forgeUi = this;
    }

    public void SetOpen(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);

        if (state)
        {
            SetCreateWeaponUi();
        }
    }

    public void SetCreateWeaponUi()
    {
        CreateWeaponUi.SetActive(true);
        CreateArmourUi.SetActive(false);

        ItemType = 0;
        mat_Id = 0;
        sec_Id = 0;
        ter_Id = 0;
        ItemCatogoryType = 0;
        Cat_ID = 0;

        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }

        weapon = Roller.roller.CreateWeapon(
            (WeaponType)ItemCatogoryType, 
            mat_Id, 
            sec_Id, 
            ter_Id, 
            Cat_ID, 
            Player.player.GetSkillLevel((int)SkillType.Smithing))
            .GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetCreateArmourUi()
    {
        CreateArmourUi.SetActive(true);
        CreateWeaponUi.SetActive(false);
        ItemType = 1;
    }

    public void SetCatogory(int Index)
    {
        ItemCatogoryType = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetPrimary(int Index)
    {
        mat_Id = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetSecoundary(int Index)
    {
        sec_Id = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetTeritiary(int Index)
    {
        ter_Id = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetCatalyst(int Index)
    {
        Cat_ID = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void PreviewItem()
    {
        Helper.helper.CreateItemDetails(weapon, itemDetailsLocation);
    }

    public void DisplayResourceCosts()
    {
        if (resourceCostRetailsLocation.childCount != 0)
        {
            Destroy(resourceCostRetailsLocation.GetChild(0).gameObject);
        }

        if (RequiredItems.Count != 0)
        {
            RequiredItems.Clear();
        }

        ItemAmount temp = RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            try
            {
                RequiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
            catch (System.ArgumentException)
            {
                RequiredItems[temp.Item[i]] += temp.Amount[i];
            }
        }

        temp = RecipesSecoundary[ItemCatogoryType].ItemsRequired[sec_Id];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            try
            {
                RequiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
            catch (System.ArgumentException)
            {
                RequiredItems[temp.Item[i]] += temp.Amount[i];
            }
        }

        temp = RecipesTeritiary[ItemCatogoryType].ItemsRequired[ter_Id];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            try
            {
                RequiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
            catch (System.ArgumentException)
            {
                RequiredItems[temp.Item[i]] += temp.Amount[i];
            }
        }

        if (Cat_ID != 0)
        {
            temp = RecipesCatalyst.ItemsRequired[Cat_ID];

            for (int i = 0; i < temp.Item.Length; i++)
            {
                try
                {
                    RequiredItems.Add(temp.Item[i], temp.Amount[i]);
                }
                catch (System.ArgumentException)
                {
                    RequiredItems[temp.Item[i]] += temp.Amount[i];
                }
            }
        }

        canCraft = Helper.helper.CreateResourceCostDetails(RequiredItems, resourceCostRetailsLocation);
    }

    public void CheckRequirements()
    {
        if (canCraft)
        {
            ConfirmCreateWeaponUi.SetActive(true);
        }
    }

    public void CancelCreateWeapon()
    {
        ConfirmCreateWeaponUi.SetActive(false);
    }

    public void CreateWeapon()
    {
        Item Item = Roller.roller.CreateWeapon(
             (WeaponType)ItemCatogoryType,
             mat_Id,
             sec_Id,
             ter_Id,
             Cat_ID,
             Player.player.GetSkillLevel((int)SkillType.Smithing));

        WeaponHolder WeaponRef = Item.GetComponent<WeaponHolder>();

        float ExpValue = 0;
        int Tracker = 0;

        for (int i = 0; i < WeaponRef.DamageRanges.Count; i++)
        {
            if (WeaponRef.DamageRanges[i].HDamage > 0)
            {
                ExpValue += WeaponRef.DamageRanges[i].LDamage + WeaponRef.DamageRanges[i].HDamage;
                Tracker += 2;
            }
        }

        Player.player.GainExp((long)((ExpValue * (1 / (float)Tracker)) * WeaponRef.ActionsPerSecond), (int)SkillType.Smithing);
        Player.player.Inventory.AddItem(Item, true, 1);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.Mode);

        ItemCatogoryType = 0;
        mat_Id = 0;
        sec_Id = 0;
        ter_Id = 0;
        Cat_ID = 0;

        Inventory pInventory = Player.player.Inventory;

        int start = pInventory.GetStart(GlobalValues.ResourceStart);
        int end = pInventory.GetStart(GlobalValues.MiscStart);

        foreach (KeyValuePair<string, int> item in RequiredItems)
        {
            for (int x = start; x < end; x++)
            {
                if (item.Key == pInventory[x].name)
                {
                    pInventory.RemoveItem(pInventory[x], item.Value);
                }
            }
        }

        ConfirmCreateWeaponUi.SetActive(false);
        PreviewItem();
        DisplayResourceCosts();
    }
}
