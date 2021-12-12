using UnityEngine;

public abstract class Identifiable : ScriptableObject
{
    public int ID = -1;

    public void OnLoad() {}
}
