using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Assembly = Tekla.Structures.Model.Assembly;
using Object = Tekla.Structures.Model.Object;
using CV = Drawing.CenterView;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterView.UnitTests
{
    [TestFixture]
    public class QuickCenterTests
    {
        private Model _model;
        private DrawingHandler _drawingHandler;

        // TODO: Figure out how to do a setup and teardown for this test.

        [SetUp]
        public Tuple<ViewBase, Point> CenterViewSetup()
        {
            _drawingHandler = new DrawingHandler();
            var drawing = _drawingHandler.GetActiveDrawing();
            var views = drawing.GetSheet().GetViews();
            Point origViewOrigin;
            if (views.GetSize() == 1)
            {
                views.MoveNext();
                var view = views.Current as View;
                origViewOrigin = view.Origin;
                views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes); // Get viewTypes
                var type = PluginForm.GetViewTypeEnum(viewTypes);

                if (type is PluginForm.ViewType.None)
                {
                    Assert.Ignore("No view valid type found");
                }
                else
                {
                    return new Tuple<ViewBase, Point>((ViewBase)view, origViewOrigin);
                }
            }
            else if (views.GetSize() > 1)
            {
                int member = 0;
                var viewList = new ArrayList();

                while (views.MoveNext())
                {
                    var curr = views.Current as View;
                    if (curr == null) Assert.Inconclusive("Null current view found");
                    curr.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                    var type = PluginForm.GetViewTypeEnum(viewTypes);

                    if (type != PluginForm.ViewType.None)
                    {
                        member++;
                        viewList.Add(curr);
                    }
                }

                if (member == 1)
                {
                    var viewArray = (ViewBase[])viewList.ToArray();
                    origViewOrigin = viewArray[0].Origin;
                    return new Tuple<ViewBase, Point>(viewArray[0], origViewOrigin);
                }
                else if (member > 1)
                {
                    Assert.Inconclusive("More than one \"valid\" view found");
                }
            }
            else
            {
                Assert.Inconclusive("No views found");
            }

            return null;
        }

        // TODO: Refactor these tests to use the Setup and Teardown methods
        [Test]
        public void CenterView_NothingToDo_ReturnNC()
        {
            _drawingHandler = new DrawingHandler();
            var drawing = _drawingHandler.GetActiveDrawing();
            var views = drawing.GetSheet().GetViews();
            if (views.GetSize() == 1)
            {
                views.MoveNext();
                var view = views.Current as View;
                var origViewOrigin = view.Origin;
                views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes); // Get viewTypes
                var type = PluginForm.GetViewTypeEnum(viewTypes);

                if (type is PluginForm.ViewType.None)
                {
                    Assert.Ignore("No view valid type found");
                }
                else
                {
                    if (view == null) Assert.Inconclusive("Null view found");
                    Console.WriteLine("\n\nTest View Origin: " + view.Origin.ToString());
                    var result = QuickCenterClass.CenterView((ViewBase)view, (int)type,
                        out Tuple<Tekla.Structures.Drawing.Drawing, string> drawingTuple);
                    Assert.True(result.Contains("Nothing To Do.") && drawingTuple.Item2.Equals("NC"),
                        "Fail, Had something to do.");
                }

                //Reset everything
                view.Origin = origViewOrigin;
                view.Modify();
            }
            else if (views.GetSize() > 1)
            {
                int member = 0;

                while (views.MoveNext())
                {
                    var curr = views.Current as View;
                    if (curr == null) Assert.Inconclusive("Null current view found");
                    curr.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                    var type = PluginForm.GetViewTypeEnum(viewTypes);

                    if (type != PluginForm.ViewType.None)
                    {
                        member++;
                    }
                }

                if (member > 1)
                {
                    Assert.Inconclusive("More than one \"valid\" view found");
                }
            }
            else
            {
                Assert.Inconclusive("No views found");
            }
        }

        [Test]
        public void CenterView_SomethingToDo_ReturnC()
        {
            _drawingHandler = new DrawingHandler();
            var drawing = _drawingHandler.GetActiveDrawing();
            var views = drawing.GetSheet().GetViews();
            if (views.GetSize() == 1)
            {
                views.MoveNext();
                var view = views.Current as View;
                views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes); // Get viewTypes
                var type = PluginForm.GetViewTypeEnum(viewTypes);

                if (type is PluginForm.ViewType.None)
                {
                    Assert.Ignore("No view valid type found");
                }
                else
                {
                    if (view == null) Assert.Inconclusive("Null view found");
                    Console.WriteLine("\n\nTest View Origin: " + view.Origin.ToString());
                    var result = QuickCenterClass.CenterView((ViewBase)view, (int)type,
                        out Tuple<Tekla.Structures.Drawing.Drawing, string> drawingTuple);
                    Assert.True(result.Contains("Centering") && drawingTuple.Item2.Equals("C"),
                        "Fail, Had nothing to do.");
                }
            }
            else if (views.GetSize() > 1)
            {
                int member = 0;

                while (views.MoveNext())
                {
                    var curr = views.Current as View;
                    if (curr == null) Assert.Inconclusive("Null current view found");
                    curr.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                    var type = PluginForm.GetViewTypeEnum(viewTypes);

                    if (type != PluginForm.ViewType.None)
                    {
                        member++;
                    }
                }

                if (member > 1)
                {
                    Assert.Inconclusive("More than one \"valid\" view found");
                }
            }
            else
            {
                Assert.Inconclusive("No views found");
            }
        }
    }
}