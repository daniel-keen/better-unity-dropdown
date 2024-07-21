using BetterUnityDropdown;
using UnityEngine;
using UnityEngine.UI;

public class CustomOptionScript : OptionScript
{
    [SerializeField] private Button _testLogButton;

    public override void Init(int id, DropdownItemData data)
    {
        base.Init(id, data);

        var customItemData = data as CustomDropdownItemData;
        _testLogButton.onClick.AddListener(customItemData.TestButtonClicked);
    }
}
