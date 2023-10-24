using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Scaler", menuName = "Enemy Scaler")]
public class EntityScaler : ScriptableObject
{
    public int[] ThreshHolds;
    public int[] BaseMasteries = new int[GlobalValues.Masteries];
    public int[] BaseSkills = new int[GlobalValues.Skills];
    public int[] MasteryWeights = new int[GlobalValues.Masteries];
    public int[] SkillWeights = new int[GlobalValues.Skills];

    public int[] BaseAbilities = new int[3];
    public int[] AbilitiesPerLevel = new int[3];
    
    public EnemyWeaponData[] WeaponData;
    public EnemyWeaponData[] OffWeaponData;

    public EnemySpellData[] spellData;
    public EnemySpellData[] offSpellData;
}
