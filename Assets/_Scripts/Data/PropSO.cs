using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PropParameters_", menuName = "PCG/PropSO")]
public class PropSO : ScriptableObject
{
    [Header("Prop Data")]
    public Transform PropPrefab;
    public Vector2Int PropSize = Vector2Int.one;

    [Space, Header("Placement Type")]
    public bool fourSpriteDirections;
    [HideInInspector] public Sprite spriteFront;
    [HideInInspector] public Sprite spriteBack;
    [HideInInspector] public Sprite spriteRight;
    [HideInInspector] public Sprite spriteLeft;
    public bool Corner;
    public bool Inner;
    public bool NearWallUp;
    public bool NearWallDown;
    public bool NearWallRight;
    public bool NearWallLeft;
    public bool mustBeAccessible;
    public bool mustBePlacedAndAccessible;
    public bool isBrakable;
    [Min(1)] public int PlacementQuantityMin = 1;
    [Min(1)] public int PlacementQuantityMax = 1;

    [Space, Header("Group Placement")]
    public bool PlaceAsGroup;
    [Min(1)] public int GroupMinCount = 1;
    [Min(1)] public int GroupMaxCount = 1;
}

#if UNITY_EDITOR
[CustomEditor(typeof(PropSO))]
public class PropSOEditor : Editor
{
	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

		PropSO propSO = (PropSO)target;

        if (propSO.fourSpriteDirections)
		{
			propSO.spriteFront = (Sprite)EditorGUILayout.ObjectField("Sprite Front", propSO.spriteFront, typeof(Sprite), false);
            propSO.spriteBack = (Sprite)EditorGUILayout.ObjectField("Sprite Back", propSO.spriteBack, typeof(Sprite), false);
            propSO.spriteRight = (Sprite)EditorGUILayout.ObjectField("Sprite Right", propSO.spriteRight, typeof(Sprite), false);
            propSO.spriteLeft = (Sprite)EditorGUILayout.ObjectField("Sprite Left", propSO.spriteLeft, typeof(Sprite), false);
		}
	}
}
#endif