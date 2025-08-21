using UnityEngine;

public class ScreenCollector : MonoBehaviour
{
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _damageNumberContainer;

    public GameObject GetHUD()
    {
        return _hud;
    }

    public GameObject GetDamageNumberContainer()
    {
        return _damageNumberContainer;
    }
}

