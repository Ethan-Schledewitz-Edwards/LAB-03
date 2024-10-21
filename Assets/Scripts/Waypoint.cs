using UnityEngine;

[System.Serializable]
public class Waypoint
{
    [field: SerializeField] public Vector3 Pos { get; private set; }

    public Waypoint()
    {
        Pos = new Vector3(0,0,0);
    }

    public void SetPos(Vector3 newPos)
    {
        Pos = newPos;
    }
}
