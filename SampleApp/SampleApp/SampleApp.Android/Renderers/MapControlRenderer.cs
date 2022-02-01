using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Baidu.Location;
using Com.Baidu.Mapapi.Map;
using SampleApp.Controls;
using SampleApp.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Com.Baidu.Location.LocationClientOption;

[assembly: ExportRenderer(typeof(MapControl), typeof(MapControlRenderer))]
namespace SampleApp.Droid.Renderers
{
    public class MapControlRenderer : ViewRenderer<MapControl, LinearLayout>
    {
        public MapControlRenderer(Context context):base(context)
        {

        }

        private LinearLayout layout;

        protected override void OnElementChanged(ElementChangedEventArgs<MapControl> e)
        {
            if (layout == null)
            {
                layout = Inflate(Context, Resource.Layout.MapLayout, null) as LinearLayout;
            }

            var mapView = layout.FindViewById<MapView>(Resource.Id.bmapView);

            mapView.Map.MyLocationEnabled = true;

            MainActivity.mapView = mapView;

            var img = BitmapDescriptorFactory.FromResource(Resource.Drawable.monkey);
            var config = new MyLocationConfiguration(MyLocationConfiguration.LocationMode.Following, true, img);
            MainActivity.mapView.Map.SetMyLocationConfigeration(config);

            SetNativeControl(layout);
        }
    }
}