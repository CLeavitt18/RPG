using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject NewGameMenu;
    public GameObject LoadGameMenu;
    public GameObject ConfirmPlayerName;
    public GameObject InvaildNameUI;
    public GameObject PlayerNameAlreadyExists;
    public GameObject PlayerProfileButton;

    public GameObject[] LoadUiObjects;
    
    public Transform PlayerProfilesList;
    
    public TMP_InputField PlayerNameInputField;

    public string PlayerName;
    public string SaveProfile;

    private void OnEnable()
    {
        Application.targetFrameRate = -1;
        PlayerNameInputField.onSubmit.AddListener(CheckName);
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
