using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<IData> IDataRef;
    
    public UserData UserData { get; private set; }
    public SettingData SettingData { get; set; }

    private void Start()
    {
        #if UNITY_EDITOR
        Application.targetFrameRate = -1;
        #else
        Application.targetFrameRate = 60;
        #endif
        
        IDataRef = DataReference.Get();
        LoadData();
    }
    private void OnApplicationQuit()
    {
        DOTween.Clear();
        SaveData();
    }

    private void LoadData()
    {
        var _userData = FileHandler.Load<UserData>(FileName.UserData);
        var _settingData = FileHandler.Load<SettingData>(FileName.SettingData);
        
        UserData = _userData ?? new UserData();
        if (UserData.userLevel > 3) 
            UserData = new UserData();
        
        SettingData = _settingData ?? new SettingData();
        
        SendData();
    }
    private void SaveData()
    {
        FileHandler.Save(UserData, FileName.UserData);
        FileHandler.Save(SettingData, FileName.SettingData);
    }

    public void SendData()
    {
        IDataRef = DataReference.Get();
        IDataRef.ForEach(x => x.SendData(this));
    }


}
