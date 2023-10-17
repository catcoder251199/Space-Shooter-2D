public interface IState
{
    void OnStateEnter();
    void UpdateExecute();
    void FixedUpdateExecute();
    void OnStateExit();
}