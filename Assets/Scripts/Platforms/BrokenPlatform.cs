using UnityEngine;

public class BrokenPlatform : Platform
{
    [SerializeField] private float brokenTime;
    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    private Animator animator;
    private bool isBroken;

    private void Awake() => animator = GetComponent<Animator>();

    public override void OnPlayerTouched(GameObject player)
    {
        if (isBroken) return;

        Broken();
    }

    private void Broken()
    {
        isBroken = true;
        PlayerTask.platformBrokens++;
        Audio.singleton.PlayCachedAudio("PlatformBreak");
        particles.Play();
        part1.AddComponent<Rigidbody2D>().AddForce(Vector2.right * 1.25f + Vector2.down, ForceMode2D.Impulse);
        part2.AddComponent<Rigidbody2D>().AddForce(Vector2.left * 1.25f + Vector2.down, ForceMode2D.Impulse);

        GetComponent<BoxCollider2D>().enabled = false;
    }
}