public interface IElementStatus
{
    IElementEffect CurrentElementEffect { get; }
    void ApplyElement(ISpellObject spellObject);
}
