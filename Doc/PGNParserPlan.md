# Plan: Handwritten Recursive Descent PGN Parser

## 1. Define the PGN Grammar (Subset)
- Tag section: `[Key "Value"]`
- Move text: move numbers, SAN moves, comments `{...}`, variations `(...)`, NAGs `$n`, results (`1-0`, `0-1`, `1/2-1/2`, `*`)
- Optional: support for nested comments and variations

## 2. Design the AST (Abstract Syntax Tree)
- `Tag`
- `Move` (with SAN, optional comment, optional NAG, optional variations)
- `Game` (tags, moves, result)

## 3. Build Parser Primitives
- Functions to parse:
  - Whitespace
  - Identifiers (for tags, moves)
  - Quoted strings (for tag values)
  - Comments
  - NAGs
  - Results

## 4. Write Recursive Descent Functions
- `parseTag`: parses a single tag pair
- `parseTags`: parses all tags at the top
- `parseMove`: parses a move, including optional comment/NAG/variation
- `parseMoves`: parses the sequence of moves, handling variations recursively
- `parseResult`: parses the game result

## 5. Compose the Parsers
- `parseGame`: parses a full PGN game (tags + moves + result)
- Optionally, `parseGames`: parses multiple games from a file

## 6. Error Handling
- Return a result type: `Result<'a, string>` for each parser
- Provide meaningful error messages and positions

## 7. (Optional) Custom Combinators
- Write small helpers like `orElse`, `many`, `optional`, `between`, etc., to make parser composition easier and code more readable.

## 8. Testing
- Write unit tests for each parser function and for full games, including edge cases (comments, nested variations, malformed input).
