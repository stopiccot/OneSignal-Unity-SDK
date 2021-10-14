<<<<<<< HEAD
<<<<<<< HEAD
#if ONE_SIGNAL_INSTALLED
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

using UnityEngine;
using System.Collections.Generic;
using System;
using OneSignalSDK;

public class OneSignalExampleBehaviour : MonoBehaviour {

    /// <summary>
    /// set to an email address you would like to test notifications against
    /// </summary>
    public string email = "EMAIL_ADDRESS";

    /// <summary>
    /// set to an external user id you would like to test notifications against
    /// </summary>
    public string externalId = "EXTERNAL_USER_ID";

    /// <summary>
    /// set to your app id (https://documentation.onesignal.com/docs/accounts-and-keys)
    /// </summary>
    public string appId = "ONESIGNAL_APP_ID";

        // Enable lines below to debug issues with OneSignal. (logLevel, visualLogLevel)
        OneSignal.Default.LogLevel   = LogType.Log;
        OneSignal.Default.AlertLevel = LogType.Exception;

        // If you set to true, the user will have to provide consent
        // using OneSignal.UserDidProvideConsent(true) before the
        // SDK will initialize
        OneSignal.Default.RequiresPrivacyConsent = requiresUserPrivacyConsent;

        // The only required method you need to call to setup OneSignal to receive push notifications.
        // Call before using any other methods on OneSignal (except setLogLevel or SetRequiredUserPrivacyConsent)
        // Should only be called once when your app is loaded.
        OneSignal.Default.NotificationReceived += notification => { };
        OneSignal.Default.NotificationOpened   += notification => { };
        OneSignal.Default.InAppMessageClicked  += iamAction => { };

        OneSignal.Default.Initialize("99015f5e-87b1-462e-a75b-f99bf7c2822e");

        // todo - what happened to this?
        // OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;

        OneSignal.Default.PermissionStateChanged        += (current, previous) => { };
        OneSignal.Default.SubscriptionStateChanged      += (current, previous) => { };
        OneSignal.Default.EmailSubscriptionStateChanged += (current, previous) => { };

        // todo - get current state
        // var pushState = OneSignal.GetPermissionSubscriptionState();

