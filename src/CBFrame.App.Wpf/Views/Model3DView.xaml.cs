using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using CBFrame.App.Wpf.Model;
using CBFrame.App.Wpf.ViewModels.Panels;

namespace CBFrame.App.Wpf.Views
{
    public partial class Model3DView : UserControl
    {
        private Model3DViewModel? _viewModel;

        public Model3DView()
        {
            InitializeComponent();

            // Watch for DataContext changes so we can hook/unhook the document.
            DataContextChanged += Model3DView_DataContextChanged;
        }

        private void Model3DView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                UnhookDocument(_viewModel.Document);
            }

            _viewModel = DataContext as Model3DViewModel;

            if (_viewModel != null)
            {
                HookDocument(_viewModel.Document);
                RedrawScene();
            }
        }

        private void HookDocument(FrameDocument document)
        {
            document.Nodes.CollectionChanged += Document_CollectionChanged;
            document.Members.CollectionChanged += Document_CollectionChanged;
        }

        private void UnhookDocument(FrameDocument document)
        {
            document.Nodes.CollectionChanged -= Document_CollectionChanged;
            document.Members.CollectionChanged -= Document_CollectionChanged;
        }

        private void Document_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RedrawScene();
        }

        private void RedrawScene()
        {
            if (_viewModel == null)
                return;

            var document = _viewModel.Document;

            SceneCanvas.Children.Clear();

            // Draw members first (so nodes are on top)
            foreach (var member in document.Members)
            {
                var line = new Line
                {
                    X1 = member.Start.X,
                    Y1 = member.Start.Y,
                    X2 = member.End.X,
                    Y2 = member.End.Y,
                    Stroke = Brushes.LightSteelBlue,
                    StrokeThickness = 2
                };

                SceneCanvas.Children.Add(line);
            }

            // Draw nodes as small circles
            const double radius = 4.0;

            foreach (var node in document.Nodes)
            {
                var ellipse = new Ellipse
                {
                    Width = radius * 2,
                    Height = radius * 2,
                    Fill = Brushes.Orange
                };

                Canvas.SetLeft(ellipse, node.Position.X - radius);
                Canvas.SetTop(ellipse, node.Position.Y - radius);

                SceneCanvas.Children.Add(ellipse);
            }
        }

        private void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel is null)
                return;

            // Use canvas coordinates so clicks line up with drawing.
            Point p = e.GetPosition(SceneCanvas);
            var world = new Point3D(p.X, p.Y, 0);

            _viewModel.OnViewportLeftClick(world);
            Focus();
        }

        private void Viewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (_viewModel is null)
                return;

            Point p = e.GetPosition(SceneCanvas);
            var world = new Point3D(p.X, p.Y, 0);

            _viewModel.OnViewportMouseMove(world);
        }
    }
}
