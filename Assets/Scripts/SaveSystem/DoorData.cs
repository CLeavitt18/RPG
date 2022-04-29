using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorData
{
    public bool Locked;

    public DoorData(Doors Door)
    {
        Locked = Door.Locked;
    }
}
