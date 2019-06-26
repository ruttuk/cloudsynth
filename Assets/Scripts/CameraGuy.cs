using UnityEngine;
using UnityEngine.UI;

public class CameraGuy : MonoBehaviour
{
    float cameraSpeed = 12f;
    float upperZBound = 30f;
    float lowerZBound = -10f;
    float upperXBound = 110f;
    float lowerXBound = -10f;

    Vector3 currentPos;
    Vector3 originalPos;
    Quaternion originalRotation;
    Vector3 topdownPos;
    Quaternion topdownRotation;
    bool topdownEnabled;

    [SerializeField]
    private GameObject newSongInputField;

    void Start()
    {
        originalPos = Camera.main.transform.position;
        originalRotation = Camera.main.transform.rotation;
        topdownPos = new Vector3(0, 30, 10);
        topdownRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        topdownEnabled = false;
    }

    void Update()
    {
        // ONLY accept input if the text field is not also accepting input
        if (!newSongInputField.GetComponent<InputField>().isFocused && ReceivingInput())
        {
            if(!topdownEnabled)
            {
                UpdateCameraTransform(originalPos, originalRotation);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ChangePerspective(topdownPos, topdownRotation);
                }
            }
            else
            {
                UpdateCameraTransform(topdownPos, topdownRotation);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ChangePerspective(originalPos, originalRotation);
                }
            }
        }
    }

    private void ChangePerspective(Vector3 updatedPos, Quaternion updatedRotation)
    {
        currentPos.z = updatedPos.z;
        currentPos.y = updatedPos.y;
        Camera.main.transform.SetPositionAndRotation(currentPos, updatedRotation);
        topdownEnabled = !topdownEnabled;
    }

    private void UpdateCameraTransform(Vector3 position, Quaternion rotation)
    {
        float horizontalSpeed, verticalSpeed;

        horizontalSpeed = SetAxisSpeed("Horizontal", upperXBound, lowerXBound, Camera.main.transform.position.x);
        verticalSpeed = SetAxisSpeed("Vertical", upperZBound, lowerZBound, Camera.main.transform.position.z);

        position = new Vector3(horizontalSpeed * Time.deltaTime * cameraSpeed, 0, verticalSpeed * Time.deltaTime * cameraSpeed);
        position = Quaternion.Inverse(rotation) * position;
        Camera.main.transform.Translate(position);

        currentPos = Camera.main.transform.position;
    }

    private bool ReceivingInput()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetKeyDown(KeyCode.C); 
    }

    private float SetAxisSpeed(string axis, float upperBound, float lowerBound, float cameraPos)
    {
        float inputWeight = Input.GetAxis(axis);

        if(inputWeight > 0 && cameraPos > upperBound)
        {
            return 0f;
        }
        else if (inputWeight < 0 && cameraPos < lowerBound)
        {
            return 0f;
        }
        else
        {
            return inputWeight;
        }
    }
}
