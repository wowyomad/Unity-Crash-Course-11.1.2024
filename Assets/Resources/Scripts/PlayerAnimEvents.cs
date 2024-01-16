using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void OnTriggerEvent()
    {
        player.AttackOver();
    }
}
