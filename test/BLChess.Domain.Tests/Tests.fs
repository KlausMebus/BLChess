module Tests

open System
open Xunit
open BLChess.Domain
open System.IO

let isWhitePawn (pos: Position) =
    match pos.Piece with
    | Some p when p.Color = White && p.PieceType = Pawn -> true
    | _ -> false

let isBlackPawn (pos: Position) =
    match pos.Piece with
    | Some p when p.Color = Black && p.PieceType = Pawn -> true
    | _ -> false

let isWhitePiece pieceType (pos: Position) =
    match pos.Piece with
    | Some p when p.Color = White && p.PieceType = pieceType -> true
    | _ -> false

let isBlackPiece pieceType (pos: Position) =
    match pos.Piece with
    | Some p when p.Color = Black && p.PieceType = pieceType -> true
    | _ -> false

[<Fact>]
let ``Board can be created with default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Assert
    Assert.Equal(64, board.Positions.Length)

[<Fact>]
let ``Rank 2 is all white pawns in default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank2 = Board.getRank (Rank.ofInt 1) board
    // Assert
    Assert.All(rank2, fun pos -> Assert.True(isWhitePawn pos))

[<Fact>]
let ``Rank 7 is all black pawns in default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank7 = Board.getRank (Rank.ofInt 6) board
    // Assert
    Assert.All(rank7, fun pos -> Assert.True(isBlackPawn pos))

[<Fact>]
let ``Rank 1 contains correct white pieces in default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank1 = Board.getRank (Rank.ofInt 0) board
    // Assert
    let expected = [| Rook; Knight; Bishop; Queen; King; Bishop; Knight; Rook |]
    Assert.All(rank1, fun pos index -> Assert.True(isWhitePiece expected.[index] pos))

[<Fact>]
let ``Rank 8 contains correct black pieces in default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank8 = Board.getRank (Rank.ofInt 7) board
    // Assert
    let expected = [| Rook; Knight; Bishop; Queen; King; Bishop; Knight; Rook |]
    Assert.All(rank8, fun pos index -> Assert.True(isBlackPiece expected.[index] pos))

[<Fact>]
let ``getRank returns correct squares for rank 2`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank2 = Board.getRank (Rank.ofInt 1) board
    // Assert
    Assert.Equal(8, rank2.Length)
    Assert.All(rank2, fun pos -> Assert.True(isWhitePawn pos))

let getTestFilePath fileName =
    let baseDir = AppContext.BaseDirectory
    let projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."))
    Path.Combine(projectRoot, fileName)

[<Fact>]
let ``PGN parser can load Larsen-Spassky 1970 game`` () =
    // Arrange
    let filePath = getTestFilePath "Larsen-Spassky-1970.pgn"
    // Act
    let game = PGN.loadGameFromFile filePath
    // Assert
    Assert.Equal("Bent Larsen", (game.Tags |> List.find (fun t -> t.Name = "White")).Value)
    Assert.Equal("Boris Spassky", (game.Tags |> List.find (fun t -> t.Name = "Black")).Value)
    Assert.Equal("1-0", game.Result)
    Assert.True(game.Moves.Length > 0)

[<Fact>]
let ``toFEN returns correct FEN for default board`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let fen = Board.toFEN board
    // Assert
    Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", fen)
