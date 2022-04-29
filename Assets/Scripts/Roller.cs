using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    public static Roller roller;

    public Catalyst[] Cats;

    public WeaponRoller weaponRoller;
    public ArmourRoller armourRoller;
    public SpellRoller spellRoller;
    public RuneRoller runeRoller;
    public PotionRolller potionRolller;
    public ResourceRoller resourceRoller;
    public MiscRoller miscRoller;

    private void OnEnable()
    {
        if (roller == null)
        {
            roller = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
