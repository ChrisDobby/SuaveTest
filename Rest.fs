namespace SuaveRestApi.Rest

open Suave.Json

module Rest =
    let json getFunc = async{
            let jsonBytes = toJson (getFunc())
            return System.Text.Encoding.UTF8.GetString jsonBytes
        }
