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

    [Space, Header("Placement Type")]
    public bool Corner;
    public bool Inner;
    public bool NearWallUp;
    public bool NearWallDown;
    public bool NearWallRight;
    public bool NearWallLeft;
    public bool mustBeAccessible;
    public bool mustBePlacedAndAccessible;
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
    public bool hasVariants;
    [HideInInspector] public Sprite[] variants;
    public bool twoSpriteDirections;
    public bool fourSpriteDirections;

    [Serializable] public struct PropGraphics
    {
        public Sprite sprite;
        public AnimationClip animationClip;
    }

    public List<PropGraphics> propGraphics = new();

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

        if (propSO.hasVariants)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("variants"));

            serializedObject.ApplyModifiedProperties();    
        }

        if (propSO.twoSpriteDirections && propSO.fourSpriteDirections)
        {
            propSO.fourSpriteDirections = false;
        }

        if (propSO.twoSpriteDirections)
        {
            if (propSO.propGraphics.Count > 2)
            {
                while (propSO.propGraphics.Count > 2)
                {
                    propSO.propGraphics.RemoveAt(2);
                }

            } else if (propSO.propGraphics.Count < 2)
            {
                while (propSO.propGraphics.Count < 2)
                {
                    propSO.propGraphics.Add(new PropSO.PropGraphics());
                }
            }
        }

        if (propSO.fourSpriteDirections)
		{
            if (propSO.propGraphics.Count > 4)
            {
                while (propSO.propGraphics.Count > 4)
                {
                    propSO.propGraphics.RemoveAt(4);
                }

            } else if (propSO.propGraphics.Count < 4)
            {
                while (propSO.propGraphics.Count < 4)
                {
                    propSO.propGraphics.Add(new PropSO.PropGraphics());
                }
            }
		}

        if (!propSO.twoSpriteDirections && !propSO.fourSpriteDirections)
        {
            propSO.propGraphics.Clear();
        }
	}
}
#endif