using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementEffect : IElementEffect
{
    private Timer _fireTickTimer;
    private float _fireAmount;
    private IHealth health;
    
    public IEntity Actor { get; private set; }
    public ElementType ElementType { get; private set; }
    public Color EffectColor { get; private set; }

    public FireElementEffect(Color effectColor)
    {
        ElementType = ElementType.FIRE;
        EffectColor = effectColor;
        Actor = null;
        health = null;
    }

    public void SetActor(IEntity actor)
    {
        Actor = actor;
        health = Actor as IHealth;
        StartFireTickTimer();
    }

    public void OnEffectHit(ISpellObject spellObject)
    {
    }
    
    public void ProcessElementChange(ISpellStats spellStats)
    {
        switch (spellStats.GetElementEffect().ElementType)
        {
            case ElementType.WATER:
                // Do extra damage
                if (health != null)
                    health.TakeDamage(50, Color.gray);
                break;
            case ElementType.FIRE:
                // Apply more fire
                _fireAmount += spellStats.GetFireApplication();
                Mathf.Min(_fireAmount, 5.0f);  // Temporary Maximum
                break;
        }
    }

    public void StartFireTickTimer()
    {
        if (_fireTickTimer != null) return;
        
        _fireTickTimer = new Timer();
        _fireTickTimer.EnableLooping();
        _fireTickTimer.OnTimerEnd += FireTickDamage;
        _fireTickTimer.timerDuration = 0.5f;
        _fireTickTimer.StartTimer();
        Debug.Log("FireTickTimer Started");
    }

    public void FireTickDamage()
    {
        if (health != null)
            health.TakeDamage(Mathf.CeilToInt(_fireAmount), EffectColor);
            
        Debug.Log("FireTickDamage");
        _fireAmount--;
    }

    public void Update(float deltaTime)
    {
        if (_fireTickTimer.isCounting)
            _fireTickTimer.CountTimer(deltaTime);
    }
    
    public void FixedUpdate(float deltaTime)
    {
    }

    public IPrototype Clone()
    {
        return new FireElementEffect(EffectColor);
    }
}
