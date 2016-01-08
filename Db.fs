namespace SuaveRestApi.Db

open System
open Suave.Json
open System.Runtime.Serialization

[<DataContract>]
type Ward =
    {
        [<field: DataMember(Name = "id")>]
        Id : int;
        [<field: DataMember(Name = "name")>]
        WardName : string;
    }

type AdministrationStatus =
    | Administered = 0
    | NotGiven = 1
    | Due = 2   

[<DataContract>]
type Administration =
    {
        [<field: DataMember(Name = "admindatetime")>]
        AdminDateTime : DateTime;
        [<field: DataMember(Name = "status")>]
        Status : AdministrationStatus;
    }

[<DataContract>]
type Prescription =
    {
        [<field: DataMember(Name = "id")>]
        Id : int;
        [<field: DataMember(Name = "product")>]
        Drug : string;
        [<field: DataMember(Name = "dose")>]
        Dose : string;
        [<field: DataMember(Name = "start")>]
        Start : string;
        [<field: DataMember(Name = "prescriber")>]
        Prescriber : string;
        [<field: DataMember(Name = "administrations")>]
        Administrations : Administration array;
    }

[<DataContract>]
type Patient =
    {
        [<field: DataMember(Name = "id")>]
        Id : int;
        [<field: DataMember(Name = "name")>]
        Name : string;
        [<field: DataMember(Name = "nhsnumber")>]
        NhsNumber : string;
        [<field: DataMember(Name = "imagename")>]
        ImageName : string;
        WardId : int;
    }

[<DataContract>]
type PatientRecord =
    {
        [<field: DataMember(Name = "id")>]
        Id : int;
        [<field: DataMember(Name = "name")>]
        Name : string;
        [<field: DataMember(Name = "description")>]
        Description : string;
        [<field: DataMember(Name = "nhsnumber")>]
        NhsNumber : string;
        [<field: DataMember(Name = "dateofbirth")>]
        DateOfBirth : string;
        [<field: DataMember(Name = "gender")>]
        Gender : string;
        [<field: DataMember(Name = "age")>]
        Age : string;
        [<field: DataMember(Name = "height")>]
        Height : string;
        [<field: DataMember(Name = "weight")>]
        Weight : string;
        [<field: DataMember(Name = "ward")>]
        Ward : string;
        [<field: DataMember(Name = "consultant")>]
        Consultant : string;
        [<field: DataMember(Name = "imagename")>]
        ImageName : string;
        [<field: DataMember(Name = "maincontact")>]
        MainContact : string;
        [<field: DataMember(Name = "prescriptions")>]
        Prescriptions : Prescription array
    }

[<DataContract>]
type NewPrescription =
    {
        [<field: DataMember(Name = "patientid")>]
        PatientId : int;
        [<field: DataMember(Name = "product")>]
        Drug : string;
        [<field: DataMember(Name = "dose")>]
        Dose : string;
        [<field: DataMember(Name = "start")>]
        Start : string;
    }

