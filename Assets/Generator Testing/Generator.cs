using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Generator : MonoBehaviour
{
    #region Test 1
    /*[SerializeField] private Transform _startPosition;

    [SerializeField] private Tile[] _starts;
    [SerializeField] private Tile[] _rooms;
    [SerializeField] private Tile[] _corridors;
    [SerializeField] private Tile[] _bossRooms;
    [SerializeField] private Tile[] _ends;

    [SerializeField] private long _seed;

    private void OnEnable()
    {
        Generate(_seed);
    }

    private void Generate(long seed = 0)
    {
        if (seed == 0)
        {
            seed = CreateSeed();
        }

        Debug.Log("Seed: " + seed);

        _seed = seed;

        string seedString = seed.ToString();

        List<int> digits = new List<int>();

        for (int i = 0; i < seedString.Length; i++)
        {
            digits.Add((int)char.GetNumericValue(seedString[i]));
        }

        for (int i = 0; i < digits.Count; i++)
        {
            Debug.Log("Digit: " + i + " = " + digits[i]);
        }

        List<Tile> tiles = new List<Tile>();

        Tile currentTile = Instantiate(_starts[digits[0] - 1], _startPosition.position, _startPosition.rotation);

        digits.RemoveAt(0);

        tiles.Add(currentTile);

        int amountOfRoomIds = digits[0];

        digits.RemoveAt(0);

        for (int i = 0; i < amountOfRoomIds; i++)
        {
            int roomId = digits[0];

            digits.RemoveAt(0);

            int roomAmount = digits[0];

            digits.RemoveAt(0);

            for (int x = 0; x < roomAmount; x++)
            {
                Tile room = Instantiate(_rooms[roomId]);

                tiles.Add(room);
                
                int numOfExits = currentTile.GetNumOfExits();

                Orinantate(currentTile, room, numOfExits);

                Tile corridor = Instantiate(_corridors[digits[0]]);
                
                numOfExits = corridor.GetNumOfExits();

                tiles.Add(corridor);

                Orinantate(room, corridor, numOfExits);

                currentTile = corridor;

            }
            
            digits.RemoveAt(0);
        }

        /*Transform tileTransform = currentTile.GetExit(0);

        for (int i = 0; i < 2; i++)
        {
            Tile HallWay = Instantiate(_rooms[i]);

            tiles.Add(HallWay);

            int numOfExits = currentTile.GetNumOfExits();

            Orinantate(currentTile, HallWay, numOfExits);

            currentTile = HallWay;
        }*/
/*
        currentTile.BuildNavMesh();
    }

    //first number is the starting tile id
    //second number is the amount of rooms tpyes to generate

    //Going in set of 2s fisrt number represents the room id, second is the amount of rooms of that type

    private long CreateSeed()
    {
        //starting tile id
        long seed = Random.Range(1, _starts.Length + 1);

        //amount of room types
        long amount = Random.Range(2, 5);

        seed *= 10;
        seed += amount;

        for (long i = 0; i < amount; i++)
        {
            seed *= 10;
            //room type id
            seed += Random.Range(0, _rooms.Length);

            seed *= 10;
            //amount of rooms of that type
            seed += Random.Range(1, 4);

            seed *= 10;
            //Corridor id
            seed += Random.Range(0, _corridors.Length);
        }

        seed *= 10;

        seed += Random.Range(1, _ends.Length + 1);

        return seed;
    }

    private void Orinantate(Tile currentTile, Tile nextTile, int numOfExits)
    {
        Transform exit = null;

        Debug.Log("Num of Exits: " + numOfExits);

        for (int j = 0; j < numOfExits; j++)
        {
            if ((exit = currentTile.GetExit(j)) != null)
            {
                float rotattionOffSet = exit.rotation.eulerAngles.y - 360.0f;

                nextTile.transform.Rotate(new Vector3(0, rotattionOffSet, 0));

                Vector3 offSet = exit.position - nextTile.GetExit(0).position;

                nextTile.transform.position += offSet;

                currentTile.SetUsed(j);
                nextTile.SetUsed(0);

                break;
            }
        }
    }*/
    #endregion
    #region Test 2
    public Tile[] BottomRooms;
    public Tile[] TopRooms;
    public Tile[] LeftRooms;
    public Tile[] RightRooms;
    #endregion
}
