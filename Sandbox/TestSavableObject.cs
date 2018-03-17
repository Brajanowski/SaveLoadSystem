using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB.SaveLoadSystem;

[Serializable]
public class TestData
{
    [SerializeField]
    public int test = 911;

    [SerializeField]
    public float pi = 3.14f;
}

public class TestSavableObject : MonoBehaviour
{
    [Savable]
    public float pi = 3.14f;

    [Savable]
    public Color barrelColor = Color.red;

    [Savable]
    public Vector3 position = Vector3.up;

    public Vector2 down = Vector2.down;

    public int dontSaveThis = 1337;

    [Savable]
    public TestData testData = new TestData();
}
