namespace BLChess.Domain

// Domain model based on SolutionIdea.md

/// Represents a color of a chess piece
type Color = White | Black

/// Represents the type of a chess piece
type PieceType = King | Queen | Rook | Bishop | Knight | Pawn

/// Represents a chess piece
type Piece = { Color: Color; PieceType: PieceType }

module Piece =
    let toChar (piece: Piece) : char =
        match piece.Color, piece.PieceType with
        | White, King -> 'K'
        | White, Queen -> 'Q'
        | White, Rook -> 'R'
        | White, Bishop -> 'B'
        | White, Knight -> 'N'
        | White, Pawn -> 'P'
        | Black, King -> 'k'
        | Black, Queen -> 'q'
        | Black, Rook -> 'r'
        | Black, Bishop -> 'b'
        | Black, Knight -> 'n'
        | Black, Pawn -> 'p'

    let fromChar (c: char) : Piece option =
        match c with
        | 'K' -> Some { Color = White; PieceType = King }
        | 'Q' -> Some { Color = White; PieceType = Queen }
        | 'R' -> Some { Color = White; PieceType = Rook }
        | 'B' -> Some { Color = White; PieceType = Bishop }
        | 'N' -> Some { Color = White; PieceType = Knight }
        | 'P' -> Some { Color = White; PieceType = Pawn }
        | 'k' -> Some { Color = Black; PieceType = King }
        | 'q' -> Some { Color = Black; PieceType = Queen }
        | 'r' -> Some { Color = Black; PieceType = Rook }
        | 'b' -> Some { Color = Black; PieceType = Bishop }
        | 'n' -> Some { Color = Black; PieceType = Knight }
        | 'p' -> Some { Color = Black; PieceType = Pawn }
        | _ -> None

/// Represents a position on the board (e.g., e4, d5)
type Position = { Coord: Coordinate; Piece: Piece option }

/// Represents a move in a chess game
type Move = {
    Piece: Piece
    From: Coordinate
    To: Coordinate
    Annotation: string option // e.g., check, checkmate, etc.
}

/// Represents a single chess game
type Game = {
    Metadata: Map<string, string> // e.g., players, date, result
    Moves: Move list
    Result: string
}

/// Represents a database of chess games
type Database = Game list
