using System;

namespace BetterUnityDropdown
{
    [Serializable]
    public class BetterDropdownOption
    {
        public DropdownItemData Data;

        public BetterDropdownOption(DropdownItemData data)
        {
            Data = data;
        }
    }
}