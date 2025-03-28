using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public Dictionary<string, bool> boolValues = new Dictionary<string, bool>();
    public Dictionary<string, int> intValues = new Dictionary<string, int>();
    public Dictionary<string, float> floatValues = new Dictionary<string, float>();
    public Dictionary<string, string> stringValues = new Dictionary<string, string>();
}
