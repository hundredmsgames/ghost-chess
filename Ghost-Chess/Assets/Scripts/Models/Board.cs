﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public delegate void BoardStyleChangedHandler(BoardStyle boardStyle);
    public event BoardStyleChangedHandler BoardStyleChanged;

    public delegate void PieceSpriteChangedHandler(Tile oldTile, Tile newTile);
    public event PieceSpriteChangedHandler PieceSpriteChanged;

    // This probably should not be here
    private string[,] pieceMap = new string[,]
    {
        { "br", "bn", "bb", "bq", "bk", "bb", "bn", "br" },
        { "bp", "bp", "bp", "bp", "bp", "bp", "bp", "bp" },
        { "em", "em", "em", "em", "em", "em", "em", "em" },
        { "em", "em", "em", "em", "em", "em", "em", "em" },
        { "em", "em", "em", "em", "em", "em", "em", "em" },
        { "em", "em", "em", "em", "em", "em", "em", "em" },
        { "wp", "wp", "wp", "wp", "wp", "wp", "wp", "wp" },
        { "wr", "wn", "wb", "wq", "wk", "wb", "wn", "wr" }
    };

    private BoardStyle style;
    private int rows;
    private int cols;

    private Tile[,] tiles;
    private List<Piece> pieces;

    public Board(int rows, int cols, BoardStyle style)
    {
        this.rows = rows;
        this.cols = cols;
        this.style = style;
        pieces = new List<Piece>();

        CreateTiles();
        CreatePieces();
        if (BoardStyleChanged != null)
            BoardStyleChanged(style);
    }

    private void CreateTiles()
    {
        tiles = new Tile[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                tiles[r, c] = new Tile(r, c);
            }
        }
    }

    private void CreatePieces()
    {
        if (pieceMap.GetLength(0) != rows || pieceMap.GetLength(1) != cols)
        {
            Debug.LogError("Piece map does not match with the given width and height.");
            return;
        }

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                CreatePiece(r, c);
            }
        }
    }

    private void CreatePiece(int r, int c)
    {
        
        Piece piece;
        PieceColor color = PieceColor.White;

        if (pieceMap[rows - r - 1, c][0] == 'w')
        {
            color = PieceColor.White;
        }
        else if (pieceMap[rows - r - 1, c][0] == 'b')
        {
            color = PieceColor.Black;
        }
        else
            return;
        switch (pieceMap[rows - r - 1, c][1])
        {
            case 'r':
                piece = new Rook(color, r, c, "Rook");
                break;
            case 'n':
                piece = new Knight(color, r, c,"Knight");
                break;
            case 'b':
                piece = new Bishop(color, r, c, "Bishop");
                break;
            case 'q':
                piece = new Queen(color, r, c, "Queen");
                break;
            case 'k':
                piece = new King(color, r, c, "King");
                break;
            case 'p':
                piece = new Pawn(color, r, c, "Pawn");
                break;
            default:
                piece = null;
                break;
        }

        pieces.Add(piece);
    }

    public Tile GetTileAt(int r, int c)
    {
        if (r >= rows || r < 0 || c >= cols || c < 0)
            return null;

        return tiles[r, c];
    }

    public int Rows
    {
        get { return rows; }
        set { rows = value; }
    }

    public int Cols
    {
        get { return cols; }
        set { cols = value; }
    }

    public BoardStyle Style
    {
        get { return style; }
        set { style = value; }
    }

    public List<Piece> Pieces
    {
        get
        {
            return pieces;
        }

        protected set
        {
            pieces = value;
        }
    }
}
