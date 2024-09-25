using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{


    PlayerInputActions playerInputActions;
    private InputAction nav;
    private InputAction select;

    public GameObject firstSelectdObject;

    private EventSystem eventSystem;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        nav = playerInputActions.UI.Navigate;
        nav.Enable();

        select = playerInputActions.UI.Submit;
        select.Enable();


        playerInputActions.UI.Submit.performed += OnSubmit;
        playerInputActions.UI.Navigate.performed += OnNavigate;

    }

    private void OnDisable()
    {
        nav.Disable();
        select.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {

        
        //this.gameObject.SetActive(false);
    }




    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame() 
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 

    public void ExitGame() 
    {


         
        SceneManager.LoadScene(2);
    
    
    }

    public void Settings() 
    {
       //Load settings screen stuff
    
    }

    public void HowTo() 
    {
       //could be another scene
    
    }


    private GameObject GetNextSelectable(GameObject current)
    {
        // Implement logic to find the next selectable UI element
        // This can be based on the hierarchy or a predefined list of menu items
        return null; // Placeholder
    }

    private GameObject GetPreviousSelectable(GameObject current)
    {
        // Implement logic to find the previous selectable UI element
        return null; // Placeholder
    }


    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            // Use input to navigate through menu items
            // Assume vertical navigation for simplicity
            if (input.y > 0)
            {
                // Move up in the menu
                eventSystem.SetSelectedGameObject(GetPreviousSelectable(eventSystem.currentSelectedGameObject));
            }
            else if (input.y < 0)
            {
                // Move down in the menu
                eventSystem.SetSelectedGameObject(GetNextSelectable(eventSystem.currentSelectedGameObject));
            }
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            StartGame();
            // Execute the action of the currently selected menu item
            // Example: if the selected item is a Button, call its onClick
        }
    }

}
