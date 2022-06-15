using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{
    [SerializeField] private int _numOfExits;
    [SerializeField] private bool[] _isUsed;

    [SerializeField] private Transform[] _exits;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale * 10);

        foreach (Transform point in _exits)
        {
            Gizmos.DrawRay(point.position, point.forward * 5);
            
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireCube(point.position, point.localScale);

            Gizmos.color = Color.yellow;
        }

    }*/

    public void BuildNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }

    public void SetUsed(int id)
    {
        _isUsed[id] = true;
    }

    public int GetNumOfExits()
    {
        return _numOfExits;
    }

    public Transform GetExit(int id)
    {
        if (_isUsed[id])
        {
            return null;
        }

        return _exits[id];
    }
}
