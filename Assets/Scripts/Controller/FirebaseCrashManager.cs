using System.Collections;
using System.Collections.Generic;
#if !UNITY_WEBGL
using Firebase;
using Firebase.Crashlytics;
#endif
using UnityEngine;
public class FirebaseCrashManager : MonoBehaviour {
    private int updatesBeforeException = 0;
    private void Start () {
#if !UNITY_WEBGL
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync ().ContinueWith (task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here for indicating that your project is ready to use Firebase.
                updatesBeforeException = 0;
                Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
            } else {
                UnityEngine.Debug.LogError (System.String.Format (
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
#endif
    }
    private void Update () {
        // Call the exception-throwing method here so that it's run
        // every frame update
        if (Debug.isDebugBuild)
            throwExceptionEvery60Updates ();
    }
    void throwExceptionEvery60Updates () {
        if (updatesBeforeException > 0) {
            updatesBeforeException--;
        } else {
            // Set the counter to 60 updates
            updatesBeforeException = 60;

            // Throw an exception to test your Crashlytics implementation
            throw new System.Exception ("test exception please ignore");
        }
    }
}