module CoordinateTests

open Xunit
open BLChess.Domain
open BLChess.Domain.Coordinate

[<Fact>]
let ``ofFileRank produces correct coordinate for a1 and h8`` () =
    let a1 = ofFileRank 0 0
    let h8 = ofFileRank 7 7
    Assert.Equal(0uy, a1)
    Assert.Equal(119uy, h8)

[<Fact>]
let ``ofAlgebraic parses e4 and h1 correctly`` () =
    let e4 = ofAlgebraic "e4"
    let h1 = ofAlgebraic "h1"
    Assert.Equal(ofFileRank 4 3, e4)
    Assert.Equal(ofFileRank 7 0, h1)

[<Fact>]
let ``file and rank extract correct values`` () =
    let c = ofFileRank 2 5
    Assert.Equal(2, file c)
    Assert.Equal(5, rank c)

[<Fact>]
let ``toAlgebraic returns correct string`` () =
    let c = ofFileRank 3 6
    Assert.Equal("d7", toAlgebraic c)
