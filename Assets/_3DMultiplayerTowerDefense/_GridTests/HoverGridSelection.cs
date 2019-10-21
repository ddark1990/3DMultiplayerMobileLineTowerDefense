using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class HoverGridSelection : MonoBehaviour
{
    #region Variables
    public Vector3 MousePositionOutput;

    [SerializeField]
    private string gridShaderName = "TowerCraft/TestShaders/SelectableGrid";
    [SerializeField]
    private string gridSizePropertyName = "_GridSize";
    [SerializeField]
    private string gridSizeXPropertyName = "_GridSizeX";
    [SerializeField]
    private string gridSizeZPropertyName = "_GridSizeZ";
    [SerializeField]
    private string gridSelectionEnablePropertyName = "_SelectCell";
    [SerializeField]
    private string gridSelectXPropertyName = "_SelectedCellX";
    [SerializeField]
    private string gridSelectYPropertyName = "_SelectedCellY";

    public int gridSizeProperyID = 0;
    public int gridSizeXProperyID = 0;
    public int gridSizeZProperyID = 0;
    public int gridSelectionEnableID = 0;
    public int gridSelectXID = 0;
    public int gridSelectYID = 0;

    private Camera cam = null;
    private Material mat = null;
    private RaycastHit hit;
    public float selectedX = 0;
    public float selectedY = 0;

    #endregion

    #region MonoBehavours
    private void Start() {

        InitalizeCameraReference();
        InitializeMaterialReference();
        InitializePropertyIDs();
        if (enabled) //if all previous methods initialized without issue
            EnableGridSelection(false);
    }

    private void OnMouseEnter() {
        EnableGridSelection(true);
    }
    private void OnMouseOver() {
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        UVToGrid(hit.textureCoord, out selectedX, out selectedY);
        Debug.Log(hit.textureCoord + " | " + hit.point);
        SetGridSelection(selectedX, selectedY);

        if (Input.GetKeyDown(KeyCode.Mouse0)) //how I would find the center of grid point with hard coded values and no scaling
        {
            var objToSpawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var vector = new Vector3(Mathf.FloorToInt(hit.point.x) + 0.5f, 0, Mathf.FloorToInt(hit.point.z) + 0.5f);
            Debug.Log(vector);
            objToSpawn.transform.position = vector;
        }
    }
    private void OnMouseExit() {
        EnableGridSelection(false);
    }
    #endregion

    #region Methods
    private void InitalizeCameraReference() {
        cam = Camera.main;
        if (cam == null) {
            Debug.LogError("No main camera assigned. Disabling MonoBehaviour.");
            enabled = false;
        }
    }
    private void InitializeMaterialReference() {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) {
            Debug.LogError("No renderer. Disabling MonoBehaviour.");
            enabled = false;
        }
        else {
            mat = rend.material;
            Shader gridShader = Shader.Find(gridShaderName);
            if (mat.shader != gridShader) {
                Debug.LogError("No grid shader. Disabling MonoBehaviour.");
                enabled = false;
            }
        }
    }
    private void InitializePropertyIDs() {
        gridSizeProperyID = Shader.PropertyToID(gridSizePropertyName);
        gridSizeXProperyID = Shader.PropertyToID(gridSizeXPropertyName);
        gridSizeZProperyID = Shader.PropertyToID(gridSizeZPropertyName);
        gridSelectionEnableID = Shader.PropertyToID(gridSelectionEnablePropertyName);
        gridSelectXID = Shader.PropertyToID(gridSelectXPropertyName);
        gridSelectYID = Shader.PropertyToID(gridSelectYPropertyName);
    }

    private void UVToGrid(Vector2 uv, out float x, out float y) {
        float gridSize = GetGridSize();
        x = Mathf.Floor(uv.x / (1.0f / gridSize));
        y = Mathf.Floor(uv.y / (1.0f / gridSize));
    }
    private float GetGridSize() {
        return mat.GetFloat(gridSizeProperyID);
    }
    private void SetGridSelection(float x, float y) {
        mat.SetFloat(gridSelectXID, x);
        mat.SetFloat(gridSelectYID, y);
    }
    private void EnableGridSelection(bool value) {
        if (value)
            mat.SetFloat(gridSelectionEnableID, 1);
        else
            mat.SetFloat(gridSelectionEnableID, 0);
    }
    #endregion
}
