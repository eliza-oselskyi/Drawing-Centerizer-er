using System;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Interfaces;

public interface IGetViewTypeStrategy
{
    Enum GetViewType(View view, out string typeString);
}