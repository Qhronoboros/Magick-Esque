using UnityEngine;

public class AddSpellCommand : ICommand
{
    private SpellCaster _spellCaster;
    private ISpell _spell;

    public AddSpellCommand(SpellCaster spellCaster, ISpell spell)
    {
        _spellCaster = spellCaster;
        _spell = spell;
    }

    public void Execute(GameObject actor)
    {
        _spellCaster.AddElementSpell(_spell.Clone() as ISpell);
    }
}
