//using System;
//using System.Linq;
//using Android.Widget;
//using TOI_MobileClient.Android;
//using TOI_MobileClient.Dependencies;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using Android.Graphics;

//[assembly: ResolutionGroupName("MyCompany")]
//[assembly: ExportEffect(typeof(LabelShadowEffect), "LabelShadowEffect")]
//namespace TOI_MobileClient.Android
//{
//    public class LabelShadowEffect : PlatformEffect
//    {
//        protected override void OnAttached()
//        {
//            try
//            {
//                var control = Control as StackLayout;
//                var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);
//                if (effect != null)
//                {
//                    float radius = effect.Radius;
//                    float distanceX = effect.DistanceX;
//                    float distanceY = effect.DistanceY;
//                    var color = effect.Color.ToAndroid();
//                    control.SetShadowLayer(radius, distanceX, distanceY, color);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
//            }
//        }

//        protected override void OnDetached()
//        {
//        }

//    }
//}