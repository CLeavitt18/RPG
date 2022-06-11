using UnityEngine;
using UnityEngine.AI;

public class Generator : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;

    [SerializeField] private Tile _hallWay;

    [SerializeField] private Tile[] _starts;
    [SerializeField] private Tile[] _rooms;
    [SerializeField] private Tile[] _corridors;
    [SerializeField] private Tile[] _bossRooms;
    [SerializeField] private Tile[] _ends;

    [SerializeField] private long _seed;

    private void OnEnable() 
    {
        Generate();
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
        
        int[] digits = new int[seedString.Length];

        int intArrayId = seedString.Length - 1;

        for (int i = 0; i < seedString.Length; i++)
        {
            digits[i] = (int)char.GetNumericValue(seedString[i]);
            intArrayId--;
        } 

        for (int i = 0; i < digits.Length; i++)
        {
            Debug.Log("Digit: " + i + " = " + digits[i]);
        }

        Tile currentTile = Instantiate(_starts[digits[0] - 1], _startPosition.position, _startPosition.rotation);

        for (int i = 0; i < 2; i++)
        {
            Tile HallyWay = Instantiate(_hallWay, currentTile.GetExit(i).position, currentTile.GetExit(i).rotation);

            Vector3 offSet = currentTile.GetExit(i).position - HallyWay.GetExit(i).position;

            HallyWay.transform.position += offSet;
            
            currentTile = HallyWay;
        }

        currentTile.BuildNavMesh();
    }

    //first number is the starting tile id
    //second number is the amount of rooms tpyes to generate

    //Going in set of 2s fisrt number represents the room id, second is the amount of rooms of that type

    //third number is the room tile id
    //fourth number is the amount of 
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
        }
        
        seed *= 10;

        seed += Random.Range(1, _ends.Length + 1);

        return seed;
    }
}
