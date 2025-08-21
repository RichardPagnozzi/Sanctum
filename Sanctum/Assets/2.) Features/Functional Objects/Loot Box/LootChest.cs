using UnityEngine;
using DG.Tweening;

public class LootChest : MonoBehaviour
{
    #region Members
    // Scene Ref
    [SerializeField] private GameObject _lid;
    [SerializeField] private Light _light;
    #endregion

    #region Public
    public void OpenChest()
    {
        _lid.transform.DOLocalRotate(new Vector3(-90, 0, 0), 1);
        _light.enabled = true;
    }
    public void CloseChest()
    {
        _lid.transform.DOLocalRotate(new Vector3(90, 0, 0), 1);
        _light.enabled = false;
    }
    #endregion
}