module Db =
    open System.Collections.Generic

    let private wards = new Dictionary<int, Ward>()
    let private patients = new Dictionary<int, Patient>()
    let private patientRecords = new Dictionary<int, PatientRecord>()

    let private resetWards() =
        wards.Clear()
        wards.Add(1,
            {
                Id = 1
                WardName = "Paediatrics"
            })

        wards.Add(2,
            {
                Id = 2
                WardName = "Oncology"
            })

        wards.Add(3,
            {
                Id = 3
                WardName = "Physciatry"
            })

    let private resetPatients() =
        patients.Clear()
        patientRecords.Clear()

        patients.Add(1,
            {
                Id = 1
                Name = "Jacqueline Saunders"
                NhsNumber = "942 842 5729"
                ImageName = "blank.png"
                WardId = 3
            })

        patientRecords.Add(1,
            {
                Id = 1
                Name = "Jacqueline Saunders"
                Description = "SAUNDERS, Jacqueline (Mrs)"
                NhsNumber = "942 842 5729"
                DateOfBirth = "23-Sep-1947"
                Gender = "female"
                Age = "68y"
                Height = ""
                Weight = ""
                Ward = "Physciatry"
                Consultant = "BURGESS, Neil (Dr)"
                ImageName = "blank.png"
                MainContact = "(01945) 310216"
                Prescriptions = 
                    [|
                        { Id = 1 
                          Drug = "Diamorphine Hydrochloride"
                          Dose = "intravenous infusion at 5 mg/hour, Single Dose"
                          Start = "01 Jan 2015 at 08:00"
                          Prescriber = "Chris Dobson"
                          Administrations = 
                            [0..int((System.DateTime.Now.Date - System.DateTime.Now.AddMonths(-2)).TotalDays)]
                            |> List.map(fun dt -> 
                                            let adminDateTime = System.DateTime.Now.AddMonths(-2).AddDays(float dt)
                                            {
                                                AdminDateTime = adminDateTime;
                                                Status = if adminDateTime.Date = DateTime.Now.Date then AdministrationStatus.NotGiven else AdministrationStatus.Due
                                            })
                            |> List.toArray                   
                        }
                        { Id = 2
                          Drug = "Paracetamol"
                          Dose = "250mg oral, Four times a day"
                          Start = "01 Jan 2015 at 08:00"
                          Prescriber = "Chris Dobson"
                          Administrations = 
                            [0..int((System.DateTime.Now.Date - System.DateTime.Now.AddMonths(-2)).TotalDays)]
                            |> List.map(fun dt -> 
                                            let adminDateTime = System.DateTime.Now.AddMonths(-2).AddDays(float dt)
                                            {
                                                AdminDateTime = adminDateTime;
                                                Status = if adminDateTime.Date = DateTime.Now.Date then AdministrationStatus.NotGiven else AdministrationStatus.Due
                                            })
                            |> List.toArray                   
                        }
                     |]
             })

        patients.Add(2,
            {
                Id = 2
                Name = "Alan Riley"
                NhsNumber = "533 730 0075"
                ImageName = "blank.png"
                WardId = 3
            })

        patientRecords.Add(2,
            {
                Id = 2
                Name = "Alan Riley"
                Description = "RILEY, Alan (Mr)"
                NhsNumber = "533 730 0075"
                DateOfBirth = "17-Feb-1944"
                Gender = "male"
                Age = "71y"
                Height = ""
                Weight = ""
                Ward = "Physciatry"
                Consultant = "BURGESS, Neil (Dr)"
                ImageName = "blank.png"
                MainContact = ""
                Prescriptions = [||]
            })

        patients.Add(3,
            {
                Id = 3
                Name = "Michelle Bond"
                NhsNumber = "851 078 0277"
                ImageName = "blank.png"
                WardId = 3
            })

        patients.Add(4,
            {
                Id = 4
                Name = "Helen Birch"
                NhsNumber = "766 482 4494"
                ImageName = "blank.png"
                WardId = 2
            })

        patients.Add(5,
            {
                Id = 5
                Name = "Peter Bond"
                NhsNumber = "987 106 0491"
                ImageName = "blank.png"
                WardId = 2
            })

        patients.Add(6,
            {
                Id = 6
                Name = "Martin Johnson"
                NhsNumber = "712 754 7998"
                ImageName = "blank.png"
                WardId = 2
            })

        patients.Add(7,
            {
                Id = 7
                Name = "Mark Slater"
                NhsNumber = "506 094 9648"
                ImageName = "blank.png"
                WardId = 2
            })

    let reset() = async {
            resetWards()
            resetPatients()
        }

    let getWards() =
        wards.Values |> Seq.map(fun w -> w) |> Seq.toArray

    let getPatients wardId =
        patients.Values |> Seq.map(fun p -> p) |> Seq.filter (fun p -> p.WardId = wardId) |> Seq.toArray

    let getPatientRecord patientId =
        patientRecords.Values |> Seq.map(fun p -> p) |> Seq.filter (fun p -> p.Id = patientId) |> Seq.exactlyOne

    let addPrescription newPrescription =
        let patient = patientRecords.[newPrescription.PatientId]
        patientRecords.Remove(newPrescription.PatientId) |> ignore
        patientRecords.Add(newPrescription.PatientId, 
            { patient 
                with Prescriptions = patient.Prescriptions 
                    |> Array.toList
                    |> List.append [{ Id = 999
                                      Drug = newPrescription.Drug
                                      Dose = newPrescription.Dose
                                      Start = newPrescription.Start
                                      Prescriber = "Oops not logged in"
                                      Administrations = [||]}
                                   ]
                    |> List.toArray
            }
        )
        patientRecords.[newPrescription.PatientId]