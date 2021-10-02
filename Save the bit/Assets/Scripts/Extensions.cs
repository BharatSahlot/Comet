using UnityEngine;

public static class Extensions
{
    public static Vector2 xy(this Vector3 vec) => new Vector2(vec.x, vec.y);
    public static Vector2 xz(this Vector3 vec) => new Vector2(vec.x, vec.z);
    public static Vector2 yz(this Vector3 vec) => new Vector2(vec.y, vec.z);

    public static bool IsPartOfLayer(this GameObject go, string layer)
    {
        return go.layer == LayerMask.NameToLayer(layer);
    }
    
    public static bool IsPartOfLayer<T>(this T go, string layer) where T : Behaviour
    {
        return go.gameObject.layer == LayerMask.NameToLayer(layer);
    }
}