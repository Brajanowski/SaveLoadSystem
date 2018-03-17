using UnityEngine;
using BB.SaveLoadSystem;

public class SaveLoadOnKeys : MonoBehaviour
{
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    private void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            SaveLoadManager.Instance.Save("TestSave.save");
        }

        if (Input.GetKeyDown(loadKey))
        {
            SaveLoadManager.Instance.Load("TestSave.save");
        }
    }
}
