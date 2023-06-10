using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class LengthyLockdownScripts : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMBombModule Module;

   public KMSelectable[] Buttons;
   public TextMesh[] DisplayTexts;

   public Color solveColor = new Color(63f / 255f, 255f / 255f, 15f / 255f);
   public Color logColor = new Color(253f / 255f, 235f / 255f, 1f / 255f);

   int currentStage = 0;
   string[] nums = {"12", "5", "14", "7", "20", "8"}; //up to 7 cases
   string queryEntireLog = "125147208"; //all cases of the positive results
   string currentLog = ""; //what your current log is

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */


      //accept
      Buttons[0].OnInteract += delegate () { PressAccept(); return false; };
      //refuse
      Buttons[1].OnInteract += delegate () { PressRefuse(); return false; };
      //query
      Buttons[2].OnInteract += delegate () { PressQuery(); return false; };

      DisplayTexts[4].text = "";
      DisplayTexts[4].color = logColor;

      Buttons[0].AddInteractionPunch();
      Buttons[1].AddInteractionPunch();
      Buttons[2].AddInteractionPunch();
   }

   void PressQuery(){
     Audio.PlaySoundAtTransform("Query", Buttons[1].transform);
     Debug.Log("Query entered.");
     switch (currentStage)
        {
        case 9:
        if (currentLog.Equals("125147208")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "8";
        }
        DisplayTexts[4].text = currentLog;
        Debug.Log(currentLog);
        StartCoroutine("ActualQueryWait");
        break;
        case 8:
        Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
        Module.HandleStrike();
        break;
        case 7:
        if (currentLog.Equals("12514720")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "20";
        }
        Debug.Log(currentLog);
        DisplayTexts[4].text = currentLog;
            break;
        case 6:
        Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
        Module.HandleStrike();
          break;
        case 5:
        if (currentLog.Equals("125147")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "7";
        }
        Debug.Log(currentLog);
        DisplayTexts[4].text = currentLog;
            break;
        case 4:
        if (currentLog.Equals("12514")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "14";
        }
        Debug.Log(currentLog);
        DisplayTexts[4].text = currentLog;
            break;
        case 3:
        if (currentLog.Equals("125")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "5";
        }
        Debug.Log(currentLog);
        DisplayTexts[4].text = currentLog;
          break;
      case 2:
      Audio.PlaySoundAtTransform("Strike", Buttons[2].transform);
      Module.HandleStrike();
            break;
        case 1:
        if (currentLog.Equals("12")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "12";
        }
        Debug.Log(currentLog);
        DisplayTexts[4].text = currentLog;
            break;
        default:
        if (currentLog.Equals("")) {
          Module.HandleStrike();
        }
        else{
        }
            break;
        }
}

   void PressAccept(){
     Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     Debug.Log("Accept registered.");
     Debug.Log("Current stage is: " + currentStage);
     switch (currentStage)
        {
        case 8:
        currentStage = currentStage + 1;
        DisplayTexts[0].text = "FINAL LOG";
        break;
        case 7:
        //if number is 11 and accepted (which is wrong)
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 6:
        currentStage = currentStage + 1;
        int randomNumberThird = UnityEngine.Random.Range(26, 40);
        string randomStringThird = randomNumberThird.ToString();
        DisplayTexts[0].text = randomStringThird;
          break;
        case 5:
        //if number is random and accepted (which is wrong)
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 4:
          currentStage = currentStage + 1;
          int randomNumberSecond = UnityEngine.Random.Range(26, 40);
          string randomStringSecond = randomNumberSecond.ToString();
          DisplayTexts[0].text = randomStringSecond;
            break;
        case 3:
          currentStage = currentStage + 1;
          DisplayTexts[0].text = nums[3];
          break;
      case 2:
            currentStage = currentStage + 1;
            DisplayTexts[0].text = nums[2];
            break;
        case 1:
        //if number is not 5 and accepted (which is wrong)
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        default:
            currentStage = currentStage + 1;
            int randomNumber = UnityEngine.Random.Range(26, 40);
            string randomString = randomNumber.ToString();
            DisplayTexts[0].text = randomString;
            break;
        }
    }

   void PressRefuse(){
     Audio.PlaySoundAtTransform("ButtonPress", Buttons[1].transform);
     Debug.Log("Refuse registered.");
     Debug.Log("Current stage is: " + currentStage);
     switch (currentStage)
        {
          case 9:
          //if number 25 and denied
              Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
              Module.HandleStrike();
              break;
        case 8:
        //if number is 11 and rejected which is correct
            currentStage = currentStage + 1;
            DisplayTexts[0].text = nums[6];
            break;
        case 7:
        //if number is 11 and rejected which is correct
            currentStage = currentStage + 1;
            DisplayTexts[0].text = nums[5];
            break;
        case 6:
        //if number 20 and denied
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 5:
        //if number is not 5 and rejected which is correct
            currentStage = currentStage + 1;
            DisplayTexts[0].text = nums[4];
            break;
        case 4:
        //if number 7 and denied
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 3:
        //if number 14 and denied
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 2:
            //if number is now 5 and rejected which is incorrect
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 1:
        //if number is 2 and rejected which is correct
            currentStage = currentStage + 1;
            DisplayTexts[0].text = nums[1];
            break;
        default:
            //if number 12 and denied
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        }
   }

   void OnDestroy () { //Shit you need to do when the bomb ends

   }


   private IEnumerator Wait1Sec () {
     yield return new WaitForSeconds(2f);
   }

   //for fixes
   private IEnumerator ActualQueryWait () {
    Debug.Log("Received final log...");
    DisplayTexts[0].text = "COMPARING...";
     Audio.PlaySoundAtTransform("QueryWait", Buttons[2].transform);
     yield return new WaitForSeconds(7f);
     OnQueryAwait();
   }

   void OnQueryAwait () { //Shit you need to do when you do the thing
     if (currentLog.Equals(queryEntireLog)) {
       Audio.PlaySoundAtTransform("Solve", Buttons[1].transform);
       DisplayTexts[0].text = "COMPLETE";
       DisplayTexts[0].color = solveColor;
       DisplayTexts[4].text = "";
       Solve();
     }
     else {
       currentLog = "";
       DisplayTexts[4].text = currentLog;
       currentStage = 0;
       Module.HandleStrike();
       DisplayTexts[0].color = Color.red;
       StartCoroutine("Start");
       //6 strikes for extra measure...
     }
   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   private IEnumerator Start () { //Shit
     DisplayTexts[0].text = "FETCHING...";
     yield return new WaitForSeconds(2f);
     DisplayTexts[0].color = Color.white;
     DisplayTexts[0].text = nums[0];
   }

   void Update () { //Shit that happens at any point after initialization

   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}

//This is very disgusting code. I am not very good at C#, and this makes it very evident
//Pull up in the Tonka truck!!!
//Thanks to Jah
