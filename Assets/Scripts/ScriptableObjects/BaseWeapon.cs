using UnityEngine;

[CreateAssetMenu(fileName = "New BaseWeaopn", menuName = "Base Weapon")]
public class BaseWeapon : ScriptableObject
{
    public SkillType WeaponSkillType;
    public HandType HandType;

    public int LifeSteal;
    public int PwrAttackDamage;
    public int CritDamage;
    public int Durability;

    public int[] StatusChance = new int[4];

    public int[] MinDamage = new int[4];
    public int[] MaxDamage = new int[4];

    public int[] Values = new int[11];

    public GameObject Primary;
    public WeaponParts[] Secondary = new WeaponParts[11];
    public WeaponParts[] Tertiary = new WeaponParts[11];

    public float Weigth;
    public float AttacksPerSecond;

    public WeaponType Type;

    public RuntimeAnimatorController[] Animator = new RuntimeAnimatorController[2];

    public string AttackAnimationName;
    public string PwrAttackAnimationName;
}
