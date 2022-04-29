using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    static FPSCounter Counter;

    public Text Text;

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
        else
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
