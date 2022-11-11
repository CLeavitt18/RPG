using UnityEngine;

[CreateAssetMenu(fileName = "New BaseArmour", menuName = "Base Armour")]
public class BaseArmour : ScriptableObject
{
    public int Durability;

    public int[] Armour;
    public int[] Weight;

    public IntArray[] Resistnce;

    public ArmourType ArmourType;
}
