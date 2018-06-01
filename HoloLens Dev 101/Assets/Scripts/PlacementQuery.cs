/*
 * creates a data object that is used to keep track of a spatial understanding request. do this is to 
 * with multiple cholograms, feed all the requests to spatial understanding at once in a separate thread 
 * so that the UI thread doesn’t lock up for the user which would make the HoloLens appear to lock up.  
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public enum ObjectType
{
    SquareBuilding,
    WideBuilding,
    TallBuilding,
    Tree,
    Tumbleweed,
    Mine
}

public struct PlacementQuery
{
    public readonly SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition PlacementDefinition;
    public readonly Vector3 Dimensions;
    public readonly ObjectType ObjType;
    public readonly List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> PlacementRules;
    public readonly List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> PlacementConstraints;

public PlacementQuery(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placeDef, Vector3 dimensions, ObjectType objType,
                          List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placeRules = null,
                          List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placeConstraints = null)
    {
        PlacementDefinition = placeDef;
        PlacementRules = placeRules;
        PlacementConstraints = placeConstraints;
        Dimensions = dimensions;
        ObjType = objType;
    }
}
