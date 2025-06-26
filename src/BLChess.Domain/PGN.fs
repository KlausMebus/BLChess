namespace BLChess.Domain

module PGN =
    open System.IO

    type Tag = { Name: string; Value: string }
    type Move = string
    type Game = { Tags: Tag list; Moves: Move list; Result: string }

    let parseTags (lines: string seq) =
        lines
        |> Seq.choose (fun line ->
            if line.StartsWith("[") && line.EndsWith("]") then
                let content = line.Substring(1, line.Length - 2)
                match content.IndexOf(' ') with
                | -1 -> None
                | idx ->
                    let name = content.Substring(0, idx)
                    let value = content.Substring(idx + 1).Trim().Trim('"')
                    Some { Name = name; Value = value }
            else None)
        |> Seq.toList

    let removeComments (s: string) =
        let rec loop acc i depth =
            if i >= s.Length then System.String(acc |> List.rev |> List.toArray)
            else
                match s.[i] with
                | '{' -> loop acc (i+1) (depth+1)
                | '}' when depth > 0 -> loop acc (i+1) (depth-1)
                | c when depth = 0 -> loop (c::acc) (i+1) depth
                | _ -> loop acc (i+1) depth
        loop [] 0 0

    let parseMoves (lines: string seq) =
        lines
        |> Seq.filter (fun line -> not (line.StartsWith("[")))
        |> String.concat " "
        |> removeComments
        |> fun s ->
            s.Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries)
            |> Array.toList
            |> List.filter (fun m ->
                // Remove move numbers and results
                not (System.Char.IsDigit m.[0] && m.Contains(".")) &&
                m <> "1-0" && m <> "0-1" && m <> "1/2-1/2" && m <> "*")

    let parseResult (lines: string seq) =
        lines
        |> Seq.tryFind (fun line ->
            let trimmed = line.Trim()
            trimmed = "1-0" || trimmed = "0-1" || trimmed = "1/2-1/2" || trimmed = "*")
        |> Option.defaultValue "*"

    let parseGame (pgnText: string) : Game =
        let lines = pgnText.Split([| '\n'; '\r' |], System.StringSplitOptions.RemoveEmptyEntries) |> Seq.ofArray
        let tags = parseTags lines
        let moves = parseMoves lines
        let result =
            match tags |> List.tryFind (fun t -> t.Name = "Result") with
            | Some t -> t.Value
            | None -> parseResult lines
        { Tags = tags; Moves = moves; Result = result }

    let loadGameFromFile (filePath: string) : Game =
        let text = File.ReadAllText(filePath)
        parseGame text
