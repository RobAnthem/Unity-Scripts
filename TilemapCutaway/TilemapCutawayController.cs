using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCutawayController : MonoBehaviour
{
    [SerializeField] private TilemapRenderer tilemapRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float cutawayRadius = 1.25f;
    [SerializeField] private float fadeSmoothness = 1.0f;
    private Material _material;
	private void Awake()
	{       
        if (tilemapRenderer == null)
            tilemapRenderer = GetComponent<TilemapRenderer>();
        if (tilemapRenderer)
            _material = tilemapRenderer.material;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            _material = spriteRenderer.material;
        _material.SetFloat("_CutawayRadius", cutawayRadius);
        _material.SetFloat("_FadeSmoothness", fadeSmoothness);
    }
    /// <summary>
    /// Applies a world-space cutaway effect to the tilemap shader.
    /// </summary>
    /// <param name="worldPosition">The world position to center the cutaway.</param>
    public void ApplyCutaway(Vector2 worldPosition)
    {
        if (_material == null)
            return;
        //Material mat = tilemapRenderer.material;
        _material.SetVector("_CutawayCenter", new Vector4(worldPosition.x, worldPosition.y, 0f, 0f));
    }
    void Update()
    {
        if (!OptionsControl.Cutaway)
            return;
        Vector2 worldPos = PlayerController.Instance.transform.position;
        worldPos.y += 1.5f;
        ApplyCutaway(worldPos);
    }

}
