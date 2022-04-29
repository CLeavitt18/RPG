using UnityEngine;

[CreateAssetMenu(fileName = "New Catalyst", menuName = "Catalyst")]
public class Catalyst : ScriptableObject
{
    public int ValueMulti;

    public int[] CatMultis = new int[4];

    public Color Rarity;

    public string Suffix;
}
