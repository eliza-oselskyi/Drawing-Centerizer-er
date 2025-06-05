using System;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Strategies;

public class FabViewCenteringStrategy : IViewCenteringStrategy
{
    public bool Center(View view)
    {
        Console.WriteLine("Centering a Fab view...");
        return true;
    }
}