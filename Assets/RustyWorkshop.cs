using UnityEngine.UI;
using UnityEngine;
using Steamworks;
using System.IO;
using System;
using System.Text;

public class RustyWorkshop : MonoBehaviour
{
    [SerializeField] private GameObject _notice, _noticeFooterText, _progressBar;
    [SerializeField] private Text _noticeText, _statusText, _noticeHeaderText;
    [SerializeField] private InputField _itemNameInput, _itemIconInput;
    [SerializeField] private Image _progressBarImage;
    private const string _manifest = @"{""Version"":3,""ItemType"":""Rock"",""AuthorId"":{0},""PublishDate"":""{1}"",""Groups"":[{""Textures"":{},""Floats"":{""_Cutoff"":0.0,""_BumpScale"":1.0,""_Glossiness"":1.0,""_OcclusionStrength"":1.0,""_DetailNormalMapScale"":0.65,""_DetailOcclusionStrength"":1.0,""_DetailOverlaySmoothness"":0.0,""_DetailOverlaySpecular"":0.0},""Colors"":{""_Color"":{""r"":1.0,""g"":1.0,""b"":1.0},""_SpecColor"":{""r"":1.0,""g"":1.0,""b"":1.0},""_EmissionColor"":{""r"":0.0,""g"":0.0,""b"":0.0},""_DetailColor"":{""r"":1.0,""g"":1.0,""b"":1.0}}}]}";
    private const string _status = "Steam: {0}\n\nНазвание предмета: {1}\nИконка: {2}\n\nСтатус: {3}";
    private const string _faq = "<b>Что делать, если не получается подключиться к Steam</b>: попробуйте перезапустить Steam (вы обязательно должны быть авторизованы в нем) и RustyWorkshop, иных вариантов нет.\n\n<b>Требования к иконке</b>: должна весить не более 1 мегабайта и быть в формате изображения (не работает с .psd)";
    private bool _isInitialized = false;
    private bool _isAllReady = false;
    private void Start() 
    {
        if(_isInitialized) return;
        try 
        {
            SteamClient.Init(252490);
            _isInitialized = true;
        }
        catch
        {
            ShowNotice("Не удалось подключиться к сессии клиента Steam!\nПроверьте, запущен ли у вас Steam, и попробуйте переподключиться снова.", "Не удалось подключиться");
            _isInitialized = false;
        }
        UpdateStatus();
    }
    public async void TryUpload() 
    {
        if(!_isAllReady) 
        {
            ShowNotice("Исправьте все ошибки перед созданием иконки, они указаны в статусе!", "Ошибка");
        }
        else 
        {
            ShowNotice("Иконка загружается, пожалуйста подождите...", "Загрузка", true);

            string newDirectoryPath = Application.dataPath + "/Temp";
            Directory.CreateDirectory(newDirectoryPath);

            string newFilePath = $"{newDirectoryPath}/icon{Path.GetExtension(_itemIconInput.text)}";
            File.Copy(_itemIconInput.text, newFilePath);

            File.WriteAllText($"{newDirectoryPath}/manifest.txt", _manifest.Replace("{0}", Steamworks.SteamClient.SteamId.ToString()).Replace("{1}", DateTime.Now.ToString("o")));

            System.Progress<float> progress = new System.Progress<float>(value =>
            {
                OnProgressChanged(value);
            });

            var task = Steamworks.Ugc.Editor.NewMicrotransactionFile
                      .WithTitle(_itemNameInput.text)
                      .WithDescription("RustyWorkshop - бесплатный, мультиплатформенный загрузчик иконок для Rust (только на rustyplugin.ru)")
                      .WithContent(newDirectoryPath)
                      .WithPreviewFile(newFilePath)
                      .WithMetaData("")
                      .ForAppId(252490)
                      .WithPublicVisibility()
                      .WithTag("Skin")
                      .WithTag("Rock Skin")
                      .WithTag("version2");
                    
            var result = await task.SubmitAsync(progress);        

            if(result.Success) 
            {
                ShowNotice($"Иконка была успешно загружена!\nID иконки в мастерской: {result.FileId}", "Иконка загружена");
                Application.OpenURL($"https://steamcommunity.com/sharedfiles/filedetails/?id={result.FileId}");
            }
            else 
            {
                ShowNotice($"Не удалось загрузить иконку!\n{result.Result}", "Ошибка");
            }

            _progressBarImage.fillAmount = 0;
            Directory.Delete(Application.dataPath + "/Temp", true);
        }
    }
    private void OnProgressChanged(float progress) => _progressBarImage.fillAmount = progress;
    private void ShowNotice(string text, string header, bool enableProgressBar = false) 
    {
        _notice.SetActive(true);

        _noticeText.text = text;
        _noticeHeaderText.text = header;

        _noticeFooterText.SetActive(!enableProgressBar);
        _progressBar.SetActive(enableProgressBar);
    }
    private void OnApplicationQuit() => SteamClient.Shutdown();
    public void UpdateStatus() 
    {
        string status = _status;

        bool isNameEntered = false;
        bool isIconReady = false;

        if(_isInitialized) 
        {
            status = status.Replace("{0}", "<color=green>подключен</color>");
        }
        else 
        {   
            status = status.Replace("{0}", "<color=red>не подключен</color>");
        }

        if(string.IsNullOrEmpty(_itemNameInput.text)) 
        {
            status = status.Replace("{1}", "<color=red>не указано</color>");
            isNameEntered = false;
        }
        else 
        {
            status = status.Replace("{1}", "<color=green>указано</color>");
            isNameEntered = true;
        }

        if(string.IsNullOrEmpty(_itemIconInput.text)) 
        {
            status = status.Replace("{2}", "<color=red>не найдена</color>");
            isIconReady = false;
        }
        else 
        {
            if(File.Exists(_itemIconInput.text))
            {
                if(Path.GetExtension(_itemIconInput.text) != ".png") 
                {
                    status = status.Replace("{2}", "<color=red>должна быть в формате PNG!</color>");
                    isIconReady = false;
                }
                else 
                {
                    status = status.Replace("{2}", "<color=green>готова</color>");
                    isIconReady = true;
                }
            }
            else 
            {
                status = status.Replace("{2}", "<color=red>не найдена</color>");
                isIconReady = false;
            }
        }

        if(_isInitialized && isNameEntered && isIconReady) 
        {
            status = status.Replace("{3}", "<color=green>можно создавать иконку</color>");
            _isAllReady = true;
        }
        else 
        {
            status = status.Replace("{3}", "<color=red>имеются ошибки</color>");
            _isAllReady = false;
        }

        _statusText.text = status;
    }
    public void TryReconnect() => Start();
    public void ShowFAQ() => ShowNotice(_faq, "RustyWorkshop: FAQ");
    public void CloseNotice() => _notice.SetActive(false);
}
