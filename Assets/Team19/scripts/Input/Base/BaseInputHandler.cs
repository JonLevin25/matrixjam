namespace MatrixJam.Team19.Input.Base
{
    public abstract class BaseInputHandler : UnityEngine.ScriptableObject
    {
        public abstract bool IsInputAvailable { get; }

        public abstract UnityEngine.Vector3 GetNextDirection();
        public abstract void UpdateInput();
    }
}
