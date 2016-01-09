namespace SuaveRestApi.Rest

open Suave.Http.Successful
open Suave.Json

module Rest =
    let json getFunc = async{
//            let data = getFunc()
            let jsonBytes = toJson (getFunc())
//            let jsonBytes = toJson (data)
            return System.Text.Encoding.UTF8.GetString jsonBytes
        }
