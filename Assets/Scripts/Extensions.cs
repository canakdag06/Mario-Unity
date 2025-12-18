using UnityEngine;

public static class Extensions
{
    private static LayerMask defaultMask = LayerMask.GetMask("Default");

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.bodyType == RigidbodyType2D.Kinematic)
        {
            return false;
        }
        float radius = 0.25f;
        float distance = 0.375f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, defaultMask);
        return hit.collider != null;
    }

    public static bool IsInThisDirection(this Transform a, Transform b, Vector2 testDirection)
    {
        Vector2 direction = b.position - a.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}
