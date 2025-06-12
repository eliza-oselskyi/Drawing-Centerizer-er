using System;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Interfaces;

public interface IGetViewTypeStrategy
{
    Enum GetViewType(ViewBase view, out string typeString);
}