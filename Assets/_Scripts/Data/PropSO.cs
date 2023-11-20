using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PropParameters_", menuName = "PCG/PropSO")]
public class PropSO : ScriptableObject
{
    [Header("Prop Data")]
    public Transform PropPrefab;
    public Vector2Int PropSize = Vector2Int.one;
    [Tooltip("The distance from the pivot point to the bottom left corner")]
    public Vector2 PlacementOffset;

    [Space, Header("Placement Type")]
    public bool Corner;
    public bool Inner;
    public bool NearWallUp;
    public bool NearWallDown;
    public bool NearWallRight;
    public bool NearWallLeft;
    public bool mustBeAccessible;
    public bool isBrakable;
    public bool placeOnePerFloor;
    [Tooltip("Amount placed per room")]
    [Min(1)] public int PlacementQuantityMin = 1;
    [Tooltip("Amount placed per room")]
    [Min(1)] public int PlacementQuantityMax = 1;
    

    [Space, Header("Group Placement")]
    public bool PlaceAsGroup;
    [Min(1)] public int GroupMinCount = 1;
    [Min(1)] public int GroupMaxCount = 1;

    [Header("Custom Inspector")]
    public bool HasVariants;
    [HideInInspector] public Sprite[] Variants;
    public bool TwoSpriteDirections;
    public bool FourSpriteDirections;

    [Serializable] public struct PropGraphicsData
    {
        public Sprite sprite;
        public AnimationClip animationClip;
    }

    public List<PropGraphicsData> PropGraphics = new();

    public enum PropGraphicOrder
    {
        Front,
        Back,
        Right,
        Left,
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PropSO))]
public class PropSOEditor : Editor
{
	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();
        
		PropSO propSO = (PropSO)target;

        if (propSO.HasVariants)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Variants"));

            serializedObject.ApplyModifiedProperties();    
        }

        if (propSO.TwoSpriteDirections && propSO.FourSpriteDirections)
        {
            propSO.FourSpriteDirections = false;
        }

        if (propSO.TwoSpriteDirections)
        {
            if (propSO.PropGraphics.Count > 2)
            {
                while (propSO.PropGraphics.Count > 2)
                {
                    propSO.PropGraphics.RemoveAt(2);
                }

            } else if (propSO.PropGraphics.Count < 2)
            {
                while (propSO.PropGraphics.Count < 2)
                {
                    propSO.PropGraphics.Add(new PropSO.PropGraphicsData());
                }
            }
        }

        if (propSO.FourSpriteDirections)
		{
            if (propSO.PropGraphics.Count > 4)
            {
                while (propSO.PropGraphics.Count > 4)
                {
                    propSO.PropGraphics.RemoveAt(4);
                }

            } else if (propSO.PropGraphics.Count < 4)
            {
                while (propSO.PropGraphics.Count < 4)
                {
                    propSO.PropGraphics.Add(new PropSO.PropGraphicsData());
                }
            }
		}

        if (!propSO.TwoSpriteDirections && !propSO.FourSpriteDirections)
        {
            propSO.PropGraphics.Clear();
        }
	}
}
#endif