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
        SceneManager.LoadScene(2);
    }

    // Player click choose Elf map
    public void LoadElfMap()
    {
        SceneManager.LoadScene(2);
    }

    // Player click choose bear map
    public void LoadAngelMap()
    {
        SceneManager.LoadScene(4);
    }

    // Player click choose dwarf map
    public void LoadDwarfMap()
    {
        SceneManager.LoadScene(3);
    }

    // Player click choose goblin map
    public void LoadGoblinMap()
    {
        SceneManager.LoadScene(2);
    }

    // Player click choose human map


}
