using UnityEngine;

namespace MatrixJam.Team17
{
    public static class Mouse
    {
        public static Vector3 GetWorldPosition()
        {
            Vector3 mouseWorldPos = Vector3.zero;
            Camera camera = Camera.main;
            if (camera != null)
            {
                mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
            }

            return mouseWorldPos;
        }
    }
}