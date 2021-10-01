using UnityEngine;

namespace Game
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;

        [SerializeField] private Joystick joystick;

        public static Vector2 GetMoveDirection()
        {
            if (_instance == null) return Vector2.zero;
            return new Vector2(_instance.joystick.X, _instance.joystick.Y);
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}