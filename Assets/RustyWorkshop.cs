using UnityEngine.UI;
using UnityEngine;
using Steamworks;
using System.IO;
using System;

public class RustyWorkshop : MonoBehaviour
{
    public GameObject _notice, _noticeFooterText, _progressBar, _interfaceNewItem, _interfaceUpdateItem, _panelStart;
    public Text _noticeText, _statusText, _noticeHeaderText, _statusTextStart, _statusTextUpdate, _headerItemIconText, _languageText;
    public InputField _itemNameInput, _itemIconInput, _itemUpdateIDinput, _itemUpdateNameInput, _itemUpdateIconInput, _itemUpdateChangelogInput;
    public Image _progressBarImage;
    private const string _manifest = @"{""Version"":3,""ItemType"":""Rock"",""AuthorId"":{0},""PublishDate"":""{1}"",""Groups"":[{""Textures"":{},""Floats"":{""_Cutoff"":0.0,""_BumpScale"":1.0,""_Glossiness"":1.0,""_OcclusionStrength"":1.0,""_DetailNormalMapScale"":0.65,""_DetailOcclusionStrength"":1.0,""_DetailOverlaySmoothness"":0.0,""_DetailOverlaySpecular"":0.0},""Colors"":{""_Color"":{""r"":1.0,""g"":1.0,""b"":1.0},""_SpecColor"":{""r"":1.0,""g"":1.0,""b"":1.0},""_EmissionColor"":{""r"":0.0,""g"":0.0,""b"":0.0},""_DetailColor"":{""r"":1.0,""g"":1.0,""b"":1.0}}}]}";
    private const string _statusRus = "Steam: {0}\n\nНазвание предмета: {1}\nИконка: {2}\n\nСтатус: {3}";
    private const string _statusEng = "Steam: {0}\n\nItem name: {1}\nIcon: {2}\n\nStatus: {3}";
    private const string _faqRus = "<b>Что делать, если не получается подключиться к Steam</b>: попробуйте перезапустить Steam (вы обязательно должны быть авторизованы в нем) и RustyWorkshop.\n\n<b>Требования к иконке</b>: желательно должна быть размером 512x512, должна весить не более 1 мб и быть в формате изображения (.psd не поддерживается)";
    private const string _faqEng = "<b>What to do if you can't connect to Steam</b>: try restarting Steam (you must be logged in) and RustyWorkshop.\n\n<b>Icon requirements</b>: should preferably be 512x512 in size, weigh no more than 1 mb and be in image format (.psd is not supported)";
    private int _selectedLanguage => PlayerPrefs.GetInt("LocalizeKey", 0);
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
            if(_selectedLanguage == 0) ShowNotice("Failed connect to Steam!\nCheck, is Steam running, and try reconnect again.", "Connect Failed");
            else ShowNotice("Не удалось подключиться к сессии клиента Steam!\nПроверьте, запущен ли у вас Steam, и попробуйте переподключиться снова.", "Не удалось подключиться");
            
