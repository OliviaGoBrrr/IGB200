using NUnit.Framework;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections.Generic;

public class PlayerSpriteLibrarySet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    SpriteLibraryAsset LibraryAsset;
    [SerializeField] SpriteLibraryType type;
    [SerializeField] List<SpriteLibraryAsset> ListAAA;

    enum SpriteLibraryType
    {
        BANGS, 
        HAIR,
        EYES,
        HIGHLIGHT,
        SCLERA,
        HAT,
        ACCESSORY
    }
    void Start()
    {
        switch (type)
        {
            case SpriteLibraryType.BANGS:
                LibraryAsset = ListAAA[CustomiseData.bangsType];
                break;
            case SpriteLibraryType.HAIR:
                LibraryAsset = ListAAA[CustomiseData.hairType];
                break;
            case SpriteLibraryType.EYES:
                LibraryAsset = ListAAA[CustomiseData.eyeType];
                break;
            case SpriteLibraryType.HIGHLIGHT:
                LibraryAsset = ListAAA[CustomiseData.eyeType];
                break;
            case SpriteLibraryType.SCLERA:
                LibraryAsset = ListAAA[CustomiseData.eyeType];
                break;
            case SpriteLibraryType.HAT:
                LibraryAsset = ListAAA[CustomiseData.hatType];
                break;
            case SpriteLibraryType.ACCESSORY:
                LibraryAsset = ListAAA[CustomiseData.accessoryType];
                break;
        }
        GetComponent<SpriteLibrary>().spriteLibraryAsset = LibraryAsset;
    }
}
