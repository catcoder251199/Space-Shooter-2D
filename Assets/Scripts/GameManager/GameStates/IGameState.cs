public interface IGameState
{
    void OnStateEnter();
    void UpdateExecute();
    void FixedUpdateExecute();
    void OnStateExit();
}