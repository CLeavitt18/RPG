using UnityEngine;

public class DayNight : MonoBehaviour
{
    public int DayDuration;

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, DayDuration * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }
}
