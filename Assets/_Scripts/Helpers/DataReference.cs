using System.Collections.Generic;

public static class DataReference
{
    private static readonly List<IData> _datas = new();


    public static void Add(IData iData)
    {
        if(!_datas.Contains(iData)) 
            _datas.Add(iData);
    }
    public static void Remove(IData iData)
    {
        if (_datas.Contains(iData))
            _datas.Remove(iData);
    }

    public static List<IData> Get() => _datas;
    
    public static void Clear() => _datas.Clear();
}
