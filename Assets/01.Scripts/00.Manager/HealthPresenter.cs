public class HealthPresenter
{
    private readonly HealthModel _model;
    private readonly HealthBarView _view;

    public HealthPresenter(HealthModel model, HealthBarView view)
    {
        _model = model;
        _view = view;

        // 1. Model의 이벤트 구독: "피가 깎이면 View를 업데이트해라"
        _model.OnHealthChanged += UpdateView;
        // 2. 초기 값 반영
        UpdateView(_model.CurrentHp, _model.MaxHp);
    }

    private void UpdateView(float currentHp, float maxHp)
    {
        float ratio = currentHp / maxHp;
        _view.UpdateFill(ratio);
    }
  
    public void Dispose()
    {
        _model.OnHealthChanged -= UpdateView;
    }
}