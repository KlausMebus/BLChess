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
    let rank2 = Board.getRank board 1
    // Assert
    Assert.All(rank2, fun pos -> Assert.True(isWhitePawn pos))

[<Fact>]
let ``Rank 7 is all black pawns in default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank7 = Board.getRank board 6
    // Assert
    Assert.All(rank7, fun pos -> Assert.True(isBlackPawn pos))

[<Fact>]
let ``Rank 1 contains correct white pieces in default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank1 = Board.getRank board 0
    // Assert
    let expected = [| Rook; Knight; Bishop; Queen; King; Bishop; Knight; Rook |]
    Assert.All(rank1, fun pos index -> Assert.True(isWhitePiece expected.[index] pos))

[<Fact>]
let ``Rank 8 contains correct black pieces in default setup`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank8 = Board.getRank board 7
    // Assert
    let expected = [| Rook; Knight; Bishop; Queen; King; Bishop; Knight; Rook |]
    Assert.All(rank8, fun pos index -> Assert.True(isBlackPiece expected.[index] pos))

[<Fact>]
let ``getRank returns correct squares for rank 2`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let rank2 = Board.getRank board 1
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

[<Fact>]
let ``toConsoleString returns correct console representation for default board`` () =
    // Arrange
    let board = Board.createDefault()
    // Act
    let consoleString = Board.toConsoleString board
    // Assert
    let expected = "rnbqkbnr\npppppppp\n........\n........\n........\n........\nPPPPPPPP\nRNBQKBNR"
    Assert.Equal(expected, consoleString)

[<Fact>]
let ``toConsoleString shows empty board correctly`` () =
    // Arrange
    let emptyBoard = { Positions = Array.create 64 { Coord = 0uy; Piece = None } }
    // Act
    let consoleString = Board.toConsoleString emptyBoard
    // Assert
    let expected = "........\n........\n........\n........\n........\n........\n........\n........"
    Assert.Equal(expected, consoleString)

[<Fact>]
let ``toConsoleString shows partial board correctly`` () =
    // Arrange - create a board with just a few pieces
    let positions = Array.create 64 { Coord = 0uy; Piece = None }
    // Add white king on e1 (file 4, rank 0 -> index 4)
    positions.[4] <- { Coord = 4uy; Piece = Some { Color = White; PieceType = King } }
    // Add black king on e8 (file 4, rank 7 -> index 60)
    positions.[60] <- { Coord = 60uy; Piece = Some { Color = Black; PieceType = King } }
    let partialBoard = { Positions = positions }
    // Act
    let consoleString = Board.toConsoleString partialBoard
    // Assert
    let expected = "....k...\n........\n........\n........\n........\n........\n........\n....K..."
    Assert.Equal(expected, consoleString)
