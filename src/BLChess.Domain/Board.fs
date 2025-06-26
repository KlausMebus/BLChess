namespace BLChess.Domain

open BLChess.Domain
open System.Text

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
    let getRank (rank: int) (board: Board) : Position[] =
        board.Positions |> Array.filter (fun pos -> Coordinate.rank pos.Coord = rank)

    let toFEN (board: Board) : string =
        let sb = StringBuilder()
        // FEN ranks are from 8 (top) to 1 (bottom)
        for rank = 7 downto 0 do
            let mutable empty = 0
            for file = 0 to 7 do
                let pos = board.Positions |> Array.find (fun p -> Coordinate.rank p.Coord = rank && Coordinate.file p.Coord = file)
                match pos.Piece with
                | Some p ->
                    if empty > 0 then sb.Append(empty) |> ignore; empty <- 0
                    sb.Append(Piece.toChar p) |> ignore
                | None ->
                    empty <- empty + 1
            if empty > 0 then sb.Append(empty) |> ignore
            if rank > 0 then sb.Append('/') |> ignore
        // Standard FEN for default board: white to move, all castling rights, no en passant, 0 halfmove, 1 fullmove
        sb.Append(" w KQkq - 0 1") |> ignore
        sb.ToString()
