using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class LocalizeText : MonoBehaviour
{
    private Text _localizeText;
    public string rusText, engText;
    private void Start() => _localizeText = GetComponent<Text>();
    private void Localize() 
    {
        if(_localizeText == null) Start();

        if(PlayerPrefs.GetInt("LocalizeKey", 0) == 0) _localizeText.text = engText;
        else _localizeText.text = rusText;
    }
    private void OnEnable() => Localize();
    public static void OnLanguageChanged() => GameObject.FindObjectsOfType<LocalizeText>().ToList().ForEach(x => x.Localize());
}
