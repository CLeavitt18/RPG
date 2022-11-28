using UnityEngine;

public class BaseSpell : ScriptableObject
{
    public RangesArray[] Ranges;

    public FloatArray[] CastsPerSecond;

    public IntArray[] Value;
    public IntArray[] ManaCost;

    public ResourceList[] SpellAffects;
}
