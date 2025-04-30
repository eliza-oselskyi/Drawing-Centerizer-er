using System;
using System.Collections.Generic;

namespace Drawing.CenterView.Views;
public interface IView
{
    void Center(IViewVisitor visitor);
    bool IsValidViewForCenter(IViewVisitor visitor);
    Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor);
    Enum GetViewTypeEnum(IViewVisitor visitor);
}
