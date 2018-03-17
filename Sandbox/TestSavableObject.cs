using System;
using UnityEngine;
using BB.SaveLoadSystem;

[Serializable]
public class TestData
{
    public int test = 911;
    public float pi = 3.14f;
    public string stringTest = "Siema";
}

public class TestSavableObject : MonoBehaviour
{
    [Savable]
    public float pi = 3.14f;

    [Savable]
    public Color barrelColor = Color.red;

    [Savable]
    public Vector3 position = Vector3.up;

    [Savable]
    public Vector2 down = Vector2.down;

    public int dontSaveThis = 1337;

    [Savable]
    public TestData testData = new TestData();
}
