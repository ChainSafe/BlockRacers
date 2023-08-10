using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EnablePostProcessing : MonoBehaviour
{
    #region Fields

    // Post processing
    private PostProcessVolume volume;

    #endregion

    #region Methods

    private void Start()
    {
        // Enables post processing so it doesn't error in the editor
        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.GetComponent<PostProcessVolume>().enabled = true;
    }

    #endregion
}