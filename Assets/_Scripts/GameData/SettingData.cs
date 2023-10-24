using System;

[Serializable]
public class SettingData
{
    public bool sound;
    public bool music;

    
    public SettingData()
    {
        sound = true;
        music = true;
    }

}
