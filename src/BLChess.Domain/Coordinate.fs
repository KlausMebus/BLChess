namespace BLChess.Domain

type Coordinate = byte

module Coordinate =
    /// Create Coordinate from file (0-7) and rank (0-7)
    let ofFileRank (file: int) (rank: int) : Coordinate =
        ((rank &&& 0x7) <<< 4 ||| (file &&& 0x7)) |> byte

    /// Create Coordinate from algebraic notation (e.g. "e4")
    let ofAlgebraic (s: string) : Coordinate =
        let file = int s.[0] - int 'a'
        let rank = int s.[1] - int '1'
        ofFileRank file rank

    /// Get file (0-7) from Coordinate
    let file (c: Coordinate) = int (c &&& 0xFuy)

    /// Get rank (0-7) from Coordinate
    let rank (c: Coordinate) = int ((c >>> 4) &&& 0x7uy)

    /// To algebraic notation (e.g. "e4")
    let toAlgebraic (c: Coordinate) =
        let f = char (file c + int 'a')
        let r = char (rank c + int '1')
        sprintf "%c%c" f r
