using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUi : MonoBehaviour
{
    public static PlayerUi playerUi;

    public TextMeshProUGUI PlayerInstuctionText { get{return playerIntrustionText;} private set{}}
    [SerializeField] private TextMeshProUGUI playerIntrustionText;

    [SerializeField] private IUi[] uis;

    [SerializeField] private Image EnemyHealthBar;

    [SerializeField] private Image[] AttributeBars = new Image[3];
    [SerializeField] private Image[] ReserveBars = new Image[3];

    [SerializeField] private TextMeshProUGUI EnemyInfoText;

    [SerializeField] private TextMeshProUGUI[] AttributeTexts = new TextMeshProUGUI[3];

    [SerializeField] private GameObject PlayerCanvas;
    [SerializeField] private GameObject EnemyInfoUi;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] public GameObject StatusHolder;
    [SerializeField] public GameObject BuringIndicator;
    [SerializeField] public GameObject ShockedIndicator;
    [SerializeField] public GameObject ChilldedIndicator;
    [SerializeField] public GameObject FrozenIndicator;

    private float NextCheck;
    private float WaitTime = .1f;


    public void OnEnable()
    {
        playerUi = this;
    }

    public void Update()
    {
        if (Time.time >= NextCheck)
        {
            EnemyInfoUi.SetActive(false);
        }
    }

    public void CallSetStats()
    {
        for (int i = 0; i < uis.Length; i++)
        {
            if ((int)UiType.Stats == i)
            {
                uis[i].Set();
            }
            else
            {
                uis[i].Clear();
            }
        }
    }

    public void CallSetInventory()
    {
        for (int i = 0; i < uis.Length; i++)
        {
            uis[i].Clear();
        }

        uis[(int)UiType.Inventory].Set();
    }

    public void CallSetQuestInventory()
    {
        for (int i = 0; i < uis.Length; i++)
        {
            uis[i].Clear();
        }

        uis[(int)UiType.Quests].Set();
    }

    public void CallSetPassivetree()
    {
        for (int i = 0; i < uis.Length; i++)
        {
            uis[i].Clear();
        }

        uis[(int)UiType.PassiveTree].Set();
    }

    public void CallSetPassiveTreeButtons()
    {
        uis[(int)UiType.PassiveTree].Set();
    }

    public void StartPause(bool pause)
    {
        PauseMenu.SetActive(pause);
        PlayerCanvas.SetActive(!pause);
    }

    public void ExitPauseMenu()
    {
        PauseMenu.GetComponent<PauseMenu>().ExitMenu();
    }

    public void SetPlayerCanvas(bool state)
    {
        PlayerCanvas.SetActive(state);
    }

    public void Close()
    {
        for (int i = 0; i < uis.Length; i++)
        {
            uis[i].Clear();
            uis[i].Close();
        }

        PauseMenu.GetComponent<PauseMenu>().SetPauseMenuDefault();
        PauseMenu.SetActive(false);
        PlayerCanvas.SetActive(true);
    }

    public void SetEnemyInfoUI(AIController enemy)
    {
        StringBuilder sb = new StringBuilder(enemy.GetName());

        EnemyInfoUi.SetActive(true);

        if(OptionsManager.instance.GetShowEnemyNumToggle())
        {
            sb.Append(" ");
            sb.Append(enemy.GetCurrentHealth());
            sb.Append(" / ");
            sb.Append(enemy.GetMaxHealth());
        }

        EnemyInfoText.text = sb.ToString();
        EnemyHealthBar.fillAmount = (float)enemy.GetCurrentHealth() / enemy.GetMaxHealth();

        NextCheck = Time.time + WaitTime;
    }

    public void SetPlayerAttributeUI(int attribute)
    {
        long reserved = Player.player.GetResrvedAttribute(attribute);
        long current = Player.player.GetCurrentAttribute(attribute);
        long max = Player.player.GetMaxAttribute(attribute);

        AttributeBars[attribute].fillAmount = (float)((decimal)current / max);
        ReserveBars[attribute].fillAmount = (float)((decimal)reserved / max);

        if (OptionsManager.instance.GetShowPlayerHSMNumToggle() == false)
        {
            AttributeTexts[attribute].text = "";
            return;
        }

        StringBuilder sb = new StringBuilder(current.ToString());
        sb.Append(" / ");
        sb.Append(max);

        if (reserved != 0)
        {
            sb.Append("   (");
            sb.Append(reserved);
            sb.Append(")");
        }

        AttributeTexts[attribute].text = sb.ToString();
    }

    public void CallSetSleepUi()
    {
        uis[(int)UiType.Sleep].Set();
    }

    public void CallSetLevelUpUi()
    {
        for (int i = 0; i < uis.Length; i++)
        {
            if ((int)UiType.LevelUp == i)
            {
                continue;
            }

            uis[i].Clear();
        }

        uis[(int)UiType.LevelUp].Set();
    }

    public void CallSetLevelCounter(int level)
    {
        (uis[(int)UiType.LevelUp] as LevelUpUi).SetLevelCounter(level);
    }

    public void CallBurning(int ticks, float waitTime)
    {
        StartCoroutine(Burning(ticks, waitTime));
    }

    public void CallShocked(int Chains)
    {
        StartCoroutine(Shocked(Chains));
    }

    public IEnumerator Burning(int ticks, float waitTime)
    {
        GameObject BS = Instantiate(BuringIndicator, StatusHolder.transform);

        for (int i = 0; i < ticks; i++)
        {
            StringBuilder sb = new StringBuilder((Mathf.Round(((float)(ticks - i) * waitTime) * 10) / 10).ToString("0.0"));
            sb.Append("s");

            BS.transform.GetChild(0).GetComponent<Text>().text = sb.ToString();
            BS.GetComponent<RawImage>().color = new Color(.8f, .1f, .1f, .8f);
            //Debug.Log((Mathf.Round(((float)(ticks - i) * waitTime))).ToString());

            yield return new WaitForSeconds(waitTime * .5f);

            BS.GetComponent<RawImage>().color = new Color(.8f, .1f, .1f, 1f);

            yield return new WaitForSeconds(waitTime * .5f);
        }

        Destroy(BS);
    }

    public IEnumerator Shocked(int Chains)
    {
        GameObject LC = Instantiate(ShockedIndicator, StatusHolder.transform);

        for (int i = 0; i < Chains; i++)
        {
            StringBuilder sb = new StringBuilder((Chains - (i + 1)).ToString());

            LC.transform.GetChild(0).GetComponent<Text>().text = sb.ToString();
            LC.GetComponent<RawImage>().color = new Color(.02f, .05f, 1f, .8f);

            yield return new WaitForSeconds(.20f);

            LC.GetComponent<RawImage>().color = new Color(.02f, .05f, 1f, 1f);
        }

        Destroy(LC);
    }
}
