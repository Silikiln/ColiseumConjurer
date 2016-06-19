using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    [SerializeField]
    protected int BaseHealth = 1;

    [SerializeField]
    protected int BaseDamage = 1;

    [SerializeField]
    protected float BaseSize = 1;

    [SerializeField]
    protected float MinimumSize = .5f;

    [SerializeField]
    protected float MaximumSize = 2f; 

    public int Health { get; protected set; }

    protected virtual void Start()
    {
        Health = MaxHealth;
        BaseSize = transform.localScale.x;
        transform.localScale = new Vector3(Size, Size, 1);
    }

    protected virtual float HealthMultiplier { get { return 1; } }
    protected virtual float HealthAdded { get { return 0; } }

    protected virtual float DamageDealtMultiplier { get { return 1; } }
    protected virtual float DamageDealtAdded { get { return 0; } }

    protected virtual float DamageRecievedMultiplier { get { return 1; } }
    protected virtual float DamageRecievedAdded { get { return 0; } }

    protected virtual float HealMultiplier { get { return 1; } }
    protected virtual float HealAdded { get { return 0; } }

    protected virtual float SizeMultiplier { get { return 1; } }
    protected virtual float SizeAdded { get { return 0; } }

    public float HealthPercent { get { return (float)Health / MaxHealth; } }
    public int Damage { get { return Mathf.Clamp((int)(BaseDamage * DamageDealtMultiplier + DamageDealtAdded), 0, int.MaxValue); } }
    public int MaxHealth { get { return (int)(BaseHealth * HealthMultiplier + HealthAdded); } }
    public float Size { get { return Mathf.Clamp(BaseSize * SizeMultiplier + SizeAdded, MinimumSize, MaximumSize); } }

    public virtual void Hurt(int damage)
    {
        damage = (int)(damage * DamageRecievedMultiplier + DamageRecievedAdded);
        if ((Health -= damage) <= 0)
            Destroy(gameObject);
    }

    public virtual void Heal(int amount)
    {
        amount = (int)(amount * HealMultiplier + HealAdded);
        if ((Health += amount) > MaxHealth)
            Health = MaxHealth;
    }
}
