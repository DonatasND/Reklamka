using System;
using System.Collections.Generic;

public enum M1Color { Blue, Green, Yellow, Red }
public enum M1SolidKind { Original, Foam }
public enum M1StarState { Contained, Released, Collected }
public enum M1TurnState
{
    PlayerReady, ColorSelected, Burst, LiquidCreated, FlowAndMassSettling, FlowSettling,
    LiquidClassification, Foaming, FoamSolidification, FinalSettling, FinalizeStarStates,
    WinCheck, HandUpdate, LoseCheck, LevelComplete, LevelFailed
}

public sealed class M1Fragment
{
    public int Id;
    public M1Color Color;
    public M1SolidKind Kind;
    public int Column;
    public int Row;
    public int VisualSeed;
}

public sealed class M1Star
{
    public int Id;
    public int HostFragmentId;
    public int Column;
    public int Row;
    public M1StarState State;
}

public sealed class M1Liquid
{
    public M1Color Color;
    public int Column;
    public int Row;
    public int VisualSeed;
}

/// <summary>
/// Deterministic spatial state for M1 Prototype Level 01. The coarse board is a connectivity
/// representation only; presentation supplies the soft, gravity-like M0-compatible movement.
/// </summary>
public sealed class M1BoardModel
{
    public const int Width = 6;
    public const int Height = 7;

    public readonly List<M1Fragment> Solids = new List<M1Fragment>();
    public readonly List<M1Liquid> Liquids = new List<M1Liquid>();
    public readonly List<M1Star> Stars = new List<M1Star>();
    public readonly M1Color?[] Hand = new M1Color?[4];

    private readonly List<M1Color> queue = new List<M1Color>
    {
        M1Color.Yellow, M1Color.Red, M1Color.Green, M1Color.Yellow, M1Color.Red
    };
    private int queueIndex;
    private int selectedSlot = -1;
    private M1Color selectedColor;
    private bool yellowShifted;

    public M1Color? Next { get; private set; }
    public M1TurnState State { get; private set; }
    public bool InputLocked { get; private set; }
    public bool DynamicRouteOpenedThisTurn { get; private set; }
    public int DrainedThisTurn { get; private set; }
    public int FoamCreatedThisTurn { get; private set; }
    public int TurnNumber { get; private set; }

    public M1BoardModel()
    {
        Hand[0] = M1Color.Blue;
        Hand[1] = M1Color.Green;
        Hand[2] = M1Color.Yellow;
        Hand[3] = M1Color.Red;
        Next = M1Color.Blue;
        State = M1TurnState.PlayerReady;
        CreateLevel01();
    }

    public bool IsChargeEnabled(int slot)
    {
        return slot >= 0 && slot < Hand.Length && Hand[slot].HasValue && IsColorEnabled(Hand[slot].Value);
    }

    public bool IsColorEnabled(M1Color color)
    {
        for (var i = 0; i < Solids.Count; i++)
            if (Solids[i].Color == color) return true;
        return false;
    }

    public bool SelectCharge(int slot)
    {
        if (State != M1TurnState.PlayerReady || InputLocked || !IsChargeEnabled(slot)) return false;
        selectedSlot = slot;
        selectedColor = Hand[slot].Value;
        Hand[slot] = null;
        InputLocked = true;
        DynamicRouteOpenedThisTurn = false;
        DrainedThisTurn = 0;
        FoamCreatedThisTurn = 0;
        State = M1TurnState.ColorSelected;
        return true;
    }

    public void Burst()
    {
        Require(M1TurnState.ColorSelected);
        State = M1TurnState.Burst;
        var removed = new List<M1Fragment>();
        for (var i = Solids.Count - 1; i >= 0; i--)
        {
            var solid = Solids[i];
            if (solid.Color != selectedColor) continue;
            removed.Add(solid);
            Solids.RemoveAt(i);
            Liquids.Add(new M1Liquid { Color = solid.Color, Column = solid.Column, Row = solid.Row, VisualSeed = solid.VisualSeed });
            ReleaseHostedStar(solid);
        }

        if (removed.Count == 0) throw new InvalidOperationException("Enabled charge selected without matching Solid Fragment.");
        State = M1TurnState.LiquidCreated;
    }

    public void FlowAndSettle()
    {
        Require(M1TurnState.LiquidCreated);
        State = M1TurnState.FlowAndMassSettling;
        if (selectedColor == M1Color.Green)
        {
            // The lower yellow support slides aside only once Green removes its structural wall.
            // This is a deterministic, visible settling event that opens the drain channel.
            ShiftYellowSupport();
            DynamicRouteOpenedThisTurn = true;
            MoveReleasedStars(); // Star A can collect during Flow, before finalization.
        }
    }

