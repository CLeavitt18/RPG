using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager instance;

    [SerializeField] private bool ShowPlayerHSMNumToggle;
    [SerializeField] private bool ShowEnemyNumToggle;

    private void OnEnable() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void GetPrefs()
    {
        if (PlayerPrefs.GetInt("ShowPlayerHSMNumToggle") == 0)
        {
            ShowPlayerHSMNumToggle = false;
        }
        else
        {
            ShowPlayerHSMNumToggle = true;
        }

        if (PlayerPrefs.GetInt("ShowEnemyNumToggle") == 0)
        {
            ShowEnemyNumToggle = false;
        }
        else
        {
            ShowEnemyNumToggle = true;
        }
    }

    public bool GetShowPlayerHSMNumToggle()
    {
        return ShowPlayerHSMNumToggle;
    }

    public bool GetShowEnemyNumToggle()
    {
        return ShowEnemyNumToggle;
    }
}
