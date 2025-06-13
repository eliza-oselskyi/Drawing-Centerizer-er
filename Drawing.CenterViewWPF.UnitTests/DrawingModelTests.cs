using System;
using Drawing.CenterViewWPF.Centering.Strategies;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Drawing.CenterViewWPF.Common.Strategies;
using NUnit.Framework;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterViewWPF.UnitTests
{
    [TestFixture]
    [TestOf(typeof(DrawingModel))]
    public class DrawingModelTests
    {
        [TestCase("Crane Beam Plan")]
        [TestCase("Cross Section")]
        public void IsValidToCenter_ValidGaDrawing_ReturnsTrue(string viewType)
        {
            try
            {
                var d = new GADrawing();
                var v = new View(d.GetSheet(), new CoordinateSystem(), new CoordinateSystem(), new AABB());
                v.Insert();
                v.SetUserProperty("ViewType", viewType);
                v.Modify();

                var drawingModel = new DrawingModel(d);
                var view = new Centering.TeklaWrapper.View(v, new GetGaViewTypeStrategy());

                Assert.That(drawingModel.ToString() == "G", Is.True, "Should be a GA drawing");
                Assert.That(drawingModel.IsValid, Is.True, "Should be valid");
            }
            catch (TypeInitializationException e)
            {
                Assert.Inconclusive("Connection to Tekla Structures not established.");
            }
        }

        [TestCase("TEST")]
        [TestCase("")]
        public void IsNotValidToCenter_InvalidGaDrawing_ReturnsFalse(string viewType)
        {
            try
            {
                var d = new GADrawing();
                var v = new View(d.GetSheet(), new CoordinateSystem(), new CoordinateSystem(), new AABB());
                v.Insert();
                v.SetUserProperty("ViewType", viewType);
                v.Modify();

                var drawingModel = new DrawingModel(d);
                var view = new Centering.TeklaWrapper.View(v, new GetGaViewTypeStrategy());

                Assert.That(drawingModel.ToString() == "G", Is.True, "Should be a GA drawing");
                Assert.That(drawingModel.IsValid, Is.False, "Should be invalid");
            }
            catch (TypeInitializationException e)
            {
                Assert.Inconclusive("Connection to Tekla Structures not established.");
            }
        }

        [TestCase("TEST")]
        [TestCase("")]
        public void IsNotValidToCenter_GaDrawingTryCentering_ReturnsFalse(string viewType)
        {
            try
            {
                var d = new GADrawing();
                var v = new View(d.GetSheet(), new CoordinateSystem(), new CoordinateSystem(), new AABB());
                v.Insert();
                v.SetUserProperty("ViewType", viewType);
                v.Modify();

                var drawingModel = new DrawingModel(d);
                var view = new Centering.TeklaWrapper.View(v, new GetGaViewTypeStrategy());

                Assert.That(drawingModel.ToString() == "G", Is.True, "Should be a GA drawing");
                Assert.That(drawingModel.CenterDrawing(new GaViewCenteringStrategy()), Is.False, "Should be invalid");
            }
            catch (TypeInitializationException e)
            {
                Assert.Inconclusive("Connection to Tekla Structures not established.");
            }
        }

        [TestCase("Cross Section")]
        [TestCase("Crane Beam Plan")]
        public void IsValidToCenter_GaDrawingTryCentering_ReturnsTrue(string viewType)
        {
            try
            {
                var d = new GADrawing();
                var v = new View(d.GetSheet(), new CoordinateSystem(), new CoordinateSystem(), new AABB());
                v.Insert();
                v.SetUserProperty("ViewType", viewType);
                v.Modify();

                var drawingModel = new DrawingModel(d);
                var view = new Centering.TeklaWrapper.View(v, new GetGaViewTypeStrategy());

                Assert.That(drawingModel.ToString() == "G", Is.True, "Should be a GA drawing");
                Assert.That(drawingModel.CenterDrawing(new GaViewCenteringStrategy()), Is.True, "Should be valid");
            }
            catch (TypeInitializationException e)
            {
                Assert.Inconclusive("Connection to Tekla Structures not established.");
            }
        }

        [TestCase("Cross Section", "Crane Beam Plan")]
        [TestCase("Crane Beam Plan", "Cross Section")]
        public void IsNotValidToCenter_TwoValidViews_GaDrawingTryCentering_ReturnsFalse(string viewType,
            string viewType2)
        {
            try
            {
                var d = new GADrawing();
                var v = new View(d.GetSheet(), new CoordinateSystem(), new CoordinateSystem(), new AABB());
                var v2 = new View(d.GetSheet(), new CoordinateSystem(), new CoordinateSystem(), new AABB());
                v.Insert();
                v2.Insert();
                v.SetUserProperty("ViewType", viewType);
                v2.SetUserProperty("ViewType", viewType2);
                v.Modify();
                v2.Modify();

                var drawingModel = new DrawingModel(d);

                Assert.That(drawingModel.ToString() == "G", Is.True, "Should be a GA drawing");
                Assert.That(drawingModel.CenterDrawing(new GaViewCenteringStrategy()), Is.False, "Should be invalid");
            }
            catch (TypeInitializationException e)
            {
                Assert.Inconclusive("Connection to Tekla Structures not established.");
            }
        }

        [Test]
        public void IsValidToCenter_OneValidView_FabDrawingTryCentering_ReturnsTrue()
        {
            var model = new Model();

            try
            {
                var beam = new Beam(new Point(0, 0), new Point(100, 100, 100))
                {
                    Profile =
                    {
                        ProfileString = "380*380"
                    }
                };
                beam.Insert();
                var assembly = beam.GetAssembly();
                var d = new AssemblyDrawing(assembly.Identifier);
                var coordinateSystem = new CoordinateSystem(new Point(), new Vector(1, 0, 0), new Vector(0, 1, 0));
                var v = new View(d.GetSheet(),
                    coordinateSystem,
                    coordinateSystem,
                    new AABB(new Point(0,
                            0),
                        new Point(1000,
                            1000,
                            1000)));
                v.Insert();

                var drawingModel = new DrawingModel(d);
                beam.Delete();
                model.CommitChanges();

                Assert.That(drawingModel.ToString() == "A", Is.True, "Should be a Fab drawing");
                Assert.That(drawingModel.CenterDrawing(new FabViewCenteringStrategy()), Is.True, "Should be valid");
            }
            catch (TypeInitializationException e)
            {
                Assert.Inconclusive("Connection to Tekla Structures not established.");
            }
        }

        [Test]
        public void TryCentering_GaDrawingValid()
        {
            var selectedDrawings = DrawingHandler.Instance.GetDrawingSelector().GetSelected();

            if (selectedDrawings.GetSize() != 1) Assert.Inconclusive("More than one drawing selected.");
            selectedDrawings.MoveNext();
            if (selectedDrawings.Current.GetType() != typeof(GADrawing)) Assert.Inconclusive("Should be a GA drawing.");

            var drawingModel = new DrawingModel(selectedDrawings.Current);
            drawingModel.CenterDrawing(new GaViewCenteringStrategy());
        }

        [Test]
        public void TryCentering_FabDrawingValid()
        {
            var selectedDrawings = DrawingHandler.Instance.GetDrawingSelector().GetSelected();

            if (selectedDrawings.GetSize() != 1) Assert.Inconclusive("More than one drawing selected.");
            selectedDrawings.MoveNext();
            if (selectedDrawings.Current.GetType() != typeof(AssemblyDrawing))
                Assert.Inconclusive("Should be a Fab drawing.");

            var drawingModel = new DrawingModel(selectedDrawings.Current);
            drawingModel.CenterDrawing(new FabViewCenteringStrategy());
        }
    }
}