    public void FlowSettling()
    {
        Require(M1TurnState.FlowAndMassSettling);
        State = M1TurnState.FlowSettling;
        DrainReachableLiquids();
    }

    public void ClassifyLiquid()
    {
        Require(M1TurnState.FlowSettling);
        State = M1TurnState.LiquidClassification;
        DrainReachableLiquids(); // Re-check after all same-turn structural movement.
    }

    public void BeginFoaming()
    {
        Require(M1TurnState.LiquidClassification);
        State = M1TurnState.Foaming;
    }

    public void SolidifyFoam()
    {
        Require(M1TurnState.Foaming);
        State = M1TurnState.FoamSolidification;
        for (var i = 0; i < Liquids.Count; i++)
        {
            var liquid = Liquids[i];
            var cell = FindFoamCell(liquid.Column, liquid.Row);
            Solids.Add(new M1Fragment
            {
                Id = NextFragmentId(), Color = liquid.Color, Kind = M1SolidKind.Foam,
                Column = cell.column, Row = cell.row, VisualSeed = liquid.VisualSeed
            });
            FoamCreatedThisTurn++;
        }
        Liquids.Clear();
    }

    public void FinalSettle()
    {
        Require(M1TurnState.FoamSolidification);
        State = M1TurnState.FinalSettling;
        MoveReleasedStars();
    }

    public void FinalizeStars()
    {
        Require(M1TurnState.FinalSettling);
        State = M1TurnState.FinalizeStarStates;
        MoveReleasedStars();
    }

    public bool WinCheck()
    {
        Require(M1TurnState.FinalizeStarStates);
        State = M1TurnState.WinCheck;
        for (var i = 0; i < Stars.Count; i++)
            if (Stars[i].State != M1StarState.Collected) return false;
        State = M1TurnState.LevelComplete;
        return true;
    }

    public bool HandUpdateAndLoseCheck()
    {
        Require(M1TurnState.WinCheck);
        State = M1TurnState.HandUpdate;
        Hand[selectedSlot] = Next;
        Next = queueIndex < queue.Count ? queue[queueIndex++] : (M1Color?)null;

        State = M1TurnState.LoseCheck;
        if (AllStarsCollected()) throw new InvalidOperationException("Win must be resolved before Hand Update.");
        if (Next == null && queueIndex >= queue.Count && !AnyEnabledCharge())
        {
            State = M1TurnState.LevelFailed;
            return true;
        }

        State = M1TurnState.PlayerReady;
        InputLocked = false;
        TurnNumber++;
        return false;
    }

    private void DrainReachableLiquids()
    {
        for (var i = Liquids.Count - 1; i >= 0; i--)
        {
            var liquid = Liquids[i];
            if (!HasDrainPath(liquid.Column, liquid.Row)) continue;
            Liquids.RemoveAt(i);
            DrainedThisTurn++;
        }
    }

    private bool HasDrainPath(int startColumn, int startRow)
    {
        var seen = new bool[Width, Height];
        var frontier = new Queue<Cell>();
        frontier.Enqueue(new Cell(startColumn, startRow));
        seen[startColumn, startRow] = true;
        while (frontier.Count > 0)
        {
            var cell = frontier.Dequeue();
            if (cell.row == 0 && (cell.column == 2 || cell.column == 3)) return true;
            TryVisit(cell.column - 1, cell.row, seen, frontier);
            TryVisit(cell.column + 1, cell.row, seen, frontier);
            TryVisit(cell.column, cell.row - 1, seen, frontier);
            TryVisit(cell.column, cell.row + 1, seen, frontier);
        }
        return false;
    }

    private void TryVisit(int column, int row, bool[,] seen, Queue<Cell> frontier)
    {
        if (column < 0 || column >= Width || row < 0 || row >= Height || seen[column, row] || HasSolid(column, row)) return;
        seen[column, row] = true;
        frontier.Enqueue(new Cell(column, row));
    }

    private void MoveReleasedStars()
    {
        for (var i = 0; i < Stars.Count; i++)
        {
            var star = Stars[i];
            if (star.State != M1StarState.Released) continue;
            var moved = true;
            while (moved)
            {
                moved = false;
                if (star.Row == 0 && (star.Column == 2 || star.Column == 3))
                {
                    star.State = M1StarState.Collected;
                    break;
                }
                if (star.Row > 0 && !HasSolid(star.Column, star.Row - 1))
                {
                    star.Row--;
                    moved = true;
                    continue;
                }
                if (star.Row == 0)
                {
                    var target = star.Column < 2 ? star.Column + 1 : (star.Column > 3 ? star.Column - 1 : star.Column);
                    if (target != star.Column && !HasSolid(target, 0))
                    {
                        star.Column = target;
                        moved = true;
                    }
                }
            }
        }
    }

