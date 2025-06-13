using System.ComponentModel;

namespace Drawing.CenterViewWPF.Common.Enums;

/// <summary>
///     Represents the type of a drawing in the system.
/// </summary>
/// <remarks>
///     The DrawingType enum categorizes drawings into specific types, providing
///     easier identification and differentiation of drawings for various workflows.
/// </remarks>
public enum DrawingType
{
    [Description("A")] Assembly,
    [Description("G")] GeneralArrangement
}