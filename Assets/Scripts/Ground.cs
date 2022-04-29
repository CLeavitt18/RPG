using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Ground : MonoBehaviour
{
    public NavMeshSurface NavMesh;

    public void OnEnable()
    {
        NavMesh.BuildNavMesh();
    }
    public void CallBakeNavMeshSurface()
    {
        StartCoroutine(BakeNavMeshSurface());
    }

    IEnumerator BakeNavMeshSurface()
    {
        yield return new WaitForEndOfFrame();
        NavMesh.BuildNavMesh();
    }
}