    private void ShiftYellowSupport()
    {
        if (yellowShifted) return;
        var support = FindSolid(2, 1);
        if (support != null && support.Color == M1Color.Yellow && !HasSolid(1, 1))
        {
            support.Column = 1;
            yellowShifted = true;
        }
    }

    private void ReleaseHostedStar(M1Fragment fragment)
    {
        for (var i = 0; i < Stars.Count; i++)
        {
            var star = Stars[i];
            if (star.State == M1StarState.Contained && star.HostFragmentId == fragment.Id)
            {
                star.State = M1StarState.Released;
                star.Column = fragment.Column;
                star.Row = fragment.Row;
            }
        }
    }

    private (int column, int row) FindFoamCell(int column, int row)
    {
        // Never solidify inside the drain. Prefer a lower local support so trapped foam can
        // visibly retain/re-block a released Star until a future matching burst removes it.
        var candidates = new[] { new Cell(column, row - 1), new Cell(column, row), new Cell(column - 1, row), new Cell(column + 1, row), new Cell(column, row + 1) };
        for (var i = 0; i < candidates.Length; i++)
        {
            var cell = candidates[i];
            if (cell.column >= 0 && cell.column < Width && cell.row >= 0 && cell.row < Height && !IsDrain(cell.column, cell.row) && !HasSolid(cell.column, cell.row))
                return (cell.column, cell.row);
        }
        return (column, Math.Max(1, row));
    }

    private bool HasSolid(int column, int row) { return FindSolid(column, row) != null; }

    private M1Fragment FindSolid(int column, int row)
    {
        for (var i = 0; i < Solids.Count; i++)
            if (Solids[i].Column == column && Solids[i].Row == row) return Solids[i];
        return null;
    }

    private bool AnyEnabledCharge()
    {
        for (var i = 0; i < Hand.Length; i++) if (IsChargeEnabled(i)) return true;
        return false;
    }

    private bool AllStarsCollected()
    {
        for (var i = 0; i < Stars.Count; i++) if (Stars[i].State != M1StarState.Collected) return false;
        return true;
    }

    private int NextFragmentId() { return 1000 + Solids.Count + Liquids.Count + FoamCreatedThisTurn; }
    private bool IsDrain(int column, int row) { return row == 0 && (column == 2 || column == 3); }
    private void Require(M1TurnState expected) { if (State != expected) throw new InvalidOperationException("Expected " + expected + ", found " + State + "."); }

    private void CreateLevel01()
    {
        var id = 1;
        Add(ref id, M1Color.Blue, 1, 6); Add(ref id, M1Color.Red, 4, 6, true);
        Add(ref id, M1Color.Green, 0, 5); Add(ref id, M1Color.Blue, 2, 5, true); Add(ref id, M1Color.Blue, 3, 5); Add(ref id, M1Color.Red, 5, 5);
        Add(ref id, M1Color.Blue, 1, 4); Add(ref id, M1Color.Blue, 4, 4); Add(ref id, M1Color.Yellow, 4, 1);
        Add(ref id, M1Color.Green, 0, 3); Add(ref id, M1Color.Green, 1, 3); Add(ref id, M1Color.Green, 2, 3); Add(ref id, M1Color.Green, 3, 3); Add(ref id, M1Color.Green, 4, 3); Add(ref id, M1Color.Green, 5, 3);
        Add(ref id, M1Color.Yellow, 1, 2); Add(ref id, M1Color.Green, 2, 2, true); Add(ref id, M1Color.Yellow, 4, 2);
        Add(ref id, M1Color.Yellow, 2, 1); Add(ref id, M1Color.Red, 5, 1); Add(ref id, M1Color.Red, 0, 0);
    }

    private void Add(ref int id, M1Color color, int column, int row, bool hostStar = false)
    {
        var fragment = new M1Fragment { Id = id++, Color = color, Kind = M1SolidKind.Original, Column = column, Row = row, VisualSeed = id * 7 };
        Solids.Add(fragment);
        if (hostStar) Stars.Add(new M1Star { Id = Stars.Count + 1, HostFragmentId = fragment.Id, Column = column, Row = row, State = M1StarState.Contained });
    }

    private struct Cell
    {
        public readonly int column;
        public readonly int row;
        public Cell(int column, int row) { this.column = column; this.row = row; }
    }
}
