using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Vector2 MouseLook;
    public Vector2 SmoothV;

    public float Sens = 2;
    public float Smoothing = 4;
    public float LookRange = 70;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {

    }
    void Update()
    {
        if (Player.player.GetMode() == PlayerState.Active)
        {
           Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            md = Vector2.Scale(md, new Vector2(Sens * Smoothing, Sens * Smoothing));
            SmoothV.x = Mathf.Lerp(SmoothV.x, md.x, 1f / Smoothing);
            SmoothV.y = Mathf.Lerp(SmoothV.y, md.y, 1f / Smoothing);
            MouseLook += SmoothV;
            MouseLook.y = Mathf.Clamp(MouseLook.y, -LookRange, LookRange);

            transform.localRotation = Quaternion.AngleAxis(-MouseLook.y, Vector3.right);
            Player.player.transform.localRotation = Quaternion.AngleAxis(MouseLook.x, Player.player.transform.up);

            //Debug.Log("Mouse Look: " + MouseLook);
        }
    }
}
