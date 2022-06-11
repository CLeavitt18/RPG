using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{
    [SerializeField] private int numOfExits;
    [SerializeField] private Transform[] _exits;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (Transform point in _exits)
        {
            Gizmos.DrawRay(point.position, point.forward * 5);
        }

        Gizmos.DrawWireCube(transform.position, transform.localScale * 5);
    }

    public void BuildNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }

    public int GetNumOfExits()
    {
        return numOfExits;
    }

    public Transform GetExit(int id)
    {
        return _exits[id];
    }
}
