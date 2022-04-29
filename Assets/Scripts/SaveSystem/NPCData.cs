using UnityEngine;


[System.Serializable]
public class NPCData : LivingEntitiesData
{
    public int CurrentSpeachBranch;
    public int CurrentQuest;

    public bool IsDead;

    public NPCData(AIController NPC) : base(NPC)
    {
        NPC npc = NPC.Controller as NPC;

        CurrentSpeachBranch = npc.CurrentSpeachBranch;
        CurrentQuest = npc.CurrentQuest;

        IsDead = NPC.GetDead();
    }
}
