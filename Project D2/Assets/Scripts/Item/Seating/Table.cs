using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>This class represents a Table, with a set number of seats, that a Customer might sit in. </summary>
public abstract class Table : Seating
{
    /// <summary>The number of seats at this Table. </summary>
    private int numSeats;

    /// <summary><strong>Constructor: </strong>A Table with name <c>name</c> and number of seats <c>seats</c>.</summary>
    public Table(string name, int seats) : base(name){
        Assert.IsTrue(seats > 0, "Number of seats must be > 0.");
        numSeats = seats;
    }
}
