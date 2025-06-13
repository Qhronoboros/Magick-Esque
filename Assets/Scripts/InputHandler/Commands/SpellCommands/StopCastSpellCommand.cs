using UnityEngine;

public class StopCastSpellCommand : ICommand
{
    private SpellCaster _spellCaster;
    
    public StopCastSpellCommand(SpellCaster spellCaster)
    {
        _spellCaster = spellCaster;
    }

    public void Execute(GameObject actor)
    {
        Debug.Log("Stop Cast Spell");
        _spellCaster.StopCastSpell(actor);
    }
}