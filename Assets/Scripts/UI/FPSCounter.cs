using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public static FPSCounter Counter;

    public TextMeshProUGUI Text;

    public int FPS;

    private float WaitIntervale = 1.0f;
    private float NextUpdate;
    public void OnEnable()
    {
        if (Counter == null)
        {
            Counter = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Counter != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        FPS++;
        if (Time.realtimeSinceStartup >= NextUpdate)
        {
            NextUpdate = Time.realtimeSinceStartup + WaitIntervale;
            Text.text = "FPS: " + FPS;
            FPS = 0;
        }
    }
}
