using BetterUnityDropdown;
using UnityEngine;

public class CustomDropdownDataExample : MonoBehaviour
{
    [SerializeField] private BetterDropdown _dropdown;

    private void Start()
    {
        var customItemData = new CustomDropdownItemData("Test item", () =>
        {
            Debug.Log("Hello world!");
            _dropdown.Close();
        });
        _dropdown.Add(customItemData);
    }
}
