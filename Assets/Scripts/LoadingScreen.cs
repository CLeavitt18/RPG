using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public bool Update = false;

    public Image LoadingBar;

    int target = 1;
    int tracker = 0;

    public void OnEnable()
    {
        Time.timeScale = 0;
        LoadingBar.fillAmount = (float)tracker / (float)target;
        StartCoroutine(CheckLoadingStatus());
    }

    public IEnumerator CheckLoadingStatus()
    {
        if (!Update)
        {
            yield break;
        }

        yield return new WaitForSecondsRealtime(Time.deltaTime);

        target = SceneManagerOwn.Manager.Target;
        tracker = SceneManagerOwn.Manager.Tracker;

        do
        {
            tracker = SceneManagerOwn.Manager.Tracker;
            target = SceneManagerOwn.Manager.Target;
            LoadingBar.fillAmount = (float)SceneManagerOwn.Manager.Tracker / (float)SceneManagerOwn.Manager.Target;

        } while (tracker < target);

        yield return new WaitForSecondsRealtime(.5f);

        Time.timeScale = 0;

        transform.GetChild(0).gameObject.SetActive(false);
        
        Player.player.SetPlayerStateActive();
        
        yield return new WaitForSecondsRealtime(.25f);

        Time.timeScale = 1;

        Destroy(gameObject);
    }
}
