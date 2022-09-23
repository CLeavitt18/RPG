using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public static NPCDialogue npcD;

    [SerializeField] private int CurrentSpeachBranch;
    [SerializeField] private int CurrentQuest;

    public NPCChatState Mode = NPCChatState.None;

    public GameObject DialogueBox;
    public GameObject StoreButton;
    public GameObject LeaveButton;
    public GameObject QuestButton;
    public GameObject CompleteQuestButton;
    public GameObject ContinueButton;
    public GameObject Accept;
    public GameObject Reject;
    public GameObject ContainerUi;

    [SerializeField] private NPC TargetNPC;

    [SerializeField] private int NextDialogue = 0;

    public QuestHolder[] Quests;

    [SerializeField] private DialogueSet[] NPCSpeach;
    [SerializeField] private DialogueSet VendorSpeach;

    public string[] SpeachOptions;

    public Text NPCDialogueText;

    public Text[] SpeachOptionsText;

    public void OnEnable()
    {
        npcD = this;
    }

    public void StartDialogue(bool State)
    {
        DialogueBox.SetActive(State);

        if (State == false)
        {
            StoreButton.SetActive(true);
            LeaveButton.SetActive(true);

            ContinueButton.SetActive(false);
            Accept.SetActive(false);
            Reject.SetActive(false);

            StartCoroutine(CheckForInput());
        }
        else
        {
            bool QuestComplete = false;
            bool PlayerHasQuest = false;

            Cursor.visible = State;
            Cursor.lockState = CursorLockMode.None;

            TargetNPC = Player.player.GetHit().GetComponent<NPC>();
            CurrentQuest = TargetNPC.CurrentQuest;
            Quests = TargetNPC.GetQuests();
            NPCSpeach = TargetNPC.GetDialogue();
            VendorSpeach = TargetNPC.GetVendorSpeach();
            CurrentSpeachBranch = TargetNPC.CurrentSpeachBranch;
            NPCDialogueText.text = NPCSpeach[CurrentSpeachBranch].Dialogue[0];

            for (int i = 0; i < Quests.Length; i++)
            {
                for (int x = 0; x < QuestTracker.questTracker.Quests.Count; x++)
                {
                    if (Quests[i].Equals(QuestTracker.questTracker.Quests[x]))
                    {
                        PlayerHasQuest = true;
                        
                        if (QuestTracker.questTracker.Quests[x].GetComplete())
                        {
                            QuestButton.SetActive(true);
                            QuestComplete = true;
                            break;
                        }
                    }
                }
            }

            NextDialogue = 0;
            
            if (CurrentQuest == Quests.Length || (PlayerHasQuest && !QuestComplete))
            {
                QuestButton.SetActive(false);
            }
            else if (!QuestComplete)
            {
                QuestButton.SetActive(!PlayerHasQuest);
            }
        }
    }

    private IEnumerator CheckForInput()
    {
        while(!Input.GetButtonUp("Fire1"))
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        Player.player.SetPlayerStateActive();

        yield break;
    }

    public void SetNPCQuest()
    {
        if (CurrentSpeachBranch > CurrentQuest)
        {
            Mode = NPCChatState.TurnInQuest;
        }
        else
        {
            Mode = NPCChatState.Quest;
            ContinueButton.SetActive(true);
        }

        QuestButton.SetActive(false);
        StoreButton.SetActive(false);

        ContinueDialogue();
    }

    public void CompleteQuest()
    {
        QuestReward[] reward = Quests[CurrentQuest].GetReward();

        for (int i = 0; i < reward.Length; i++)
        {
            for (int x = 0; x < reward[i].Rewards.Length; x++)
            {
                Item Item = Instantiate(reward[i].Rewards[x]);
                Item.SetAmount(reward[i].Amount[x]);

                Player.player.Inventory.AddItem(Item, true, Item.GetAmount());
                Destroy(Item);
            }
        }

        Inventory Inventory = Player.player.Inventory;

        for (int i = 0; i < Quests[CurrentQuest].QuestItems.Length; i++)
        {
            QuestItemCompleteConditon item = Quests[CurrentQuest].QuestItems[i];

            Inventory.RemoveItem(item.item.GetName(), item.amount, GlobalValues.MiscTag);
        }

        QuestTracker.questTracker.RemoveQuest(Quests[CurrentQuest]);

        TargetNPC.GetComponent<NPC>().CurrentQuest++;
        CurrentQuest++;

        NextDialogue = 0;

        Mode = NPCChatState.None;

        SetQuest();
    }

    public void SetNPCStore()
    {
        Mode = NPCChatState.Store;
        Player.player.SetPlayerStatInStore();

        QuestButton.SetActive(false);
        StoreButton.SetActive(false);
        ContinueButton.SetActive(true);

        ContinueDialogue();
    }

    public void SetNPCTraining()
    {
        Mode = NPCChatState.Training;
        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (Mode == NPCChatState.Store)
        {
            if (NPCDialogueText.text != VendorSpeach.Dialogue[0])
            {
                NPCDialogueText.text = VendorSpeach.Dialogue[0];
                return;
            }
            
            if (NPCDialogueText.text == VendorSpeach.Dialogue[0])
            {
                TargetNPC.GetComponent<Containers>().Interact(true);

                Player.player.SetPlayerStatInStore();

                InventoryUi.playerUi.CallSetInventory(InventoryState.AllItems);
                DialogueBox.SetActive(false);
                ContinueButton.SetActive(false);

                return;
            }
        }

        NextDialogue++;

        if (Mode == NPCChatState.TurnInQuest && NextDialogue == NPCSpeach[CurrentSpeachBranch].Dialogue.Length - 2)
        {
            Debug.Log("Branch " + CurrentSpeachBranch + " dialogue " + NextDialogue);
            NPCDialogueText.text = NPCSpeach[CurrentSpeachBranch].Dialogue[NextDialogue];
            SetButtons(true);
            return;
        }

        if (Mode == NPCChatState.Quest)
        {
            if (NextDialogue == NPCSpeach[CurrentSpeachBranch].Dialogue.Length - 3)
            {
                SetButtons(true);
            }

            if (NextDialogue == NPCSpeach[CurrentSpeachBranch].Dialogue.Length - 2)
            {
                NextDialogue++;
                SetButtons(false);
            }
        }

        NPCDialogueText.text = NPCSpeach[CurrentSpeachBranch].Dialogue[NextDialogue];
    }

    public void SetButtons(bool State)
    {
        if (Mode == NPCChatState.TurnInQuest)
        {
            CompleteQuestButton.SetActive(true);
            return;
        }

        StoreButton.SetActive(!State);
        LeaveButton.SetActive(!State);
        QuestButton.SetActive(false);
        ContinueButton.SetActive(false);
        CompleteQuestButton.SetActive(false);
        Accept.SetActive(State);
        Reject.SetActive(State);
    }

    public void SetQuest()
    {
        if (Mode == NPCChatState.Quest)
        {
            QuestTracker.questTracker.AddQuest(Quests[CurrentQuest]);
            NPCDialogueText.text = NPCSpeach[CurrentSpeachBranch].Dialogue[NPCSpeach[CurrentSpeachBranch].Dialogue.Length - 2];
        }
        else
        {
            NPCDialogueText.text = NPCSpeach[CurrentSpeachBranch].Dialogue[NPCSpeach[CurrentSpeachBranch].Dialogue.Length - 1];
            CompleteQuestButton.SetActive(false);
            ContinueButton.SetActive(false);
        }

        StoreButton.SetActive(true);
        LeaveButton.SetActive(true);
        Accept.SetActive(false);
        Reject.SetActive(false);

        TargetNPC.GetComponent<NPC>().CurrentSpeachBranch++;
        CurrentSpeachBranch++;
    }
}
