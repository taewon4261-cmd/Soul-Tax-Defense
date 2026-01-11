using UnityEngine;

[CreateAssetMenu(menuName = "SoulTax/Effects/Economy")]
public class EconomyEffectSO : ContractEffectSO
{
    public enum EconomyType { InstantGold, TaxMod, InterestGold }
    public EconomyType type;
    public float amount; // 500(고정) 또는 0.1(10%)

    public override void ApplyEffect()
    {
        GameManager gm = GameManager.Instance;
        switch (type)
        {
            case EconomyType.InstantGold:
                gm.AddGold((int)amount);
                break;

            case EconomyType.TaxMod:
                // amount가 -0.2면 20% 감면, 0.3이면 30% 증가
                gm.ModifyNextTaxRate(amount);
                break;

            case EconomyType.InterestGold:
                // 현재 골드의 n% 지급
                int interest = Mathf.FloorToInt(gm.gold * amount);
                gm.AddGold(interest);
                Debug.Log($"[이자] {interest} 골드 획득!");
                break;
        }
    }
}