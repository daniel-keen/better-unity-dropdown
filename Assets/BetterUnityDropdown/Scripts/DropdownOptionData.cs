using System;

[Serializable]
public class DropdownItemData
{
    public string Id;
    public string Text;

    public DropdownItemData(string id, string text)
    {
        Id = id;
        Text = text;
    }
}