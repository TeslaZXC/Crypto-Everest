using UnityEngine;

public class SidePoint : MonoBehaviour
{
    [SerializeField] private Transform oppositePoint;
    [SerializeField] private Transform target;

    [SerializeField] private bool leftSide;

    private void Update()
    {
        if (leftSide)
        {
            if (transform.position.x > target.position.x)
            {
                MoveToOppositePoint();
            }
        }
        else
        {
            if (transform.position.x < target.position.x)
            {
                MoveToOppositePoint();
            }
        }
    }

    private void MoveToOppositePoint() => target.position = new Vector2(oppositePoint.position.x, target.position.y);
}