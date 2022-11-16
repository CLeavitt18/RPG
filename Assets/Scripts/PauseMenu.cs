using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public Transform SaveSlotHolder;
    public Transform LoadSlotHolder;

    public GameObject SaveButton;
    public GameObject LoadButton;
    public GameObject MainMeunButton;
    public GameObject QuitGameButton;
    public GameObject SaveGameUI;
    public GameObject LoadGameUI;
    public GameObject ExistingSaveSlot;
    public GameObject LoadSaveSlot;
    public GameObject InvaildNameUI;

    public InputField ProfileNameInputField;

    private PauseUiState Mode = PauseUiState.Paused;

    public void SetPauseMenuDefault()
    {
        Mode = PauseUiState.Paused;

        SaveButton.SetActive(true);
        LoadButton.SetActive(true);
        MainMeunButton.SetActive(true);
        QuitGameButton.SetActive(true);
        SaveGameUI.SetActive(false);
        SaveGameUI.transform.GetChild(1).gameObject.SetActive(false);
        LoadGameUI.SetActive(false);

        if (SaveSlotHolder.childCount != 1)
        {
            int childCount = SaveSlotHolder.childCount;

            for (int i = 0; i < childCount; i++)
            {
                if (i != 0)
                {
                    Destroy(SaveSlotHolder.GetChild(i).gameObject);
                }
            }
        }

        if (LoadSlotHolder.childCount != 0)
        {
            int childCount = LoadSlotHolder.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(LoadSlotHolder.GetChild(i).gameObject);
            }
        }
    }

    public void SaveGame()
    {
        string name = ProfileNameInputField.text;

        if (name == string.Empty)
        {
            InvaildNameUI.SetActive(true);
            return;
        }

        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == '/')
            {
                InvaildNameUI.SetActive(true);
                return;
            }
        }

        if (name != WorldStateTracker.Tracker.PlayerName)
        {
            StringBuilder path = new StringBuilder(Application.persistentDataPath);
            path.Append('/');
            path.Append(WorldStateTracker.Tracker.PlayerName);
            path.Append('/');
            path.Append(WorldStateTracker.Tracker.SaveProfile);
            path.Append("/Levels");

            DirectoryInfo Info = new DirectoryInfo(path.ToString());
            DirectoryInfo[] DirInfo = Info.GetDirectories();

            StringBuilder newPath = new StringBuilder(Application.persistentDataPath);
            newPath.Append('/');
            newPath.Append(WorldStateTracker.Tracker.PlayerName);
            newPath.Append('/');
            newPath.Append(name);
            newPath.Append("/Levels");

            Directory.CreateDirectory(newPath.ToString());

            foreach (DirectoryInfo dirInfo in DirInfo)
            {
                StringBuilder newDirectory = new StringBuilder(newPath.ToString());
                newDirectory.Append('/');
                newDirectory.Append(dirInfo.Name);

                Directory.CreateDirectory(newDirectory.ToString());

                DirectoryInfo[] FileInfo = dirInfo.GetDirectories();

                foreach (DirectoryInfo file in FileInfo)
                {
                    StringBuilder filePath = new StringBuilder(newDirectory.ToString());
                    filePath.Append('/');
                    filePath.Append(file.Name);

                    Directory.CreateDirectory(filePath.ToString());

                    FileInfo[] files = file.GetFiles();

                    foreach (FileInfo _file in files)
                    {
                        StringBuilder sb = new StringBuilder(filePath.ToString());
                        sb.Append('/');
                        sb.Append(_file.Name);

                        StreamReader sr = _file.OpenText();

                        File.WriteAllText(sb.ToString(), sr.ReadToEnd());

                        sr.Close();
                    }
                }
            }

            FileInfo[] levelFiles = Info.GetFiles();

            foreach (FileInfo levelData in levelFiles)
            {
                StringBuilder sb = new StringBuilder(newPath.ToString());
                sb.Append('/');
                sb.Append(levelData.Name);

                StreamReader sr = levelData.OpenText();

                File.WriteAllText(sb.ToString(), sr.ReadToEnd());

                sr.Close();
            }
        }


        WorldStateTracker.Tracker.SaveProfile = ProfileNameInputField.text;
        ProfileNameInputField.text = string.Empty;
        WorldStateTracker.Tracker.CallSaveGame();
        SetPauseMenuDefault();
    }

    public void SaveGameOverExistingProfile(string saveName)
    {
        WorldStateTracker.Tracker.SaveProfile = saveName;
        ProfileNameInputField.text = string.Empty;
        WorldStateTracker.Tracker.CallSaveGame();
        SetPauseMenuDefault();
    }

    public void LoadGame(string saveProfile)
    {
        WorldStateTracker.Tracker.SaveProfile = saveProfile;
        WorldStateTracker.Tracker.LoadGame();
    }

    public void MainMenu()
    {
        SaveSystem.DeleteAllTempFiles();
        Destroy(WorldStateTracker.Tracker.gameObject);
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        SaveSystem.DeleteAllTempFiles();
        Application.Quit();
    }

    public void SetSaveUI()
    {
        Mode = PauseUiState.Saving;

        SaveButton.SetActive(false);
        LoadButton.SetActive(false);
        MainMeunButton.SetActive(false);
        QuitGameButton.SetActive(false);
        SaveGameUI.SetActive(true);
        SaveGameUI.transform.GetChild(1).gameObject.SetActive(false);

        DirectoryInfo Info = new DirectoryInfo(Application.persistentDataPath + "/" + WorldStateTracker.Tracker.PlayerName);

        DirectoryInfo[] DirInfo = Info.GetDirectories();

        foreach (DirectoryInfo File in DirInfo)
        {
            GameObject Profile = Instantiate(ExistingSaveSlot, SaveSlotHolder);

            Profile.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = File.Name;
            Profile.GetComponent<ExistingSaveProfileButton>().ProfileName = File.Name;
            Profile.GetComponent<ExistingSaveProfileButton>().Parent = this;
        }
    }

    public void BackToPauseMenuFromSaveUI()
    {
        Mode = PauseUiState.Paused;

        SaveButton.SetActive(true);
        LoadButton.SetActive(true);
        MainMeunButton.SetActive(true);
        QuitGameButton.SetActive(true);
        SaveGameUI.SetActive(false);
        SaveGameUI.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SetLoadUI()
    {
        Mode = PauseUiState.Loading;

        SaveButton.SetActive(false);
        LoadButton.SetActive(false);
        MainMeunButton.SetActive(false);
        QuitGameButton.SetActive(false);
        LoadGameUI.SetActive(true);

        DirectoryInfo Info = new DirectoryInfo(Application.persistentDataPath + "/" + WorldStateTracker.Tracker.PlayerName);

        DirectoryInfo[] DirInfo = Info.GetDirectories();

        foreach (DirectoryInfo File in DirInfo)
        {
            GameObject Profile = Instantiate(LoadSaveSlot, LoadSlotHolder);

            Profile.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = File.Name;
            Profile.GetComponent<LoadProfileButton>().ProfileName = File.Name;
            Profile.GetComponent<LoadProfileButton>().Parent = this;
        }
    }

    public void BackToPauseMenuFromLoadUI()
    {
        Mode = PauseUiState.Paused;

        SaveButton.SetActive(true);
        LoadButton.SetActive(true);
        MainMeunButton.SetActive(true);
        QuitGameButton.SetActive(true);
        LoadGameUI.SetActive(false);
    }

    public void ExitMenu()
    {
        switch (Mode)
        {
            case PauseUiState.Paused:
                SetPauseMenuDefault();
                Player.player.SetPlayerStateActive();
                break;
            case PauseUiState.Saving:
                BackToPauseMenuFromSaveUI();
                break;
            case PauseUiState.Loading:
                BackToPauseMenuFromLoadUI();
                break;
        }
    }
}
