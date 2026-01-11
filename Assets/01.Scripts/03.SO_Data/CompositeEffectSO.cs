using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoulTax/Effects/Composite(Bundle)")]
public class CompositeEffectSO : ContractEffectSO
{
    [Header("여러 효과를 여기에 넣으면 순서대로 다 실행됩니다")]
    public List<ContractEffectSO> effects = new List<ContractEffectSO>();

    public override void ApplyEffect()
    {
        foreach (var effect in effects)
        {
            effect.ApplyEffect();
        }
    }
}