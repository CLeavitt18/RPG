using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    bool IsEquiped { get; set; }

    SkillType SkillType { get; set; }
}
