using UnityEngine;

public static class Util
{
    public static Vector2 FacingDirection(SpriteRenderer spriteRenderer)
    {
        return spriteRenderer.flipX ? Vector2.left : Vector2.right;
    }

    public static Vector3 RandomOffsetByRadius(float radius)
    {
        Vector3 offset = Random.insideUnitCircle * radius;
        return new Vector3(offset.x, offset.y, 0f);
    }

    public static Vector3 PointByRadius(float radius, float degress)
    {
        float rad = degress * Mathf.PI / 180f;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector3(cos, sin, 0f) * radius;
    }

    public static Bounds CameraBounds(Camera camera)
    {
        Vector3 origin = camera.transform.position;
        origin.z = 0;

        float screenAspect = (float) Screen.width / (float) Screen.height;
        float cameraHeight = camera.orthographicSize * 2;

        return new Bounds(origin, new Vector3(cameraHeight * screenAspect, cameraHeight, 0f));
    }

    public static Quaternion RotationFromDirection(Vector2 direction)
    {
        float tan = Mathf.Atan2(direction.y, direction.x);
        float angle = tan * Mathf.Rad2Deg;

        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static Vector2 RandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0f);
    }

    public static Vector2 RandomPointInCamera(Camera camera)
    {
        return RandomPointInBounds(CameraBounds(camera));
    }

    public static float RandomFactor()
    {
        return Random.Range(0, 2) == 0 ? 1f : -1f;
    }

    public static bool FloatToBool(float value)
    {
        return value != -1f;
    }

    public static float BoolToFloat101(bool value)
    {
        return value ? 1f : -1f;
    }

    public static string BonusToStr(float bonus)
    {
        return (bonus > 0 ? "+" : "") + bonus.ToString("0.##");
    }
}
