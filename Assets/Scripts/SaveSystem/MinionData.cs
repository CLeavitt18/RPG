using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinionData : EnemyData
{
    public int Mode;
    public int Id;
    public int HandSource;
    public int SourceId;

    public MinionData(AIController minion) : base(minion)
    {
        Mode = (int)minion.GetMode();

        Minion refM = minion.GetController() as Minion; 

        for (int i = 0; i < PrefabIDs.prefabIDs.Minions.Length; i++)
        {
            if (minion.GetName() == PrefabIDs.prefabIDs.Minions[i].transform.GetChild(0).GetComponent<AIController>().GetName())
            {
                Id = i;
                break;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            Item heldItem = refM.Owner.GetHeldItem(i);

            if (heldItem == null)
            {
                continue;
            }

            if (heldItem.CompareTag("Spell"))
            {
                SpellHolder spellH = heldItem.GetComponent<SpellHolder>();

                for (int x = 0; x < 3; x++)
                {
                    Spell spellRef = spellH.GetRune(x);
                    if (spellRef != null && spellRef == refM.SourceSpell)
                    {
                        HandSource = i;
                        SourceId = x;
                    }
                }
            }
        }
    }
}
