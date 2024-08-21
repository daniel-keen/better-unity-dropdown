using System.Collections.Generic;
using BetterUnityDropdown;
using UnityEngine;

public class CustomDropdownDataExample : MonoBehaviour
{
    [SerializeField] private BetterDropdown _dropdown;

    public void RemoveItemById(string id)
    {
        _dropdown.RemoveById(id);
    }

    private void Start()
    {
        var customItemData1 = new CustomDropdownItemData("item1", "Item 1", () =>
        {
            Debug.Log("Hello world!");
            _dropdown.Close();
        });
        var customItemData2 = new CustomDropdownItemData("item2", "Item 2", () =>
        {
            Debug.Log("Hello world!");
            _dropdown.Close();
        });
        _dropdown.AddRange(new List<CustomDropdownItemData>
        {
            customItemData1,
            customItemData2
        });
    }
}
