using UnityEngine;
using TMPro;

namespace BetterUnityDropdown
{
    /// <summary>
    /// Script for options object
    /// </summary>
    public class OptionScript : MonoBehaviour
    {
        public BetterDropdown mainScript;
        public int id; //Id in list
        public DropdownItemData Data;
        public TextMeshProUGUI targetText; //Text component
        public GameObject selector; //Selector object

        /// <summary>
        /// Initialize items
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="data">Item Data</param>
        public virtual void Init(int id, DropdownItemData data)
        {
            this.id = id;
            Data = data;
            targetText.text = data.Text;
            selector.SetActive(id == mainScript.Value);
        }

        /// <summary>
        /// Click event by pressing option
        /// </summary>
        public void ClickSelf()
        {
            mainScript.SelectOption(id);
        }

        /// <summary>
        /// Set select state (Called from main script)
        /// </summary>
        /// <param name="isSelected">Is selected</param>
        public void SetSelectState(bool isSelected)
        {
            selector.SetActive(isSelected);
        }
    }
}