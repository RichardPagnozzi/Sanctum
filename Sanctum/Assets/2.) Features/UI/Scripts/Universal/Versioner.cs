using UnityEngine;
using TMPro; 
public class Versioner : MonoBehaviour
{

    [SerializeField] private TMP_Text _versionText;

    private void OnEnable()
    {
        if(_versionText != null)
        {
            string version = Application.version;
            _versionText.text = $"developer version: {version}";
        }

    }
}
