using UnityEngine;

namespace Game
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Joystick joystick;

        public Vector2 GetMoveDirection()
        {
            return new Vector2(joystick.X, joystick.Y);
        }
    }
}