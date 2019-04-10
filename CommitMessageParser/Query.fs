module Query

open FSharp.Data.HttpRequestHeaders
open FSharp.Data
open FSharp.Data.JsonExtensions

let token = "b981b772b2dff479b9c8388fd021bf6445b0022a"
let url = "https://api.github.com/graphql"


let query = """
{
   rateLimit {
     cost
     remaining
     resetAt
   }
   search(query: "rafaeldelboni", type: REPOSITORY, first: 100) {
     repositoryCount
     pageInfo {
       endCursor
       hasNextPage
     }
     edges {
       node {
         ... on Repository {
           id
           name
           createdAt
           description
           isArchived
           isPrivate
           url
           owner {
             login
             id
             __typename
             url
           }
           assignableUsers {
             totalCount
           }
           licenseInfo {
             key
           }
           defaultBranchRef {
             target {
               ... on Commit {
                 history {
                   totalCount
                   edges {
                     node {
                       ... on Commit {
                         committedDate
                         message
                       }
                     }
                   }
                 }
               }
             }
           }
         }
       }
     }
   }
 }
"""

let parsedQuery = query.Replace("\"","\\\"").Replace("\r\n","\\r\\n");
let body = sprintf """ { "query": "%s"} """ parsedQuery

let getJson () =
     Http.AsyncRequestString
          (url, 
            headers = ["Authorization", "bearer " + token; ContentType HttpContentTypes.Json; "user-agent", "api" ],
            body = TextRequest body)


let getCommits () = async {
   let! json = getJson()
   let parsed = JsonValue.Parse(json)
   let repos = parsed?data?search?edges 
   let commits = repos.AsArray()
                    |> Array.map (fun x -> x?node?defaultBranchRef?target?history?edges.AsArray()) 
                    |> Array.collect(id)
                    |> Array.map(fun x -> x?node?message.AsString()) 

   return commits

}

