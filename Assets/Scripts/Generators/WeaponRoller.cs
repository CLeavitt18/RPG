using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRoller : MonoBehaviour
{
    [SerializeField] private BaseWeapon[] WeaponBase;

    [SerializeField] private Catalyst[] Cats;

    [SerializeField] private MatMults[] Mats;
    
    [SerializeField] private DropTable[] MatDropT;
    [SerializeField] private DropTable[] CatTable;

    [SerializeField] private Material[] WeaponMaterials;
    [SerializeField] private Material[] FireMaterials;
    [SerializeField] private Material[] LightningMaterials;
    [SerializeField] private Material[] IceMaterials;

    public Item RollWeapon()
    {
        int chance;
        int Cat_Id = 0;
        int Pri_Id = 0;
        int Sec_Id = 0;
        int Ter_Id = 0;

        WeaponType Type;

        //Determines Weapon Type Sword vs Axe
        Type = (WeaponType)Random.Range(0, 4);

        //Determines Weapon Material Iron vs Steel
        chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);


        for (int i = 0; i < MatDropT[0].Chances.Length; i++)
        {
            if (chance <= MatDropT[0].Chances[i])
            {
                Pri_Id = i;
                break;
            }
        }

        //Debug.Log("Weapon Material: " + Mat_Id);
        //Determines What Catalyst is used
        // i = 0 = tier 1
        // i = 1 = tier 2
        // i = 2 = tier 3
        // i = 3 = tier 4
        // i = 4 = tier 5
        // i = 5 = tier 6
        chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        if (chance <= 748)
        {
            //Physical
            chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = i;
                    break;
                }
            }
        }
        else if (chance >= 749 && chance <= 832)
        {
            //Fire
            chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = 6 + i;
                    break;
                }
            }
        }
        else if (chance >= 833 && chance <= 915)
        {
            //Lightning
            chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = 12 + i;
                    break;
                }
            }
        }
        else if (chance >= 916 && chance <= 999)
        {
            //Ice
            chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = 18 + i;
                    break;
                }
            }
        }

        //Determaines what the secondary will be. This affects the durability of the weapon
        chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        for (int i = 0; i < MatDropT[0].Chances.Length; i++)
        {
            if (chance <= MatDropT[0].Chances[i])
            {
                Sec_Id = i;
                break;
            }
        }

        //Determaines what the Tertitary will be.
        chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        for (int i = 0; i < MatDropT[0].Chances.Length; i++)
        {
            if (chance <= MatDropT[0].Chances[i])
            {
                Ter_Id = i;
                break;
            }
        }

        Item Item = CreateWeapon(Type, Pri_Id, Sec_Id, Ter_Id, Cat_Id);

        return Item;
    }

    public Item CreateWeapon(WeaponType type, int pri_Id, int sec_Id, int ter_Id, int cat_Id, int level = 0)
    {
        GameObject weapon = Instantiate(PrefabIDs.prefabIDs.WeaponHolder);
        WeaponHolder WeaponRef = weapon.GetComponent<WeaponHolder>();

        WeaponStats stats = new WeaponStats();

        stats.Materials[0] = (MaterialType)pri_Id;
        stats.Materials[1] = (MaterialType)sec_Id;
        stats.Materials[2] = (MaterialType)ter_Id;

        float multi;

        BaseWeapon baseWeapon = WeaponBase[(int)type];

        MatMults matMulti = Mats[pri_Id];

        GameObject primary = baseWeapon.Primary;
        WeaponParts secondary = baseWeapon.Secondary[sec_Id];
        WeaponParts tertiary = baseWeapon.Tertiary[ter_Id];

        Catalyst cat = Cats[cat_Id];

        int rarityId = cat_Id % 6;

        StringBuilder sb = new StringBuilder();
        sb.Append(((MaterialType)pri_Id).ToString());
        sb.Append(baseWeapon.Type.ToString());
        sb.Append(cat.Suffix);

        string name = sb.ToString();

        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];

            if (char.IsUpper(c) && i != 0)
            {
                name = name.Insert(i, " ");
                i++;
            }
        }

        stats.Name = name;

        stats.Type = baseWeapon.Type;
        stats.WeaponSkillType = baseWeapon.WeaponSkillType;
        stats.HandType = baseWeapon.HandType;

        stats.CritDamage = baseWeapon.CritDamage;

        stats.Primary = primary;

        stats.Secoundary = secondary.WeaponPart;
        stats.Teritiary = tertiary.WeaponPart;

        stats.ActionsPerSecond = baseWeapon.AttacksPerSecond - (baseWeapon.AttacksPerSecond * ((sec_Id * 0.005f) + (ter_Id * 0.005f)));
        stats.ActionsPerSecond = Mathf.Round(stats.ActionsPerSecond * 1000) / 1000;

        int weight = (int)(baseWeapon.Weigth * (1f + (0.02f * ((float)pri_Id + sec_Id + ter_Id))));

        stats.Weight = weight;

        int averageDamage = 0;

        if (level == 0)
        {
            int tempLevel = Player.player.GetLevel() * 4 + 1;

            if (tempLevel > 101)
            {
                tempLevel = 101;
            }

            int chance = Random.Range(0, tempLevel);

            multi = 1f + ((float)chance / GlobalValues.LevelCap);
        }
        else
        {
            multi = 1f + ((float)level / GlobalValues.LevelCap);
        }

        for (int i = 0; i < 4; i++)
        {
            if (cat.CatMultis[i] == 0)
            {
                continue;
            }

            int temp;

            DamageTypeStruct damageStruct = new DamageTypeStruct
            {
                Type = (DamageTypeEnum)i,

                LDamage = (int)(baseWeapon.MinDamage[i] * matMulti.Multi * cat.CatMultis[i] *
                (1f + (.01f * (sec_Id + ter_Id))) * multi),

                HDamage = (int)(baseWeapon.MaxDamage[i] * matMulti.Multi * cat.CatMultis[i] *
                (1f + (.01f * (sec_Id + ter_Id))) * multi),

            };

            stats.DamageRanges.Add(damageStruct);

            temp = damageStruct.LDamage;
            temp += damageStruct.HDamage;
            temp /= 2;

            averageDamage += temp;
        }

        averageDamage = (int)(averageDamage * stats.ActionsPerSecond);


        float value = baseWeapon.Values[pri_Id];
        value += secondary.Value;
        value += tertiary.Value;
        value *= 1.1f;
        value *= 1.0f + (averageDamage / 100.0f);
        value *= cat.ValueMulti;

        stats.Value = (int)value;

        for (int i = 0; i < stats.DamageRanges.Count; i++)
        {
            stats.StatusChance.Add(60);
        }

        stats.MaxDurability = (int)(baseWeapon.Durability *
            (1f + (.2f * (pri_Id + sec_Id + ter_Id))) * multi);
        stats.CurrentDurability = stats.MaxDurability;

        for (int i = 0; i < 2; i++)
        {
            stats.Animator[i] = baseWeapon.Animator[i];
        }

        stats.LifeSteal = baseWeapon.LifeSteal;
        stats.PwrAttackDamage = baseWeapon.PwrAttackDamage;

        stats.AttackAnimationName = baseWeapon.AttackAnimationName;
        stats.PwrAttackAnimationName = baseWeapon.PwrAttackAnimationName;

        stats.Rarity = GlobalValues.rarities[rarityId];

        if (cat_Id < 6)
        {
            stats.Material = WeaponMaterials[pri_Id];
        }
        else if (cat_Id < 12)
        {
            stats.Material = FireMaterials[pri_Id];
        }
        else if (cat_Id < 18)
        {
            stats.Material = LightningMaterials[pri_Id];
        }
        else
        {
            stats.Material = IceMaterials[pri_Id];
        }

        WeaponRef.SetWeaponState(stats);

        return weapon.GetComponent<Item>();
    }
}
