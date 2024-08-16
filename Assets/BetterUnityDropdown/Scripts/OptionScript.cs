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
        public TextMeshProUGUI targetText;
        public GameObject selector;

        public int Id { get; private set; }
        public DropdownItemData Data { get; private set; }

        /// <summary>
        /// Initialize items
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="data">Item Data</param>
        public virtual void Init(int id, DropdownItemData data)
        {
            Id = id;
            Data = data;
            targetText.text = data.Text;
            selector.SetActive(id == mainScript.Value);
        }

        /// <summary>
        /// Click event by pressing option
        /// </summary>
        public void ClickSelf()
        {
            mainScript.SelectOption(Id);
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