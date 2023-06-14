using Firebase.Analytics;

namespace Assets._SDK.Logger
{

    public class FirebaseLogServer : IUserLog
    {
        
        public void LogEvent(string name)
        {
            if (FirebaseService.Instance.IsConnected)
            {
                FirebaseAnalytics.LogEvent(name);
            }
        }

        public void LogEvent(string name, params LogParameter[] parameters)
        {
            var firebaseParameters = new Parameter[parameters.Length];
            for (int i = 0; i < firebaseParameters.Length; i++)
            {
                firebaseParameters[i] = parameters[i].GetFirebaseParameter();
            }

            if (FirebaseService.Instance.IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, firebaseParameters);
            }
            
        }

        public void LogEvent(string name, string parameterName, int parameterValue)
        {
            if (FirebaseService.Instance.IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
            }
            
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            if (FirebaseService.Instance.IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
            }
            
        }

        public void LogEvent(string name, string param1, string value1, string param2, string value2)
        {
            if (FirebaseService.Instance.IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, new Parameter(param1, value1), new Parameter(param2, value2));
            }
            
        }

        public void LogScene(string sceneName)
        {
            // NOTE: Dont need yet
        }
    }
}