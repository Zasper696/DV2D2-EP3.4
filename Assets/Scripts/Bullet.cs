using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum DamageType { Physical, Fire, Ice, Poison, Magical }

    [SerializeField] private int damage;
    [SerializeField] private DamageType damageType;

    public int GetDamage()
    {
        return damage;
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }
}
