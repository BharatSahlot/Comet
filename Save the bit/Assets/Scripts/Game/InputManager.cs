using UnityEngine;
using UnityEngine.Scripting;

namespace Game
{
    public enum InputMode
    {
        Keyboard,
        Joystick
    }
    
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputMode inputMode = InputMode.Joystick;
        [SerializeField] private Joystick joystick;
        [SerializeField] private float keyboardSensitivity = 5;

        private Vector2 _currentDir;
        
        public void SetInputMode(InputMode mode)
        {
            inputMode = mode;
        }
        
        public Vector2 GetMoveDirection()
        {
            switch (inputMode)
            {
                case InputMode.Joystick:
                    return new Vector2(joystick.X, joystick.Y);
                case InputMode.Keyboard:
                {
                    float angle = Vector2.Angle(Vector2.up, _currentDir);
                    angle += Input.GetAxis("Horizontal") * keyboardSensitivity;
                    return _currentDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    //return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }
                default:
                    return Vector2.zero;
            }
        }
    }
}