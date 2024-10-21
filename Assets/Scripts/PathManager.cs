using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector, SerializeField] public List<Waypoint> Path;
    [SerializeField] private GameObject _prefab;
    private int currentPointIndex;

    private List<GameObject> prefabPoints = new List<GameObject>();

    private void Start()
    {
        foreach (var i in Path)
        {
            GameObject instance = Instantiate(_prefab);
            instance.transform.position = i.Pos;
            prefabPoints.Add(instance);
        }
    }

    private void Update()
    {
        for (int i = 0; i < Path.Count; i++)
        {
            Waypoint p = Path[i];
            GameObject g = prefabPoints[i];
            g.transform.position = p.Pos;
        }
    }

    public List<Waypoint> GetPath()
    {
        if (Path == null)
            Path = new List<Waypoint>();

        return Path;
    }

    public void CreateAddPoint()
    {
        Waypoint go = new Waypoint();
        Path.Add(go);
    }

    public Waypoint GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex + 1) % (Path.Count);
        currentPointIndex = nextPointIndex;
        return Path[nextPointIndex];
    }
}
