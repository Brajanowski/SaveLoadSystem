using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB.SaveLoadSystem;

[System.Serializable]
public class CustomData
{
    public string name = "John";
    public int energy = 100;
}

public class Entity : MonoBehaviour
{
    // If we want to have our variable saved, we must add Savable attribute. Yea, it's that simple
    [Savable]
    public int startingHealth = 100;

    // It works even on private variables.
    [Savable]
    private int health = 100;

    // It works also on custom types
    [Savable]
    public CustomData additionalData = new CustomData();
}
