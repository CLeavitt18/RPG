using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeUI : MonoBehaviour
{
    public static ForgeUI forgeUi;

    [SerializeField, Range(0, 1)] public int ItemType;
    [SerializeField] public int ItemCatogoryType = 0;
    [SerializeField] public int mat_Id = 0;
    [SerializeField] public int sec_Id = 0;
    [SerializeField] public int ter_Id = 0;
    [Range(0, 23)] public int Cat_ID = 0;

    [SerializeField] private bool canCraft = false;

    [SerializeField] public GameObject CreateWeaponUi;
    [SerializeField] public GameObject CreateArmourUi;
    [SerializeField] public GameObject ConfirmCreateWeaponUi;
    [SerializeField] private GameObject weaponPartButtonPrefab;
    [SerializeField] private GameObject catalystButtonPrefab;

    [SerializeField] private Transform primaryHolder;
    [SerializeField] private Transform secondaryHolder;
    [SerializeField] private Transform teritiaryHolder;
    [SerializeField] private Transform catHolder;
    [SerializeField] private Transform itemDetailsLocation;
    [SerializeField] private Transform resourceCostDetailsLocation;


    [SerializeField] public BaseRecipes RecipesCatalyst;

    [SerializeField] public BaseRecipes[] RecipesPrimary;
    [SerializeField] public BaseRecipes[] RecipesSecoundary;
    [SerializeField] public BaseRecipes[] RecipesTeritiary;

    [SerializeField] private DictionaryOfStringAndInt RequiredItems;

    [SerializeField] public WeaponHolder weapon;

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
        else
        {
            Destroy(weapon.gameObject);
            Destroy(itemDetailsLocation.GetChild(0).gameObject);
            Destroy(resourceCostDetailsLocation.GetChild(0).gameObject);
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

        Button button;

        for (MaterialType i = MaterialType.Bone; i <= MaterialType.Ebony; i++)
        {
            int id = (int)i;
            string text = i.ToString();

            button = Helper.helper.CreateCraftingButton(weaponPartButtonPrefab, text, primaryHolder);
            button.onClick.AddListener(delegate { SetPrimary(id);});

            button = Helper.helper.CreateCraftingButton(weaponPartButtonPrefab, text, secondaryHolder);
            button.onClick.AddListener(delegate { SetSecoundary(id);});

            button = Helper.helper.CreateCraftingButton(weaponPartButtonPrefab, text, teritiaryHolder);
            button.onClick.AddListener(delegate { SetTeritiary(id);});
        }

        for (CatType i = CatType.T1Phys; i <= CatType.T6Ice; i++)
        {
            int id = (int)i;

            button = Helper.helper.CreateCraftingButton(catalystButtonPrefab, i.ToString(), catHolder);
            button.onClick.AddListener(delegate { SetCatalyst(id);});
        }

        PreviewItem();
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

        PreviewItem();
    }

    public void SetPrimary(int Index)
    {
        mat_Id = Index;

        PreviewItem();
    }

    public void SetSecoundary(int Index)
    {
        sec_Id = Index;

        PreviewItem();
    }

    public void SetTeritiary(int Index)
    {
        ter_Id = Index;

        PreviewItem();
    }

    public void SetCatalyst(int Index)
    {
        Cat_ID = Index;

        PreviewItem();
    }

    public void PreviewItem()
    {
        if (weapon != null)
        {
            Destroy(itemDetailsLocation.GetChild(0).gameObject);
            Destroy(weapon.gameObject);
        }

        weapon = (Roller.roller.CreateWeapon(
            (WeaponType)ItemCatogoryType,
            mat_Id,
            sec_Id,
            ter_Id,
            Cat_ID, Player.player.GetSkillLevel(SkillType.Smithing))).GetComponent<WeaponHolder>();

        Helper.helper.CreateItemDetails(weapon, itemDetailsLocation);

        DisplayResourceCosts();
    }

    public void DisplayResourceCosts()
    {
        if (resourceCostDetailsLocation.childCount != 0)
        {
            Destroy(resourceCostDetailsLocation.GetChild(0).gameObject);
        }

        if (RequiredItems.Count != 0)
        {
            RequiredItems.Clear();
        }

        ItemAmount temp = RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            if (RequiredItems.ContainsKey(temp.Item[i]))
            {
                RequiredItems[temp.Item[i]] += temp.Amount[i];
            }
            else
            {
                RequiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
        }

        temp = RecipesSecoundary[ItemCatogoryType].ItemsRequired[sec_Id];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            if (RequiredItems.ContainsKey(temp.Item[i]))
            {
                RequiredItems[temp.Item[i]] += temp.Amount[i];
            }
            else
            {
                RequiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
        }

        temp = RecipesTeritiary[ItemCatogoryType].ItemsRequired[ter_Id];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            if (RequiredItems.ContainsKey(temp.Item[i]))
            {
                RequiredItems[temp.Item[i]] += temp.Amount[i];
            }
            else
            {
                RequiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
        }

        if (Cat_ID != 0)
        {
            temp = RecipesCatalyst.ItemsRequired[Cat_ID];

            for (int i = 0; i < temp.Item.Length; i++)
            {
                if (RequiredItems.ContainsKey(temp.Item[i]))
                {
                    RequiredItems[temp.Item[i]] += temp.Amount[i];
                }
                else
                {
                    RequiredItems.Add(temp.Item[i], temp.Amount[i]);
                }
            }
        }

        canCraft = Helper.helper.CreateResourceCostDetails(RequiredItems, resourceCostDetailsLocation);
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
        float ExpValue = 0;
        int Tracker = 0;

        for (int i = 0; i < weapon.GetDamageRangesCount(); i++)
        {
            if (weapon.GetUpperRange(i) > 0)
            {
                ExpValue += weapon.GetLowerRange(i) + weapon.GetUpperRange(i);
                Tracker += 2;
            }
        }

        Player.player.GainExp((long)((ExpValue * (1 / (float)Tracker)) * weapon.GetAttackSpeed()), (int)SkillType.Smithing);
        Player.player.Inventory.AddItem(weapon, true, 1);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.GetMode());

        ItemCatogoryType = 0;
        mat_Id = 0;
        sec_Id = 0;
        ter_Id = 0;
        Cat_ID = 0;

        Inventory pInventory = Player.player.Inventory;

        foreach (KeyValuePair<string, int> item in RequiredItems)
        {
            pInventory.RemoveItem(item.Key, item.Value, GlobalValues.ResourceTag);
        }

        ConfirmCreateWeaponUi.SetActive(false);
        PreviewItem();
    }
}
