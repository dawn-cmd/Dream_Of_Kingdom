using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject {
    public int maxValue;
    public int currentValue;
    public void SetValue(int value) {
        currentValue = value;
    }
}
