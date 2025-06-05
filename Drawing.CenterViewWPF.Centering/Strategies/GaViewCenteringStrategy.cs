using System;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Strategies;

public class GaViewCenteringStrategy : IViewCenteringStrategy
{
    public bool Center(View view)
    {
        Console.WriteLine("Centering a GA view...");
        return true;
    }
}