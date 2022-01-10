using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>This class attaches to some Structure that can connect to another Structure. </summary>
public class Connector : MonoBehaviour
{
    /// <summary> The Connector the player is dragging; <c>null</c> if they aren't dragging one.</summary>
    public static Connector ConnectorDragging;

    /// <summary>The Structure component on this Connector's GameObject. Can NEVER be <c>null</c>.</summary>
    [SerializeField]
    private Structure structure;

    /// <summary>The IConnectable associated with to this Connector's <c>structure</c>.</summary>
    private IConnectable connectable;

    /// <summary>The maximum number of lines that this Connector can use to connect to other objects.</summary>
    [SerializeField]
    private int maxConnectors;

    /// <summary>This Connector's LineRenderer component.</summary>
    [SerializeField]
    private LineRenderer lineRenderer;

    /// <summary> This Connector's VERTEX NUMBERS (not positions!) and the Structures they are connected to.</summary>
    private Dictionary<Vector2Int, Structure> connections = new Dictionary<Vector2Int, Structure>();

    /// <summary> The start and end vertices of the line the player is currently dragging; (-1, -1) if they aren't. </summary>
    private Vector2Int verticesDragging = new Vector2Int(-1, -1);

    public void OnClick(Tile t, Structure s)
    {
        if (IsDraggingLine()) //Already dragging and clicks on this Connector again
        {
            DropLine();
            return;
        }

        Vector2Int connectionVertices = GetExistingConnection(s);
        if (connections.Count < maxConnectors) CreateLine();
        else if (AreValidVertices(connectionVertices)) StartDragLine(connectionVertices);
        
    }
    
    /// <summary>Creates a pair of vertices and adds it to <c>lineRenderes</c> with its value being this Structure.
    /// <br></br><em>Precondition:</em> the number of existing connections is less than <c>maxConnectors</c>. </summary>
    private void CreateLine()
    {
        Assert.IsTrue(connections.Count < maxConnectors, "You cannot create another line because this Connector has reached its capacity.");
        lineRenderer.positionCount += 2;
        int startVertex = connections.Count * 2;
        int endVertex = startVertex + 1;
        Vector2Int vertexVector = new Vector2Int(startVertex, endVertex);
        AddConnection(vertexVector, structure); //When we first create a line, add this Structure as a temporary occupant.
        StartDragLine(vertexVector);
    }

    /// <summary><strong>Returns:</strong> true if <c>vertices</c> contains positive X and Y values.</summary>
    private bool AreValidVertices(Vector2Int vertices)
    {
        if ((vertices.x >= 0 && vertices.y >= 0) && vertices.y - vertices.x == 1) return true;
        return false;
    }

    /// <summary>Drops the line the player is dragging.
    /// <br></br><em>Precondition:</em> the player is dragging a line.</summary>
    public void DropLine()
    {
        Assert.IsTrue(IsDraggingLine(), "You must be dragging a line to call DropLine().");
        Vector2Int dropVertex = verticesDragging;
        RemoveVerticesDragging();
        lineRenderer.SetPosition(dropVertex.x, GameObjectPosition());
        lineRenderer.SetPosition(dropVertex.y, GameObjectPosition());
    }

    /// <summary>Starts dragging a line with vertex positions <c>positions</c>. 
    /// <br></br><em>Precondition:</em> <c>vertices</c> has positive X and Y values, with its Y value one greater than its X value.
    /// <br></br><em>Precondition:</em> the player is not dragging any other line.</summary>
    private void StartDragLine(Vector2Int vertices)
    {
        Assert.IsNull(ConnectorDragging, "You are already dragging something because ConnectorDragging is not null.");
        Assert.IsTrue(AreValidVertices(vertices), "Paramter vertices with X: " + vertices.x + " and Y: " + vertices.y + " is not valid.");
        Assert.IsFalse(IsDraggingLine(), "You are already dragging something because the current vertices are > 0.");
        SetVerticesDragging(vertices);
    }

