namespace BLChess.Domain

type PackedCoordinate = byte

module Coordinate =
    /// Create Coordinate from file (0-7) and rank (0-7)
    let ofFileRank (file: int) (rank: int) : PackedCoordinate =
        let highBits = rank &&& 0b111 <<< 4
        let lowBits = file &&& 0b111
        byte (highBits ||| lowBits)

    /// Create Coordinate from algebraic notation (e.g. "e4")
    let ofAlgebraic (s: string) : PackedCoordinate =
        let file = int s.[0] - int 'a'
        let rank = int s.[1] - int '1'
        ofFileRank file rank

    /// Get file (0-7) from Coordinate
    let file (c: PackedCoordinate) = int (c &&& 0xFuy)

    /// Get rank (0-7) from Coordinate
    let rank (c: PackedCoordinate) = int ((c >>> 4) &&& 0x7uy)

    /// To algebraic notation (e.g. "e4")
    let toAlgebraic (c: PackedCoordinate) =
        System.String [| char (file c + int 'a'); char (rank c + int '1') |]

