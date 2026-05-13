using UnityEngine;

public class TriggerBrushMesh : MonoBehaviour
{
    // THAT THIS ENTIRE CODE WAS SOURCE FROM CLAUDE AI SONNET 4.6
    // PROMPT: in unity how would you give a box collider a texture that acts like a developer trigger where it can be seen in the editor but cannot be seen in-game?

    [SerializeField] private Color gizmoColor = new Color(0f, 1f, 0f, 0.3f); // semi-transparent green
    [SerializeField] private Color wireColor = new Color(0f, 1f, 0f, 0.8f);

    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(
            transform.TransformPoint(box.center),
            transform.rotation,
            transform.lossyScale
        );

        // Filled semi-transparent cube
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(Vector3.zero, box.size);

        // Solid wireframe outline
        Gizmos.color = wireColor;
        Gizmos.DrawWireCube(Vector3.zero, box.size);

        Gizmos.matrix = oldMatrix;
    }
}
