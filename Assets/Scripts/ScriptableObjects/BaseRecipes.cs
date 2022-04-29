using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class BaseRecipes : ScriptableObject
{
    public ItemAmount[] ItemsRequired;
}
