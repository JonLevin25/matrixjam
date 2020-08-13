namespace MatrixJam.Team25.Scripts.Managers
{
    public interface IState
    {
        void Tick();
        void OnEnter();
        void OnExit();
    }
}