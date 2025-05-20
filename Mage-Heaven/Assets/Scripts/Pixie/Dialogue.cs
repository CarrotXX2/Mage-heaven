using System.Collections;
using UnityEngine;
using TMPro;
public class Dialogue : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private string messageToType;
    [SerializeField] private float typingSpeed;
    [SerializeField] private float wobbleIntensity;
    [SerializeField] private float wobbleSpeed;
    [SerializeField] private float displayDuration;

    [Header("Trigger Settings")]
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private bool testTrigger = false;
    
    private bool hasTriggered = false;
    private int currentTypedCharacters = 0;

    void Update()
    {
        if (testTrigger && !hasTriggered)
        {
            hasTriggered = true;
            testTrigger = false;
            StartCoroutine(TypeText());
        }

        if (hasTriggered)
        {
            AnimateWobble(currentTypedCharacters - 1);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && triggerCollider != null && other == triggerCollider)
        {
            hasTriggered = true;
            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        textField.text = ""; 
        currentTypedCharacters = 0;
        
        yield return null; 
        
        for (int i = 0; i < messageToType.Length; i++)
        {
            textField.text += messageToType[i];
            currentTypedCharacters++;
            
            float timer = 0f;
            while (timer < typingSpeed)
            {
                AnimateWobble(currentTypedCharacters - 1);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        yield return new WaitForSeconds(displayDuration);

        // Reset de tekst na de weergavetijd
        textField.text = "";
        textField.ForceMeshUpdate(); // Important: forces mesh to update and clears ghost text
        currentTypedCharacters = 0;
    }

    void AnimateWobble(int maxCharIndex = int.MaxValue)
    {
        textField.ForceMeshUpdate();
        TMP_TextInfo textInfo = textField.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible || i > maxCharIndex)
                continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            Vector3 offset = WobbleEffect(i);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textField.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    Vector2 WobbleEffect(int index)
    {
        float wobble = Mathf.Sin(Time.time * wobbleSpeed + index) * wobbleIntensity;
        return new Vector2(0, wobble);
    }
}
