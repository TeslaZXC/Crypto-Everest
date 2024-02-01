using UnityEngine;

public class Platform : MonoBehaviour 
{
    [SerializeField] private float jumpForce = 10f;
    private bool platformHited;

    [Header("Effects")]
    public ParticleSystem particles;
    public Material usedColor;
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y <= 0f)
            OnPlayerTouched(collision.gameObject);
    }

	public virtual void OnPlayerTouched(GameObject player)
	{
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb)
        {
            PlayerTask.platformJumps++;

            Vector2 velocity = rb.velocity;
            velocity.y = jumpForce;
            rb.velocity = velocity;

            player.GetComponent<Player>().PlayJumpAnimation();
            
            if (!platformHited)
            {
                particles.Play();
                Audio.singleton.PlayCachedAudio($"Platform{Random.Range(1, 4)}");
                GetComponent<Animator>().SetTrigger("Jump");
                renderer.material = usedColor;
                platformHited = true;
            }
        }
    }
}