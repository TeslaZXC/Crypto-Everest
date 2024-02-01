using UnityEngine;

public interface IDetectItem
{
    void DetectItem();
}

public abstract class Item : MonoBehaviour, IDetectItem
{
    public virtual void DetectItem()
    {
        print("die");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DetectItem();
        }
    }
}