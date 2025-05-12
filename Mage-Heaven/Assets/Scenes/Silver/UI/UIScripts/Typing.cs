using System.Collections;
using UnityEngine;
using TMPro;

public class Typing : MonoBehaviour
{
    [Header("Text Instellingen")]
    public TextMeshProUGUI textField;
    public string messageToType;
    public float typingSpeed = 0.05f;
    public float wobbleIntensity = 5f;
    public float wobbleSpeed = 10f;
    public float displayDuration = 3f;

    [Header("Trigger Instellingen")]
    public Collider triggerCollider;
    public bool testTrigger = false;

    [Header("Particle Volger")]
    public ParticleSystem typingParticles;
    public Transform particleFollowTransform;
    public Transform particleStartPoint;
    public Transform particleEndPoint;
    public float particleTravelSpeed = 1f;

    private bool hasTriggered = false;
    private bool typingFinished = false;
    private int currentTypedCharacters = 0;
    private Coroutine particleMoveRoutine;

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
        textField.text = ""; // Wis eerst de tekst
        typingFinished = false;
        currentTypedCharacters = 0;

        // Voeg een kleine vertraging in zodat de eerste letter niet wordt overgeslagen
        yield return null; // Voegt vertraging toe voordat de tekst begint

        // Stop eventuele vorige particle bewegingen en start een nieuwe
        if (particleMoveRoutine != null)
            StopCoroutine(particleMoveRoutine);

        particleMoveRoutine = StartCoroutine(MoveParticlesWithSpeed());

        // Begin met het typen van de tekst
        for (int i = 0; i < messageToType.Length; i++)
        {
            textField.text += messageToType[i];
            currentTypedCharacters++;

            // Timer om de snelheid van het typen te regelen
            float timer = 0f;
            while (timer < typingSpeed)
            {
                AnimateWobble(currentTypedCharacters - 1);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        typingFinished = true;
        yield return new WaitForSeconds(displayDuration);

        // Reset de tekst na de weergavetijd
        textField.text = "";
        typingFinished = false;
        currentTypedCharacters = 0;

        // Stop de particle system zodra de tekst is gewist
        if (typingParticles != null)
        {
            typingParticles.Stop();
        }
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

    IEnumerator MoveParticlesWithSpeed()
    {
        if (typingParticles == null || particleFollowTransform == null || particleStartPoint == null || particleEndPoint == null)
            yield break;

        Vector3 start = particleStartPoint.position;
        Vector3 end = particleEndPoint.position;

        typingParticles.Play();
        particleFollowTransform.position = start;

        float distance = Vector3.Distance(start, end);
        float totalTime = distance / Mathf.Max(particleTravelSpeed, 0.001f); // voorkomen van delen door 0

        float elapsed = 0f;
        while (elapsed < totalTime)
        {
            float t = elapsed / totalTime;
            particleFollowTransform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        particleFollowTransform.position = end;
    }
}
