using UnityEngine.Events;

public class CustomDropdownItemData : DropdownItemData
{
    public UnityAction TestButtonClicked;

    public CustomDropdownItemData(string id, string text, UnityAction testButtonClicked) : base(id, text)
    {
        TestButtonClicked = testButtonClicked;
    }
}