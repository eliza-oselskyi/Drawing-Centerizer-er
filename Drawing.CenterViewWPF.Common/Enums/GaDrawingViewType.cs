using System.ComponentModel;

namespace Drawing.CenterViewWPF.Common.Enums;

/// <summary>
///     Enum representing various types of General Arrangement (GA) drawing views used in a drawing package.
/// </summary>
public enum GaDrawingViewType
{
    [Description("None")] None,
    [Description("Cover Sheet")] CoverSheet,
    [Description("Building Sheet 1")] BuildingSheetOne,
    [Description("Building Sheet 2")] BuildingSheetTwo,

    [Description(
        "Plane Identification Plan")]
    // TODO: Does this need to be excluded from the valid centering candidates?
    PlaneIdentificationPlan,
    [Description("Anchor Rod Plan")] AnchorRodPlan,
    [Description("Base Plate Details")] BasePlateDetails,
    [Description("Reactions")] Reactions,
    [Description("Shakeout Plan")] ShakeOut,

    [Description("Roof Framed Opening Plan")]
    RoofFramedOpeningPlan,
    [Description("Crane Beam Plan")] CraneBeamPlan,
    [Description("Cross Section")] CrossSection,
    [Description("Portal Cross Section")] PortalCrossSection,
    [Description("Roof Framing Plan")] RoofFramingEverything,

    [Description("Roof Framing Plan - Secondary")]
    RoofFramingSecondaryOnly,

    [Description("Roof Framing Plan - Openings")]
    RoofFramingOpeningsOnly,

    [Description("Roof Framing Plan - Purlin Bracing")]
    RoofFramingPurlinBracingOnly,
    [Description("Endwall Framing")] EndwallFraming,

    [Description("Endwall Partition Framing")]
    EndWallPartitionFraming,
    [Description("Sidewall Framing")] SidewallFraming,

    [Description("Sidewall Partition Framing")]
    SidewallPartitionFraming,
    [Description("Wall Sheeting")] WallSheeting,

    [Description("Partition Wall Sheeting")]
    PartitionWallSheeting,
    [Description("Roof Sheeting")] RoofSheeting,
    [Description("Wall Liner")] WallLiner,
    [Description("Partition Wall Liner")] PartitionWallLiner,
    [Description("Roof Liner")] RoofLiner,

    [Description("Roof Panel Clip Layout")]
    PanelClipLayout,

    [Description("Mezzanine Plan - Framing Only")]
    MezzaninePlan,

    [Description("Mezzanine Plan - Joists Only")]
    MezzaninePlanJoistsOnly,

    [Description("Mezzanine Plan - Decking")]
    MezzaninePlanDeckingOnly
}