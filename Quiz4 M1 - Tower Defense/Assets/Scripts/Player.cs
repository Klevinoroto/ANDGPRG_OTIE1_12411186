using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public GameObject playerUI;
    [SerializeField] private LayerMask walkableLayer;

    void Start()
    {
        foreach (Transform child in playerUI.transform)
        {
            if (child.transform.name.Contains("Tower"))
            {

                void OnButtonClicked()
                {
                    Debug.Log(child.transform.name);
                }

                Button button = child.gameObject.GetComponent<Button>();
                button.onClick.AddListener(OnButtonClicked);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //// 1. Get Mouse Screen Position
        //Vector3 mouseScreenPos = Input.mousePosition;
        //// Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        //// 2. Create a ray from the camera through the mouse point
        //Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        //// 3. Perform a Raycast to see what the mouse is pointing at
        //if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, walkableLayer))
        //{
        //    // Move this object to the impact point
        //    transform.position = hitInfo.point;
        //}
    }
}
