using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Com.Baidu.Mapapi;
using Com.Baidu.Location;
using static Com.Baidu.Location.LocationClientOption;
using Xamarin.Forms;
using Com.Baidu.Mapapi.Map;

namespace SampleApp.Droid
{
    [Activity(Label = "SampleApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static LocationClient locationClient;

        public static MapView mapView;

        private MapControlLocationListener myLocationListener;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            if (locationClient == null)
            {
                //定位初始化
                locationClient = new LocationClient(ApplicationContext);

                //注册LocationListener监听器
                myLocationListener = new MapControlLocationListener();
                locationClient.RegisterLocationListener(myLocationListener);

                LocationClientOption option = new LocationClientOption();
                
                option.SetLocationMode(LocationMode.HightAccuracy);
                //可选，设置定位模式，默认高精度
                //LocationMode.Hight_Accuracy：高精度；
                //LocationMode. Battery_Saving：低功耗；
                //LocationMode. Device_Sensors：仅使用设备；

                option.LocationNotify = true;
                //可选，设置是否当GPS有效时按照1S/1次频率输出GPS结果，默认false

                option.ScanSpan = 1001;
                //可选，设置发起定位请求的间隔，int类型，单位ms
                //如果设置为0，则代表单次定位，即仅定位一次，默认为0
                //如果设置非0，需设置1000ms以上才有效

                option.SetNeedNewVersionRgc(true);
                //可选，设置是否需要最新版本的地址信息。默认需要，即参数为true

                option.SetIsNeedAddress(true);

                option.IsNeedAltitude = true;

                //option.CoorType = "bd09ll";
                //可选，设置返回经纬度坐标类型，默认GCJ02
                //GCJ02：国测局坐标；
                //BD09ll：百度经纬度坐标；
                //BD09：百度墨卡托坐标；
                //海外地区定位，无需设置坐标类型，统一返回WGS84类型坐标

                option.OpenGps = true;
                //可选，设置是否使用gps，默认false
                //使用高精度和仅用设备两种定位模式的，参数必须设置为true

                //option.setIgnoreKillProcess(false);
                //可选，定位SDK内部是一个service，并放到了独立进程。
                //设置是否在stop的时候杀死这个进程，默认（建议）不杀死，即setIgnoreKillProcess(true)

                option.SetIgnoreCacheException(false);
                //可选，设置是否收集Crash信息，默认收集，即参数为false

                option.SetWifiCacheTimeOut(1000);
                //可选，V7.2版本新增能力
                //如果设置了该接口，首次启动定位时，会先判断当前Wi-Fi是否超出有效期，若超出有效期，会先重新扫描Wi-Fi，然后定位

                //option.setEnableSimulateGps(false);
                //可选，设置是否需要过滤GPS仿真结果，默认需要，即参数为false

                locationClient.LocOption = option;
                //mLocationClient为第二步初始化过的LocationClient对象
                //需将配置好的LocationClientOption对象，通过setLocOption方法传递给LocationClient对象使用
                //更多LocationClientOption的配置，请参照类参考中LocationClientOption类的详细说明

                MessagingCenter.Subscribe<object, BDLocation>(this, MessageString.ReceiveLocation, (_, location) =>
                {
                    //mapView 销毁后不在处理新接收的位置
                    if (location == null || mapView == null)
                    {
                        return;
                    }
                    else
                    {
                        MyLocationData locData = new MyLocationData.Builder()
                            .Accuracy(location.Radius)
                            // 此处设置开发者获取到的方向信息，顺时针0-360
                            .Direction(location.Direction)
                            .Latitude(location.Latitude)
                            .Longitude(location.Longitude)
                            .Build();
                        mapView.Map.SetMyLocationData(locData);
                    }
                });
            }

            SDKInitializer.Initialize(Application);

            //开启地图定位图层
            locationClient.Start();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}