            _isInitialized = false;
        }

        UpdateLocalization();
        UpdateStatus();
    }
    public async void TryUpload() 
    {
        if(_interfaceNewItem.activeInHierarchy) 
        {
            if(!_isAllReady) 
            {
                if(_selectedLanguage == 0) ShowNotice("Correct all errors before creating the icon, they are indicated in the status!", "Error");
                else ShowNotice("Исправьте все ошибки перед созданием иконки, они указаны в статусе!", "Ошибка");
            }
            else 
            {
                if(_selectedLanguage == 0) ShowNotice("Icon is loading, please wait...", "Loading", true);
                else ShowNotice("Иконка загружается, пожалуйста подождите...", "Загрузка", true);

                string newDirectoryPath = Application.dataPath + "/Temp";

                if(Directory.Exists(newDirectoryPath)) 
                {
                    Directory.Delete(newDirectoryPath, true);
                }

                Directory.CreateDirectory(newDirectoryPath);

                string newFilePath = $"{newDirectoryPath}/icon{Path.GetExtension(_itemIconInput.text)}";
                File.Copy(_itemIconInput.text, newFilePath);

                File.WriteAllText($"{newDirectoryPath}/manifest.txt", _manifest.Replace("{0}", Steamworks.SteamClient.SteamId.ToString()).Replace("{1}", DateTime.Now.ToString("o")));

                System.Progress<float> progress = new System.Progress<float>(value => OnProgressChanged(value));

                var task = Steamworks.Ugc.Editor.NewMicrotransactionFile
                        .WithTitle(_itemNameInput.text)
                        .WithDescription("RustyWorkshop - free, multi-platform icon uploader for Rust with open source")
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
                    if(_selectedLanguage == 0) ShowNotice($"Icon uploaded successfully!\nIcon ID in Worshop: {result.FileId}", "Icon Uploaded");
                    else ShowNotice($"Иконка была успешно загружена!\nID иконки в мастерской: {result.FileId}", "Иконка загружена");

                    Application.OpenURL($"https://steamcommunity.com/sharedfiles/filedetails/?id={result.FileId}");
                }
                else 
                {
                    if(_selectedLanguage == 0) ShowNotice($"Failed to load icon!\n{result.Result}", "Error");
                    else ShowNotice($"Не удалось загрузить иконку!\n{result.Result}", "Ошибка");
                }

                _progressBarImage.fillAmount = 0;
                Directory.Delete(Application.dataPath + "/Temp", true);
            }
        }
        if(_interfaceUpdateItem.activeInHierarchy) 
        {
            if(!_isInitialized) 
            {
                if(_selectedLanguage == 0) ShowNotice("You not connected to Steam!", "Error");
                else ShowNotice("Вы не подключены к Steam!", "Ошибка");

                return;
            }

            ulong id = 0;
            try 
            {
                id = Convert.ToUInt64(_itemUpdateIDinput.text);
            }
            catch 
            {
                if(_selectedLanguage == 0) ShowNotice("ID is not correct!", "Error");
                else ShowNotice("ID введен неверно!", "Ошибка");
                return;
            }

            if(_selectedLanguage == 0) ShowNotice("Icon updating, please wait...", "Update", true);
            else ShowNotice("Иконка обновляется, пожалуйста подождите...", "Обновление", true);

            bool isIconExists = false;
            string newDirectoryPath = Application.dataPath + "/Temp";
            string newFilePath = "";

            if(File.Exists(_itemUpdateIconInput.text))
            {
                if(Path.GetExtension(_itemUpdateIconInput.text) != ".psd") 
                {
                    isIconExists = true;
                    newFilePath = $"{newDirectoryPath}/icon{Path.GetExtension(_itemUpdateIconInput.text)}";
                }
            }
            else 
            {
                isIconExists = false;
            }

            if(isIconExists) 
            {
                Directory.CreateDirectory(newDirectoryPath);

                File.Copy(_itemUpdateIconInput.text, newFilePath);

                File.WriteAllText($"{newDirectoryPath}/manifest.txt", _manifest.Replace("{0}", Steamworks.SteamClient.SteamId.ToString()).Replace("{1}", DateTime.Now.ToString("o")));
            }

            System.Progress<float> progress = new System.Progress<float>(value =>
            {
                OnProgressChanged(value);
            });

            var obj = new Steamworks.Ugc.Editor(id);

            if(!string.IsNullOrEmpty(_itemUpdateNameInput.text))     
            {
                obj.WithTitle(_itemUpdateNameInput.text);
            }   

            obj.WithDescription("RustyWorkshop - free, multi-platform icon uploader for Rust with open source") ;
            obj.WithChangeLog(_itemUpdateChangelogInput.text);
            obj.WithMetaData("");
            obj.ForAppId(252490);
            obj.WithPublicVisibility();
            obj.WithTag("Skin");
            obj.WithTag("Rock Skin");
            obj.WithTag("version2");

            if(isIconExists)
            {
                obj.WithContent(newDirectoryPath);
                obj.WithPreviewFile(newFilePath);
            }

            var result = await obj.SubmitAsync(progress);            

            if(result.Success) 
            {
                if(_selectedLanguage == 0) ShowNotice($"Icon was successfully updated!\nIcon ID in Workshop: {result.FileId}", "Icon Updated");
                else ShowNotice($"Иконка была успешно обновлена!\nID иконки в мастерской: {result.FileId}", "Иконка обновлена");

                Application.OpenURL($"https://steamcommunity.com/sharedfiles/filedetails/?id={result.FileId}");
            }
            else 
            {
                if(_selectedLanguage == 0) ShowNotice($"Failed to update icon!\n{result.Result}", "Error");
                else ShowNotice($"Не удалось обновить иконку!\n{result.Result}", "Ошибка");
            }

            _progressBarImage.fillAmount = 0;
            if(isIconExists) Directory.Delete(Application.dataPath + "/Temp", true);
        }
    }
    private void OnProgressChanged(float progress, bool isUpdate = false) => _progressBarImage.fillAmount = progress;
    private void ShowNotice(string text, string header, bool enableProgressBar = false) 
    {
        _notice.SetActive(true);

        _noticeText.text = text;
        _noticeHeaderText.text = header;

        _noticeFooterText.SetActive(!enableProgressBar);
        _progressBar.SetActive(enableProgressBar);
    }
    public void OpenItemCreation() 
    {
        _panelStart.SetActive(false);
        _interfaceNewItem.SetActive(true);

        UpdateLocalization();
        UpdateStatus();
    }
    public void OpenItemUpdater() 
    {
        _panelStart.SetActive(false);
        _interfaceUpdateItem.SetActive(true);

        UpdateLocalization();
        UpdateStatus();
    }
    public void BackToMenu() 
    {
        _interfaceUpdateItem.SetActive(false);
        _interfaceNewItem.SetActive(false);
        _panelStart.SetActive(true);
        _notice.SetActive(false);

        UpdateLocalization();
        UpdateStatus();
    }
    private void OnApplicationQuit() => SteamClient.Shutdown();
    public void UpdateStatus() 
    {
        if(_panelStart.activeInHierarchy) 
        {
            string status = "Steam: {0}";

            if(_isInitialized) 
            {
                if(_selectedLanguage == 0) status = status.Replace("{0}", "<color=green>connected</color>");
                else status = status.Replace("{0}", "<color=green>подключен</color>");
            }
            else 
            {   
                if(_selectedLanguage == 0) status = status.Replace("{0}", "<color=red>not connected</color>");
                else status = status.Replace("{0}", "<color=red>не подключен</color>");
            }

            _statusTextStart.text = status;
        }
        else 
        {
            if(_interfaceNewItem.activeInHierarchy) 
            {
                string status = _statusEng;

                if(_selectedLanguage != 0) status = _statusRus;

                bool isNameEntered = false;
                bool isIconReady = false;

                if(_isInitialized) 
                {
                    if(_selectedLanguage == 0) status = status.Replace("{0}", "<color=green>connected</color>");
                    else status = status.Replace("{0}", "<color=green>подключен</color>");
                }
                else 
                {   
                    if(_selectedLanguage == 0) status = status.Replace("{0}", "<color=red>not connected</color>");
                    else status = status.Replace("{0}", "<color=red>не подключен</color>");
                }

                if(string.IsNullOrEmpty(_itemNameInput.text)) 
                {
                    if(_selectedLanguage == 0) status = status.Replace("{1}", "<color=red>not entered</color>");
                    else status = status.Replace("{1}", "<color=red>не указано</color>");

                    isNameEntered = false;
                }
                else 
                {
                    if(_selectedLanguage == 0) status = status.Replace("{1}", "<color=green>entered</color>");
                    else status = status.Replace("{1}", "<color=green>указано</color>");

                    isNameEntered = true;
                }

                if(string.IsNullOrEmpty(_itemIconInput.text)) 
                {
                    if(_selectedLanguage == 0) status = status.Replace("{2}", "<color=red>not finded</color>");
                    else status = status.Replace("{2}", "<color=red>не найдена</color>");

                    isIconReady = false;
                }
                else 
                {
                    if(File.Exists(_itemIconInput.text))
                    {
                        if(Path.GetExtension(_itemIconInput.text) == ".psd") 
                        {
                            if(_selectedLanguage == 0) status = status.Replace("{2}", "<color=red>must be in image format, .psd is not supported!</color>");
                            else status = status.Replace("{2}", "<color=red>должна быть в формате изображения, .psd не поддерживается!</color>");

                            isIconReady = false;
                        }
                        else 
                        {
                            if(_selectedLanguage == 0) status = status.Replace("{2}", "<color=green>ready</color>");
                            else status = status.Replace("{2}", "<color=green>готова</color>");

                            isIconReady = true;
                        }
                    }
                    else 
                    {
                        if(_selectedLanguage == 0) status = status.Replace("{2}", "<color=red>not finded</color>");
                        else status = status.Replace("{2}", "<color=red>не найдена</color>");

                        isIconReady = false;
                    }
                }

                if(_isInitialized && isNameEntered && isIconReady) 
                {
                    if(_selectedLanguage == 0) status = status.Replace("{3}", "<color=green>you can create an icon</color>");
                    else status = status.Replace("{3}", "<color=green>можно создавать иконку</color>");

                    _isAllReady = true;
                }
                else 
                {
                    if(_selectedLanguage == 0) status = status.Replace("{3}", "<color=red>contains errors</color>");
                    else status = status.Replace("{3}", "<color=red>имеются ошибки</color>");

                    _isAllReady = false;
                }

                _statusText.text = status;
            }

            if(_interfaceUpdateItem.activeInHierarchy) 
            {
                if(_isInitialized) 
                {
                    if(_selectedLanguage == 0) _statusTextUpdate.text = "Steam: <color=green>connected</color>";
                    else _statusTextUpdate.text = "Steam: <color=green>подключен</color>";
                }
                else 
                {   
                    if(_selectedLanguage == 0) _statusTextUpdate.text = "Steam: <color=red>not connected</color>";
                    else _statusTextUpdate.text = "Steam: <color=red>не подключен</color>";
                }

                if(string.IsNullOrEmpty(_itemUpdateIconInput.text))
                {
                    if(_selectedLanguage == 0) _headerItemIconText.text = "Item icon (path to file)";
                    else _headerItemIconText.text = "Иконка предмета (путь до файла)";
                }
                else
                {
                    if(File.Exists(_itemUpdateIconInput.text))
                    {
                        if(Path.GetExtension(_itemUpdateIconInput.text) == ".psd") 
                        {
                            if(_selectedLanguage == 0) _headerItemIconText.text = "<color=red>Item icon (path to file)</color>";
                            else _headerItemIconText.text = "<color=red>Иконка предмета (путь до файла)</color>";
                        }
                        else 
                        {
                            if(_selectedLanguage == 0) _headerItemIconText.text = "<color=green>Item icon (path to file)</color>";
                            else _headerItemIconText.text = "<color=green>Иконка предмета (путь до файла)</color>";
                        }
                    }
                    else 
                    {
                        if(_selectedLanguage == 0) _headerItemIconText.text = "<color=red>Item icon (path to file)</color>";
                        else _headerItemIconText.text = "<color=red>Иконка предмета (путь до файла)</color>";
                    }
                }
            }
        }
    }
    private void UpdateLocalization()
    {
        LocalizeText.OnLanguageChanged();

        if(_selectedLanguage == 0) _languageText.text = "Язык: <b><i><color=white>русский</color></i></b>, <b><i><color=silver>english</color></i></b>";
        else _languageText.text = "Язык: <b><i><color=silver>русский</color></i></b>, <b><i><color=white>english</color></i></b>";
    }
    public void SelectLanguage(int lang) 
    {
        PlayerPrefs.SetInt("LocalizeKey", lang);
        LocalizeText.OnLanguageChanged();

        UpdateLocalization();
        UpdateStatus();
    }
    public void TryReconnect() => Start();
    public void ShowFAQ() 
    {
        string faq = _faqEng;

        if(_selectedLanguage != 0) faq = _faqRus;

        ShowNotice(faq, "RustyWorkshop: FAQ");
    }
    public void CloseNotice() => _notice.SetActive(false);
}
