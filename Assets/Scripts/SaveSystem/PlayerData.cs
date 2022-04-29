
[System.Serializable]
public class PlayerData : LivingEntitiesData
{
    public int AttributePoints;
    public int LevelProgress;
    public int StoredLevel;
    public int NumOfQuests;
    
    public QuestData[] Quests;

    public float[] CRotation = new float[3];

    public Skill[] Masteries = new Skill[GlobalValues.Masteries];
    public Skill[] Skills = new Skill[GlobalValues.Skills];

    public PlayerData(Player Player) : base(Player)
    {
        for (int i = 0; i < 3; i++)
        {
            Attributes[i].Ability = Player.GetAbility(i);
        }

        AttributePoints = Player.GetAPoints();
        LevelProgress = Player.GetLevelProgress();
        StoredLevel = Player.GetStoredLevels();

        CRotation[0] = Player.gameObject.transform.GetChild(0).rotation.eulerAngles.x;
        CRotation[1] = Player.gameObject.transform.GetChild(0).rotation.eulerAngles.y;
        CRotation[2] = Player.gameObject.transform.GetChild(0).rotation.eulerAngles.z;

        for (int i = 0; i < Masteries.Length; i++)
        {
            Masteries[i].Level = Player.GetMasteryLevel(i);
            Masteries[i].Exp = Player.GetMasteryExp(i);
            Masteries[i].RExp = Player.GetMasteryRExp(i);
        }

        for (int i = 0; i < Skills.Length; i++)
        {
            Skills[i].Level = Player.GetSkillLevel(i);
            Skills[i].Exp = Player.GetSkillExp(i);
            Skills[i].RExp = Player.GetSkillRExp(i);
        }

        NumOfQuests = QuestTracker.questTracker.Quests.Count;
        Quests = new QuestData[NumOfQuests];

        for (int i = 0; i < NumOfQuests; i++)
        {
            Quests[i] = new QuestData();

            QuestHolder QuestH = QuestTracker.questTracker.Quests[i];

            LoadSystem.LoadItem(QuestH, Quests[i]);
        }

        FirstOpen = Player.GetFirstOpen();
    }
}
