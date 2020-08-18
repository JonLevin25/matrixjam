namespace MatrixJam.Team17
{
    public interface ITrigger : IInputHandler
    {
        void TriggerDown();
        void TriggerUp();
    }
}