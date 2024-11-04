using UniRx;
using UnityEngine;

public class Unit {
    //public int Health { get; private set; }
    public ReactiveProperty<int> Health { get; private set; }
    public int MaxHealth {  get; private set; }

    public Effect effectBarrier = new Effect();
    private Effect effectRegeneration = new Effect();
    private Effect effectBurning = new Effect();

    public int[] abilitiesCooldowns = new int[5];

    public int lastAbilityId=-1;

    private ServerGameManager serverManager;
    public Unit(ServerGameManager serverManager,int health) {
        this.serverManager = serverManager;
        MaxHealth = health;
        //Health = health;
        //Health.Value = health;
        Health = new ReactiveProperty<int>(health);
    }

    public void TakeDamage(int damage) {
        if (effectBarrier.isActive) {
            damage = Mathf.Max(0, damage - effectBarrier.amount);
            serverManager.ShowDamage(this, damage, HealthChangeType.BarrierAttack);
        }
        else {
            serverManager.ShowDamage(this, damage, HealthChangeType.Attack);
        }
        //Health = Mathf.Max(0, Health - damage);
        Health.Value = Mathf.Max(0, Health.Value - damage);

    }

    public void ApplyBarrier(int amount, int duration) {
        effectBarrier.amount = amount;
        effectBarrier.duration = duration;
    }

    public void ApplyRegeneration(int amount, int duration) {
        effectRegeneration.amount = amount;
        effectRegeneration.duration = duration;
    }

    public void ApplyBurning(int amount, int duration) {
        effectBurning.amount = amount;
        effectBurning.duration = duration;
    }

    public void RemoveBurning() {
        effectBurning.amount = 0;
        effectBurning.duration = 0;
    }

    public void UpdateAbilitiesCooldowns() {
        for (int i = 0; i < abilitiesCooldowns.Length; i++) {
            if (abilitiesCooldowns[i] > 0) {
                abilitiesCooldowns[i]--;
            }
        }
    }

    public void UpdateEffects() {
        if (effectBarrier.isActive) {
            effectBarrier.duration--;
        }

        if (effectRegeneration.isActive) {
            serverManager.ShowDamage(this, effectRegeneration.amount, HealthChangeType.Regeneration);

            //Health = Mathf.Min(MaxHealth, Health + effectRegeneration.amount);
            Health.Value = Mathf.Min(MaxHealth, Health.Value + effectRegeneration.amount);
            effectRegeneration.duration--;
        }

        if (effectBurning.isActive) {
            serverManager.ShowDamage(this, effectBurning.amount, HealthChangeType.Burning);
            //Health = Mathf.Max(0, Health - effectBurning.amount);
            Health.Value = Mathf.Max(0, Health.Value - effectBurning.amount);
            effectBurning.duration--;
        }

    }

    public int[] GetEffectsDuration() {
        return new int[] { 0, effectBarrier.duration, effectRegeneration.duration, effectBurning.duration, 0 };
    }

    public int[] GetAbilitiesCooldowns() {
        return abilitiesCooldowns;

    }

    public void Reset() {
        //Health = 100;
        Health.Value = 100;
        effectBarrier = new Effect();
        effectRegeneration = new Effect();
        effectBurning = new Effect();
        abilitiesCooldowns = new int[5];
    }
}
