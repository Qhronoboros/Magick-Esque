using UnityEngine;

public class CastSpellCommand : ICommand
{
    private SpellCaster _spellCaster;

    public CastSpellCommand(SpellCaster spellCaster)
    {
        _spellCaster = spellCaster;
    }

    public void Execute(GameObject actor)
    {
        Debug.Log("Cast Spell");
        _spellCaster.CastSpell(actor);
    }
}