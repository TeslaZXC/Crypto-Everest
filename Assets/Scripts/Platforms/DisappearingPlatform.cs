using System.Collections;
using UnityEngine;

public class DisappearingPlatform : Platform
{
    [SerializeField] private float targetAlpha = 0.0f;
    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Renderer rend;
    private bool isDdisappear;

    //private void Start()
    //{
    //    if (rend == null)
    //    {
    //        rend = GetComponent<Renderer>();
    //    }
    //}

    public override void OnPlayerTouched(GameObject player)
    {
        if (isDdisappear) return;

        Ddisappear();
    }

    public void Ddisappear()
    {
        StartCoroutine(ChangeAlphaOverTime(targetAlpha, duration));
    }

    private IEnumerator ChangeAlphaOverTime(float target, float time)
    {
        Color startColor = rend.material.color;
        float startAlpha = startColor.a;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / time;
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startAlpha, target, t);
            rend.material.color = newColor;
            yield return null;
        }
    }
}