using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    int level = 0;
    string menuHint = "Type 'q' to exit.";
    string[] l1password = { "cow", "frog", "fish" };
    string[] l2password = { "cow", "frog", "fish" };
    string[] l3password = { "cow", "frog", "fish" };

    string password; 

    enum Scene {StartMenu, Password, Win};
    Scene currentScene;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    void OnUserInput(string input)
    {
        if (input.ToUpper() == "Q")
        {
            ShowMainMenu();
        }
        else if (currentScene == Scene.StartMenu)
        {
            RunMainMenu(input);
        }
        else if (currentScene == Scene.Password)
        {
            CheckPassword(input);
        }
    }

    void ShowMainMenu()
    {
        currentScene = Scene.StartMenu;
        Terminal.ClearScreen();
        Terminal.WriteLine("========= PEGASUS INDUSTRIES =========");
        Terminal.WriteLine("=======------------------------=======");
        Terminal.WriteLine("");
        Terminal.WriteLine("Welcome to terminal n° 0x354a-AFD");
        Terminal.WriteLine("Please, write the number:");
        Terminal.WriteLine("");
        Terminal.WriteLine("1 - Experimental Animals class-1");
        Terminal.WriteLine("2 - Experimental Animals class-2");
        Terminal.WriteLine("3 - Experimental Animals class-3");
        Terminal.WriteLine(menuHint);
    }

    void RunMainMenu(string input)
    {

        bool isValidLevelInput = (input == "1" || input == "2" || input == "3");
        if (isValidLevelInput)
        {
            level = int.Parse(input);
            AskForPassword();
        }
        else
        {
            Terminal.WriteLine("Please choose a valid option.");
            Terminal.WriteLine(menuHint);
        }
    }

    private void AskForPassword()
    {
        currentScene = Scene.Password;
        Terminal.ClearScreen();
        Terminal.WriteLine("You have chosen the option " + level);

        SetRandomPassword();

        Terminal.WriteLine("Please provide your password:");
        Terminal.WriteLine("Hint (animal): " + password.Anagram());

    }

    private void SetRandomPassword()
    {
        switch (level)
        {
            case 1:
                password = l1password[UnityEngine.Random.Range(0, l1password.Length)];
                break;
            case 2:
                password = l2password[UnityEngine.Random.Range(0, l2password.Length)];
                break;
            case 3:
                password = l3password[UnityEngine.Random.Range(0, l3password.Length)];
                break;
            default:
                Debug.LogError("No password - Invalid level");
                break;
        }
    }

    private void CheckPassword(string input)
    {
        if (input == password)
        {
            DisplayWinScreen();
        }
        else
        {
            AskForPassword();
        }
    }

    private void DisplayWinScreen()
    {
        currentScene = Scene.Win;
        Terminal.ClearScreen();
        ShowReward();
    }

    private void ShowReward()
    {
        Terminal.WriteLine("Authorization verified.");
        switch (level)
        {
            case 1:
                Terminal.WriteLine("Option 1 completed. Here is a monkey.");
                Terminal.WriteLine(@"
  /~\
 C oo
 _( ^)
/   ~\
                ");
                Terminal.WriteLine(menuHint);

                break;
            case 2:
                Terminal.WriteLine("Option 2 completed. Here is a fish.");
                Terminal.WriteLine(@"
|\   \\\\__     o
| \_/    o \    o 
> _   (( <_  oo  
| / \__+___/      
|/     |/
                ");
                Terminal.WriteLine(menuHint);

                break;
            case 3:
                Terminal.WriteLine("Option 3 completed. Here is a cow.");
                Terminal.WriteLine(@"
\|/          (__)    
     `\------(oo)
       ||    (__)
       ||w--||     \|/
   \|/
                ");
                Terminal.WriteLine(menuHint);

                break;
            default:
                Debug.LogError("Invalid level");
                break;
        }
    }
}
