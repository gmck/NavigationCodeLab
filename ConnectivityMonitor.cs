using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Util;
using Java.Net;
//using Microsoft.AppCenter.Crashes;

namespace com.companyname.NavigationCodeLab
{
    public class ConnectivityMonitor
    {
        private readonly string logTag = "ConnectivityMonitor";
        private readonly Context context;
        private ConnectivityManager connectivityManager;
        private NetworkCallbackClass networkCallbackClass;
        public NetworkStatus NetworkStatus;

        public ConnectivityMonitor(Context context)
        {
            this.context = context;
        }

        // RegisterDefaultNetworkCallback available API 24+ Android 7+
        public void RegisterDefaultNetworkCallback()
        {
            try
            {
                connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                networkCallbackClass = new NetworkCallbackClass(this);
                connectivityManager?.RegisterDefaultNetworkCallback(networkCallbackClass);
            }
            catch (Exception ex)
            {
                Log.Debug(logTag, ex.Message);
            }
            Log.Debug(logTag, "RegisterDefaultNetworkCallback");
        }


        public void RegisterNetworkCallback()
        {
            try
            {
                NetworkRequest networkRequest = new NetworkRequest.Builder()
                   .AddTransportType(TransportType.Wifi)
                   .AddTransportType(TransportType.Cellular)
                   .Build();

                connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                networkCallbackClass = new NetworkCallbackClass(this);

                connectivityManager?.RegisterNetworkCallback(networkRequest, networkCallbackClass);
            }
            catch (Exception ex)
            {
                Log.Debug(logTag, ex.Message);
            }
            Log.Debug(logTag, "RegisterNetworkCallback");
        }

        public void UnregisterNetworkCallback()
        {
            try
            {
                connectivityManager?.UnregisterNetworkCallback(networkCallbackClass);
            }
            catch (Java.Lang.IllegalArgumentException)
            {
                Log.Debug(logTag, "NetworkCallback was already unregistered");
            }
            Log.Debug(logTag, "UnregisterNetworkCallback");
        }

