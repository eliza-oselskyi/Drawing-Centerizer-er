using System;
using System.Collections.Generic;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView.Views;
public class FabView : IView
{
    public FabView(View view)
    {
        View = view;
    }

    public View View { get; set; }

    public void Center(IViewVisitor visitor)
    {
        visitor.CenterVisit(this);
    }

    public bool IsValidViewForCenter(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Enum GetViewTypeEnum(IViewVisitor visitor)
    {
        return visitor.GetViewTypeEnumVisit(this);
    }
}

public class GaView : IView
{
    public GaView(View view)
    {
        View = view;
    }

    public View View { get; set; }

    public void Center(IViewVisitor visitor)
    {
        visitor.CenterVisit(this);
    }

    public bool IsValidViewForCenter(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Enum GetViewTypeEnum(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }
}