        OneSignalInAppMessageTriggerExamples();
        OneSignalOutcomeEventsExamples();
    }

    // Examples of using OneSignal External User Id
    private void OneSignalExternalUserIdCallback(Dictionary<string, object> results) {
        // The results will contain push and email success statuses
        print($"External user id updated with results: {Json.Serialize(results)}");

        // Push can be expected in almost every situation with a success status, but
        // as a pre-caution its good to verify it exists
        if (results.ContainsKey("push")) {
            var pushStatusDict = results["push"] as Dictionary<string, object>;

            if (pushStatusDict.ContainsKey("success")) {
                Console.WriteLine(
                    "External user id updated for push with results: " + pushStatusDict["success"] as string);
            }
        }

        // Verify the email is set or check that the results have an email success status
        if (results.ContainsKey("email")) {
            var emailStatusDict = results["email"] as Dictionary<string, object>;

            if (emailStatusDict.ContainsKey("success")) {
                Console.WriteLine(
                    "External user id updated for email with results: " + emailStatusDict["success"] as string);
            }
        }
    }

    private void OneSignalExternalUserIdCallbackFailure(Dictionary<string, object> error) {
        // The results will contain push and email success statuses
        Console.WriteLine("External user id failed with error: " + Json.Serialize(error));
    }

    /// <summary>
    /// Examples of using OneSignal In-App Message triggers
    /// https://documentation.onesignal.com/docs/in-app-message-examples
    /// </summary>
    private static void OneSignalInAppMessageTriggerExamples() {
        // Add a single trigger
        OneSignal.Default.AddTrigger("key", "value");

        // Get the current value to a trigger by key
        var triggerKey   = "key";
        var triggerValue = OneSignal.Default.GetTriggerValueForKey(triggerKey);
        Console.WriteLine($"Trigger key: {triggerKey} value: {triggerValue}");

        // Add multiple triggers
        OneSignal.Default.AddTriggers(new Dictionary<string, object>() { { "key1", "value1" }, { "key2", 2 } });

        // Delete a trigger
        OneSignal.Default.RemoveTriggerForKey("key");

        // Delete a list of triggers
        OneSignal.Default.RemoveTriggersForKeys(new List<string>() { "key1", "key2" });

        // Temporarily pause In-App messages; If true is passed in.
        // Great to ensure you never interrupt your user while they are in the middle of a match in your game.
        OneSignal.Default.InAppMessagesArePaused = true;
    }

    private void OneSignalOutcomeEventsExamples() {
        OneSignal.Default.SendOutcome("normal_1");
        
        // todo - this
        //OneSignal.Default.SendOutcome("normal_2", (OutcomeEvent outcomeEvent) => { printOutcomeEvent(outcomeEvent); });

        OneSignal.Default.SendUniqueOutcome("unique_1");
        
        // todo - this
        //OneSignal.Default.SendUniqueOutcome("unique_2", (OutcomeEvent outcomeEvent) => { printOutcomeEvent(outcomeEvent); });

        OneSignal.Default.SendOutcomeWithValue("value_1", 3.2f);
        
        // todo - this
        //OneSignal.Default.SendOutcomeWithValue("value_2", 3.2f,
            // (OutcomeEvent outcomeEvent) => { printOutcomeEvent(outcomeEvent); });
    }

    private void printOutcomeEvent(OutcomeEvent outcomeEvent) {
        Console.WriteLine(outcomeEvent.session + "\n" +
            string.Join(", ", outcomeEvent.notificationIds) + "\n" +
            outcomeEvent.name + "\n" +
            outcomeEvent.timestamp + "\n" +
            outcomeEvent.weight);
    }

    // Called when your app is in focus and a notification is received.
    // The name of the method can be anything as long as the signature matches.
    // Method must be static or this object should be marked as DontDestroyOnLoad
    private static void HandleNotificationReceived(Notification notification) {
        var payload = notification.Payload;
        string                message = payload.body;

        print("GameControllerExample:HandleNotificationReceived: " + message);
        print("displayType: " + notification.DisplayType);
        extraMessage = "Notification received with text: " + message;

<<<<<<< HEAD
        if (payload.additionalData == null)
            print("[HandleNotificationReceived] Additional Data == null");
        else if (Json.Serialize(payload.additionalData) is string dataString)
            print($"[HandleNotificationReceived] message {message}, additionalData: {dataString}");
=======
        Dictionary<string, object> additionalData = payload.additionalData;
        if (additionalData == null)
            Debug.Log("[HandleNotificationReceived] Additional Data == null");
>>>>>>> 8187a27 (Heavy WIP on core SDK and public interface)
        else
            Debug.Log("[HandleNotificationReceived] message " + message + ", additionalData: " +
                Json.Serialize(additionalData) as string);
    }

    // Called when a notification is opened.
    // The name of the method can be anything as long as the signature matches.
    // Method must be static or this object should be marked as DontDestroyOnLoad
    public static void HandleNotificationOpened(NotificationOpenedResult result) {
        var payload  = result.notification.Payload;
        var message  = payload.body;
        var actionID = result.action.ActionId;

        print("GameControllerExample:HandleNotificationOpened: " + message);
<<<<<<< HEAD
        _logMessage = "Notification opened with text: " + message;

        if (payload.additionalData == null)
            print("[HandleNotificationOpened] Additional Data == null");
        else if (Json.Serialize(payload.additionalData) is string dataString)
            print($"[HandleNotificationOpened] message {message}, additionalData: {dataString}");
        else
            print("[HandleNotificationOpened] Additional Data could not be serialized");
=======
        extraMessage = "Notification opened with text: " + message;

        var additionalData = payload.additionalData;
        if (additionalData == null)
            Debug.Log("[HandleNotificationOpened] Additional Data == null");
        else {
            Debug.Log("[HandleNotificationOpened] message " + message + ", additionalData: " +
                Json.Serialize(additionalData) as string);
        }
>>>>>>> 8187a27 (Heavy WIP on core SDK and public interface)

        if (actionID != null) {
            // actionSelected equals the id on the button the user pressed.
            // actionSelected will equal "__DEFAULT__" when the notification itself was tapped when buttons were present.
            _logMessage = "Pressed ButtonId: " + actionID;
        }
    }

    public static void HandlerInAppMessageClicked(InAppMessageAction action) {
        var logInAppClickEvent = "In-App Message Clicked: " +
            "\nClick Name: " + action.ClickName +
            "\nClick Url: " + action.ClickUrl +
            "\nFirst Click: " + action.FirstClick +
            "\nCloses Message: " + action.ClosesMessage;

        print(logInAppClickEvent);
        _logMessage = logInAppClickEvent;
    }

    // Test Menu
    // Includes SendTag/SendTags, getting the userID and pushToken, and scheduling an example notification
    private void OnGUI() {
        var customTextSize = new GUIStyle("button") {
            fontSize = 30
        };

        var guiBoxStyle = new GUIStyle("box") {
            fontSize = 30
        };

        var textFieldStyle = new GUIStyle("textField") {
            fontSize = 30
        };

        float itemOriginX      = 50.0f;
        float itemWidth        = Screen.width - 120.0f;
        float boxWidth         = Screen.width - 20.0f;
        float boxOriginY       = 120.0f;
        float boxHeight        = requiresUserPrivacyConsent ? 980.0f : 890.0f;
        float itemStartY       = 200.0f;
        float itemHeightOffset = 90.0f;
        float itemHeight       = 60.0f;

    private static string _logMessage;

    private GUIStyle _customTextSize;
    private GUIStyle _guiBoxStyle;

<<<<<<< HEAD
    private static float ItemWidth => Screen.width - 120.0f;
    private static float BoxWidth => Screen.width - 20.0f;
    private float BoxHeight => requiresUserPrivacyConsent ? 980.0f : 890.0f;

    private Rect MainMenuRect => new Rect(10, BoxOriginY, BoxWidth, BoxHeight);

    private static Rect ItemRect(ref int position) => new Rect(
        ItemOriginX,
        ItemStartY + position++ * ItemHeightOffset,
        ItemWidth,
        ItemHeight
    );

    private bool MenuButton(ref int position, string label)
        => GUI.Button(ItemRect(ref position), label, _customTextSize);

    // Test Menu
    // Includes SendTag/SendTags, getting the userID and pushToken, and scheduling an example notification
    private void OnGUI() {
        _customTextSize = _customTextSize ?? new GUIStyle(GUI.skin.button) {
            fontSize = 30
        };
        
        _guiBoxStyle = _guiBoxStyle ?? new GUIStyle(GUI.skin.box) {
            fontSize  = 30,
            alignment = TextAnchor.UpperLeft,
            wordWrap  = true
        };

        GUI.Box(MainMenuRect, "Test Menu", _guiBoxStyle);

        int position = 0;

        if (MenuButton(ref position, "Send Example Tags")) {
=======
        if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            "SendTags", customTextSize)) {
>>>>>>> 8187a27 (Heavy WIP on core SDK and public interface)
            // You can tags users with key value pairs like this:
            OneSignal.Default.SendTag("UnityTestKey", "TestValue");

            // Or use an IDictionary if you need to set more than one tag.
            OneSignal.Default.SendTags(new Dictionary<string, string>()
                { { "UnityTestKey2", "value2" }, { "UnityTestKey3", "value3" } });

            // You can delete a single tag with it's key.
            // OneSignal.DeleteTag("UnityTestKey");
            // Or delete many with an IList.
            // OneSignal.DeleteTags(new List<string>() {"UnityTestKey2", "UnityTestKey3" });
        }

        count++;

        if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight), "GetIds",
            customTextSize)) {
            // todo - substitute with getDeviceState
            // OneSignal.IdsAvailable((userId, pushToken) => {
            //     extraMessage = "UserID:\n" + userId + "\n\nPushToken:\n" + pushToken;
            // });
        }

        if (MenuButton(ref position, "Test Notification")) {
            _logMessage = "Waiting to get a OneSignal userId. Uncomment OneSignal.SetLogLevel in the Start method if " +
                "it hangs here to debug the issue.";

        if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            "TestNotification", customTextSize)) {
            extraMessage
                = "Waiting to get a OneSignal userId. Uncomment OneSignal.SetLogLevel in the Start method if it hangs here to debug the issue.";

            // todo - substitute with getDeviceState
            // OneSignal.IdsAvailable((userId, pushToken) => {
            //     if (pushToken != null) {
            //         // See https://documentation.onesignal.com/reference/create-notification for a full list of options.
            //         // You can not use included_segments or any fields that require your OneSignal 'REST API Key' in your app for security reasons.
            //         // If you need to use your OneSignal 'REST API Key' you will need your own server where you can make this call.
            //
            //         var notification = new Dictionary<string, object>();
            //         notification["contents"] = new Dictionary<string, string>() { { "en", "Test Message" } };
            //
            //         // Send notification to this device.
            //         notification["include_player_ids"] = new List<string>() { userId };
            //
            //         // Example of scheduling a notification in the future.
            //         //notification["send_after"] = System.DateTime.Now.ToUniversalTime().AddSeconds(30).ToString("U");
            //
            //         extraMessage = "Posting test notification now.";
            //
            //         OneSignal.PostNotification(notification,
            //             (responseSuccess) => {
            //                 extraMessage
            //                     = "Notification posted successful! Delayed by about 30 seconds to give you time to press the home button to see a notification vs an in-app alert.\n" +
            //                     Json.Serialize(responseSuccess);
            //             },
            //             (responseFailure) => {
            //                 extraMessage = "Notification failed to post:\n" + Json.Serialize(responseFailure);
            //             });
            //     }
            //     else {
            //         extraMessage = "ERROR: Device is not registered.";
            //     }
            // });
        }

        email = GUI.TextField(ItemRect(ref position), email, _customTextSize);

        email = GUI.TextField(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            email, customTextSize);

        count++;

        if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            "SetEmail", customTextSize)) {
            extraMessage = "Setting email to " + email;

            // todo - this
            // OneSignal.SetEmail(email, () => { Debug.Log("Successfully set email"); },
                // (error) => { Debug.Log("Encountered error setting email: " + Json.Serialize(error)); });
        }

        if (MenuButton(ref position, "Logout Email")) {
            _logMessage = "Logging Out of example@example.com";

        if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            "LogoutEmail", customTextSize)) {
            extraMessage = "Logging Out of example@example.com";

            // todo - this
            // OneSignal.LogoutEmail(() => { Debug.Log("Successfully logged out of email"); },
            //     (error) => { Debug.Log("Encountered error logging out of email: " + Json.Serialize(error)); });
        }

        externalId = GUI.TextField(ItemRect(ref position), externalId, _customTextSize);

        externalId = GUI.TextField(
            new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight), externalId,
            customTextSize);

        if (MenuButton(ref position, "Remove External Id"))
            OneSignal.RemoveExternalUserId(OnUpdatedExternalUserId);

        if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            "SetExternalId", customTextSize)) {
            
            // todo - this
            // OneSignal.SetExternalUserId(externalId, OneSignalExternalUserIdCallback);

            // Auth external id method
            // OneSignal.SetExternalUserId(externalId, "your_auth_hash_token", OneSignalExternalUserIdCallback, OneSignalExternalUserIdCallbackFailure);
        }

            if (GUI.Button(ItemRect(ref position), consentText, _customTextSize)) {
                _logMessage = "Providing user privacy consent";

        if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            "RemoveExternalId", customTextSize)) {
            // todo - this
            // OneSignal.RemoveExternalUserId(OneSignalExternalUserIdCallback);
        }

        if (requiresUserPrivacyConsent) {
            count++;

            // todo - this
            // if (GUI.Button(new Rect(itemOriginX, itemStartY + (count * itemHeightOffset), itemWidth, itemHeight),
            //     (OneSignal.UserProvidedConsent() ? "Revoke Privacy Consent" : "Provide Privacy Consent"),
            //     customTextSize)) {
            //     extraMessage = "Providing user privacy consent";
            //
            //     OneSignal.UserDidProvideConsent(!OneSignal.UserProvidedConsent());
            // }
        }

        if (extraMessage != null) {
            guiBoxStyle.alignment = TextAnchor.UpperLeft;
            guiBoxStyle.wordWrap  = true;
            GUI.Box(
                new Rect(10, boxOriginY + boxHeight + 20, Screen.width - 20,
                    Screen.height - (boxOriginY + boxHeight + 40)), extraMessage, guiBoxStyle);
        }
    }

    private static void printError(object message) => Debug.LogError(message);
}
=======
#if ONE_SIGNAL_INSTALLED
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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace OneSignalSDK {
    /// <summary>
    /// 
    /// </summary>
    public class OneSignalExampleBehaviour : MonoBehaviour {
        /// <summary>
        /// set to an email address you would like to test notifications against
        /// </summary>
        public string email = "EMAIL_ADDRESS";

        /// <summary>
        /// set to an external user id you would like to test notifications against
        /// </summary>
        public string externalId = "EXTERNAL_USER_ID";

        /// <summary>
        /// set to your app id (https://documentation.onesignal.com/docs/accounts-and-keys)
        /// </summary>
        public string appId = "ONESIGNAL_APP_ID";

        /// <summary>
        /// whether you would prefer OneSignal Unity SDK prevent initialization until consent is granted via
        /// <see cref="OneSignal.UserDidProvideConsent"/> in this test MonoBehaviour
        /// </summary>
        public bool requiresUserPrivacyConsent;

        // added to shorten total code
        private readonly OneSignalSDK.OneSignal _onesignal = OneSignalSDK.OneSignal.Default;

        /// <summary>
        /// we recommend initializing OneSignal early in your application's lifecycle such as in the Start method of a
        /// MonoBehaviour in your opening Scene
        /// </summary>
        private void Start() {
            _logMessage = null;

            // Enable lines below to debug issues with OneSignal. (logLevel, visualLogLevel)
            _onesignal.LogLevel   = LogType.Log;
            _onesignal.AlertLevel = LogType.Exception;

            /*
             * If you set to true, the user will have to provide consent via OneSignal.UserDidProvideConsent(true) before
             * the SDK will initialize
             */
            _onesignal.RequiresPrivacyConsent = requiresUserPrivacyConsent;

            /*
             * The only required method you need to call to setup OneSignal to receive push notifications.
             * Call before using any other methods on OneSignal (except setLogLevel or SetRequiredUserPrivacyConsent)
             * Should only invoke once when your app is loaded.
             */
            _onesignal.NotificationReceived += HandleNotificationReceived;
            _onesignal.NotificationOpened   += HandleNotificationOpened;
            _onesignal.InAppMessageClicked  += OnInAppMessageClicked;

            _onesignal.Initialize(appId);

            // todo
            // // Control how OneSignal notifications will be shown when one is received while your app is in focus
            // OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;

            // Each of these events can inform your application when the user's OneSignal states have change
            _onesignal.PermissionStateChanged        += OnPermissionStateChange;
            _onesignal.PushSubscriptionStateChanged      += OnPushSubscriptionStateChange;
            _onesignal.EmailSubscriptionStateChanged += OnEmailSubscriptionStateChange;

            // todo
            // // You can also get the current states directly
            // var fullUserState          = OneSignal.GetPermissionSubscriptionState();
            // var permissionState        = fullUserState.permissionStatus;
            // var subscriptionState      = fullUserState.subscriptionStatus;
            // var emailSubscriptionState = fullUserState.emailSubscriptionStatus;

            OneSignalInAppMessageTriggerExamples();
            OneSignalOutcomeEventsExamples();
        }

        /// <summary>
        /// Examples of using OneSignal External User Id
        /// </summary>
        private void OnUpdatedExternalUserId(Dictionary<string, object> results) {
            // The results will contain push and email success statuses
            print($"External user id updated with results: {Json.Serialize(results)}");

            // Push can be expected in almost every situation with a success status, but
            // as a pre-caution its good to verify it exists
            if (results.ContainsKey("push")) {
                if (results["push"] is Dictionary<string, object> pushStatus && pushStatus.ContainsKey("success"))
                    print($"External user id updated for push with results: {pushStatus["success"]}");
            }

            // Verify the email is set or check that the results have an email success status
            if (results.ContainsKey("email")) {
                if (results["email"] is Dictionary<string, object> emailStatus && emailStatus.ContainsKey("success"))
                    print($"External user id updated for email with results: {emailStatus["success"]}");
            }
        }

        private void OnUpdatedExternalUserIdFailure(Dictionary<string, object> error) {
            // As above the results will contain push and email statuses
            print($"External user id failed with error: {Json.Serialize(error)}");
        }

        /// <summary>
        /// Examples of using OneSignal In-App Message triggers
        /// https://documentation.onesignal.com/docs/in-app-message-examples
        /// </summary>
        private void OneSignalInAppMessageTriggerExamples() {
            // Add a single trigger
            _onesignal.SetTrigger("key", "value");

            // Get the current value to a trigger by key
            var triggerKey   = "key";
            var triggerValue = _onesignal.GetTrigger(triggerKey);
            print($"Trigger key: {triggerKey} value: {triggerValue}");

            // Add multiple triggers
            _onesignal.SetTriggers(new Dictionary<string, object> {
                { "key1", "value1" },
                { "key2", 2 }
            });

            // Delete a trigger
            _onesignal.RemoveTrigger("key");

            // Delete a list of triggers
            _onesignal.RemoveTriggers("key1", "key2");

            //_onesignal.RemoveTriggers(new string[] { "key1", "key2" });

            // Temporarily pause In-App messages; If true is passed in.
            // Great to ensure you never interrupt your user while they are in the middle of a match in your game.
            _onesignal.InAppMessagesArePaused = false;
        }

        /// <summary>
        /// Send data to OneSignal which will allow you to track the result of notifications
        /// https://documentation.onesignal.com/docs/outcomes
        /// </summary>
        private void OneSignalOutcomeEventsExamples() {
            // Send a result which can occur multiple times. In this case the return is being ignored.
            _onesignal.SendOutcome("normal_1");

            // If you'd like to monitor whether the send was successful you can await the task like below
            _onesignal.SendOutcome("normal_2")
               .ContinueWith(task => OnSendOutcomeSuccess(task.Result));

            // Send a result which can only occur once
            _onesignal.SendUniqueOutcome("unique_1");

            // todo
            // var result = await _onesignal.SendUniqueOutcome("unique_2");

            // Send a result which can occur multiple times with a float value
            _onesignal.SendOutcomeWithValue("value_1", 3.2f);
            _onesignal.SendOutcomeWithValue("value_2", 3.2f);
        }

        private static void OnSendOutcomeSuccess(OutcomeEvent outcomeEvent) {
            print(outcomeEvent.sessionType + "\n" +
                string.Join(", ", outcomeEvent.notificationIds) + "\n" +
                outcomeEvent.id + "\n" +
                outcomeEvent.timestamp + "\n" +
                outcomeEvent.weight);
        }

        /*
         * State change events provide both the new (to) and previous (from) states
         */

        private void OnPushSubscriptionStateChange(PushSubscriptionState current, PushSubscriptionState previous) {
            print("SUBSCRIPTION stateChanges: " + current);
            print("SUBSCRIPTION stateChanges.to.userId: " + current.userId);
            print("SUBSCRIPTION stateChanges.to.subscribed: " + current.subscribed);
        }

        private void OnPermissionStateChange(PermissionState current, PermissionState previous) {
            print($"PERMISSION stateChanges.from.status: {previous.status}");
            print($"PERMISSION stateChanges.to.status: {current.status}");
        }

        private void OnEmailSubscriptionStateChange(EmailSubscriptionState current, EmailSubscriptionState previous) {
            print($"EMAIL stateChanges.from.status: {previous.emailUserId}, {previous.emailAddress}");
            print($"EMAIL stateChanges.to.status: {current.emailUserId}, {current.emailAddress}");
        }

        /// <summary>
        /// Called when your app is in focus and a notification is received.
        /// The name of the method can be anything as long as the signature matches.
        /// Method must be static or this object should be marked as DontDestroyOnLoad
        /// </summary>
        private static void HandleNotificationReceived(Notification notification) {
            var payload = notification.payload;
            var message = payload.body;

            print("GameControllerExample:HandleNotificationReceived: " + message);
            print("displayType: " + notification.displayType);
            _logMessage = "Notification received with text: " + message;

            if (payload.additionalData == null)
                print("[HandleNotificationReceived] Additional Data == null");
            else if (Json.Serialize(payload.additionalData) is { } dataString)
                print($"[HandleNotificationReceived] message {message}, additionalData: {dataString}");
            else
                print("[HandleNotificationReceived] Additional Data could not be serialized");
        }

        /// <summary>
        /// Called when a notification is opened.
        /// The name of the method can be anything as long as the signature matches.
        /// Method must be static or this object should be marked as DontDestroyOnLoad
        /// </summary>
        private static void HandleNotificationOpened(NotificationOpenedResult notificationOpenedResult) {
            var payload  = notificationOpenedResult.notification.payload;
            var message  = payload.body;
            var actionID = notificationOpenedResult.action.id;

            print("GameControllerExample:HandleNotificationOpened: " + message);
            _logMessage = "Notification opened with text: " + message;

            if (payload.additionalData == null)
                print("[HandleNotificationOpened] Additional Data == null");
            else if (Json.Serialize(payload.additionalData) is { } dataString)
                print($"[HandleNotificationOpened] message {message}, additionalData: {dataString}");
            else
                print("[HandleNotificationOpened] Additional Data could not be serialized");

            if (actionID != null) {
                // actionSelected equals the id on the button the user pressed.
                // actionSelected will equal "__DEFAULT__" when the notification itself was tapped when buttons were present.
                _logMessage = "Pressed ButtonId: " + actionID;
            }
        }

        private static void OnInAppMessageClicked(InAppMessageAction inAppMessageAction) {
            var logInAppClickEvent = "In-App Message Clicked: " +
                "\nClick Name: " + inAppMessageAction.clickName +
                "\nClick Url: " + inAppMessageAction.clickUrl +
                "\nFirst Click: " + inAppMessageAction.firstClick +
                "\nCloses Message: " + inAppMessageAction.closesMessage;

            print(logInAppClickEvent);
            _logMessage = logInAppClickEvent;
        }

        /// <summary>
        /// See https://documentation.onesignal.com/reference/create-notification for a full list of options.
        /// </summary>
        /// <remarks>
        /// You can not use included_segments or any fields that require your OneSignal 'REST API Key' in your app for
        /// security reasons.
        /// If you need to use your OneSignal 'REST API Key' you will need your own server where you can make this call.
        /// </remarks>
        private async void SendTestNotification(string userId) {
            var notification = new Dictionary<string, object> {
                ["contents"] = new Dictionary<string, string> { { "en", "Test Message" } },

                // Send notification to this user
                ["include_player_ids"] = new List<string> { userId },

                // Example of scheduling a notification in the future.
                ["send_after"] = DateTime.Now.ToUniversalTime().AddSeconds(30).ToString("U")
            };

            _logMessage
                = "Posting test notification now. By default the example should arrive 30 seconds in the future." +
                "If you would like to see it as a Push and not an In App Alert then please leave the application.";

            var task = _onesignal.PostNotification(notification);

            try {
                if (await task is { } response)
                    OnNotificationPostSuccess(response);
            }
            catch (TaskCanceledException cancelException) {
                printError(cancelException.Message);
            }
        }

        private static void OnNotificationPostSuccess(Dictionary<string, object> response)
            => _logMessage = "Notification post success!\n" + Json.Serialize(response);

        private static void OnNotificationPostFailure(Dictionary<string, object> response)
            => _logMessage = "Notification failed to post:\n" + Json.Serialize(response);

        /*
         * UI Rendering 
         */

        private const float ItemOriginX = 50.0f;
        private const float BoxOriginY = 120.0f;
        private const float ItemStartY = 200.0f;
        private const float ItemHeightOffset = 90.0f;
        private const float ItemHeight = 60.0f;

        private static string _logMessage;

        private GUIStyle _customTextSize;
        private GUIStyle _guiBoxStyle;

        private static float ItemWidth => Screen.width - 120.0f;
        private static float BoxWidth => Screen.width - 20.0f;
        private float BoxHeight => requiresUserPrivacyConsent ? 980.0f : 890.0f;

        private Rect MainMenuRect => new Rect(10, BoxOriginY, BoxWidth, BoxHeight);

        private static Rect ItemRect(ref int position) => new Rect(
            ItemOriginX,
            ItemStartY + position++ * ItemHeightOffset,
            ItemWidth,
            ItemHeight
        );

        private bool MenuButton(ref int position, string label)
            => GUI.Button(ItemRect(ref position), label, _customTextSize);

        // Test Menu
        // Includes SendTag/SendTags, getting the userID and pushToken, and scheduling an example notification
        private void OnGUI() {
            _customTextSize ??= new GUIStyle(GUI.skin.button) {
                fontSize = 30
            };

            _guiBoxStyle ??= new GUIStyle(GUI.skin.box) {
                fontSize  = 30,
                alignment = TextAnchor.UpperLeft,
                wordWrap  = true
            };

            GUI.Box(MainMenuRect, "Test Menu", _guiBoxStyle);

            int position = 0;

            if (MenuButton(ref position, "Send Example Tags")) {
                // You can tags users with key value pairs like this:
                _onesignal.SendTag("UnityTestKey", "TestValue");

                // Or use an IDictionary if you need to set more than one tag.
                _onesignal.SendTags(new Dictionary<string, object> {
                    { "UnityTestKey2", "value2" },
                    { "UnityTestKey3", "value3" }
                });

                // You can delete a single tag with it's key.
                // OneSignal.DeleteTag("UnityTestKey");
                // Or delete many with an IList.
                // OneSignal.DeleteTags(new List<string>() {"UnityTestKey2", "UnityTestKey3" });
            }

            if (MenuButton(ref position, "Get Ids")) {
                // todo
                // OneSignal.IdsAvailable((userId, pushToken) => {
                //     _logMessage = $"UserID:\n{userId}\n\nPushToken:\n{pushToken}";
                // });
            }

            if (MenuButton(ref position, "Test Notification")) {
                _logMessage
                    = "Waiting to get a OneSignal userId. Uncomment OneSignal.SetLogLevel in the Start method if " +
                    "it hangs here to debug the issue.";

                // todo
                // // Checking to make sure this device is registered or you will not receive the notification
                // OneSignal.IdsAvailable((userId, pushToken) => {
                //     if (pushToken != null)
                //         SendTestNotification(userId);
                //     else
                //         _logMessage = "ERROR: Device is not registered.";
                // });
            }

            email = GUI.TextField(ItemRect(ref position), email, _customTextSize);

            if (MenuButton(ref position, "Set Email")) {
                _logMessage = "Setting email to " + email;

                // todo
                // _onesignal.SetEmail(email,
                //     () => print("Successfully set email"),
                //     error => printError("Error setting email: " + Json.Serialize(error))
                // );
            }

            if (MenuButton(ref position, "Logout Email")) {
                _logMessage = "Logging Out of example@example.com";

                // todo
                // OneSignal.LogoutEmail(
                //     () => print("Successfully logged out of email"),
                //     error => printError("Error logging out of email: " + Json.Serialize(error))
                // );
            }

            externalId = GUI.TextField(ItemRect(ref position), externalId, _customTextSize);

            // todo
            // if (MenuButton(ref position, "Set External Id"))
            //     OneSignal.SetExternalUserId(externalId, OnUpdatedExternalUserId);
            //
            // if (MenuButton(ref position, "Remove External Id"))
            //     _onesignal.RemoveExternalUserId(OnUpdatedExternalUserId);

            if (requiresUserPrivacyConsent) {
                var consentText = _onesignal.PrivacyConsent
                    ? "Revoke Privacy Consent"
                    : "Provide Privacy Consent";

                if (GUI.Button(ItemRect(ref position), consentText, _customTextSize)) {
                    _logMessage = "Providing user privacy consent";

                    _onesignal.PrivacyConsent = !_onesignal.PrivacyConsent;
                }
            }

            if (_logMessage != null) {
                var logRect = new Rect(
                    10,
                    BoxOriginY + BoxHeight + 20,
                    Screen.width - 20,
                    Screen.height - (BoxOriginY + BoxHeight + 40)
                );

                GUI.Box(logRect, _logMessage, _guiBoxStyle);
            }
        }

        private static void printError(object message) => Debug.LogError(message);
    }
}
>>>>>>> 80dc462 (Updated public APIs to support async in correct methods)
#endif
=======
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace OneSignalSDK {
    public class OneSignalExampleBehaviour : MonoBehaviour {
        /// <summary>
        /// set to an email address you would like to test notifications against
        /// </summary>
        public string email = "EMAIL_ADDRESS";

        /// <summary>
        /// set to an external user id you would like to test notifications against
        /// </summary>
        public string externalId = "EXTERNAL_USER_ID";
        
        /// <summary>
        /// set to an external user id you would like to test notifications against
        /// </summary>
        public string phoneNumber = "PHONE_NUMBER";

        /// <summary>
        /// set to your app id (https://documentation.onesignal.com/docs/accounts-and-keys)
        /// </summary>
        public string appId = "ONESIGNAL_APP_ID";

        /// <summary>
        /// whether you would prefer OneSignal Unity SDK prevent initialization until consent is granted via
        /// <see cref="OneSignal.PrivacyConsent"/> in this test MonoBehaviour
        /// </summary>
        public bool requiresUserPrivacyConsent;

        /// <summary>
        /// 
        /// </summary>
        public string tagKey;

        /// <summary>
        /// 
        /// </summary>
        public string tagValue;

        /// <summary>
        /// 
        /// </summary>
        public string triggerKey;

        /// <summary>
        /// 
        /// </summary>
        public string triggerValue;
        
        /// <summary>
        /// 
        /// </summary>
        public string outcomeKey;

        /// <summary>
        /// 
        /// </summary>
        public float outcomeValue;
        
        /// <summary>
        /// 
        /// </summary>
        public string outcomeUniqueKey;
        
        /// <summary>
        /// we recommend initializing OneSignal early in your application's lifecycle such as in the Start method of a
        /// MonoBehaviour in your opening Scene
        /// </summary>
        private void Start() {
            // Enable lines below to debug issues with OneSignal
            OneSignal.Default.LogLevel   = LogType.Log;
            OneSignal.Default.AlertLevel = LogType.Exception;

            // Setting RequiresPrivacyConsent to true will prevent the OneSignalSDK from operating until
            // PrivacyConsent is also set to true
            OneSignal.Default.RequiresPrivacyConsent = requiresUserPrivacyConsent;
            
            // Setup the below to listen for and respond to events from notifications
            OneSignal.Default.NotificationWasOpened   += _notificationOpened;
            OneSignal.Default.NotificationReceived += _notificationReceived;
            
            // Setup the below to listen for and respond to events from in app messages
            OneSignal.Default.InAppMessageTriggeredAction += IamTriggeredAction;
            
            // Setup the below to listen for and respond to state changes
            OneSignal.Default.PermissionStateChanged        += _permissionStateChanged;
            OneSignal.Default.PushSubscriptionStateChanged  += _pushStateChanged;
            OneSignal.Default.EmailSubscriptionStateChanged += _emailStateChanged;
            OneSignal.Default.SMSSubscriptionStateChanged   += _smsStateChanged;
        }
        
        /*
         * SDK events
         */

        private void _notificationOpened(NotificationOpenedResult result) {
            _log($"Notification was opened with result:\n{JsonUtility.ToJson(result)}");
        }

        private void _notificationReceived(Notification notification) {
            _log($"Notification was received in foreground:\n{JsonUtility.ToJson(notification)}");
        }

        private void IamTriggeredAction(InAppMessageAction inAppMessageAction) {
            _log($"IAM triggered action:\n{JsonUtility.ToJson(inAppMessageAction)}");
        }

        private void _permissionStateChanged(PermissionState current, PermissionState previous) {
            _log($"Permission state changed to:\n{JsonUtility.ToJson(current)}");
        }

        private void _pushStateChanged(PushSubscriptionState current, PushSubscriptionState previous) {
            _log($"Push state changed to:\n{JsonUtility.ToJson(current)}");
        }

        private void _emailStateChanged(EmailSubscriptionState current, EmailSubscriptionState previous) {
            _log($"Email state changed to:\n{JsonUtility.ToJson(current)}");
        }

        private void _smsStateChanged(SMSSubscriptionState current, SMSSubscriptionState previous) {
            _log($"SMS state changed to:\n{JsonUtility.ToJson(current)}");
        }
        
        /*
         * SDK setup
         */

        public void Initialize() {
            _log("Initializing...");
            OneSignal.Default.Initialize(appId);
        }

        public void ToggleRequiresPrivacyConsent() {
            _log($"Toggling RequiresPrivacyConsent to {!OneSignal.Default.RequiresPrivacyConsent}");
            OneSignal.Default.RequiresPrivacyConsent = !OneSignal.Default.RequiresPrivacyConsent;
        }

        public void TogglePrivacyConsent() {
            _log($"Toggling PrivacyConsent to {!OneSignal.Default.PrivacyConsent}");
            OneSignal.Default.PrivacyConsent = !OneSignal.Default.PrivacyConsent;
        }

        public void SetLogLevel() {
            var newLevel = _nextEnum(OneSignal.Default.LogLevel);
            _log($"Setting LogLevel to {newLevel}");
            
            // LogLevel uses the standard Unity LogType
            OneSignal.Default.LogLevel = newLevel;
        }

        public void SetAlertLevel() {
            var newLevel = _nextEnum(OneSignal.Default.AlertLevel);
            _log($"Setting AlertLevel to {newLevel}");
            
            // AlertLevel uses the standard Unity LogType
            OneSignal.Default.AlertLevel = newLevel;
        }
        
        /*
         * User identification
         */

        public async void SetEmail() {
            _log($"Calling SetEmail({email}) and awaiting result...");
            
            var result = await OneSignal.Default.SetEmail(email);
            
            if (result)
                _log("Set succeeded");
            else
                _error("Set failed");
        }

        public async void SetExternalId() {
            _log($"Calling SetExternalUserId({externalId}) and awaiting result...");
            
            var result = await OneSignal.Default.SetExternalUserId(externalId);
            
            if (result)
                _log("Set succeeded");
            else
                _error("Set failed");
        }

        public async void SetSMSNumber() {
            _log($"Calling SetSMSNumber({phoneNumber}) and awaiting result...");
            
            var result = await OneSignal.Default.SetSMSNumber(phoneNumber);
            
            if (result)
                _log("Set succeeded");
            else
                _error("Set failed");
        }
        
        /*
         * Push
         */

        public async void PromptForPush() {
            _log("Calling PromptForPushNotificationsWithUserResponse and awaiting result...");

            var result = await OneSignal.Default.PromptForPushNotificationsWithUserResponse();

            _log($"Prompt completed with <b>{result}</b>");
        }

        public void ClearPush() {
            _log("Clearing existing OneSignal push notifications...");
            OneSignal.Default.ClearOneSignalNotifications();
        }

        public async void SendPushToSelf() {
            _log("Sending push notification to this device via PostNotification...");
            
            // Check out our API docs at https://documentation.onesignal.com/reference/create-notification
            // for a full list of possibilities for notification options.
            var pushOptions = new Dictionary<string, object> {
                ["contents"] = new Dictionary<string, string> {
                    ["en"] = "Test Message"
                },

                // Send notification to this user
                ["include_external_user_ids"] = new List<string> { externalId },

                // Example of scheduling a notification in the future
                ["send_after"] = DateTime.Now.ToUniversalTime().AddSeconds(30).ToString("U")
            };

            var result = await OneSignal.Default.PostNotification(pushOptions);
            _log($"Notification sent with result {result}");
        }
        
        /*
         * In App Messages
         */

        public void SetTrigger() {
            _log($"Setting trigger with key {triggerKey} and value {triggerValue}");
            OneSignal.Default.SetTrigger(triggerKey, triggerValue);
        }

        public void GetTrigger() {
            _log($"Getting trigger for key {triggerKey}");
            var value = OneSignal.Default.GetTrigger(triggerKey);
            _log($"Trigger for key {triggerKey} is of value {value}");
        }

        public void RemoveTrigger() {
            _log($"Removing trigger for key {triggerKey}");
            OneSignal.Default.RemoveTrigger(triggerKey);
        }

        public void GetTriggers() {
            _log("Getting all trigger keys and values");
            var triggers = OneSignal.Default.GetTriggers();
            
            if (Json.Serialize(triggers) is string triggersString)
                _log("Current triggers are " + triggersString);
            else
                _error("Could not serialize triggers");
        }
        
        public void ToggleInAppMessagesArePaused() {
            _log($"Toggling InAppMessagesArePaused to {!OneSignal.Default.InAppMessagesArePaused}");
            OneSignal.Default.InAppMessagesArePaused = !OneSignal.Default.InAppMessagesArePaused;
        }
        
        /*
         * Tags
         */

        public async void SetTag() {
            _log($"Setting tag with key {tagKey} and value {tagValue} and awaiting result...");

            var result = await OneSignal.Default.SendTag(tagKey, tagValue);
            
            if (result)
                _log("Set succeeded");
            else
                _error("Set failed");
        }

        public async void RemoveTag() {
            _log($"Removing tag for key {triggerKey} and awaiting result...");

            var result = await OneSignal.Default.DeleteTag(tagKey);
            
            if (result)
                _log("Remove succeeded");
            else
                _error("Remove failed");
        }

        public async void GetTags() {
            _log("Requesting all tag keys and values for this user...");
            var tags = await OneSignal.Default.GetTags();
            
            if (Json.Serialize(tags) is string tagsString)
                _log("Current tags are " + tagsString);
            else
                _error("Could not serialize tags");
        }
        
        /*
         * Outcomes
         */

        public async void SetOutcome() {
            _log($"Sending outcome with key {outcomeKey} and awaiting result...");

            var result = await OneSignal.Default.SendOutcome(outcomeKey);
            
            if (result)
                _log("Send succeeded");
            else
                _error("Send failed");
        }

        public async void SetUniqueOutcome() {
            _log($"Setting outcome with key {outcomeKey} and awaiting result...");

            var result = await OneSignal.Default.SendUniqueOutcome(outcomeUniqueKey);
            
            if (result)
                _log("Send succeeded");
            else
                _error("Send failed");
        }

        public async void SetOutcomeWithValue() {
            _log($"Setting outcome with key {outcomeKey} and value {outcomeValue} and awaiting result...");

            var result = await OneSignal.Default.SendOutcomeWithValue(outcomeKey, outcomeValue);
            
            if (result)
                _log("Send succeeded");
            else
                _error("Send failed");
        }
        
        /*
         * Location
         */

        public void PromptLocation() {
            _log("Opening prompt to ask for user consent to access location");
            OneSignal.Default.PromptLocation();
        }
        
        public void ToggleShareLocation() {
            _log($"Toggling ShareLocation to {!OneSignal.Default.ShareLocation}");
            OneSignal.Default.ShareLocation = !OneSignal.Default.ShareLocation;
        }

    #region Rendering
        /*
         * You can safely ignore everything in this region and below
         */

        public Text console;

        public void SetExternalIdString(string newVal) => externalId = newVal;
        public void SetEmailString(string newVal) => email = newVal;
        public void SetPhoneNumberString(string newVal) => phoneNumber = newVal;
        
        public void SetTriggerKey(string newVal) => triggerKey = newVal;
        public void SetTriggerValue(string newVal) => triggerValue = newVal;
        
        public void SetTagKey(string newVal) => tagKey = newVal;
        public void SetTagValue(string newVal) => tagValue = newVal;
        
        public void SetOutcomeKey(string newVal) => outcomeKey = newVal;
        public void SetOutcomeValue(string newVal) => outcomeValue = Convert.ToSingle(newVal);
        public void SetOutcomeUniqueKey(string newVal) => outcomeUniqueKey = newVal;
        
        private void Awake() {
            SDKDebug.LogIntercept   += _log;
            SDKDebug.WarnIntercept  += _warn;
            SDKDebug.ErrorIntercept += _error;
        }

        private void _log(object message) {
            Debug.Log(message);
            console.text += $"\n<color=green><b>I></b></color> {message}";
        }

        private void _warn(object message) {
            Debug.LogWarning(message);
            console.text += $"\n<color=orange><b>W></b></color> {message}";
        }

        private void _error(object message) {
            Debug.LogError(message);
            console.text += $"\n<color=red><b>E></b></color> {message}";
        }
    #endregion

    #region Helpers
        private static T _nextEnum<T>(T src) where T : struct {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            var vals = (T[])Enum.GetValues(src.GetType());
            var next = Array.IndexOf(vals, src) + 1;
            return vals.Length == next ? vals[0] : vals[next];
        }
    #endregion
    }
}
>>>>>>> 2088c30 (WIP on example scene and behaviour)
