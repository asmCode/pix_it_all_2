using UnityEngine;

public class GSpark : Singleton<GSpark>
{
    public event System.Action<bool> Authenticated;

    public bool IsAuthenticated
    {
        get;
        private set;
    }

    public bool IsAuthenticating
    {
        get;
        private set;
    }

    public void Authenticate(System.Action<bool> callback)
    {
        if (IsAuthenticating)
            return;

        IsAuthenticating = true;

        new GameSparks.Api.Requests.DeviceAuthenticationRequest()
            .Send((response) =>
            {
                Dispatcher.Dispatch((what) =>
                {
                    OnAuthenticated(response);
                });
            });
    }

    private void OnAuthenticated(GameSparks.Api.Responses.AuthenticationResponse response)
    {
        IsAuthenticating = false;
        IsAuthenticated = !response.HasErrors;

        if (Authenticated != null)
            Authenticated(IsAuthenticated);
    }
}
