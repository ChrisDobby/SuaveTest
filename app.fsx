#r "packages/Suave/lib/net40/Suave.dll"
#r "System.Runtime.Serialization.dll"
#load "Db.fs"
#load "Rest.fs"

open Suave.Types
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Json
open SuaveRestApi.Db
open SuaveRestApi.Rest

let jsonOK json = 
    (OK (json)
        >>= Writers.setHeader "Access-Control-Allow-Origin" "*" 
        >>= Writers.setMimeType "application/json; charset=utf-8")
        
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

// ----------------------------------------------------------------------------
// Entry point - the 'build' script assumes there is a top-level value
// called `app` - so define `app` to refer to the current step!
// ----------------------------------------------------------------------------

let setupDb() = Db.reset()

// TODO: Add prescriptions to patient record, Add prescription including call to signalr hub, get all drugs available (create json file from chemical table?)
let app =
        choose
            [ 
                GET >>= choose
                    [
                        path "/wards" >>= wards
                        path "/drugs" >>= OK ("Get Drugs");
                        pathScan "/wardpatients/%d" (fun wardId -> patientsOnWard wardId);
                        pathScan "/patient/%d" (fun patientId -> patient patientId);
                        path "/resetdata" >>= resetData;
                    ]
                POST >>= choose
                    [
                        path "/prescribe" >>= mapJson(fun newPrescription -> Db.addPrescription newPrescription)
                    ]
                    
                RequestErrors.NOT_FOUND "Found no handlers"
            ]
