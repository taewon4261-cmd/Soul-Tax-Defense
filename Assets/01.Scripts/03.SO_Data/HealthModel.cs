using System;

public class HealthModel
{
    public float MaxHp { get; private set; }
    public float CurrentHp { get; private set; }

    // 데이터가 변경되었을 때 Presenter에게 알릴 이벤트 (현재체력, 최대체력)
    public event Action<float, float> OnHealthChanged;

    public HealthModel(float maxHp)
    {
        MaxHp = maxHp;

        CurrentHp = maxHp;
    }

    public void ChangeHealth(float amount)
    {
        CurrentHp = Math.Max(0, Math.Min(MaxHp, CurrentHp + amount));
        // 데이터가 변했으니 구독자(Presenter)들에게 알림
        OnHealthChanged?.Invoke(CurrentHp, MaxHp);
    }
}