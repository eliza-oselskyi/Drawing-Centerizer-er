using NUnit.Framework;
using System;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView.UnitTests;

public class TestClass
{
    [Test]
    public void TestMethod1()
    {
        var drawingHandler = new Tekla.Structures.Drawing.DrawingHandler();
        var allDwgs = drawingHandler.GetDrawingSelector().GetSelected();
        while (allDwgs.MoveNext())
        {
            if (allDwgs.Current is GADrawing)
            {
                drawingHandler.SetActiveDrawing(allDwgs.Current, false);
                Console.WriteLine();
               var x = allDwgs.Current.GetSheet().GetAllViews();
                Console.WriteLine(((GADrawing)allDwgs.Current).Name + " : " + x.GetSize());
               drawingHandler.CloseActiveDrawing();

            }
        }
    }
}