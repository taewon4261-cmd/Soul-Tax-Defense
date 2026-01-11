using UnityEngine;

[CreateAssetMenu(menuName = "SoulTax/Effects/Special Flag")]
public class SpecialFlagEffectSO : ContractEffectSO
{
    public enum FlagType { SlimeDoubleProd, SoulHarvest }
    public FlagType flagType;

    public override void ApplyEffect()
    {
        switch (flagType)
        {
            case FlagType.SlimeDoubleProd:
                GameManager.Instance.isSlimeDoubleBuffActive = true;
                break;
            case FlagType.SoulHarvest:
                GameManager.Instance.isSoulHarvestActive = true;
                break;
        }
    }
}