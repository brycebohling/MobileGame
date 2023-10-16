using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LootParms_", menuName = "LootSO")]
public class LootSO : ScriptableObject
{
    public List<LootObject> lootTable = new();
}

[System.Serializable]
public class LootObject
{
    public GameObject gameObject;
    [Min(1)] public int weight;
    [ReadOnlyInspector] public float percentChance;
}

#if UNITY_EDITOR
[CustomEditor(typeof(LootSO))]
public class LootSOEditor : Editor
{
	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();
        
		LootSO lootSO = (LootSO)target;

        int totalWeight = 0;

        foreach (var item in lootSO.lootTable)
        {
            totalWeight += item.weight;
        }

        foreach (var item in lootSO.lootTable)
        {
            Debug.Log(item.weight / totalWeight);
            item.percentChance = (float)item.weight / totalWeight;
        }
	}
}
#endif


