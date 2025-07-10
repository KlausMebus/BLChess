namespace BLChess.Domain

open BLChess.Domain

/// Strongly-typed chessboard rank (0 = rank 1, 7 = rank 8)
type Rank = private Rank of int

module Rank =
    let ofInt n =
        if n < 0 || n > 7 then invalidArg "n" "Rank must be between 0 and 7"
        Rank n
    let toInt (Rank n) = n

/// Represents the chessboard (8x8 positions)
type Board = { Positions: Position array }

module Board =
    /// Create a board with the default chess setup
    let createDefault () : Board =
        let positions =
            [|
                for rank in 0..7 do
                    for file in 0..7 do
                        let coord = Coordinate.ofFileRank file rank
                        let piece =
                            match rank, file with
                            // White back rank
                            | 0, f ->
                                match f with
                                | 0 | 7 -> Some { Color = White; PieceType = Rook }
                                | 1 | 6 -> Some { Color = White; PieceType = Knight }
                                | 2 | 5 -> Some { Color = White; PieceType = Bishop }
                                | 3 -> Some { Color = White; PieceType = Queen }
                                | 4 -> Some { Color = White; PieceType = King }
                                | _ -> None
                            // White pawns
                            | 1, _ -> Some { Color = White; PieceType = Pawn }
                            // Black pawns
                            | 6, _ -> Some { Color = Black; PieceType = Pawn }
                            // Black back rank
                            | 7, f ->
                                match f with
                                | 0 | 7 -> Some { Color = Black; PieceType = Rook }
                                | 1 | 6 -> Some { Color = Black; PieceType = Knight }
                                | 2 | 5 -> Some { Color = Black; PieceType = Bishop }
                                | 3 -> Some { Color = Black; PieceType = Queen }
                                | 4 -> Some { Color = Black; PieceType = King }
                                | _ -> None
                            | _ -> None
                        yield { Coord = coord; Piece = piece }
            |]
        { Positions = positions }

    /// Get all positions for a given rank (0 = rank 1, 7 = rank 8)
    let getRank (board: Board) rank : Position[] =
        board.Positions.[rank * 8 .. rank * 8 + 7]

    /// Helper to build FEN for a single rank recursively
    let rec fenRank (positions: Position list) (empty: int) (acc: string) : string =
        match positions with
        | [] -> if empty > 0 then acc + string empty else acc
        | pos :: rest ->
            match pos.Piece with
            | Some p ->
                let acc' = if empty > 0 then acc + string empty + string (Piece.toChar p) else acc + string (Piece.toChar p)
                fenRank rest 0 acc'
            | None -> fenRank rest (empty + 1) acc

    let toFEN (board: Board) : string =
        let ranks = [ for r in 7 .. -1 .. 0 -> getRank board r |> Array.toList ]
        let fenBody =
            ranks
            |> List.map (fun rankPositions -> fenRank rankPositions 0 "")
            |> String.concat "/"
        fenBody + " w KQkq - 0 1"

    /// Convert board to console string representation
    let toConsoleString (board: Board) : string =
        let rankToString (positions: Position[]) : string =
            positions
            |> Array.map (fun pos -> 
                match pos.Piece with
                | Some piece -> Piece.toChar piece
                | None -> '.')
            |> System.String
        
        let ranks = [ for r in 7 .. -1 .. 0 -> getRank (Rank.ofInt r) board ]
        ranks
        |> List.map rankToString
        |> String.concat "\n"




