public interface ISpeedManager
{
    float GetCurrentSpeed();
    void SetIsMove(bool isMove);
    void ReduceSpeedAfterCrush();
}
