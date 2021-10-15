/*
 * Modified MIT License
 *
 * Copyright 2021 OneSignal
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * 1. The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * 2. All copies of substantial portions of the Software may only be used in connection
 * with services provided by OneSignal.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using Laters;
using UnityEngine;

namespace OneSignalSDK {
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class OneSignalIOS : OneSignal {
        private delegate void BooleanResponseDelegate(bool response);
        private delegate void StringResponseDelegate(string response);
        private delegate void StateChangeDelegate(string current, string previous);
        
        private interface ICallbackProxy<in TReturn> {
            void OnResponse(TReturn response);
        }

        private abstract class CallbackProxy<TReturn> : BaseLater<TReturn>, ICallbackProxy<TReturn> {
            public abstract void OnResponse(TReturn response);
        }
        
        private sealed class BooleanCallbackProxy : CallbackProxy<bool> {
            [AOT.MonoPInvokeCallback(typeof(BooleanResponseDelegate))]
            public override void OnResponse(bool response) => _complete(response);
        }
        
        private sealed class StringCallbackProxy : CallbackProxy<string> {
            [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
            public override void OnResponse(string response) => _complete(response);
        }
        
        /*
         * Global Callbacks
         */

        private static OneSignalIOS _instance;

        /// <summary>
        /// Used to provide a reference for and sets up the global callbacks
        /// </summary>
        public OneSignalIOS() {
            if (_instance != null)
                SDKDebug.Error("Additional instance of OneSignalAndroid created.");

            _setNotificationReceivedCallback(_onNotificationReceived);
            _setNotificationOpenedCallback(_onNotificationOpened);
            
            _setInAppMessageWillDisplayCallback(_onInAppMessageWillDisplay);
            _setInAppMessageDidDisplayCallback(_onInAppMessageDidDisplay);
            _setInAppMessageWillDismissCallback(_onInAppMessageWillDismiss);
            _setInAppMessageDidDismissCallback(_onInAppMessageDidDismiss);
            _setInAppMessageClickedCallback(_onInAppMessageClicked);
            
            _setPermissionStateChangedCallback(_onPermissionStateChanged);
            _setSubscriptionStateChangedCallback(_onSubscriptionStateChanged);
            _setEmailSubscriptionStateChangedCallback(_onEmailSubscriptionStateChanged);
            _setSMSSubscriptionStateChangedCallback(_onSMSSubscriptionStateChanged);
            
            _instance = this;
        }
        
        [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
        private static void _onNotificationReceived(string response)
            => _instance.NotificationReceived?.Invoke(JsonUtility.FromJson<Notification>(response));

        [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
        private static void _onNotificationOpened(string response)
            => _instance.NotificationOpened?.Invoke(JsonUtility.FromJson<NotificationOpenedResult>(response));
        
        [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
        private static void _onInAppMessageWillDisplay(string response)
            => _instance.InAppMessageWillDisplay?.Invoke(new InAppMessage { id = response });

        [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
        private static void _onInAppMessageDidDisplay(string response)
            => _instance.InAppMessageDidDisplay?.Invoke(new InAppMessage { id = response });

        [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
        private static void _onInAppMessageWillDismiss(string response)
            => _instance.InAppMessageWillDismiss?.Invoke(new InAppMessage { id = response });

        [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
        private static void _onInAppMessageDidDismiss(string response)
            => _instance.InAppMessageDidDismiss?.Invoke(new InAppMessage { id = response });

        [AOT.MonoPInvokeCallback(typeof(StringResponseDelegate))]
        private static void _onInAppMessageClicked(string response)
            => _instance.InAppMessageTriggeredAction?.Invoke(JsonUtility.FromJson<InAppMessageAction>(response));

        [AOT.MonoPInvokeCallback(typeof(StateChangeDelegate))]
        private static void _onPermissionStateChanged(string current, string previous) {
            var curr = JsonUtility.FromJson<PermissionState>(current);
            var prev = JsonUtility.FromJson<PermissionState>(previous);
            _instance.PermissionStateChanged?.Invoke(curr, prev);
        }
        
        [AOT.MonoPInvokeCallback(typeof(StateChangeDelegate))]
        private static void _onSubscriptionStateChanged(string current, string previous) {
            var curr = JsonUtility.FromJson<PushSubscriptionState>(current);
            var prev = JsonUtility.FromJson<PushSubscriptionState>(previous);
            _instance.PushSubscriptionStateChanged?.Invoke(curr, prev);
        }
        
        [AOT.MonoPInvokeCallback(typeof(StateChangeDelegate))]
        private static void _onEmailSubscriptionStateChanged(string current, string previous) {
            var curr = JsonUtility.FromJson<EmailSubscriptionState>(current);
            var prev = JsonUtility.FromJson<EmailSubscriptionState>(previous);
            _instance.EmailSubscriptionStateChanged?.Invoke(curr, prev);
        }
        
        [AOT.MonoPInvokeCallback(typeof(StateChangeDelegate))]
        private static void _onSMSSubscriptionStateChanged(string current, string previous) {
            var curr = JsonUtility.FromJson<SMSSubscriptionState>(current);
            var prev = JsonUtility.FromJson<SMSSubscriptionState>(previous);
            _instance.SMSSubscriptionStateChanged?.Invoke(curr, prev);
        }
    }
}