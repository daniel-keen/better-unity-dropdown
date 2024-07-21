using UnityEngine.Events;

public class CustomDropdownItemData : DropdownItemData
{
    public UnityAction TestButtonClicked;

    public CustomDropdownItemData(string text, UnityAction testButtonClicked) : base(text)
    {
        TestButtonClicked = testButtonClicked;
    }
}