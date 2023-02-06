/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor
{
    public static class UIResources
    {
        //Editor Window Stuff
        public static readonly PngResource EditorWindowIcon = new PngResource("EditorWindowIcon", 16, 16, SkinMode.Unity);

        public static readonly PngResource HomeIcon = new PngResource("home", 16, 16, SkinMode.Unity);
        public static readonly PngResource ShowGridIcon = new PngResource("grid", 16, 16, SkinMode.Unity);
        public static readonly PngResource ZoomIcon = new PngResource("zoom", 16, 16, SkinMode.Unity);
        public static readonly PngResource ViewIcon = new PngResource("view", 16, 16, SkinMode.Unity);

        public static readonly PngResource ConnectorLine = new PngResource("AALine", 4, 9);

        public static readonly PngResource DragHandle = new PngResource("reorder", 16, 16);

        public static readonly PngResource BreakPoint = new PngResource("breakpoint", 16, 16);
        public static readonly PngResource BreakPointHit = new PngResource("breakpointHit", 16, 16);

        public static readonly PngResource ConnectorOpen = new PngResource("connectorOpen", 16, 16);
        public static readonly PngResource ConnectorUsed = new PngResource("connectorClosed", 16, 16);
        public static readonly PngResource ConnectorActive = new PngResource("connectorClosedScored", 32, 16);

        public static readonly PngResource ActionMarker = new PngResource("indentArrow", 16, 16);

        public static readonly PngResource ExpandIcon = new PngResource("dropDownClosed", 16, 16);

        public static readonly PngResource CollapseIcon = new PngResource("dropDownOpen", 16, 16);

        public static readonly PngResource ViewAIIcon = new PngResource("viewAI", 16, 16);

        public static readonly PngResource VisualizedEntityBackground = new PngResource("VisualizeContainer", 32, 32);
        public static readonly PngResource PinOnIcon = new PngResource("pinOn", 16, 16);
        public static readonly PngResource PinOffIcon = new PngResource("pinOff", 16, 16);
        public static readonly PngResource CancelIcon = new PngResource("cancel", 16, 16);

        public static readonly PngResource SelectorBackground = new PngResource("selectorBack", 32, 32);
        public static readonly PngResource SelectorSelectedBackground = new PngResource("selectorBackSelected", 32, 32);
        public static readonly PngResource SelectorRootBackground = new PngResource("selectorRootBack", 32, 32);
        public static readonly PngResource SelectorRootSelectedBackground = new PngResource("selectorRootBackSelected", 32, 32);

        public static readonly PngResource ActionBackground = new PngResource("actionBack", 32, 32, SkinMode.Custom);
        public static readonly PngResource ActionSelectedBackground = new PngResource("actionBackSelected", 32, 32, SkinMode.Custom);
        public static readonly PngResource ActionScoredBackground = new PngResource("actionBackScored", 32, 32, SkinMode.Custom);
        public static readonly PngResource ActionScoredSelectedBackground = new PngResource("actionBackScoredSelected", 32, 32, SkinMode.Custom);
        public static readonly PngResource ActionUnscoredBackground = new PngResource("actionBackUnscored", 32, 32, SkinMode.Custom);
        public static readonly PngResource ActionUnscoredSelectedBackground = new PngResource("actionBackUnscoredSelected", 32, 32, SkinMode.Custom);

        public static readonly PngResource QualifierBackground = new PngResource("qualifierBack", 32, 32);
        public static readonly PngResource QualifierSelectedBackground = new PngResource("qualifierBackSelected", 32, 32);
        public static readonly PngResource QualifierScoredBackground = new PngResource("qualifierBackScored", 32, 32);
        public static readonly PngResource QualifierScoredSelectedBackground = new PngResource("qualifierBackScoredSelected", 32, 32);
        public static readonly PngResource QualifierUnscoredBackground = new PngResource("qualifierBackUnscored", 32, 32);
        public static readonly PngResource QualifierUnscoredSelectedBackground = new PngResource("qualifierBackUnscoredSelected", 32, 32);

        public static readonly PngResource ScorerBackground = new PngResource("scorerBack", 20, 20);

        //Inspector Icons
        public static readonly PngResource SetRootButtonSmall = new PngResource("setroot", 16, 16, SkinMode.Unity);

        public static readonly PngResource InspectorDragHandle = new PngResource("insreorder", 16, 16);
    }
}
