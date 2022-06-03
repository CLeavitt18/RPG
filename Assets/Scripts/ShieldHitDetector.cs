using UnityEngine;

public class ShieldHitDetector : MonoBehaviour
{
    [SerializeField] private LivingEntities _parent;
    
    [SerializeField] private bool _hit;
    [SerializeField] private bool _protecting;

    [SerializeField] private int _armour;


    public void SetProtecting(bool state)
    {
        _protecting = state;
    }

    public void SetHit(bool state)
    {
        _hit = state;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!_protecting)
        {
            return;
        }

        _hit = true;
        
        if (_parent is Player)
        {
            float tempExp = _armour;
            tempExp *= 1 + (Player.player.GetSkillLevel((int)SkillType.Blocking) * .01f);

            Player.player.GainExp((long)tempExp, (int)SkillType.Blocking);
        }
    }

    /*private void OnTriggerExit(Collider other) 
    {
        _hit = false;    
    }*/

    public bool GetProtecting()
    {
        return _protecting;
    }

    public bool GetHit()
    {
        return _hit;
    }

    public void SetState(int armour, LivingEntities parent)
    {
        _armour = armour;
        _parent = parent;
    }
}
