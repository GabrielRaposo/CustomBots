using UnityEngine;

[System.Serializable]
public class UpgradeInstall
{
    public string name;
    public GameObject prefab;
    [TextArea(3,5)] public string description;
}
