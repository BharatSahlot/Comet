using UnityEngine;

public static class Utility
{
    private static Camera _camera;

    public static Bounds GetScreenBounds(float depth)
    {
        if (_camera == null) _camera = Camera.main;
        
        Vector3 center = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, depth));
        Vector3 size = ScreenToWorldSize(depth);
        return new Bounds(center, size);
    }
    
    public static Vector2 ScreenToWorldSize(float depth)
    {
        if (_camera == null) _camera = Camera.main;
        
        var corners = new Vector3[4]
        {
            _camera.ViewportToWorldPoint(new Vector3(0, 0, depth)),
            _camera.ViewportToWorldPoint(new Vector3(0, 1, depth)),
            _camera.ViewportToWorldPoint(new Vector3(1, 0, depth)),
            _camera.ViewportToWorldPoint(new Vector3(1, 1, depth)),
        };
        
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        foreach (var localCorner in corners)
        {
            var corner = _camera.transform.TransformVector(localCorner);
            minX = Mathf.Min(minX, corner.x);
            minY = Mathf.Min(minY, corner.y);
            maxX = Mathf.Max(maxX, corner.x);
            maxY = Mathf.Max(maxY, corner.y);
        }

        return new Vector2(maxX - minX, maxY - minY);
    }
    
    public static Vector3 WorldPosToBorder(Vector3 worldPos, float screenBorder)
    {
        if (_camera == null) _camera = Camera.main;
        
        var corners = new Vector3[4]
        {
            _camera.ViewportToWorldPoint(new Vector3(0, 0, 0)),
            _camera.ViewportToWorldPoint(new Vector3(0, 1, 0)),
            _camera.ViewportToWorldPoint(new Vector3(1, 0, 0)),
            _camera.ViewportToWorldPoint(new Vector3(1, 1, 0)),
        };
    
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        foreach (var localCorner in corners)
        {
            var corner = _camera.transform.TransformVector(localCorner);
            minX = Mathf.Min(minX, corner.x);
            minY = Mathf.Min(minY, corner.y);
            maxX = Mathf.Max(maxX, corner.x);
            maxY = Mathf.Max(maxY, corner.y);
        }
    
        minX += screenBorder;
        maxX -= screenBorder;
        minY += screenBorder;
        maxY -= screenBorder;
    
        var pos = worldPos;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
}