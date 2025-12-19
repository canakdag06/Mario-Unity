using UnityEngine;

public static class Extensions
{
    private static LayerMask defaultMask = LayerMask.GetMask("Default");
    private static LayerMask enemyMask = LayerMask.GetMask("Enemy");

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction, LayerMask collisionMask)
    {
        if (rigidbody.bodyType == RigidbodyType2D.Kinematic)
        {
            return false;
        }
        float radius = 0.25f;
        float distance = 0.375f;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(rigidbody.position, radius, direction.normalized, distance, collisionMask);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != rigidbody.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsInThisDirection(this Transform a, Transform b, Vector2 testDirection)
    {
        Vector2 direction = b.position - a.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}
