using UnityEngine;
using System.Collections;

public class AutoTransparent : MonoBehaviour
{
    private Shader m_OldShader = null;
    private Color m_OldColor = Color.black;
    private float m_Transparency = 0.3f;
    private const float m_TargetTransparancy = 0.3f;
    private const float m_FallOff = 1f; // returns to 100% in 0.1 sec
    private Renderer objectRenderer;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    public void BeTransparent()
    {
        // reset the transparency;
        m_Transparency = m_TargetTransparancy;
        if (m_OldShader == null)
        {
            // Save the current shader
            m_OldShader = objectRenderer.material.shader;
            m_OldColor = objectRenderer.material.color;
            objectRenderer.material.shader = Shader.Find("Transparent/Diffuse");
        }
    }
    void Update()
    {
        if (m_Transparency < 1.0f)
        {
            Color C = objectRenderer.material.color;
            C.a = m_Transparency;
            objectRenderer.material.color = C;
        }
        else
        {
            // Reset the shader
            objectRenderer.material.shader = m_OldShader;
            objectRenderer.material.color = m_OldColor;
            // And remove this script
            Destroy(this);
        }
        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;
    }
}