using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SleepUi : IUi
{
    [SerializeField] private int SleepDuration;

    [SerializeField] private GameObject _sleepUi;

    [SerializeField] private Text SleepText;

    [SerializeField] private Slider SleepBar;

    [SerializeField] private bool IsSleeping;

    public override void Set()
    {
        _sleepUi.SetActive(true);

        SetSleepDuration();
    }

    public override void Clear()
    {
        _sleepUi.SetActive(false);
    }

    public void IncrementDuration()
    {
        if (IsSleeping)
        {
            return;
        }

        SleepBar.value++;
        SetSleepDuration();
    }

    public void DecrementDuration()
    {
        if (IsSleeping)
        {
            return;
        }

        SleepBar.value--;
        SetSleepDuration();
    }

    public void SetSleepDuration()
    {
        if (IsSleeping)
        {
            return;
        }

        SleepDuration = (int)SleepBar.value;

        StringBuilder sb = new StringBuilder(SleepDuration.ToString("n0"));

        SleepText.text = sb.ToString();
    }

    public void CancelSleep()
    {
        IsSleeping = false;
        SleepBar.value = 1;
        SleepDuration = 1;
        Player.player.SetPlayerStateActive();
        StopAllCoroutines();
        Clear();
    }

    public void StartSleep()
    {
        if (!IsSleeping)
        {
            StartCoroutine(Sleep());
        }
    }

    public IEnumerator Sleep()
    {
        Player.player.SetPlayerStateSleeping();

        //Debug.Log("Currenet Time: " + Time.time);
        IsSleeping = true;

        for (int i = 0; i < SleepDuration; i++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (Player.player.GetCurrentAttribute(x) == Player.player.GetMaxAttribute(x))
                {
                    continue;
                }

                int Heal = (int)((float)Player.player.GetMaxAttribute(x) * .041f);

                Player.player.GainAttribute(Heal, (AttributesEnum)x);
            }

            GameObject.Find("LightManager").GetComponent<LightingManager>().TimeOfDay++;
            SleepBar.value--;

            StringBuilder sb = new StringBuilder(((int)SleepBar.value).ToString());
            SleepText.text = sb.ToString();

            yield return new WaitForSecondsRealtime(1);
        }

        SleepDuration = 1;

        int storedLevels = Player.player.GetStoredLevels();

        if (storedLevels > 0)
        {
            Player.player.SubStoredLevel(1);

            _sleepUi.SetActive(false);

            IsSleeping = false;

            PlayerUi.playerUi.CallSetLevelUpUi();

            yield break;
        }

        yield return new WaitForSecondsRealtime(.5f);

        for (int i = 0; i < 3; i++)
        {
            PlayerUi.playerUi.SetPlayerAttributeUI(i);
        }

        PlayerUi.playerUi.SetPlayerCanvas(true);
        Player.player.SetPlayerStateActive();
        _sleepUi.SetActive(false);
        IsSleeping = false;
        //Debug.Log("Time After Sleeping: " + Time.time);
    }
}
