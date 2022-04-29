using UnityEngine;
using UnityEngine.UI;

public class DoubleClickAction : MonoBehaviour
{
    private bool Clicked = false;

    private float NextTime;
    private float WaitTime = 1f;

    private Button Me;

    protected void OnEnable()
    {
        Me = GetComponent<Button>();
        Me.onClick.AddListener(OnClickListener);
    }

    protected void OnClickListener()
    {
        if (Time.realtimeSinceStartup >= NextTime)
        {
            Clicked = false;
        }

        if (Clicked)
        {
            DoubleAction();
        }
        else
        {
            Clicked = true;
            NextTime = Time.realtimeSinceStartup + WaitTime;

            SingleAction();
        }
    }

    protected virtual void SingleAction()
    {

    }

    protected virtual void DoubleAction()
    {

    }
}
