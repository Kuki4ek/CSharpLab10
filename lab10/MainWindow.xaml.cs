using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _lastMousePos;
        private double _angleH = 30, _angleV = 20, _radius = 8;
        private PerspectiveCamera _camera;
        private ModelVisual3D _letterK, _letterS1, _letterS2;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            SliderX.ValueChanged += Slider_ValueChanged;
            SliderY.ValueChanged += Slider_ValueChanged;
            SliderZ.ValueChanged += Slider_ValueChanged;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _camera = new PerspectiveCamera
            {
                Position = new Point3D(10, 7, 10),
                LookDirection = new Vector3D(-3, -3, -5),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 60
            };
            viewport.Camera = _camera;

            var light = new ModelVisual3D
            {
                Content = new Model3DGroup
                {
                    Children =
                    {
                        new DirectionalLight(Colors.White, new Vector3D(0, 0, -1)),
                        //new AmbientLight(Colors.Gray)
                    }
                }
            };
            viewport.Children.Add(light);

            _letterK = AddLetterK(new Point3D(0, 0, 0));
            _letterS1 = AddLetterS(new Point3D(3, 0, 0));
            _letterS2 = AddLetterS(new Point3D(5.5, 0, 0));

            viewport.Children.Add(_letterK);
            viewport.Children.Add(_letterS1);
            viewport.Children.Add(_letterS2);
        }
        private ModelVisual3D AddLetterS(Point3D begin)
        {
            
            var light = new DirectionalLight(Colors.White, new Vector3D(-1, -1, -2));

            int count = 9;
            Point3D[] points = new Point3D[count*4];
            double start = 60;
            double length = 240;
            double in_radius = 0.75;
            double out_radius = 1.5;
            Point3D center = new Point3D(begin.X + out_radius, begin.Y + out_radius, begin.Z);
            int direction = 1;
            double step = (length / (count - 1)) * direction;
            double z_length = -1.0;

            for (int i = 0; i < count; i++)
            {
                double angle = (start + step * i) * Math.PI / 180;
                points[i] = new Point3D(
                    center.X + in_radius * Math.Cos(angle),
                    center.Y + in_radius * Math.Sin(angle),
                    center.Z
                );
            }

            for (int i = 0; i < count; i++)
            {
                double angle = (start + step * i) * Math.PI / 180;
                points[count + i] = new Point3D(
                    center.X + out_radius * Math.Cos(angle),
                    center.Y + out_radius * Math.Sin(angle),
                    center.Z
                );
            }
            for (int i = 0; i < count; i++)
            {
                double angle = (start + step * i) * Math.PI / 180;
                points[count*2 + i] = new Point3D(
                    center.X + in_radius * Math.Cos(angle),
                    center.Y + in_radius * Math.Sin(angle),
                    center.Z + z_length
                );
            }

            for (int i = 0; i < count; i++)
            {
                double angle = (start + step * i) * Math.PI / 180;
                points[count*3 + i] = new Point3D(
                    center.X + out_radius * Math.Cos(angle),
                    center.Y + out_radius * Math.Sin(angle),
                    center.Z + z_length
                );
            }
            var mesh = new MeshGeometry3D
            {
                Positions = new Point3DCollection(points),
                TriangleIndices = new Int32Collection
                {
                    0,9,10,
                    0,10,1,
                    1,10,11,
                    1,11,2,
                    2,11,12,
                    2,12,3,
                    3,12,13,
                    3,13,4,
                    4,13,14,
                    4,14,5,
                    5,14,15,
                    5,15,6,
                    6,15,16,
                    6,16,7,
                    7,16,17,
                    7,17,8,

                    28,27,18,
                    19,28,18,
                    29,28,19,
                    20,29,19,
                    30,29,20,
                    21,30,20,
                    31,30,21,
                    22,31,21,
                    32,31,22,
                    23,32,22,
                    33,32,23,
                    24,33,23,
                    34,33,24,
                    25,34,24,
                    35,34,25,
                    26,35,25,



                    0,18,9,
                    18,27,9,

                    0,1,19,
                    19,18,0,
                    1,2,20,
                    20,19,1,
                    2,3,21,
                    21,20,2,
                    3,4,22,
                    22,21,3,
                    4,5,23,
                    23,22,4,
                    5,6,24,
                    24,23,5,
                    6,7,25,
                    25,24,6,
                    7,8,26,
                    26,25,7,

                    17,35,26,
                    17,26,8,

                    9,27,28,
                    9,28,10,
                    10,28,29,
                    10,29,11,
                    11,29,30,
                    11,30,12,
                    12,30,31,
                    12,31,13,
                    13,31,32,
                    13,32,14,
                    14,32,33,
                    14,33,15,
                    15,33,34,
                    15,34,16,
                    16,34,35,
                    16,35,17
                }
            };


            var material = new DiffuseMaterial(new SolidColorBrush(Colors.SkyBlue));
            var model = new GeometryModel3D(mesh, material);
            var modelGroup = new Model3DGroup();
            //modelGroup.Children.Add(light);
            modelGroup.Children.Add(model);

            return new ModelVisual3D { Content = modelGroup };
        }
        private ModelVisual3D AddLetterK(Point3D begin)
        {
            Point3D left_down = begin;
            double a = 0.1,
                    b = 1,
                    c = 1,
                    d = 3,
                    e = 1,
                    f = 1,
                    g = -1;
            var light = new DirectionalLight(Colors.White, new Vector3D(-1, -1, -2));
            Point3D[] points =
            {
                new Point3D(left_down.X             , left_down.Y           , left_down.Z       ),  // 0
                new Point3D(left_down.X             , left_down.Y + d       , left_down.Z       ),  // 1
                new Point3D(left_down.X + c         , left_down.Y           , left_down.Z       ),  // 2
                new Point3D(left_down.X + c         , left_down.Y + d       , left_down.Z       ),  // 3
                new Point3D(left_down.X + c         , left_down.Y + d/2 + a , left_down.Z       ),  // 4
                new Point3D(left_down.X + c + f     , left_down.Y + d       , left_down.Z       ),  // 5
                new Point3D(left_down.X + c + f + e , left_down.Y + d       , left_down.Z       ),  // 6
                new Point3D(left_down.X + c + b     , left_down.Y + d/2     , left_down.Z       ),  // 7
                new Point3D(left_down.X + c + f + e , left_down.Y           , left_down.Z       ),  // 8
                new Point3D(left_down.X + c + f     , left_down.Y           , left_down.Z       ),  // 9
                new Point3D(left_down.X + c         , left_down.Y + d/2 - a , left_down.Z       ),  // 10
                new Point3D(left_down.X             , left_down.Y           , left_down.Z + g   ),  // 11
                new Point3D(left_down.X             , left_down.Y + d       , left_down.Z + g   ),  // 12
                new Point3D(left_down.X + c         , left_down.Y           , left_down.Z + g   ),  // 13
                new Point3D(left_down.X + c         , left_down.Y + d       , left_down.Z + g   ),  // 15
                new Point3D(left_down.X + c         , left_down.Y + d/2 + a , left_down.Z + g   ),  // 16
                new Point3D(left_down.X + c + f     , left_down.Y + d       , left_down.Z + g   ),  // 17
                new Point3D(left_down.X + c + f + e , left_down.Y + d       , left_down.Z + g   ),  // 18
                new Point3D(left_down.X + c + b     , left_down.Y + d/2     , left_down.Z + g   ),  // 19
                new Point3D(left_down.X + c + f + e , left_down.Y           , left_down.Z + g   ),  // 20
                new Point3D(left_down.X + c + f     , left_down.Y           , left_down.Z + g   ),  // 21
                new Point3D(left_down.X + c         , left_down.Y + d/2 - a , left_down.Z + g   )   // 22
            };

            var mesh = new MeshGeometry3D
            {
                Positions = new Point3DCollection(points),
                TriangleIndices = new Int32Collection
                {
                    0,2,1,
                    1,2,3,

                    4,5,6,
                    4,6,7,

                    4,7,10,

                    7,8,10,
                    8,9,10,


                    11,13,12,
                    12,13,14,

                    15,16,17,
                    15,17,18,

                    15,18,21,

                    18,19,21,
                    19,20,21,


                    0,1,11,
                    11,1,12,

                    1,3,12,
                    3,14,12,

                    3,4,15,
                    3,15,14,

                    4,16,5,
                    4,15,16,

                    5,17,6,
                    5,16,17,

                    6,18,7,
                    6,17,18,

                    7,19,8,
                    7,18,19,

                    8,9,20,
                    20,19,8,

                    21,10,9,
                    9,20,21,

                    2,10,13,
                    10,21,13,

                    0, 2, 13,
                    11, 0, 13
                }
            };
            

            var material = new DiffuseMaterial(new SolidColorBrush(Colors.SkyBlue));

            var model = new GeometryModel3D(mesh, material);
            model.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.LightSkyBlue));


            var modelGroup = new Model3DGroup();
            //modelGroup.Children.Add(light);
            modelGroup.Children.Add(model);

            return new ModelVisual3D { Content = modelGroup };
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double offsetX = SliderX.Value;
            double offsetY = SliderY.Value;
            double offsetZ = SliderZ.Value;
            

            _letterK.Transform = new TranslateTransform3D(offsetX, offsetY, 0);
            _letterS1.Transform = new TranslateTransform3D(offsetX, offsetY, 0);
            _letterS2.Transform = new TranslateTransform3D(offsetX, offsetY, 0);

            Rotation3D rotation3d = new AxisAngleRotation3D(new Vector3D(1, 0, 0), offsetZ);
            Point3D center = new Point3D(4.5, 1.5, 0);
            _letterK.Transform = new RotateTransform3D(rotation3d, center);
            _letterS1.Transform = new RotateTransform3D(rotation3d, center);
            _letterS2.Transform = new RotateTransform3D(rotation3d, center);


        }
    }
}