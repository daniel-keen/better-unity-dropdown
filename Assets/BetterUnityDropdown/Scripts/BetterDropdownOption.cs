using System;

namespace BetterUnityDropdown
{
    [Serializable]
    public class BetterDropdownOption
    {
        public string nameText; //Text for option

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nameText">Text for option</param>
        public BetterDropdownOption(string nameText)
        {
            this.nameText = nameText;
        }
    }
}