public interface IState
{
    void OnStateEnter();
    void Execute();
    void OnStateExit();
}