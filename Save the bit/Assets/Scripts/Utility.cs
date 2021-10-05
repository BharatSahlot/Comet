using UnityEngine;

public static class Utility
{
    private static Camera _camera;
    
    public static Vector3 WorldPosToBorder(Vector3 worldPos, float screenBorder)
    {
        if (!_camera) _camera = Camera.main;
        
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