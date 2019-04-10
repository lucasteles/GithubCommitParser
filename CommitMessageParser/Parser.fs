module Parser

let prefix (p: string) (s: string) = if s.StartsWith(p) then Some () else None
let (|Feat|_|) = prefix "feat:"
let (|Fix|_|)  = prefix "fix:" 
let (|Refac|_|)  = prefix "refac:" 
let (|Test|_|)  = prefix "test:"
let (|Docs|_|)  = prefix "docs:"
let (|Chore|_|)  = prefix "chore:"
let (|Style|_|)  = prefix "style:"
let (|Add|_|)  = prefix "add:"
let (|Update|_|)  = prefix "update:"

type CommitTotals = { feat: int; fix: int; refac: int; test: int; chore: int; docs: int; style: int; add: int; update: int; other: int }
let commitTotalsZero = { feat= 0; fix= 0; refac= 0; test= 0; chore= 0; docs= 0; style= 0; add= 0; update= 0; other= 0 } 

let sumItem totals =
    function
    |Feat -> { totals with feat = totals.feat + 1 }
    |Fix -> { totals with fix = totals.fix + 1 }
    |Refac -> { totals with refac = totals.refac + 1 }
    |Test -> { totals with test  = totals.test + 1 }
    |Chore -> { totals with chore = totals.chore + 1 }
    |Docs -> { totals with docs = totals.docs + 1 }
    |Style -> { totals with style = totals.style + 1 }
    |Add -> { totals with add = totals.add + 1 }
    |Update -> { totals with update = totals.update + 1 }
    |_ -> { totals with other = totals.other + 1 }

let getTotalCommits: string seq -> CommitTotals=
    commitTotalsZero
    |> Seq.fold sumItem 
    
