using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "_/Color", order = 0)]
public class ColorSO : ScriptableObject
{
    [field: SerializeField] public Color Color { get; private set; }
}