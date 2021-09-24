using UnityEngine;
using UnityEngine.UI;

public class ImageColorFromSO : MonoBehaviour
{
    [SerializeField] private ColorSO _color;

    private Image _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Image>();
        _renderer.color = _color.Color;
    }
}