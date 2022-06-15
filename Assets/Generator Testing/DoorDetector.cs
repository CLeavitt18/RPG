using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(other.gameObject);
        }       
    }
}