    /// <summary>Drags the line between <c>vertices</c>' X and Y values.
    /// <br></br><em>Precondition:</em> <c>vertices</c> has positive X and Y values, with its Y value one greater than its X value.</summary>
    private void DragLine(Vector2Int vertices)
    {
        Assert.IsTrue(AreValidVertices(vertices), "Paramter vertices with X: " + vertices.x + " and Y: " + vertices.y + " is not valid.");
        Assert.IsTrue(IsDraggingLine());
        lineRenderer.SetPosition(vertices.x, GameObjectPosition());
        lineRenderer.SetPosition(vertices.y, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    /// <summary>Attaches the line this Connector is dragging to Structure s.
    /// <br></br><em>Precondition: </em><c>s</c> is not <c>null</c>.
    /// <br></br><em>Precondition: <c>s</c> has an existing connection from itself to itself.
    /// <br></br><em>Precondition:</em> the player is dragging a line.</summary>
    public void AttachLine(Structure s)
    {
        Assert.IsNotNull(s, "Parameter s cannot be null.");
        Assert.IsTrue(IsDraggingLine(), "You cannot call AttachLine() because you are not dragging a line.");
        Assert.IsTrue(connections.ContainsKey(verticesDragging), "This pair of vertices is not in connections.");
        s.AddAttachedConnector(this);
        UpdateConnection(verticesDragging, s);
        s.OnConnect(this);
        lineRenderer.SetPosition(verticesDragging.x, GameObjectPosition());
        lineRenderer.SetPosition(verticesDragging.y, s.StructPos());
        RemoveVerticesDragging();
    }

    /// <summary>Picks up and starts dragging a line already connected to <c>s</c> from <c>s</c>.
    /// <br></br><em>Precondition: </em><c>s</c> is not <c>null</c>.
    /// <br></br><em>Precondition: </em><c>this Connector has a line attached to <c>s</c>.</c>.
    /// <br></br><em>Precondition: </em>The player is not dragging a line. </summary>
    public void PickupLine(Structure s)
    {
        Assert.IsNotNull(s, "Parameter s cannot be null.");
        Assert.IsTrue(AreValidVertices(GetExistingConnection(s)), "There is no connection from this Connector to Structure s.");
        Assert.IsFalse(IsDraggingLine(), "You cannot call PickupLine() because the player is already dragging a line.");
        Vector2Int vertices = GetExistingConnection(s);
        UpdateConnection(vertices, structure);
        s.OnPickup(this);
        StartDragLine(vertices);
    }

    /// <summary>Adds a connection to this Connector's Dictionary of connections.
    /// <br></br><em>Precondition:</em> <c>vertices</c> has positive X and Y values, with its Y value one greater than its X value.
    /// <br></br><em>Precondition:</em> <c>vertices</c> does not already exist in <c>connections.</c>
    /// <br></br><em>Precondition:</em> <c>s</c> is not <c>null</c>.</summary>
    private void AddConnection(Vector2Int vertices, Structure s)
    {
        Assert.IsTrue(AreValidVertices(vertices), "Paramter vertices with X: " + vertices.x + " and Y: " + vertices.y + " is not valid.");
        Assert.IsNotNull(s, "Parameter s cannot be null.");
        Assert.IsFalse(connections.ContainsKey(vertices), "This Connector already has this pair of vertices.");
        connections.Add(vertices, s);
    }

    /// <summary>Updates the value of key <c>vertices</c> in <c>connections</c> to parameter <c>s</c>.
    /// <br></br><em>Precondition:</em> <c>vertices</c> has positive X and Y values, with its Y value one greater than its X value.
    /// <br></br><em>Precondition:</em> <c>vertices</c> exists in <c>connections.</c>
    /// <br></br><em>Precondition:</em> <c>s</c> is not <c>null</c>.</summary>

    private void UpdateConnection(Vector2Int vertices, Structure s)
    {
        Assert.IsTrue(AreValidVertices(vertices), "Paramter vertices with X: " + vertices.x + " and Y: " + vertices.y + " is not valid.");
        Assert.IsNotNull(s, "Parameter s cannot be null.");
        Assert.IsTrue(connections.ContainsKey(vertices), "This Connector must contain this pair of vertices.");
        connections[vertices] = s;
    }

    /// <summary>Removes a connection from this Connector's Dictionary of connections.
    /// <br></br><em>Precondition:</em> <c>vertices</c> exists in <c>connections</c>.</summary>
    private void RemoveConnection(Vector2Int vertices)
    {
        Assert.IsTrue(connections.ContainsKey(vertices), "This Connector does not have this pair of vertices as a connection.");
        connections.Remove(vertices);
    }

    /// <summary><strong>Returns:</strong> The position of the GameObject this Connector component is attached to. </summary>
    private Vector3 GameObjectPosition()
    {
        return gameObject.transform.position;
    }

    /// <summary><strong>Returns:</strong> the PowerSource component attached to the same GameObject as this Connector; if no
    /// <br></br>such PowerSource component exists, returns <c>null</c>.</summary>
    public PowerStructure GetPossiblePowerSource()
    {
        if ((structure as PowerStructure) == null) return null;
        return structure as PowerStructure;
    }

    /// <summary><strong>Returns:</strong> a possibly empty list of Structures that this Connector connects to.</summary>
    public List<Structure> ConnectedStructures()
    {
        List<Structure> conStructs = new List<Structure>();
        foreach(KeyValuePair<Vector2Int, Structure> pair in connections)
        {
            conStructs.Add(pair.Value);
        }
        return conStructs;
    }


    /// <summary><strong>Returns:</strong> true if the player is dragging one of this Connector's LineRenderers.</summary>
    public bool IsDraggingLine()
    {
        return AreValidVertices(verticesDragging) && ConnectorDragging != null;
    }

    /// <summary>Sets the current vertices for the line that this player is dragging. Also sets <c>ConnectorDragging</c>
    /// to this Connector.
    /// <br></br><em>Precondition:</em> <c>vertices</c> has positive X and Y values, with its Y value one greater than its X value.</summary>
    private void SetVerticesDragging(Vector2Int vertices)
    {
        Assert.IsTrue(AreValidVertices(vertices), "Parameter vertices must have positive X and Y values.");
        Assert.IsNull(ConnectorDragging, "You cannot call SetVerticesDragging() because ConnectorDragging is not null.");
        ConnectorDragging = this;
        verticesDragging = vertices;
    }

    /// <summary>Sets the current vertices dragging to (-1, -1).
    /// <br></br><em>Precondition:</em> <c>verticesDragging</c> has positive X and Y values, with its Y value one greater than its X value.</summary>
    private void RemoveVerticesDragging()
    {
        Assert.IsTrue(AreValidVertices(verticesDragging), "You cannot call RemoveVerticesDragging() because the vertices are already negative.");
        Assert.IsNotNull(ConnectorDragging, "You cannot call RemoveVerticesDragging() because ConnectorDragging is null.");
        ConnectorDragging = null;
        verticesDragging = new Vector2Int(-1, -1);
    }

    /// <summary><strong>Returns:</strong> the Vector2Int vertex numbers of the line from this Connector to Structure <c>s</c>.
    /// <br></br> If no connection exists, returns (-1, -1).
    /// <br></br><em>Precondition:</em> <c>s</c> is not <c>structure</c>.</summary>
    private Vector2Int GetExistingConnection(Structure s)
    {
        foreach(KeyValuePair<Vector2Int, Structure> pair in connections)
        {
            if (pair.Value.ID() == s.ID()) return pair.Key;
        }
        return new Vector2Int(-1, -1);
    }


    private void Update()
    {
        if (IsDraggingLine()) DragLine(verticesDragging);
    }

    
    /// <summary>Sets this Connector's <c>connectable</c>.</summary>
    private void SetConnectable()
    {
        IConnectable c = structure.Placeable() as IConnectable;
        Assert.IsNotNull(c, "There is no IConnectable associated with this Structure; therefore, Connectors are not allowed.");
        connectable = c;
    }

    private void Start()
    {
        Assert.IsNotNull(lineRenderer, "You cannot add a Connector component to a GameObject without a LineRenderer.");
        Assert.IsNotNull(structure, "You cannot add a Connector component to a GameObject without a Structure.");
        SetConnectable();
    }

}
