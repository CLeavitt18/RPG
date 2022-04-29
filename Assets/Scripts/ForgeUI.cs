using System.Text;
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

    public GameObject CreateWeaponUi;
    public GameObject CreateArmourUi;
    public GameObject ResourceCostUi;
    public GameObject ResourceSlot;
    public GameObject ConfirmCreateWeaponUi;

    public GameObject[] BaseItems;

    public Transform ResourceSlotHolder;

    public Text[] PreviewUi;

    public BaseWeapon[] WeaponBase;

    public BaseArmour[] ArmourBase;

    public BaseRecipes RecipesCatalyst;
    
    public BaseRecipes[] RecipesPrimary;
    public BaseRecipes[] RecipesSecoundary;
    public BaseRecipes[] RecipesTeritiary;

    public ItemAmount RequiredItems;
    public ItemAmount Temp;

    public Catalyst[] Cats;

    public MatMults[] Mats;

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

        weapon = (Roller.roller.weaponRoller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

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

        weapon = (Roller.roller.weaponRoller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetPrimary(int Index)
    {
        mat_Id = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.weaponRoller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetSecoundary(int Index)
    {
        sec_Id = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.weaponRoller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetTeritiary(int Index)
    {
        ter_Id = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.weaponRoller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void SetCatalyst(int Index)
    {
        Cat_ID = Index;

        Destroy(weapon.gameObject);

        weapon = (Roller.roller.weaponRoller.CreateWeapon((WeaponType)ItemCatogoryType, mat_Id, sec_Id, ter_Id, Cat_ID)).GetComponent<WeaponHolder>();

        PreviewItem();
        DisplayResourceCosts();
    }

    public void PreviewItem()
    {
        int TempDamage_1;
        int TempDamage_2;
        int TotalTempDamage = 0;

        PreviewUi[0].text = weapon.Name;

        for (int i = 0; i < weapon.DamageRanges.Count; i++)
        {
            TempDamage_1 = weapon.DamageRanges[i].LDamage;

            TempDamage_2 = weapon.DamageRanges[i].HDamage;

            TotalTempDamage += (TempDamage_1 + TempDamage_2) / 2;

            PreviewUi[2 + i].text = weapon.DamageRanges[i].Type.ToString() + ": " + TempDamage_1.ToString("n0") + " to " + TempDamage_2.ToString("n0");
        }

        StringBuilder sb = new StringBuilder("Average Damage: ");

        sb.Append(TotalTempDamage.ToString("n0"));

        PreviewUi[1].text = sb.ToString();

        sb.Clear();

        sb.Append("Attacks Per secound: ");
        sb.Append(weapon.ActionsPerSecond);

        PreviewUi[6].text = sb.ToString();

        sb.Clear();

        sb.Append("Durability: ");
        sb.Append(weapon.MaxDurability);

        PreviewUi[7].text = sb.ToString();

        sb.Clear();

        sb.Append("Weight: ");
        sb.Append(weapon.Weight);

        PreviewUi[8].text = sb.ToString();
    }

    public void DisplayResourceCosts()
    {
        if (ResourceSlotHolder.childCount > 0)
        {
            int Count = ResourceSlotHolder.childCount;

            for (int i = 0; i < Count; i++)
            {
                Destroy(ResourceSlotHolder.GetChild(i).gameObject);
            }

            ResourceSlotHolder.DetachChildren();

            RequiredItems = new ItemAmount(0);
        }

        bool AlreadyUsed = false;

        for (int i = 0; i < RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id].Item.Length; i++)
        {
            if (RequiredItems.Amount.Length != 0)
            {
                for (int x = 0; x < RequiredItems.Amount.Length; x++)
                {
                    if (RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id].Item[i] == RequiredItems.Item[x])
                    {
                        AlreadyUsed = true;
                        RequiredItems.Amount[x] += RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id].Amount[i];
                    }
                }

                if (!AlreadyUsed)
                {
                    Temp = new ItemAmount(RequiredItems.Amount.Length + 1);

                    for (int x = 0; x < RequiredItems.Amount.Length; x++)
                    {
                        Temp.Amount[x] = RequiredItems.Amount[x];
                        Temp.Item[x] = RequiredItems.Item[x];
                    }

                    Temp.Amount[RequiredItems.Amount.Length] = RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id].Amount[i];
                    Temp.Item[RequiredItems.Item.Length] = RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id].Item[i];

                    RequiredItems = Temp;

                    Instantiate(ResourceSlot, ResourceSlotHolder);
                }
                else
                {
                    AlreadyUsed = false;
                }
            }
            else
            {
                Temp = new ItemAmount(RequiredItems.Amount.Length + 1);

                for (int x = 0; x < RequiredItems.Amount.Length; x++)
                {
                    Temp.Amount[x] = RequiredItems.Amount[x];
                    Temp.Item[x] = RequiredItems.Item[x];
                }

                Temp.Amount[RequiredItems.Amount.Length] = RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id].Amount[i];
                Temp.Item[RequiredItems.Item.Length] = RecipesPrimary[ItemCatogoryType].ItemsRequired[mat_Id].Item[i];

                RequiredItems = Temp;

                Instantiate(ResourceSlot, ResourceSlotHolder);
            }
        }

        for (int i = 0; i < RecipesSecoundary[ItemCatogoryType].ItemsRequired[sec_Id].Item.Length; i++)
        {
            for (int x = 0; x < RequiredItems.Item.Length; x++)
            {
                if (RecipesSecoundary[ItemCatogoryType].ItemsRequired[sec_Id].Item[i] == RequiredItems.Item[x])
                {
                    AlreadyUsed = true;
                    RequiredItems.Amount[x] += RecipesSecoundary[ItemCatogoryType].ItemsRequired[sec_Id].Amount[i];
                }
            }

            if (!AlreadyUsed)
            {
                Temp = new ItemAmount(RequiredItems.Amount.Length + 1);

                for (int x = 0; x < RequiredItems.Amount.Length; x++)
                {
                    Temp.Amount[x] = RequiredItems.Amount[x];
                    Temp.Item[x] = RequiredItems.Item[x];
                }

                Temp.Amount[RequiredItems.Amount.Length] = RecipesSecoundary[ItemCatogoryType].ItemsRequired[sec_Id].Amount[i];
                Temp.Item[RequiredItems.Item.Length] = RecipesSecoundary[ItemCatogoryType].ItemsRequired[sec_Id].Item[i];

                RequiredItems = Temp;

                Instantiate(ResourceSlot, ResourceSlotHolder);
            }
            else
            {
                AlreadyUsed = false;
            }
        }

        for (int i = 0; i < RecipesTeritiary[ItemCatogoryType].ItemsRequired[ter_Id].Item.Length; i++)
        {
            for (int x = 0; x < RequiredItems.Item.Length; x++)
            {
                if (RecipesTeritiary[ItemCatogoryType].ItemsRequired[ter_Id].Item[i] == RequiredItems.Item[x])
                {
                    AlreadyUsed = true;
                    RequiredItems.Amount[x] += RecipesTeritiary[ItemCatogoryType].ItemsRequired[ter_Id].Amount[i];
                }
            }

            if (!AlreadyUsed)
            {
                Temp = new ItemAmount(RequiredItems.Amount.Length + 1);

                for (int x = 0; x < RequiredItems.Amount.Length; x++)
                {
                    Temp.Amount[x] = RequiredItems.Amount[x];
                    Temp.Item[x] = RequiredItems.Item[x];
                }

                Temp.Amount[RequiredItems.Amount.Length] = RecipesTeritiary[ItemCatogoryType].ItemsRequired[ter_Id].Amount[i];
                Temp.Item[RequiredItems.Item.Length] = RecipesTeritiary[ItemCatogoryType].ItemsRequired[ter_Id].Item[i];

                RequiredItems = Temp;

                Instantiate(ResourceSlot, ResourceSlotHolder);
            }
            else
            {
                AlreadyUsed = false;
            }
        }

        Temp = new ItemAmount(RequiredItems.Amount.Length + 1);

        for (int i = 0; i < RequiredItems.Item.Length; i++)
        {
            Temp.Amount[i] = RequiredItems.Amount[i];
            Temp.Item[i] = RequiredItems.Item[i];
        }

        Temp.Amount[RequiredItems.Amount.Length] = RecipesCatalyst.ItemsRequired[Cat_ID].Amount[0];
        Temp.Item[RequiredItems.Item.Length] = RecipesCatalyst.ItemsRequired[Cat_ID].Item[0];

        RequiredItems = Temp;

        Instantiate(ResourceSlot, ResourceSlotHolder);

        for (int i = 0; i < RequiredItems.Item.Length; i++)
        {
            StringBuilder sb = new StringBuilder(RequiredItems.Item[i]);
            sb.Append(" x ");
            sb.Append(RequiredItems.Amount[i].ToString("n0"));

            ResourceSlotHolder.GetChild(i).gameObject.GetComponent<Text>().text = sb.ToString();
            ResourceSlotHolder.GetChild(i).gameObject.GetComponent<Text>().color = Color.red;

            for (int x = Player.player.Inventory.StartIds[3]; x < Player.player.Inventory.StartIds[4]; x++)
            {
                if (Player.player.Inventory.AllItems[x].gameObject.name == RequiredItems.Item[i] &&
                    Player.player.Inventory.AllItems[x].GetComponent<ResourceHolder>().Amount >= RequiredItems.Amount[i])
                {
                    ResourceSlotHolder.GetChild(i).gameObject.GetComponent<Text>().color = Color.black;
                    break;
                }
            }
        }
    }

    public void CheckRequirements()
    {
        int Target = 0;
        int TargetTracker = 0;

        for (int i = 0; i < RequiredItems.Amount.Length; i++)
        {
            Target++;

            for (int x = Player.player.Inventory.StartIds[3]; x < Player.player.Inventory.StartIds[4]; x++)
            {
                if (Player.player.Inventory.AllItems[x].name == RequiredItems.Item[i] &&
                    Player.player.Inventory.AllItems [x].GetComponent<ResourceHolder>().Amount >= RequiredItems.Amount[i])
                {
                    TargetTracker++;
                }
            }
        }

        if (TargetTracker == Target)
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
       Item Item = Roller.roller.weaponRoller.CreateWeapon(
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

        Player.player.GainExp((long)((ExpValue * ( 1 / (float)Tracker)) * WeaponRef.ActionsPerSecond), (int)SkillType.Smithing);
        Player.player.Inventory.AddItem(Item, true, 1);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.Mode);

        ItemCatogoryType = 0;
        mat_Id = 0;
        sec_Id = 0;
        ter_Id = 0;
        Cat_ID = 0;

        for (int i = 0; i < RequiredItems.Item.Length; i++)
        {
            for (int x = Player.player.Inventory.StartIds[3]; x < Player.player.Inventory.StartIds[4]; x++)
            {
                if (RequiredItems.Item[i] == Player.player.Inventory.AllItems[x].name)
                {
                    Player.player.Inventory.RemoveItem(Player.player.Inventory.AllItems[x], RequiredItems.Amount[i]);
                }
            }
        }

        ConfirmCreateWeaponUi.SetActive(false);
        PreviewItem();
        DisplayResourceCosts();
    }
}
