using UnityEngine;

public class Spell : MonoBehaviour
{
    public string Name;

    public SpellType SpellType;
    
    public CastType CastType;

    public CastTarget Target;

    public AttributesEnum CostType;

    public SkillType SkillType;

    public int Cost;

    public GameObject SpellAffect;

    public float CastsPerSecond;

    protected float NextAttack;

    public virtual void Cast(Hand hand)
    {

    }

    public virtual bool Equals(Spell spell)
    {
        if (SpellType == spell.SpellType && 
            CastType == spell.CastType &&
            CostType == spell.CostType &&
            Cost == spell.Cost &&
            SpellAffect == spell.SpellAffect &&
            CastsPerSecond == spell.CastsPerSecond &&
            SkillType == spell.SkillType)
        {
            return true;
        }

        return false;
    }
}
