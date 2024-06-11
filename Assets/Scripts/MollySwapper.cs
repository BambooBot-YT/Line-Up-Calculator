using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MollySwapper : MonoBehaviour
{
    [Header("Molly Controller")]
    [Tooltip("link the dropdown and the player")]
    [SerializeField] private TMP_Dropdown mollyTypeDropdown;
    public MollyController MollyController;

    Dictionary<string, object[]> mollyTypeData = new Dictionary<string, object[]>
    {
        { "Paint Shells", new object[] { 3f, 9.81f, 0.4f, 4 } },
        { "FLASH/DRIVE", new object[] { 3f, 9.81f, 0.4f, 4 } },
        { "Snake Bite", new object[] { 3f, 9.81f, 0.4f, 4 } },
        { "Slow Orb", new object[] { 3f, 9.81f, 0.4f, 4 } },
        { "Poison Cloud", new object[] { 3f, 9.81f, 0.4f, 0 } },
        { "Nanoswarm", new object[] { 3f, 9.81f, 0.4f, 0 } },
        { "Mosh Pit", new object[] { 3f, 9.81f, 0.4f, 0 } },
        { "FRAG/MENT", new object[] { 3f, 9.81f, 0.4f, 0 } },
        { "GravNet", new object[] { 3f, 9.81f, 0.4f, 0 } }
    };

    private void Start()
    {
        // Add listener to dropdown value change event
        mollyTypeDropdown.onValueChanged.AddListener(OnMollyTypeDropdownValueChanged);
    }

    // Method to handle dropdown value change event
    void OnMollyTypeDropdownValueChanged(int valueIndex)
    {
        string value = mollyTypeDropdown.options[valueIndex].text;

        if (mollyTypeData.ContainsKey(value))
        {
            object[] mollyData = mollyTypeData[value];
            MollyController.molly.throwForce = (float)mollyData[0];
            MollyController.molly.gravity = (float)mollyData[1];
            MollyController.molly.dampening = (float)mollyData[2];
            MollyController.molly.targetBounces = (int)mollyData[3];
        }
    }
}