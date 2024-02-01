using UnityEngine;

public class MovingPlatform : Platform
{
    [SerializeField] private float endPos; 
    [SerializeField] private float speed;
    private bool isLeft;

    private void Start()
    {
        isLeft = transform.position.x < 0;
    }

    private void Update()
    {
        if(transform.position.x >= endPos && isLeft)
            isLeft = false;
        if(transform.position.x <= -endPos && !isLeft)
            isLeft = true;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(isLeft ? endPos : -endPos, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }
}