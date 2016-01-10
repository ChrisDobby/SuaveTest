#r "packages/Suave/lib/net40/Suave.dll"
// #r "packages/Microsoft.Bcl/lib/net40/System.IO.dll"
// #r "packages/Microsoft.Bcl/lib/net40/System.Runtime.dll"
// #r "packages/Microsoft.Bcl/lib/net40/System.Threading.Tasks.dll"
// #r "packages/Microsoft.Net.Http/lib/net40/System.Net.Http.dll"
// #r "packages/Microsoft.Net.Http/lib/net40/System.Net.Http.Extensions.dll"
// #r "packages/Microsoft.Net.Http/lib/net40/System.Net.Http.Primitives.dll"
// #r "packages/Microsoft.Net.Http/lib/net40/System.Net.Http.WebRequest.dll"
// #r "packages/Newtonsoft.Json/lib/net40/Newtonsoft.Json.dll"
// #r "packages/Microsoft.AspNet.SignalR.Client/lib/net40/Microsoft.AspNet.SignalR.Client.dll"
#r "System.Runtime.Serialization.dll"

#load "Db.fs"
#load "Rest.fs"
//#load "SignalR.fs"

open Suave
open Suave.Filters
open Suave.Successful
open Suave.Operators
open Suave.Json
open SuaveRestApi.Db
open SuaveRestApi.Rest
//open SuaveRestApi.SignalR

let jsonOK json = 
    (OK (json) 
        >=> Writers.setMimeType "application/json; charset=utf-8" 
        >=> Writers.setHeader "Access-Control-Allow-Origin" "*")

let patientsOnWard wardId = fun ctx -> async {
    let! json = Rest.json (fun () -> Db.getPatients wardId)
    return! ctx |> jsonOK json
}

let patient id = fun ctx -> async {
    let! json = Rest.json (fun () -> Db.getPatientRecord id)
    return! ctx |> jsonOK json
}

let wards = fun ctx -> async {
    let! json = Rest.json (fun () -> Db.getWards())
    return! ctx |> jsonOK json
}

let resetData = fun ctx -> async {
    do! Db.reset()
    return! ctx |> OK ("Data Reset")
}

let app =
    choose
        [GET >=> choose
            [
                path "/wards" >=> wards
                path "/drugs" >=> OK ("Get Drugs")
                pathScan "/wardpatients/%d" (fun wardId -> patientsOnWard wardId)
                pathScan "/patient/%d" (fun patientId -> patient patientId)
                path "/resetdata" >=> resetData
            ]

         POST >=> choose
            [path "/prescribe" >=> mapJson
                (fun newPrescription -> let patient = Db.addPrescription newPrescription
//                                        SignalR.patientUpdate (patient.Id) |> Async.Start
                                        patient)]

         RequestErrors.NOT_FOUND "Found no handlers"
        ]
