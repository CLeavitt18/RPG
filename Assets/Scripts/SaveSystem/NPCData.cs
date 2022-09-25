using UnityEngine;


[System.Serializable]
public class NPCData : LivingEntitiesData
{
    public int CurrentSpeachBranch;
    public int CurrentQuest;

    public bool IsDead;

    public NPCData(AIController NPC) : base(NPC)
    {
        NPC npc = NPC.GetController() as NPC;

        CurrentSpeachBranch = npc.GetCurrentSpeachBranch();
        CurrentQuest = npc.GetCurrentQuest();

        IsDead = NPC.GetDead();
    }
}
