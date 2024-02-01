using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour 
{
    [Header("Movment")]
    [SerializeField] private float speedMove;
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float timingPlayGame;
    [SerializeField] private float dieTime;
    [SerializeField] private float force;

    [Header("Fly")]
    [SerializeField] private float flySpeed;
    [SerializeField] private float flyTime;
    private GameObject jetpack;

    [Header("Other")]
    public GameObject botcoinModel;
    public static Player instance { get;private set; }
    private Rigidbody2D rb;

    [HideInInspector] public bool isFly;
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool canPlay = false;

    [Header("Other")]
    [SerializeField] private GameObject skinHolder;
    public static Coin coinSkin;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        InitSkin();
        StartCoroutine(PlayTiming());
    }

    public void InitSkin()
    {
        GameObject skin = Instantiate(coinSkin.model, skinHolder.transform);
        skin.transform.position = new Vector3(0, 0, 0);
        skin.transform.localEulerAngles = coinSkin.rotation;
        skin.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
    }

    private void Update()
    {
        if (!canPlay) return;

        if (canPlay)
        {
            if (Input.GetMouseButton(0))
            {
                rb.position = new Vector2(Mathf.MoveTowards(rb.position.x, GetCursorPositionX(), speedMove * Time.deltaTime), rb.position.y);
            }
        }

        if (isFly)
        {
            rb.velocity = new Vector2(rb.velocity.x, flySpeed);
            PlayerTask.flyJetpack += 1;
            print(PlayerTask.flyJetpack);
            //transform.Translate(Vector2.up * flySpeed);
        }
    }

    public void Fly(JetPack jetpack) => StartCoroutine(PlayerFly(jetpack));

    private IEnumerator PlayerFly(JetPack jetpack)
    {
        isFly = true;
        animator.SetBool("fly", true);

        jetpack.transform.parent = botcoinModel.transform;
        jetpack.transform.localPosition = Vector2.zero;
        jetpack.transform.localEulerAngles = new Vector3(0, 180, 90);
        jetpack.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        jetpack.GetComponent<Collider2D>().enabled = true;
        jetpack.EnableTrusters(true);
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(flyTime);

        jetpack.GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
        jetpack.EnableTrusters(false);

        jetpack.transform.parent = null;
        jetpack.gameObject.AddComponent<Rigidbody2D>().AddForce(Vector2.up * rb.velocity.y / 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        Audio.singleton.PlayCachedAudio("RocketEnd");
        Destroy(jetpack, 5f);

        animator.SetBool("fly", false);
        isFly = false;
    }


    private float GetCursorPositionX()
    {
        float halfScreen = Screen.width / 2;
        float posX = (Input.mousePosition.x - halfScreen) / halfScreen;

        return posX * sensitivity;
    }

    public void PlayJumpAnimation() => animator.SetTrigger("Jump");

    IEnumerator PlayTiming()
    {
        yield return new WaitForSeconds(timingPlayGame);
        canPlay = true;
    }

    public void Die(GameObject owner)
    {
        if(owner != null)
        {
            Vector3 direction = transform.position - owner.transform.position;
            direction = direction.normalized;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            GetComponent<BoxCollider2D>().enabled = false;
        }
        canPlay = false;

        if(SaveSystem.singleton != null)
        {
            PlayerData data = SaveSystem.singleton.GetCachedData();

            if (Score.Instance.GetScore() > SaveSystem.singleton.GetCachedData().score)
            {
                data.score = Score.Instance.GetScore();
            }

            data.health--;
            data.platformJumps += PlayerTask.platformJumps;
            data.platformBrokens += PlayerTask.platformBrokens;
            data.jumpBoster += PlayerTask.jumpBooster;
            data.flyJetpack += PlayerTask.flyJetpack;

            StartCoroutine(SaveSystem.singleton.SaveDataAsync(data));
        }

        SceneTransition.singleton.ChangeScene("Menu");
    }
}