using UnityEngine;
using static UnityEngine.ParticleSystem;

public class JumpBooster : Item
{
    [SerializeField] private float jumpForce;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void DetectItem()
    {
        if (Player.instance.isFly) return;

        Rigidbody2D rb = Player.instance.GetComponent<Rigidbody2D>();

        if (rb)
        {
            PlayerTask.platformJumps++;
            animator.SetTrigger("Jump");
            Audio.singleton.PlayCachedAudio($"Bounce{Random.Range(1, 4)}");
            Vector2 velocity = rb.velocity;
            velocity.y = jumpForce;
            rb.velocity = velocity;

            PlayerTask.jumpBooster++;
        }
    }
}