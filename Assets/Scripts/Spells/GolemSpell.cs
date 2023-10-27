
public class GolemSpell : Spell, IAura
{
    public DamageType DamageRange;

    private int Number;
    private int Alive;

    private bool Activated ;


    public override void SetStats(SpellStats stats)
    {
        GolemSpellStats statsG = stats as GolemSpellStats;

        DamageRange = statsG.range;

        Number = statsG.number;

        Activated = statsG.activated;

        base.SetStats(stats);
    }

    public void IncrementAlive()
    {
        Alive++;
        Activated = true;
    }

    public void DecrametAlive()
    {
        Alive--;

        if (Alive == 0)
        {
            Activated = false;
        }
    }

    public int GetNumber()
    {
        return Number;
    }

    public int GetAlive()
    {
        return Alive;
    }

    public bool GetActivated()
    {
        return Activated;
    }

    public override bool Equals(Spell spell)
    {
        if (spell is GolemSpell gSpell)
        {
            if (DamageRange.Type == gSpell.DamageRange.Type &&
                DamageRange.LDamage == gSpell.DamageRange.LDamage &&
                DamageRange.HDamage == gSpell.DamageRange.HDamage &&
                Number == gSpell.Number &&
                Activated == false)
            {
                return base.Equals(spell);
            }
        }

        return false;
    }
}
