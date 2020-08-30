This project is a Xamarin.Android  finished version of the code lab supplied by Google for the new Navigation Architecture Component using JetPack Navigation at https://codelabs.developers.google.com/codelabs/android-navigation/#0

Google supply in-depth documentation on Android Jetpack's Navigation component at the following link https://developer.android.com/guide/navigation.

The starting point for using the Navigation component is the code lab referred to above as it introduces all the new concepts of the Single Activity/Multiple Fragments approach to building apps which potentially reduces the amount of code for a given app as compared to the older approach of using multiple activities.

If you have used fragments before you will be aware of the complexity of Fragment Transactions and the subtle bugs that can arise from using them. The Navigation Component has the potential to eliminate fragment transactions by automatically handling them within the component. As this code lab shows, there are no fragment transactions anywhere.

Rather than reinvent the wheel I would suggest that you follow all the steps of the code lab even if you don’t fully understand the Kotlin language as this project has already done the conversion to C# for you.

Xamarin doesn’t have a designer for the navigation graph so using Android Studio’s designer does simplify the process of building a navigation graph. However the graph is only xml, so, therefore, there is no need for the designer in Visual Studio once you understand what the designer helps you achieve.  

A couple of tips you may find useful as you are learning the Navigation component. Right-click on any reference to Navigation, NavigationUI or NavController and open and keep open the definition of each as you are developing the code. If you are not familiar with these classes, this is a great way to refer to them.

Step 11 is the only part that is specific to Xamarin as compared to Android. See the following Xamarin docs https://docs.microsoft.com/en-us/xamarin/android/platform/android-manifest re the AndroidManifest.


