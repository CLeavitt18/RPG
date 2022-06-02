using UnityEngine;

public class ShieldHitDetector : MonoBehaviour
{
    [SerializeField] private LivingEntities _parent;
    
    [SerializeField] private bool _hit;
    [SerializeField] public bool Protecting;

    [SerializeField] private int _armour;

    private void OnTriggerEnter(Collider other) 
    {
        _hit = true;

        if (_parent is Player)
        {
            float tempExp = _armour;
            tempExp *= 1 + (Player.player.GetSkillLevel((int)SkillType.Blocking) * .01f);

            Player.player.GainExp((long)tempExp, (int)SkillType.Blocking);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        _hit = false;    
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
