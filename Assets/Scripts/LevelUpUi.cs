using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUi : IUi
{
    [SerializeField] private GameObject levelUpUi;
    [SerializeField] private GameObject LevelUpCounter;

    [SerializeField] private int[] TempAttributes = new int[3];

    [SerializeField] private Text AvalialbePointsText;
    [SerializeField] private Text LevelCounterText;

    [SerializeField] private Text[] AbilityTexts = new Text[3];

    [SerializeField] private Button[] AttributeButtons;


    public override void Set()
    {
        if (isActive)
        {
            return;
        }

        isActive = true;
        
        SetLevelCounter(Player.player.GetStoredLevels());
        
        levelUpUi.SetActive(true);
        Player.player.AddAPoints(GlobalValues.APointsPerLevel);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 3; i++)
        {
            AbilityTexts[i].text = "+ 0";

            sb.Append(((Abilities)i).ToString());
            sb.Append(": ");
            sb.Append(Player.player.GetAbility(i));

            AttributeButtons[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = sb.ToString();

            sb.Clear();
        }

        sb = new StringBuilder("Avaliable Points: ");
        sb.Append(Player.player.GetAPoints());

        AvalialbePointsText.text = sb.ToString();
    }

    public override void Clear()
    {
        base.Clear();
    }

    public void ConfirmLevelUp()
    {
        Player.player.LevelUp(TempAttributes);

        TempAttributes[0] = 0;
        TempAttributes[1] = 0;
        TempAttributes[2] = 0;

        for (int i = 0; i < 3; i++)
        {
            PlayerUi.playerUi.SetPlayerAttributeUI(i);
        }

        levelUpUi.SetActive(false);
        PlayerUi.playerUi.SetPlayerCanvas(true);
    }

    public void AddTempAttribute(int ID)
    {
        if (Player.player.GetAPoints() > 0)
        {
            TempAttributes[ID]++;
            Player.player.SubAPoints(1);

            StringBuilder sb = new StringBuilder("+ ");
            sb.Append(TempAttributes[ID]);

            AbilityTexts[ID].text = sb.ToString();

            sb = new StringBuilder("Avaliable Points: ");
            sb.Append(Player.player.GetAPoints());

            AvalialbePointsText.text = sb.ToString();
        }
    }

    public void SubAttribute(int ID)
    {
        if (TempAttributes[ID] > 0)
        {
            TempAttributes[ID]--;
            Player.player.AddAPoints(1);

            StringBuilder sb = new StringBuilder("+ ");
            sb.Append(TempAttributes[ID]);

            AbilityTexts[ID].text = sb.ToString();

            sb = new StringBuilder("Avaliable Points: ");
            sb.Append(Player.player.GetAPoints());

            AvalialbePointsText.text = sb.ToString();
        }
    }

    public void SetLevelCounter(int Levels)
    {
        if (Levels == 0)
        {
            LevelUpCounter.SetActive(false);
            return;
        }

        LevelUpCounter.SetActive(true);

        StringBuilder sb = new StringBuilder(Levels.ToString());

        LevelCounterText.text = sb.ToString();
    }
}
