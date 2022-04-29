using UnityEngine;

[System.Serializable]
public class DepositData
{
    public int DepositId;

    public bool IsDead;

    public float[] position = new float[3];
    public float[] Rotation = new float[3];

    public DepositData(GameObject Deposit)
    {
        for (int i = 0; i < PrefabIDs.prefabIDs.Deposits.Length; i++)
        {
            if (PrefabIDs.prefabIDs.Deposits[i].name == Deposit.name)
            {
                DepositId = i;
                break;
            }
        }

        position[0] = Deposit.transform.position.x;
        position[1] = Deposit.transform.position.y;
        position[2] = Deposit.transform.position.z;

        Rotation[0] = Deposit.transform.rotation.eulerAngles.x;
        Rotation[1] = Deposit.transform.rotation.eulerAngles.y;
        Rotation[2] = Deposit.transform.rotation.eulerAngles.z;

        IsDead = Deposit.GetComponent<ResourceDeposit>().IsDead;
    }
}
