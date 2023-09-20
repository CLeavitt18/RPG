using UnityEngine;

public class ShieldHitDetector : MonoBehaviour
{
    [SerializeField] private LivingEntities _parent;
    
    [SerializeField] private bool _protecting;

    [SerializeField] private int _armour;


    public void SetProtecting(bool state)
    {
        _protecting = state;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!_protecting)
        {
            return;
        }
        
        if (_parent is Player)
        {
            float tempExp = _armour;
            tempExp *= 1 + (Player.player.GetSkillLevel(SkillType.Blocking) * .01f);

            Player.player.GainExp((long)tempExp, (int)SkillType.Blocking);
        }
    }

    public bool GetProtecting()
    {
        return _protecting;
    }

    public void SetState(int armour, LivingEntities parent)
    {
        _armour = armour;
        _parent = parent;
    }

    public LivingEntities GetParent()
    {
        return _parent;
    }
}
