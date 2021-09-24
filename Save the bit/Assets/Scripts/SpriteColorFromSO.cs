using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteColorFromSO : MonoBehaviour
{
    [SerializeField] private ColorSO _color;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = _color.Color;
    }
}