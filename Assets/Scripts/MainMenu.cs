using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public string PlayerName;
    [SerializeField] public string SaveProfile;
    
    [SerializeField] public Transform PlayerProfilesList;

    [SerializeField] public GameObject MainMenuUI;
    [SerializeField] public GameObject NewGameMenu;
    [SerializeField] public GameObject LoadGameMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] public GameObject ConfirmPlayerName;
    [SerializeField] public GameObject InvaildNameUI;
    [SerializeField] public GameObject PlayerNameAlreadyExists;
    [SerializeField] public GameObject PlayerProfileButton;

    [SerializeField] public GameObject[] LoadUiObjects;
    
    [SerializeField] public TMP_InputField PlayerNameInputField;

    [SerializeField] private Toggle ShowPlayerHSMNumToggle;
    [SerializeField] private Toggle ShowEnemyNumToggle;
    [SerializeField] private Toggle ShowFPSToggle;

    private void OnEnable()
    {
        Application.targetFrameRate = -1;
        PlayerNameInputField.onSubmit.AddListener(CheckName);

        int toggle;

        if (PlayerPrefs.HasKey("ShowPlayerHSMNumToggle"))
        {
            toggle = PlayerPrefs.GetInt("ShowPlayerHSMNumToggle");

            if (toggle == 0)
            {
                ShowPlayerHSMNumToggle.isOn = false;
            }
            else
            {
                ShowPlayerHSMNumToggle.isOn = true;
            }
        }
        else
        {
            if (ShowPlayerHSMNumToggle.isOn)
            {
                toggle = 1;
            }
            else
            {
                toggle = 0;
            }

            PlayerPrefs.SetInt("ShowPlayerHSMNumToggle", toggle);
        }

        if (PlayerPrefs.HasKey("ShowEnemyNumToggle"))
        {
            toggle = PlayerPrefs.GetInt("ShowEnemyNumToggle");

            if (toggle == 0)
            {
                ShowEnemyNumToggle.isOn = false;
            }
            else
            {
                ShowEnemyNumToggle.isOn = true;
            }
        }
        else
        {
            if (ShowEnemyNumToggle.isOn)
            {
                toggle = 1;
            }
            else
            {
                toggle = 0;
            }

            PlayerPrefs.SetInt("ShowEnemyNumToggle", toggle);
        }

        if (PlayerPrefs.HasKey("ShowFPSToggle"))
        {
            toggle = PlayerPrefs.GetInt("ShowFPSToggle");

            if (toggle == 0)
            {
                ShowFPSToggle.isOn = false;
            }
            else
            {
                ShowFPSToggle.isOn = true;
            }
        }
        else
        {
            if (ShowFPSToggle.isOn)
            {
                toggle = 1;
            }
            else
            {
                toggle = 0;
            }

            PlayerPrefs.SetInt("ShowFPSToggle", toggle);
            
            PlayerPrefs.Save();
        }

        SetShowPlayerHSMNumToggle();
        SetShowEnemyNumToggle();
        SetShowFPSToggle();
        OptionsManager.instance.GetPrefs();
    }

    public void NewGame()
    {
        NewGameMenu.SetActive(true);
        MainMenuUI.SetActive(false);
    }

    public void SetProfilesUI()
    {
        if (PlayerProfilesList.childCount > 0)
        {
            int ChildCount = PlayerProfilesList.childCount;

            for (int i = 0; i < ChildCount; i++)
            {
                Destroy(PlayerProfilesList.GetChild(i).gameObject);
            }
        }

        LoadGameMenu.SetActive(true);
        LoadUiObjects[0].SetActive(true);
        LoadUiObjects[1].SetActive(false);
        LoadUiObjects[2].SetActive(false);
        LoadUiObjects[3].SetActive(false);
        LoadUiObjects[4].SetActive(false);
        LoadUiObjects[5].SetActive(false);
        LoadUiObjects[6].SetActive(true);
        LoadUiObjects[7].SetActive(false);
        MainMenuUI.SetActive(false);

        DirectoryInfo Info = new DirectoryInfo(Application.persistentDataPath);

        DirectoryInfo[] DirInfo = Info.GetDirectories();

        foreach (DirectoryInfo File in DirInfo)
        {
            if (File.Name != "Unity")
            {
                GameObject Profile = Instantiate(PlayerProfileButton, PlayerProfilesList);

                Profile.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = File.Name;
            }
        }
    }

    public void SetSaveFilesUI()
    {
        DirectoryInfo Info = new DirectoryInfo(Application.persistentDataPath + "/" + PlayerName);

        DirectoryInfo[] DirInfo = Info.GetDirectories();

        int temp = PlayerProfilesList.childCount;

        for (int i = 0; i < temp; i++)
        {
            Destroy(PlayerProfilesList.GetChild(i).gameObject);
        }

        PlayerProfilesList.DetachChildren();

        foreach (DirectoryInfo File in DirInfo)
        {
            GameObject Profile = Instantiate(PlayerProfileButton, PlayerProfilesList);

            Profile.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = File.Name;
            Profile.GetComponent<PlayerProfileInfo>().Mode = true;
        }

        LoadUiObjects[0].SetActive(false);
        LoadUiObjects[1].SetActive(false);
        LoadUiObjects[2].SetActive(false);
        LoadUiObjects[3].SetActive(true);
        LoadUiObjects[4].SetActive(false);
        LoadUiObjects[5].SetActive(false);
        LoadUiObjects[6].SetActive(false);
        LoadUiObjects[7].SetActive(true);
    }

    public void SetFocusedProfile(GameObject Item)
    {
        PlayerName = Item.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;

        LoadUiObjects[1].SetActive(true);
        LoadUiObjects[2].SetActive(true);
    }

    public void SetFocusedSaveFile(GameObject Item)
    {
        SaveProfile = Item.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;

        LoadUiObjects[4].SetActive(true);
        LoadUiObjects[5].SetActive(true);
    }

    public void LoadProfile()
    {
        LoadSavedGame();
    }

    public void DeleteProfile()
    {
        Directory.Delete(Application.persistentDataPath + "/" + PlayerName, true);

        SetProfilesUI();
    }

    public void DeleteSaveProfile()
    {
        Directory.Delete(Application.persistentDataPath + '/' + PlayerName + '/' + SaveProfile, true);

        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + '/' + PlayerName);

        if (info.GetDirectories().Length == 0)
        {
            Directory.Delete(Application.persistentDataPath + '/' + PlayerName, true);

            SetProfilesUI();
        }
        else
        {
            SetSaveFilesUI();
        }

    }

    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
        MainMenuUI.SetActive(false);

        OptionsManager.instance.GetPrefs();
    }

    public void SetShowPlayerHSMNumToggle()
    {
        if (ShowFPSToggle.isOn)
        {
            PlayerPrefs.SetInt("ShowPlayerHSMNumToggle", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ShowPlayerHSMNumToggle", 0);
        }

        PlayerPrefs.Save();
    }

    public void SetShowEnemyNumToggle()
    {
        if (ShowFPSToggle.isOn)
        {
            PlayerPrefs.SetInt("ShowEnemyNumToggle", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ShowEnemyNumToggle", 0);
        }

        PlayerPrefs.Save();
    }


    public void SetShowFPSToggle()
    {
        if (ShowFPSToggle.isOn)
        {
            PlayerPrefs.SetInt("ShowFPSToggle", 1);
            FPSCounter.Counter.gameObject.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("ShowFPSToggle", 0);
            FPSCounter.Counter.gameObject.SetActive(false);
        }

        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CheckNameFromButton()
    {
        CheckName(PlayerNameInputField.text);
    }

    public void CheckName(string PlayerName)
    {
        if (PlayerNameInputField.wasCanceled)
        {
            return;
        }

        if (PlayerName == string.Empty)
        {
            return;
        }

        for (int i = 0; i < PlayerName.Length; i++)
        {
            if (PlayerName[i] == '/')
            {
                InvaildPlayerName();
                return;
            }
        }

        NewGameMenu.SetActive(false);

        DirectoryInfo Info = new DirectoryInfo(Application.persistentDataPath);

        DirectoryInfo[] DirInfo = Info.GetDirectories();

        foreach (DirectoryInfo File in DirInfo)
        {
            if (File.Name == PlayerName)
            {
                NameAlreadyExists();
                return;
            }
        }

        ConfirmPlayerName.SetActive(true);
    }

    public void InvaildPlayerName()
    {
        InvaildNameUI.SetActive(true);
    }

    public void NameAlreadyExists()
    {
        PlayerNameAlreadyExists.SetActive(true);
    }

    public void BackToNewGameMenuFromInvaildNameUI()
    {
        InvaildNameUI.SetActive(false);
        NewGameMenu.SetActive(true);
    }

    public void BackToNewGameMenuFromNameAlreadyExists()
    {
        PlayerNameAlreadyExists.SetActive(false);
        NewGameMenu.SetActive(true);
    }

    public void BackToNewGameFromConfirmName()
    {
        NewGameMenu.SetActive(true);
        ConfirmPlayerName.SetActive(false);
    }

    public void BackToMainMenuFromLoadScreen()
    {
        LoadGameMenu.SetActive(false);
        MainMenuUI.SetActive(true);
    }

    public void BackToLoadScreanFromPlayerProfileScrean()
    {
        SetProfilesUI();
    }

    public void BackToMainFromOptions()
    {
        OptionsMenu.SetActive(false);
        MainMenuUI.SetActive(true);
    }

    public void CreateNewGame()
    {
        WorldStateTracker.Tracker.PlayerName = PlayerNameInputField.text;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Starting Camp");
    }

    public void BackToMainMenuFromNewGameMenu()
    {
        Destroy(WorldStateTracker.Tracker.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    public void LoadSavedGame()
    {
        WorldStateTracker.Tracker.PlayerName = PlayerName;
        WorldStateTracker.Tracker.SaveProfile = SaveProfile;
        WorldStateTracker.Tracker.LoadGame();
    }
}
