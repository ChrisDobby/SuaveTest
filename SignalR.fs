namespace SuaveRestApi.SignalR

module SignalR =
    open Microsoft.AspNet.SignalR.Client
    open System.Threading.Tasks

    let patientUpdate (patientId : int) = async {
            let connection = new HubConnection("http://drugchartupdatehub.azurewebsites.net/")
            let proxy = connection.CreateHubProxy("PatientHub")
            let start:Task = connection.Start()
            start.Wait()
            let send:Task = proxy.Invoke("Updated", patientId)
            send.Wait()
        }