        #region IsHostReachable - This is fast.
        public async Task<bool> IsHostReachableAsync(string host)
        {
            string logTag = "IsHostReachable";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool isReached = false;
            try
            {
                await Task.Run(async () =>
                {
                    URL url = new URL(host);
                    HttpURLConnection urlc = (HttpURLConnection)url.OpenConnection();
                    urlc.SetRequestProperty("User-Agent", "Android Application");
                    urlc.SetRequestProperty("Connection", "close");  // 21/01/2018 No need to disconnect as this close does it.
                    urlc.ConnectTimeout = 4500; // 01/03/2019 changed from 3000 to 6000 too slow. Changed 28/05/2019 from 2500 to 4500 to see if we can lessen the number of failures in App Center

                    await urlc.ConnectAsync();    // Connect will give a Java.IO.IOException

                    isReached = urlc.ResponseCode == HttpStatus.Ok | urlc.ResponseCode == HttpStatus.Forbidden;
                });
            }
            catch (AggregateException ae)
            {
                ReadOnlyCollection<Exception> innerExceptions = ae.InnerExceptions;
                if (innerExceptions.Count > 0)
                {
                    Exception innerException = innerExceptions[0];
                    string message = innerException.Message;
                    Dictionary<string, string> crashProperties = new Dictionary<string, string>
                    {
                        { "AggregateException Message", message }
                    };
                    //Crashes.TrackError(ae, crashProperties);
                }

                ae.Handle((x) =>
                {
                    if (x is Java.Net.UnknownHostException | x is Java.Net.SocketTimeoutException | x is Java.IO.IOException)
                    {
                        // Just catch this, until I figure out what is causing this exception
                        // Should be returning true for isReached for a network with networkCapabilities.HasTransport(TransportType.Cellular)), while device is connected to a wifi scantool
                        // if the device has Cellular capability. 23/09/2018 Don't think this (TransportType.Cellular) is possible when you have a wifi scan tool connected
                        return true;  // prevent it crashing, if our server is down
                    }
                    return false; // Let anything else stop the application.
                });
            }
            catch (Java.Net.UnknownHostException ex)
            {
                // Misspelt url - Can't use InnerException here - it appears to be null.
                string message = ex.Message;
                Dictionary<string, string> crashProperties = new Dictionary<string, string>
                {
                    {"Message", message }
                };
                //Crashes.TrackError(ex, crashProperties);
            }
            catch (Java.Net.SocketTimeoutException ex)
            {
                // 28/05/2019 This seems to be common when timeout is 2500, changed to 4500
                string message = ex.Message;
                Dictionary<string, string> crashProperties = new Dictionary<string, string>
                {
                    {"Message", message }
                };
                //Crashes.TrackError(ex, crashProperties);
            }
            catch (Java.IO.IOException ex)
            {
                // Unlikely
                string message = ex.Message;
                Dictionary<string, string> crashProperties = new Dictionary<string, string>
                {
                    {"Message", message }
                };
                //Crashes.TrackError(ex, crashProperties);
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            Log.Debug(logTag, "Time for IsHostRechable to Connect " + elapsedTime.ToString());

            return isReached;
        }
        #endregion


        private class NetworkCallbackClass : ConnectivityManager.NetworkCallback
        {
            private readonly string logTag = "NetworkCallbackClass";
            private readonly ConnectivityMonitor connectivityMonitor;

            public NetworkCallbackClass(ConnectivityMonitor connectivityMonitor)
            {
                this.connectivityMonitor = connectivityMonitor;
                connectivityMonitor.NetworkStatus = NetworkStatus.Disconnected; // Tried Unknown went back to Disconnected
            }

           
            public override void OnAvailable(Network network)
            {
                base.OnAvailable(network);
                Log.Debug(logTag, "OnAvailable");
                //NetworkAvailable?.Invoke(network);

                // Meant to work with Android N+ but my Samsung 7.1.1 tablet didn't - but does with the following
                #pragma warning disable CS0618 // Type or member is obsolete
                if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                {
                    NetworkInfo networkInfo = connectivityMonitor.connectivityManager.ActiveNetworkInfo;
                    if (networkInfo != null & networkInfo.IsConnected)
                    {
                        if (networkInfo.Type == ConnectivityType.Wifi & !string.IsNullOrEmpty(networkInfo.ExtraInfo))
                        {
                            if (networkInfo.ExtraInfo.Contains("OBD"))
                                connectivityMonitor.NetworkStatus = NetworkStatus.WifiScantoolConnected;
                            else
                                connectivityMonitor.NetworkStatus = NetworkStatus.WifiConnected;
                        }
                        else if (networkInfo.Type == ConnectivityType.Wifi & string.IsNullOrEmpty(networkInfo.ExtraInfo))
                            connectivityMonitor.NetworkStatus = NetworkStatus.WifiConnected;
                        else if (networkInfo.Type == ConnectivityType.Mobile)
                            connectivityMonitor.NetworkStatus = NetworkStatus.CellularConnected;
                    }
                    else
                        connectivityMonitor.NetworkStatus = NetworkStatus.Disconnected;
                }
                #pragma warning restore CS0618 // Type or member is obsolete
                else
                    connectivityMonitor.NetworkStatus = NetworkStatus.Connected;

            }

            public override void OnLost(Network network)
            {
                base.OnLost(network);

                Log.Debug(logTag, "OnLost: " + network.ToString());

                // Momentarily will be disconnected, until the next network provides an OnAvailable.
                // Airplane mode will get as many onLost's as there are connected networks as each gets an OnLost
                connectivityMonitor.NetworkStatus = NetworkStatus.Disconnected;
            }

            public override void OnBlockedStatusChanged(Network network, bool blocked)
            {
                base.OnBlockedStatusChanged(network, blocked);
                Log.Debug(logTag, "OnBlockedStatusChanged");
            }

            public override void OnCapabilitiesChanged(Network network, NetworkCapabilities networkCapabilities)
            {
                base.OnCapabilitiesChanged(network, networkCapabilities);

                NetworkStatus networkStatus;

                if (networkCapabilities.HasTransport(TransportType.Wifi) && networkCapabilities.HasCapability(NetCapability.Validated))
                    networkStatus = NetworkStatus.WifiConnected;
                else if (networkCapabilities.HasTransport(TransportType.Cellular))
                    networkStatus = NetworkStatus.CellularConnected;
                else if (networkCapabilities.HasTransport(TransportType.Wifi) && !networkCapabilities.HasCapability(NetCapability.Validated))
                    networkStatus = NetworkStatus.WifiScantoolConnected;
                else
                    networkStatus = NetworkStatus.Disconnected;

                connectivityMonitor.NetworkStatus = networkStatus;

                Log.Debug(logTag, "OnCapabilitiesChanged - " + connectivityMonitor.NetworkStatus.ToString());
            }

            public override void OnLinkPropertiesChanged(Network network, LinkProperties linkProperties)
            {
                base.OnLinkPropertiesChanged(network, linkProperties);

                // On Android 10 it appears as if it is possible to have TransportType.Cellular while connected to the Wifi scan tool, so that DnsServers.Count can be greater than zero.
                // Need to get around that, because if connected to a wifi scan tool, we want to report that. Can we somehow force it?
                // Devices less than Android 10 can't have a cellular connection while connected to a wifi scan tool.
                // The problem appears to be fixed by not unregistering the default callback in the OnPause of the mainactivity. 
                // So effectively we register the default callback (RegisterDefaultNetworkCallback) but we leave it to the system to unregister it when the app ends.

                // Distinguish between a Wi-Fi scan tool and normal Wi-Fi and Cellular networks
                if (linkProperties.DnsServers.Count == 0)
                    connectivityMonitor.NetworkStatus = NetworkStatus.WifiScantoolConnected;

                Log.Debug(logTag, "OnLinkPropertiesChanged - DNS Server Count " + linkProperties.DnsServers.Count.ToString());
                Log.Debug(logTag, "OnLinkPropertiesChanged - " + connectivityMonitor.NetworkStatus.ToString());
            }

            public override void OnUnavailable()
            {
                base.OnUnavailable();
                Log.Debug(logTag, "OnUnavailable");
                // Added 29/07/2020
                connectivityMonitor.NetworkStatus = NetworkStatus.Disconnected;
            }


        }
    }

    public enum NetworkStatus
    {
        Connected,
        CellularConnected,
        WifiConnected,
        WifiScantoolConnected,
        Disconnected
    }
}