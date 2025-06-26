# BLChess

BLChess is a database solution for chess games, designed to be fast and efficient. BLChess is named after Bent Larsen, which is a tribute to the legendary Danish chess player. The goal of BLChess is to provide a seamless experience for storing, retrieving, and analyzing chess game data.

## Features

- Fast and efficient storage of chess games
- Search and retrieval of games based on metadata
- Search of games based on positions
- Import and export of games in PGN format

## Technology Stack

- **General Technology**: .NET 9
- **Database**: SQLite
- **Programming Language**: F# for the domain logic, C# for the client application, Blazor for the web interface
- **Web Framework**: ASP.NET Core with Blazor for the web interface

## Architectural choices

- **Hexagonal Architecture**: The project follows a hexagonal architecture, which allows for separation of concerns and makes it easier to test and maintain the code.

## Domain model

- **Database**: A collection of chess games
- **Game**: Represents a single chess game, including the moves and metadata such as players, date, and result
- **Move**: Represents a single move in a chess game, including the piece moved, the starting and ending positions, and any special annotations (e.g., check, checkmate)
- **Piece**: Represents a chess piece, including its type (e.g., pawn, knight, bishop) and color (e.g., white, black)
- **Board**: Represents the chessboard, including the position of all pieces
- **LegalMove**: Represents a legal move for a piece, including the starting and ending positions and any special conditions (e.g., en passant, castling). So a LegalMove transform the current board state into a new board state.
- **Position**: Represents a specific position on the chessboard, including the coordinates (e.g., e4, d5) and the piece at that position
