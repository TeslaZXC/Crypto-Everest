using UnityEngine;

public class SpikesPlatform : Platform
{
    public override void OnPlayerTouched(GameObject player) => player.GetComponent<Player>().Die(gameObject);
}
