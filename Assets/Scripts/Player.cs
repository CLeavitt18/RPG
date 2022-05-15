using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : LivingEntities
{
    [SerializeField] public static Player player;

    [SerializeField] private PlayerState Mode;

    [SerializeField] private int AttributePoints;
    [SerializeField] private int SprintSpeed;
    [SerializeField] private int StoredLevel;
    [Range(0, int.MaxValue), SerializeField] private int LevelProgress;
    [Range(8, int.MaxValue), SerializeField] private int requiredLevelProgress;

    [SerializeField] private LayerMask Interactiables;

    [SerializeField] private Transform ItemSpawn;

    [SerializeField] private bool FirstOpen = true;

    public void OnEnable()
    {
        player = this;
        Inventory.InventroyHolder = GameObject.Find("PlayerInventoryHolder").transform;

        if (SceneManager.GetActiveScene().name == "Test")
        {
            InventoryUi.playerUi.CallSetInventory(InventoryState.AllItems);
            return;
        }

        CallLoadPlayer();
    }

    public void Start()
    {
        if (WorldStateTracker.Tracker.FirstOpen)
        {
            for (int i = 0; i < 3; i++)
            {
                CalculateAttribute(i);
            }

            CalculateWeight();
        }

        WorldStateTracker.Tracker.FirstOpen = false;

        ResetPowers();
        StatusManger.RunCalculs();
        CalculateSpeed();

        for (int i = 0; i < 3; i++)
        {
            PlayerUi.playerUi.SetPlayerAttributeUI(i);
        }
    }

    public void Update()
    {
        float Translate = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        float Strafe = Input.GetAxis("Horizontal") * Time.deltaTime * (Speed * .35f);

        transform.Translate(Strafe, 0, Translate);

        if (Mode == PlayerState.Active)
        {
            for (int handType = 0; handType < 2; handType++)
            {
                if (Hands[handType].HeldItem != null && Hands[handType].HeldItem is TorchHolder == false)
                {
                    if (Hands[handType].State == AttackType.Melee)
                    {
                        if (Hands[handType].HasAttacked)
                        {
                            if (Input.GetButtonUp(GlobalValues.AttackInputs[handType]))
                            {
                                Hands[handType].HasAttacked = false;
                            }

                            return;
                        }

                        if (Input.GetButtonUp(GlobalValues.AttackInputs[handType]))
                        {
                            StartCoroutine(Attack(handType));
                            Hands[handType].HasAttacked = false;
                        }
                        else if (Input.GetButton(GlobalValues.AttackInputs[handType]))
                        {
                            ChargeAttack(handType);
                        }
                    }
                    else if (Hands[handType].State == AttackType.Ranged && Input.GetButton(GlobalValues.AttackInputs[handType]))
                    {
                        Shoot();
                    }
                    else
                    {
                        SpellHolder SpellH = Hands[handType].HeldItem.GetComponent<SpellHolder>();

                        int index = handType;

                        for (int i = 0; i < SpellH.Spells.Length; i++)
                        {
                            string key = GlobalValues.AttackInputs[index];
                            Spell spell = SpellH.Spells[i];

                            if (spell.CastType == CastType.Channelled)
                            {
                                if (Input.GetButton(key))
                                {
                                    Cast(handType, spell);
                                }
                                else if (Input.GetButtonUp(key))
                                {
                                    Hands[handType].ChannelTime = 0;
                                }
                            }

                            if ((spell.CastType == CastType.Instant || spell.CastType == CastType.Aura) && Input.GetButtonDown(key))
                            {
                                Cast(handType, spell);
                            }

                            if (spell.CastType == CastType.Charged)
                            {
                                if (Input.GetButtonDown(key))
                                {
                                    Cast(handType, false, spell);

                                }
                                else if (Input.GetButtonUp(key))
                                {
                                    Cast(handType, true, spell);
                                }
                            }

                            if (i == 0)
                            {
                                index += 2 + handType;
                            }
                            else
                            {
                                index++;
                            }

                        }
                    }
                }
            }

            Ray InteractionRay = new Ray(RaySpawn.position, RaySpawn.forward);

            if (Physics.Raycast(InteractionRay, out Hit, RayDistance, Interactiables) && Mode == PlayerState.Active)
            {
                GameObject objectHit = Hit.collider.gameObject;

                if (objectHit.CompareTag(GlobalValues.EnemyTag) || objectHit.CompareTag(GlobalValues.MinionTag))
                {
                    if (objectHit.GetComponent<AIController>().GetDead())
                    {
                        objectHit.GetComponent<Interactialbes>().SetUiOpen();
                    }

                    PlayerUi.playerUi.SetEnemyInfoUI(objectHit.GetComponent<AIController>());
                }
                else if (objectHit.CompareTag(GlobalValues.NPCTag))
                {
                    if (objectHit.GetComponent<AIController>().Mode == Behaviuor.Neutrel || objectHit.GetComponent<AIController>().GetDead())
                    {
                        objectHit.GetComponent<Interactialbes>().SetUiOpen();
                    }
                    else
                    {
                        PlayerUi.playerUi.SetEnemyInfoUI(objectHit.GetComponent<AIController>());
                    }
                }
                else
                {
                    objectHit.GetComponent<Interactialbes>().SetUiOpen();
                }

                if (Input.GetButtonDown("E"))
                {

                    if (objectHit.CompareTag(GlobalValues.EnemyTag) ||
                        objectHit.CompareTag(GlobalValues.MinionTag))
                    {
                        if (objectHit.GetComponent<AIController>().GetDead())
                        {
                            objectHit.GetComponent<IInteractable>().Interact(true);
                        }
                    }
                    else if (objectHit.CompareTag(GlobalValues.NPCTag))
                    {
                        if (objectHit.GetComponent<AIController>().Mode == Behaviuor.Neutrel ||
                            objectHit.GetComponent<AIController>().GetDead())
                        {
                            objectHit.GetComponent<IInteractable>().Interact(true);
                        }
                    }
                    else
                    {
                        objectHit.GetComponent<IInteractable>().Interact(true);
                    }
                }
            }

            if (Input.GetButtonDown("Shift") && GetCurrentStamina() >= RunningStaminaCost)
            {
                Speed += SprintSpeed;
                Running = true;
            }

            if (Input.GetButton("Shift") && GetCurrentStamina() >= RunningStaminaCost)
            {
                if (Time.time >= NextStaminaDegen)
                {
                    LoseAttribute(RunningStaminaCost, AttributesEnum.Stamina);

                    NextStaminaDegen = RunStaminaDegenRate + Time.time;
                    PlayerUi.playerUi.SetPlayerAttributeUI(1);
                }
            }

            if ((Input.GetButtonUp("Shift") || GetCurrentStamina() <= RunningStaminaCost) && Running)
            {
                Speed -= SprintSpeed;
                NextStaminaDegen = 0.0f;
                Running = false;
            }
        }

        if (Input.GetButtonDown("I"))
        {
            if (Mode == PlayerState.Active)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                Mode = PlayerState.InInventoy;

                InventoryUi.playerUi.SetInventroy(true);

            }
            else if (Mode == PlayerState.InJournal)
            {
                Mode = PlayerState.InInventoy;

                InventoryUi.playerUi.SetInventroy(true);
            }
            else if (Mode == PlayerState.InInventoy)
            {
                SetPlayerStateActive();
            }
        }

        if (Input.GetButtonDown("J"))
        {
            if (Mode == PlayerState.Active)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                Mode = PlayerState.InJournal;

                InventoryUi.playerUi.SetInventroy(true);
                PlayerUi.playerUi.CallSetQuestInventory();

            }
            else if (Mode == PlayerState.InInventoy)
            {
                Mode = PlayerState.InJournal;

                PlayerUi.playerUi.CallSetQuestInventory();
            }
        }

        if (Input.GetButtonDown("C"))
        {
            if (Mode == PlayerState.Active)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                Mode = PlayerState.InInventoy;

                InventoryUi.playerUi.SetInventroy(true);
                PlayerUi.playerUi.CallSetStats();
            }
            else if (Mode == PlayerState.InInventoy)
            {
                PlayerUi.playerUi.CallSetStats();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            switch (Mode)
            {
                case PlayerState.Active:
                    SetPlayerStatePaused();
                    break;
                case PlayerState.InInventoy:
                case PlayerState.InJournal:
                    SetPlayerStateActive();
                    break;
                case PlayerState.InContainer:
                case PlayerState.InStore:
                    Hit.collider.gameObject.GetComponent<IInteractable>().Interact(false);
                    SetPlayerStateActive();
                    break;
                case PlayerState.Paused:
                    PlayerUi.playerUi.ExitPauseMenu();
                    break;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 Direction = (RaySpawn.TransformDirection(Vector3.forward * RayDistance));
        Gizmos.DrawRay(RaySpawn.position, Direction);
    }

    public void SetMinionBehaviuors(LivingEntities Target)
    {
        for (int i = 0; i < Minions.Count; i++)
        {
            Minions[i].GetComponent<AIController>().Target = Target.gameObject.transform;
        }
    }

    public override int TakeDamage(DamageStats stats)
    {
        int TotalDamage = base.TakeDamage(stats);

        PlayerUi.playerUi.SetPlayerAttributeUI(0);

        if (GetCurrentHealth() <= 0)
        {
            Death();
            return TotalDamage;
        }

        if (Minions.Count != 0)
        {
            for (int i = 0; i < Minions.Count; i++)
            {
                (Minions[i].Controller as Minion).CheckAvailable(stats.Parent.transform);
            }
        }

        return TotalDamage;
    }

    public override bool GainAttribute(int Amount, AttributesEnum attribute)
    {
        if (base.GainAttribute(Amount, attribute))
        {
            PlayerUi.playerUi.SetPlayerAttributeUI((int)attribute);

            return true;
        }

        return false;
    }

    public override bool UnReserveAttribute(int Amount, AttributesEnum attribute)
    {
        if (base.UnReserveAttribute(Amount, attribute))
        {
            PlayerUi.playerUi.SetPlayerAttributeUI(((int)attribute));
            return true;
        }

        return false;
    }

    public override bool LoseAttribute(int amount, AttributesEnum attribute)
    {
        if (base.LoseAttribute(amount, attribute))
        {
            PlayerUi.playerUi.SetPlayerAttributeUI((int)attribute);
            return true;
        }

        return false;
    }

    public override bool ReserveAttribute(int amount, AttributesEnum attribute)
    {
        if (base.ReserveAttribute(amount, attribute))
        {
            PlayerUi.playerUi.SetPlayerAttributeUI((int)attribute);
            return true;
        }

        return false;
    }

    public override void CalculateSpeed()
    {
        base.CalculateSpeed();
    }

    void Death()
    {

    }

    public void GainExp(long Exp, int skill)
    {
        double TempExpNumber;

        Skills[skill].Exp += (ulong)Exp;

        switch ((SkillType)skill)
        {
            case SkillType.Blade:
            case SkillType.Blunt:
            case SkillType.Archery:
            case SkillType.Geomancy:
            case SkillType.Pyromancy:
            case SkillType.Astromancy:
            case SkillType.Cryomancy:
            case SkillType.Syromancy:
                GainMasteryExp(Exp);
                break;
            default:
                break;
        }

        if (Skills[skill].Exp >= Skills[skill].RExp)
        {
            Skills[skill].Level++;
            Skills[skill].Exp = Skills[skill].Exp - Skills[skill].RExp;

            TempExpNumber = Skills[skill].RExp * 1.4f;
            Skills[skill].RExp = (ulong)TempExpNumber;
            LevelProgress++;

            if (Skills[skill].Exp >= Skills[skill].RExp)
            {
                GainExp(0, skill);
                return;
            }

            switch ((SkillType)skill)
            {
                case SkillType.Blade:
                case SkillType.Blunt:

                    for (int i = 0; i < 2; i++)
                    {
                        if (Hands[i].HeldItem != null)
                        {
                            MeleeDamageMulti(i, DamageTypeEnum.Physical);
                        }
                    }

                    break;
                case SkillType.Archery:
                    break;
                case SkillType.Geomancy:

                    for (int i = 0; i < 2; i++)
                    {
                        if (Hands[i].HeldItem != null)
                        {
                            SpellDamageMulti(i, DamageTypeEnum.Physical);
                        }
                    }

                    break;
                case SkillType.Pyromancy:

                    for (int i = 0; i < 2; i++)
                    {
                        if (Hands[i].HeldItem != null)
                        {
                            MeleeDamageMulti(i, DamageTypeEnum.Fire);
                            SpellDamageMulti(i, DamageTypeEnum.Fire);
                        }
                    }

                    break;
                case SkillType.Astromancy:

                    for (int i = 0; i < 2; i++)
                    {
                        if (Hands[i].HeldItem != null)
                        {
                            MeleeDamageMulti(i, DamageTypeEnum.Lightning);
                            SpellDamageMulti(i, DamageTypeEnum.Lightning);
                        }
                    }

                    break;
                case SkillType.Cryomancy:

                    for (int i = 0; i < 2; i++)
                    {
                        MeleeDamageMulti(i, DamageTypeEnum.Ice);
                        SpellDamageMulti(i, DamageTypeEnum.Ice);
                    }


                    StatusManger.RunCalculs();
                    break;

                case SkillType.Syromancy:
                    break;
                case SkillType.HeavyArmour:
                    break;
                case SkillType.LightArmour:
                    break;
                case SkillType.Blocking:
                    break;
                case SkillType.Smithing:
                    break;
                case SkillType.Enchanting:
                    break;
                case SkillType.SpellCrafting:
                    break;
                case SkillType.Brewing:
                    break;
                case SkillType.Cooking:
                    break;
                default:
                    break;
            }
        }

        StatusManger.RunCalculs();

        CheckLevelProgress();
    }

    public void GainMasteryExp(long Exp)
    {
        double TempExpNumber;
        int mastery = (int)GetMastery();

        Masteries[mastery].Exp += (ulong)Exp;

        if (Masteries[mastery].Exp >= Masteries[mastery].RExp)
        {
            Masteries[mastery].Level++;
            Masteries[mastery].Exp = Masteries[mastery].Exp - Masteries[mastery].RExp;

            TempExpNumber = Masteries[mastery].RExp * 1.4;
            Masteries[mastery].RExp = (ulong)TempExpNumber;

            LevelProgress++;

            if (Masteries[mastery].Exp >= Masteries[mastery].RExp)
            {
                GainMasteryExp(0);
                return;
            }

            CheckLevelProgress();
        }
    }

    private void CheckLevelProgress()
    {
        if (LevelProgress >= requiredLevelProgress)
        {
            LevelProgress -= requiredLevelProgress;
            StoredLevel++;
            PlayerUi.playerUi.CallSetLevelCounter(StoredLevel);

            requiredLevelProgress = (int)(requiredLevelProgress * 1.15f);
        }
    }

    public void AddAPoints(int amount)
    {
        AttributePoints += amount;
    }

    public void SubAPoints(int amount)
    {
        AttributePoints -= amount;
    }

    public void SubStoredLevel(int amount)
    {
        StoredLevel -= amount;
    }

    public void SetPlayerStateActive()
    {
        Mode = PlayerState.Active;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;

        PlayerUi.playerUi.Close();
    }

    public void SetPlayerStateInContainer()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;
        Mode = PlayerState.InContainer;
    }

    public void SetPlayerStatePaused()
    {
        Mode = PlayerState.Paused;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;

        PlayerUi.playerUi.StartPause(true);
    }

    public void SetPlayerStatInStore()
    {
        Mode = PlayerState.InStore;
    }

    public void SetPlayerStateSleeping()
    {
        Mode = PlayerState.Sleeping;
    }

    protected override void CalculateAttribute(int attribute)
    {
        base.CalculateAttribute(attribute);
    }

    public void UpDatePlayerState()
    {
        SetPlayerStateActive();

        for (int i = 0; i < 3; i++)
        {
            CalculateAttribute(i);
        }

        StatusManger.RunCalculs();

        for (int i = 0; i < 3; i++)
        {
            PlayerUi.playerUi.SetPlayerAttributeUI(i);
        }
    }

    public void LevelUp(int[] abilities)
    {
        int level = GetLevel();

        for (int i = 0; i < abilities.Length; i++)
        {
            SetAbility(i, abilities[i] + GetAbility(i));
        }

        SetLevel(level + 1);
        SetPlayerStateActive();

        for (int i = 0; i < 3; i++)
        {
            CalculateAttribute(i);
        }

        for (int i = 0; i < 2; i++)
        {
            if (Hands[i].HeldItem == null)
            {
                continue;
            }

            IEquipable item = Hands[i].HeldItem.GetComponent<IEquipable>();


            if (item is WeaponHolder weapon)
            {
                for (int x = 0; x < weapon.DamageRanges.Count; x++)
                {
                    MeleeDamageMulti(i, weapon.DamageRanges[x].Type);
                }
            }
            else //Item is SpellHolder
            {
                SpellHolder spell = item as SpellHolder;

                for (int x = 0; x < spell.Spells.Length; x++)
                {
                    if (spell.Spells[x] is DamageSpell dSpell)
                    {
                        for (int y = 0; y < dSpell.DamageRanges.Count; y++)
                        {
                            SpellDamageMulti(i, dSpell.DamageRanges[y].Type);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < Skills.Length; i++)
        {
            float temp = 1 + level * .20f;

            Skills[i].RExp = (ulong)(Skills[i].RExp * temp);
            Skills[i].Exp = (ulong)(Skills[i].Exp * temp);
        }

        for (int i = 0; i < Masteries.Length; i++)
        {
            float temp = 1 + level * .20f;

            Masteries[i].RExp = (ulong)(Masteries[i].RExp * temp);
            Masteries[i].Exp = (ulong)(Masteries[i].Exp * temp);
        }
    }

    public void SavePlayer(bool mode)
    {
        //Mode true is save
        //Mode false is tempSave

        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');

        if (WorldStateTracker.Tracker.SaveProfile == string.Empty)
        {
            WorldStateTracker.Tracker.SaveProfile = WorldStateTracker.Tracker.PlayerName;
        }

        path.Append(WorldStateTracker.Tracker.SaveProfile);

        StringBuilder MiniPath = new StringBuilder(path.ToString());

        MiniPath.Append('/');
        MiniPath.Append(GlobalValues.MinionFolder);

        if (Directory.Exists(MiniPath.ToString()))
        {
            string[] paths;

            if (mode)
            {
                paths = Directory.GetFiles(MiniPath.ToString());
            }
            else
            {
                paths = Directory.GetFiles(MiniPath.ToString(), "*.temp");
            }

            if (paths.Length != 0)
            {
                foreach (string filePath in paths)
                {
                    File.Delete(filePath);
                }
            }

            if (Minions.Count == 0 && mode)
            {
                Directory.Delete(MiniPath.ToString());
            }
            else
            {
                StringBuilder tempPath = new StringBuilder();

                for (int i = 0; i < Minions.Count; i++)
                {
                    tempPath.Append(MiniPath.ToString());
                    tempPath.Append('/');
                    tempPath.Append(Minions[i].Name);
                    tempPath.Append(i);

                    if (mode)
                    {
                        tempPath.Append(GlobalValues.SaveExtension);
                    }
                    else
                    {
                        tempPath.Append(GlobalValues.TempExtension);
                    }

                    SaveSystem.SaveMinion(Minions[i], tempPath.ToString());

                    tempPath.Clear();
                }
            }
        }
        else if (Minions.Count != 0)
        {
            Directory.CreateDirectory(MiniPath.ToString());

            StringBuilder tempPath = new StringBuilder();

            for (int i = 0; i < Minions.Count; i++)
            {
                tempPath.Append(MiniPath.ToString());
                tempPath.Append('/');
                tempPath.Append(Minions[i].Name);
                tempPath.Append(i);

                if (mode)
                {
                    tempPath.Append(GlobalValues.SaveExtension);
                }
                else
                {
                    tempPath.Append(GlobalValues.TempExtension);
                }

                SaveSystem.SaveMinion(Minions[i], tempPath.ToString());

                tempPath.Clear();
            }
        }

        path.Append(GlobalValues.PlayerFolder);

        if (mode)
        {
            path.Append(GlobalValues.SaveExtension);
        }
        else
        {
            path.Append(GlobalValues.TempExtension);
        }

        SaveSystem.SavePlayer(path.ToString());
    }

    public void CallLoadPlayer()
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);

        if (Directory.Exists(path.ToString()) && WorldStateTracker.Tracker.FirstOpen == false)
        {
            LoadPlayer();
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                CalculateAttribute(i);
            }

            WorldStateTracker.Tracker.CallSaveGame();
        }
    }

    private void LoadPlayer()
    {
        StringBuilder path = new StringBuilder(Application.persistentDataPath);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.PlayerName);
        path.Append('/');
        path.Append(WorldStateTracker.Tracker.SaveProfile);

        StringBuilder MinionPath = new StringBuilder(path.ToString());

        path.Append(GlobalValues.PlayerFolder);

        StringBuilder TempPath = new StringBuilder(path.ToString());
        TempPath.Append(GlobalValues.TempExtension);

        path.Append(GlobalValues.SaveExtension);

        PlayerData Data;

        bool mode;

        if (File.Exists(TempPath.ToString()))
        {
            Data = SaveSystem.LoadPlayer(TempPath.ToString());
            mode = false;
        }
        else
        {
            Data = SaveSystem.LoadPlayer(path.ToString());
            mode = true;
        }

        for (int i = 0; i < 3; i++)
        {
            SetAbility(i, Data.Attributes[i].Ability);
        }

        LoadEntity(Data);

        if (Minions.Count != 0)
        {
            int count = Minions.Count;

            for (int i = 0; i < count; i++)
            {
                Destroy(Minions[i].transform.parent.gameObject);
            }

            Minions.Clear();
        }

        MinionPath.Append(GlobalValues.MinionFolder);

        if (Directory.Exists(MinionPath.ToString()))
        {
            string[] files = Directory.GetFiles(MinionPath.ToString(), "*.temp");

            if (files.Length == 0 && mode)
            {
                files = Directory.GetFiles(MinionPath.ToString(), "*.save");
            }

            if (files.Length != 0)
            {
                foreach (string file in files)
                {
                    MinionData data = SaveSystem.LoadMinion(file);

                    GameObject minionH = Instantiate(PrefabIDs.prefabIDs.Minions[data.Id]);

                    Minion minion = minionH.transform.GetChild(0).GetComponent<Minion>();

                    minion.Owner = this;
                    minion.SourceSpell = (GolemSpell)Hands[data.HandSource].HeldItem.GetComponent<SpellHolder>().Spells[data.SourceId];

                    minion.LoadMinion(data);

                    Minions.Add(minion.entity);
                }
            }
        }

        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        Vector3 Position;
        Vector3 CRotation;
        Vector3 Rotation;

        AttributePoints = Data.AttributePoints;
        LevelProgress = Data.LevelProgress;
        requiredLevelProgress = Data.requiredLevelProgress;
        StoredLevel = Data.StoredLevel;

        if (StoredLevel != 0)
        {
            PlayerUi.playerUi.CallSetLevelCounter(StoredLevel);
        }

        Position.x = Data.Position[0];
        Position.y = Data.Position[1];
        Position.z = Data.Position[2];

        transform.position = Position;

        Rotation.x = Data.Rotation[0];
        Rotation.y = Data.Rotation[1];
        Rotation.z = Data.Rotation[2];

        Rotation = Vector3.MoveTowards(transform.rotation.eulerAngles, Rotation, 360);
        transform.rotation = Quaternion.Euler(Rotation);

        CRotation.x = Data.CRotation[0];
        CRotation.y = Data.CRotation[1];
        CRotation.z = Data.CRotation[2];

        CRotation = Vector3.MoveTowards(transform.GetChild(0).rotation.eulerAngles, CRotation, 360);
        transform.GetChild(0).rotation = Quaternion.Euler(CRotation);

        if (Minions.Count != 0 && WorldStateTracker.Tracker.GoingToNewLevel)
        {
            transform.Translate(transform.forward + new Vector3(0, 0, 4));

            for (int i = 0; i < Minions.Count; i++)
            {
                Vector3 vec = transform.position - (transform.forward + new Vector3(0, 0, -1));

                (Minions[i].Controller as Minion).Move(vec);
            }
        }

        transform.GetChild(0).gameObject.GetComponent<PlayerCamera>().MouseLook = new Vector2(Rotation.y, -CRotation.x);

        for (int i = 0; i < Masteries.Length; i++)
        {
            Masteries[i].Level = Data.Masteries[i].Level;
            Masteries[i].Exp = Data.Masteries[i].Exp;
            Masteries[i].RExp = Data.Masteries[i].RExp;
        }

        for (int i = 0; i < Skills.Length; i++)
        {
            Skills[i].Level = Data.Skills[i].Level;
            Skills[i].Exp = Data.Skills[i].Exp;
            Skills[i].RExp = Data.Skills[i].RExp;
        }

        for (int i = 0; i < 3; i++)
        {
            PlayerUi.playerUi.SetPlayerAttributeUI(i);
        }

        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        if (Data.NumOfQuests > 0)
        {
            for (int i = 0; i < Data.NumOfQuests; i++)
            {
                GameObject Item = Instantiate(PrefabIDs.prefabIDs.Quests[Data.Quests[i].QuestId], Inventory.InventroyHolder);

                LoadSystem.LoadItem(Data.Quests[i], Item.GetComponent<QuestHolder>());

                QuestTracker.questTracker.AddQuestOnLoad(Item.GetComponent<QuestHolder>());
            }
        }

        CalculateSpeed();
        CalculateWeight();

        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.Mode);
    }

    #region Getters

    public int GetLevelProgress()
    {
        return LevelProgress;
    }

    public int GetRequiredLevelProgress()
    {
        return requiredLevelProgress;
    }

    public PlayerState GetMode()
    {
        return Mode;
    }

    public int GetAPoints()
    {
        return AttributePoints;
    }

    public int GetStoredLevels()
    {
        return StoredLevel;
    }

    public Vector3 GetItemSpawn()
    {
        return ItemSpawn.position;
    }

    public bool GetFirstOpen()
    {
        return FirstOpen;
    }
    #endregion
}
