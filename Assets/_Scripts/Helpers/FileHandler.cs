using System;
using System.IO;
using System.Text;
using UnityEngine;


public enum FileName
{
    UserData,
    SettingData
}

public static class FileHandler
{
    public static string _path;
    public static string _jsonData;
    
    
    public static void Save<T>(T _type, FileName _fileName)
    {
        _path = Path.Combine(Application.persistentDataPath, _fileName.ToString());
        
        if(File.Exists(_path)) File.Delete(_path);

        _jsonData = JsonUtility.ToJson(_type, true);
        File.WriteAllText(_path, _jsonData);
        
        // var _encryptedText = Encrypt(_jsonData);
        // File.WriteAllText(_path, _encryptedText);
    }


    public static T Load<T>(FileName _fileName)
    {
        T _data = default;
        
        _path = Path.Combine(Application.persistentDataPath, _fileName.ToString());
        if (!File.Exists(_path)) 
            return _data;

        var encryptedText = File.ReadAllText(_path);
        _data = JsonUtility.FromJson<T>(encryptedText);

        // var _decryptedText = Decrypt(encryptedText);
        // _data = JsonUtility.FromJson<T>(_decryptedText);
        
        return _data;
    }


    private const int _lineLength = 50; // số kí tự trên mỗi dòng;
    
    private static string Encrypt(string _text)
    {
        var _byteEncode = Encoding.UTF8.GetBytes(_text);
        var _encodeText = Convert.ToBase64String(_byteEncode);

        var _stringBuilder = new StringBuilder();
        for (var i = 0; i < _encodeText.Length; i += _lineLength)
        {
            var _length = Mathf.Min(_lineLength, _encodeText.Length - i);
            _stringBuilder.AppendLine(_encodeText.Substring(i, _length));
        }
        return _stringBuilder.ToString();
    }
    private static string Decrypt(string _encryptedText)
    {
        var decodedBytes = Convert.FromBase64String(_encryptedText);
        var decodeText = Encoding.UTF8.GetString(decodedBytes);
        return decodeText;
    }


}
