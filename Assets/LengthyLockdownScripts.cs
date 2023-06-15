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
   public KMBombInfo BombInfo;
   public KMAudio Audio;
   public KMBombModule Module;

   public KMSelectable[] Buttons;
   public TextMesh[] DisplayTexts;

   public Color solveColor = new Color(63f / 255f, 255f / 255f, 15f / 255f);
   public Color logColor = new Color(253f / 255f, 235f / 255f, 1f / 255f);

   float realTime = 0; //realTime of the final log
   int currentStage = 0; //stage of integers
   int correctTime = 0; //bomb time
   int realTimeCalc;
   public List<int> excludedNumbers = new List<int> { 12, 5, 14, 7, 20, 8 }; //exclude these
   string[] nums = {"12", "5", "14", "7", "20", "8"}; //up to 7 cases
   string queryEntireLog = "125147208"; //all cases of the positive results
   string currentLog = ""; //what your current log is

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   int GenerateRandomNumber()
       {
           int randomNumber;

           do
           {
               randomNumber = UnityEngine.Random.Range(1, 21);
           } while (excludedNumbers.Contains(randomNumber));

           return randomNumber;
       }

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

   void PressQuery(){ //Query is the log button. For some reason, I decided to be a fucking dunce and call it the Query button before I actually made the rest of the module.
     Audio.PlaySoundAtTransform("Query", Buttons[1].transform);
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
        DisplayTexts[4].text = currentLog;
            break;
        case 4:
        if (currentLog.Equals("12514")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "14";
        }
        DisplayTexts[4].text = currentLog;
            break;
        case 3:
        if (currentLog.Equals("125")) {
          Module.HandleStrike();
        }
        else{
            currentLog = currentLog + "5";
        }
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
        Debug.LogFormat("[Lengthy Lockdown " + "#" + ModuleId + "] Log was pressed. Current log reads: " + currentLog + ".", ModuleId);
}

   void PressAccept(){
     Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
     Debug.LogFormat("[Lengthy Lockdown " + "#" + ModuleId + "] Current stage is: " + currentStage + ".", ModuleId);
     switch (currentStage)
        {
        case 8:
        currentStage = currentStage + 1;
        DisplayTexts[0].text = "8";
        break;
        case 7:
        //if number is random and accepted (which is wrong)
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 6:
        currentStage = currentStage + 1;
        string randomNumberThirdString = GenerateRandomNumber().ToString();
        DisplayTexts[0].text = randomNumberThirdString;
          break;
        case 5:
        //if number is random and accepted (which is wrong)
            Audio.PlaySoundAtTransform("Strike", Buttons[1].transform);
            Module.HandleStrike();
            break;
        case 4:
          currentStage = currentStage + 1;
          string randomNumberSecondString = GenerateRandomNumber().ToString();
          DisplayTexts[0].text = randomNumberSecondString;
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
            string randomNumberString = GenerateRandomNumber().ToString();
            DisplayTexts[0].text = randomNumberString;
            break;
        }
    }

   void PressRefuse(){
     Audio.PlaySoundAtTransform("ButtonPress", Buttons[1].transform);
     Debug.LogFormat("[Lengthy Lockdown " + "#" + ModuleId + "] Current stage is: " + currentStage + ".", ModuleId);
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

   void DetermineCorrectTime()
   {
       if (Bomb.GetSerialNumberLetters().Any(x => x.EqualsAny('A', 'E', 'I', 'O', 'U')))
           correctTime = 7;
       else if (Bomb.GetBatteryCount() == 2)
           correctTime = 2;
       else
           correctTime = 0;
   }

   //for fixes
   private IEnumerator ActualQueryWait () {
    realTime = Bomb.GetTime();
    Debug.LogFormat("[Lengthy Lockdown " + "#" + ModuleId + "] Final log was submitted at:  " + realTime.ToString() + ".", ModuleId);
    DisplayTexts[0].text = "COMPARING";
    DisplayTexts[4].text = "";
     Audio.PlaySoundAtTransform("QueryWait", Buttons[2].transform);
     yield return new WaitForSeconds(4f);
     OnQueryAwait();
   }

   void OnQueryAwait () { //Shit you need to do when you do the thing
     //Dogshit workaround
    realTimeCalc = (int) realTime % 10;
     if (currentLog.Equals(queryEntireLog) & realTimeCalc == correctTime) {
         Audio.PlaySoundAtTransform("Solve", Buttons[1].transform);
         DisplayTexts[0].text = "SUCCESS";
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
     }
   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   private IEnumerator Start () { //Shit
     DetermineCorrectTime(); //will rerun everytime a restart is performed - not that big of a deal... I think...
     Debug.LogFormat("[Lengthy Lockdown " + "#" + ModuleId + "] Correct time to submit final log is: " + correctTime.ToString() + ".", ModuleId);
     DisplayTexts[0].text = "..."; // Loading text
     yield return new WaitForSeconds(2f);
     DisplayTexts[0].color = Color.white;
     DisplayTexts[0].text = nums[0];
     DisplayTexts[4].text = "";
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
   private readonly string TwitchHelpMessage = @"Use !{0} accept to accept a number. Use !{0} refuse to refuse a number. Use !{0} log to add a number to the log. Use !{0} log at [7,2,0] to log the last number at the correct time.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
     Command = Command.ToUpper();
      yield return null;

      switch (Command) {
        case "ACCEPT":
          Buttons[0].OnInteract();
          break;
        case "REFUSE":
          Buttons[1].OnInteract();
          break;
        case "LOG":
          Buttons[2].OnInteract();
          break;

        case "LOG AT 7":
          while ((int)BombInfo.GetTime() % 10 == correctTime)
            yield return "trycancel";  // Fixes a really obscure bug with tp
          while ((int)BombInfo.GetTime() % 10 != correctTime)
            yield return "trycancel";
          Buttons[2].OnInteract();
          yield return new WaitForSeconds(0.1f);
          break;

        case "LOG AT 2":
          while ((int)BombInfo.GetTime() % 10 == correctTime)
            yield return "trycancel";  // Fixes a really obscure bug with tp
          while ((int)BombInfo.GetTime() % 10 != correctTime)
            yield return "trycancel";
          Buttons[2].OnInteract();
          yield return new WaitForSeconds(0.1f);
          break;

          case "LOG AT 0":
            while ((int)BombInfo.GetTime() % 10 == correctTime)
              yield return "trycancel";  // Fixes a really obscure bug with tp
            while ((int)BombInfo.GetTime() % 10 != correctTime)
              yield return "trycancel";
            Buttons[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
            break;
      }
   }

IEnumerator TwitchForceSolveWait() {
  int twitchTimer = correctTime;
  string twitchTimerCommand = twitchTimer.ToString();
  yield return ProcessTwitchCommand("SUBMIT AT " + twitchTimerCommand);
  yield return new WaitForSeconds(0.1f);
}

   IEnumerator TwitchHandleForcedSolve () {
     Debug.LogFormat("[Lengthy Lockdown " + "#" + ModuleId + "] Twitch Plays has been force-solved. Submitting time as " + correctTime + ".", ModuleId);
     yield return new WaitForSeconds(2f);
     Buttons[0].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[2].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[1].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[0].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[2].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[0].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[2].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[0].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[2].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[1].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[0].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[2].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[1].OnInteract();
     yield return new WaitForSeconds(0.1f);
     Buttons[0].OnInteract();
     StartCoroutine(TwitchForceSolveWait());
     yield return null;
}
}

//Pull up in the Tonka truck!!!
//Thanks to Jah
