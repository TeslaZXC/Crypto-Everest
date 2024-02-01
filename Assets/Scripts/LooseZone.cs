using UnityEngine;

public class LooseZone : MonoBehaviour
{
    private bool playerLose;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(!playerLose)
            {
                playerLose = true;
                Player.instance.Die(gameObject);
            }
        }
    }
}