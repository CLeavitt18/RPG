using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DictionaryOfStringAndInt : SerializableDictionary<string, int>
{
    public DictionaryOfStringAndInt(int capacity)
    {
        this.EnsureCapacity(capacity);
    }
}
