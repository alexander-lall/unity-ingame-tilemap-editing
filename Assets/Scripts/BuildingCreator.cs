using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingCreator : Singleton<BuildingCreator>
{
    [SerializeField] Tilemap previewMap;
    PlayerInput playerInput;

    TileBase tileBase;
    BuildingObjectBase selectedObj;

    Camera _camera;

    Vector2 mousePos;
    Vector3Int currentGridPosition;
    Vector3Int lastGridPosition;

    protected override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();
        _camera = Camera.main;
    }

    void OnEnable() 
    {
        playerInput.Enable();

        playerInput.Gameplay.MousePosition.performed += OnMouseMove;
        playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed += OnRightClick;
    }

    void OnDisable() 
    {
        playerInput.Disable();

        playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
        playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed -= OnRightClick;

    }

    private BuildingObjectBase SelectedObj 
    {
        set 
        {
            selectedObj = value;

            tileBase = selectedObj != null ? selectedObj.TileBase : null;

            UpdatePreview();
        }
    }

    void Update()
    {
        // If something is selected - show preview
        if(selectedObj != null)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
            Vector3Int gridPos = previewMap.WorldToCell(pos);

            if(gridPos != currentGridPosition)
            {
                lastGridPosition = currentGridPosition;
                currentGridPosition = gridPos;

                UpdatePreview();
            }
        }
    } 

    void OnMouseMove(InputAction.CallbackContext ctx) 
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    void OnLeftClick(InputAction.CallbackContext ctx) {}

    void OnRightClick(InputAction.CallbackContext ctx) {}

    public void ObjectSelected(BuildingObjectBase obj) 
    {
        SelectedObj = obj;

        // Set preview where mouse is 
        // on click draw 
        // on right click cancel drawing
    }

    void UpdatePreview()
    {
        // Remove old tile if existing
        previewMap.SetTile(lastGridPosition, null);
        // Set current tile to current mouse positions tile
        previewMap.SetTile(currentGridPosition, tileBase);
    }
}
