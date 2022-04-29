using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : ScriptableObject
{
    public RangesArray[] Ranges;

    public FloatArray[] CastsPerSecond;

    public IntArray[] ManaCost;

    public ResourceList[] SpellAffects;
}
