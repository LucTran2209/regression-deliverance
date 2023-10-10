using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunction : MonoBehaviour
{
    // Player click button new play
    public void NewPlay()
    {
        SceneManager.LoadScene(1);
    }

    //  Player click button Quit
    public void Quit()
    {

    }

    // Player click choose wolf map
    public void LoadWolfMap()
    {
        SceneManager.LoadScene(2);
    }
    
    // Player click choose dragon map
    public void LoadDragonMap()
    {
        SceneManager.LoadScene(3);
    }

    // Player click choose Elf map
    public void LoadElfMap()
    {
        SceneManager.LoadScene(4);
    }

    // Player click choose Bear map
    public void LoadBearMap()
    {
        SceneManager.LoadScene(5);
    }

    // Player click choose dwarf map
    public void LoadDwarfMap()
    {
        SceneManager.LoadScene(6);
    }

    // Player click choose goblin map
    public void LoadGoblinMap()
    {
        SceneManager.LoadScene(7);
    }

}
