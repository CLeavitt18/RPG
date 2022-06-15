using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    #region Test 2
    public int openginDirection;
    //0 --> needs a Top door
    //90 --> needs a right door
    //180 --> needs a bottom door
    //270 --> needs a left door

    public Generator gen;
    public bool _spawned = false;

    private void OnEnable()
    {
        gen = GameObject.Find("Generator").GetComponent<Generator>();
        Invoke("Spawn", .1f);
    }

    private void Spawn()
    {
        if (_spawned)
        {
            return;
        }

        int rand;

        switch (openginDirection)
        {
            case 0:
                rand = Random.Range(0, gen.BottomRooms.Length);
                Instantiate(gen.BottomRooms[rand], transform.position, Quaternion.identity);
                break;
            case 90:
                rand = Random.Range(0, gen.LeftRooms.Length);
                Instantiate(gen.LeftRooms[rand], transform.position, Quaternion.identity);
                break;
            case 180:
                rand = Random.Range(0, gen.TopRooms.Length);
                Instantiate(gen.TopRooms[rand], transform.position, Quaternion.identity);
                break;
            case 270:
                rand = Random.Range(0, gen.RightRooms.Length);
                Instantiate(gen.RightRooms[rand], transform.position, Quaternion.identity);
                break;
        }

        _spawned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpawnPoint"))
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
