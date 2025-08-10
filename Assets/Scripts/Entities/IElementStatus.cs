using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElementStatus
{
    IElementEffect CurrentElementEffect { get; }
    void ApplyElement(ISpellObject spellObject);
}
